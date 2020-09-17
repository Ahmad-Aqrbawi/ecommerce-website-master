using System.Threading.Tasks;
using CP.API.Model;
using Microsoft.EntityFrameworkCore;

namespace CP.API.Data
{
    public class SuppRepository : ISuppRepository
    {
        private readonly DataContext _context;
        public SuppRepository(DataContext context)
        {
            _context = context;
        }

         public async Task<Supplier> SupplierLogin(string email, string password)
        {
            var supplier = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (supplier == null) return null;
            // if (!VerificationPasswordHash(password, supplier.PasswordSalt, supplier.PasswordHash))
            //     return null;
            return supplier;
        }

         private bool VerificationPasswordHash(string password, byte[] passwordSalt, byte[] passwordHash)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHsh = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHsh.Length; i++)
                {
                    if (computedHsh[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }

          public async Task<Supplier> SupplierRegister(Supplier supplier, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordSalt, out passwordHash);
            // supplier.PasswordHash = passwordHash;
            // supplier.PasswordSalt = passwordSalt;
            await _context.Users.AddAsync(supplier);
            await _context.SaveChangesAsync();
            return supplier;
        }
        private void CreatePasswordHash(string password, out byte[] passwordSalt, out byte[] passwordHash)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        
         public async Task<bool> SupplierExists(string email)
        {
            if (await _context.Users.AnyAsync(x => x.Email == email))
                return true;
            return false;
        }
        
    }
}