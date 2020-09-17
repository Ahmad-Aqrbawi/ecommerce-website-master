using System.Threading.Tasks;
using CP.API.Model;

namespace CP.API.Data
{
    public interface ISuppRepository
    {
        Task<Supplier> SupplierRegister(Supplier supplier, string password);
        Task<Supplier> SupplierLogin(string email, string password);
        Task<bool> SupplierExists(string email);

    }
}