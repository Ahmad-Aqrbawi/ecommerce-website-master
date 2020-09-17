using System.Linq;
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
    public class PhotoProductController : ControllerBase
    {
        private readonly ICPRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySetting> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        public PhotoProductController(ICPRepository repo, IMapper mapper, IOptions<CloudinarySetting> cloudinaryConfig)
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

         [HttpGet("{id}", Name = "GetPhoto")]  //11-5
            public async Task<IActionResult> GetPhoto(int id)
            {
                var photoFromRepository = await _repo.GetPhotoForProduct(id);
                var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepository);
                return Ok(photo);
            }



        [Authorize(Policy = "EstablishAStore")]
         [HttpPost("{productId}")]
        public async Task<IActionResult> AddPhotoProduct( int productId , [FromForm] PhotoForCreateDto photoForCreateDto)
        {
            var product = await _repo.GetProduct(productId);
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
            var photo = _mapper.Map<PhotoForProduct>(photoForCreateDto);
            photo.ProductId =productId;
            _repo.Add(photo);
            if (await _repo.SaveAll())
            {
                var photoToRetrun = _mapper.Map<PhotoForReturnDto>(photo);
                return CreatedAtRoute("GetPhoto", new { id = photo.PhotoId }, photoToRetrun);

            }

            return BadRequest("خطا في اضافة الصورة");
        }


         [HttpPost("{productId}/{id}/setMainPhotoProduct")]
        public async Task<IActionResult> SetMainPhotoProduct(int productId,int id)
        {
            var productFromRepo = await _repo.GetProduct(productId);
            if(!productFromRepo.PhotoForProducts.Any(p=>p.PhotoId==id))
            return Unauthorized();
            var DesiredMainPhoto = await _repo.GetPhotoForProduct(id);
            if(DesiredMainPhoto.IsMain)
            return BadRequest("هذه هي الصورة الأساسية بالفعل");
            var CurrentMainPhoto = await _repo.GetMainPhotoForProduct(productId);
            CurrentMainPhoto.IsMain=false;
            DesiredMainPhoto.IsMain=true;
            if(await _repo.SaveAll())
            return NoContent();
            return BadRequest("لايمكن تعديل الصورة الأساسية");
            
        }


         [HttpDelete("{productId}/{id}")]
        public async Task<IActionResult> DeleteProduct(int productId, int id)
        {
            var productFromRepo = await _repo.GetProduct(productId);
            if (!productFromRepo.PhotoForProducts.Any(p => p.PhotoId == id))
                return Unauthorized();
            var Photo = await _repo.GetPhotoForProduct(id);
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
