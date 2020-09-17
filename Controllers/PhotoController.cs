using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CP.API.Data;
using CP.API.Dto;
using CP.API.Helpers;
using CP.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CP.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {

        private readonly ICPRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySetting> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        public PhotoController(ICPRepository repo, IMapper mapper, IOptions<CloudinarySetting> cloudinaryConfig)
        {
            _cloudinaryConfig = cloudinaryConfig;
            _mapper = mapper;
            _repo = repo;
            Account acc = new Account(
             _cloudinaryConfig.Value.CloudName,
             _cloudinaryConfig.Value.ApiKey,
             _cloudinaryConfig.Value.ApiSecret
           );
            _cloudinary = new Cloudinary(acc);
        }

        [HttpGet("{id}", Name = "GetPhotoSupplier")]  //11-5
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photoFromRepository = await _repo.GetPhotoForSupplier(id);
            var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepository);
            return Ok(photo);
        }



        [HttpPost("{supplierId}")]
        public async Task<IActionResult> AddPhotosupplier(int supplierId, [FromForm] PhotoForCreateDto photoForCreateDto)
        {
            if (supplierId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            var supplier = await _repo.GetSupplier(supplierId);
            var file = photoForCreateDto.File;
            var uploadResult = new ImageUploadResult();
            if (file != null && file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation()
                        .Width(500).Height(500).Crop("scale")
                    };
                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }
            photoForCreateDto.Url = uploadResult.Uri.ToString();
            photoForCreateDto.PublicId = uploadResult.PublicId;
            var photo = _mapper.Map<PhotoForSupplier>(photoForCreateDto);
            photo.SupplierId = supplierId;
            if (!supplier.PhotoForSuppliers.Any(p => p.IsMain))
                photo.IsMain = true;
            _repo.Add(photo);
            if (await _repo.SaveAll())
            {
                var photoToRetrun = _mapper.Map<PhotoForReturnDto>(photo);
                return CreatedAtRoute("GetPhotoSupplier", new { id = photo.PhotoId }, photoToRetrun);

            }

            return BadRequest("خطا في اضافة الصورة");
        }


        [HttpPost("{supplierId}/{id}/setMainSupplier")]
        public async Task<IActionResult> SetMainPhotoSupplier(int supplierId, int id)
        {
            if (supplierId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            var supplierFromRepo = await _repo.GetSupplier(supplierId);
            if (!supplierFromRepo.PhotoForSuppliers.Any(p => p.PhotoId == id))
                return Unauthorized();
            var DesiredMainPhoto = await _repo.GetPhotoForSupplier(id);
            if (DesiredMainPhoto.IsMain)
                return BadRequest("هذه هي الصورة الأساسية بالفعل");
            var CurrentMainPhoto = await _repo.GetMainPhotoForSupplier(supplierId);
            CurrentMainPhoto.IsMain = false;
            DesiredMainPhoto.IsMain = true;
            if (await _repo.SaveAll())
                return NoContent();
            return BadRequest("لايمكن تعديل الصورة الأساسية");

        }

        [HttpDelete("{supplierId}/{id}")]
        public async Task<IActionResult> DeleteSupplier(int supplierId, int id)
        {
            if (supplierId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            var supplierFromRepo = await _repo.GetSupplier(supplierId);
            if (!supplierFromRepo.PhotoForSuppliers.Any(p => p.PhotoId == id))
                return Unauthorized();
            var Photo = await _repo.GetPhotoForSupplier(id);
            if (Photo.IsMain)
                return BadRequest("لايمكن حذف الصورة الأساسية");
            if (Photo.PublicId != null)
            {
                var deleteParams = new DeletionParams(Photo.PublicId);
                var result = this._cloudinary.Destroy(deleteParams);
                if (result.Result == "ok")
                {
                    _repo.Delete(Photo);
                }
            }
            if (Photo.PublicId == null)
            {
                _repo.Delete(Photo);
            }

            if (await _repo.SaveAll())
                return Ok();
            return BadRequest("فشل حذف الصورة");

        }


    }
}