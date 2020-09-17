using System.Threading.Tasks;
using CP.API.Model;
using Microsoft.EntityFrameworkCore;

namespace CP.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<Customer> CustomerLogin(string email, string password)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Email == email);
            if (customer == null) return null;
            if (!VerificationPasswordHash(password, customer.PasswordSalt, customer.PasswordHash))
                return null;
            return customer;
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
        public async Task<Customer> CustomerRegister(Customer customer, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordSalt, out passwordHash);
            customer.PasswordHash = passwordHash;
            customer.PasswordSalt = passwordSalt;
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        
        private void CreatePasswordHash(string password, out byte[] passwordSalt, out byte[] passwordHash)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> CustomerExists(string email)
        {
            if (await _context.Customers.AnyAsync(x => x.Email == email))
                return true;
            return false;
        }

    }
}