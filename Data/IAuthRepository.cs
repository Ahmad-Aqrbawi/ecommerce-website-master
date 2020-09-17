using System.Collections.Generic;
using System.Threading.Tasks;
using CP.API.Model;

namespace CP.API.Data
{
    public interface IAuthRepository
    {
         Task<Customer> CustomerRegister (Customer customer,string password);
         Task<Customer> CustomerLogin (string email , string password);
         Task<bool> CustomerExists(string email);

      





          
    }
}