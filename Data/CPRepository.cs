using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CP.API.Dto;
using CP.API.Helpers;
using CP.API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CP.API.Data
{
    public class CPRepository : ICPRepository
    {

        private readonly DataContext _context;
        public CPRepository(DataContext context)
        {
            _context = context;

        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }
        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }



        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            var customer = await _context.Customers.ToListAsync();
            return customer;
        }

        public async Task<Customer> GetCustomer(int id)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(u => u.CustomerId == id);
            return customer;
        }

        public async Task<Category> GetCategory(int id)
        {
            var category = await _context.Categories.Include(p => p.Sections).FirstOrDefaultAsync(u => u.CategoryId == id);
            return category;
        }

        public async Task<IEnumerable<Category>> GetCategorys()
        {
            var categorys = await _context.Categories.Include(p => p.Sections).ToListAsync();
            return categorys;
        }

        public async Task<Product> GetProduct(int id)
        {
            var product = await _context.Products.Include(s => s.Supplier).Include(e=>e.Section).Include(w => w.PhotoForProducts).FirstOrDefaultAsync(u => u.ProductId == id);

            return product;
        }


        public async Task<PagedList<Product>> GetProducts(ProductParams productParams)
        {
            var products = _context.Products.Include(p => p.Section).Include(s => s.Supplier).Include(w => w.PhotoForProducts).AsQueryable();

            products = products.Where(f => (f.ProductNumber == productParams.ProductNumber ||
                                           f.ProductNameEnglish == productParams.ProductNameEnglish ||
                                            f.ProductNameArabic == productParams.ProductNameArabic));
            return await PagedList<Product>.CreateAsync(products, productParams.PageNumber, productParams.PageSize);

        }

        public async Task<Payment> GetPayment(int id)
        {
            var payment = await _context.Payments.FirstOrDefaultAsync(u => u.PaymentId == id);

            return payment;
        }

        public async Task<IEnumerable<Payment>> GetPayments()
        {
            var payments = await _context.Payments.ToListAsync();
            return payments;
        }

        public async Task<Order> GetOrder(int id)
        {
            var order = await _context.Orders.Include(s => s.Shipper).Include(p => p.Payment).Include(p => p.OrderDetails).FirstOrDefaultAsync(u => u.OrderId == id);

            return order;
        }

        public async Task<IEnumerable<Order>> GetOrders()
        {
            var orders = await _context.Orders.ToListAsync();
            return orders;
        }

        public async Task<Shipper> GetShipper(int id)
        {
            var shipper = await _context.Shippers.Include(o => o.Orders).FirstOrDefaultAsync(u => u.ShipperId == id);

            return shipper;
        }

        public async Task<PagedList<Shipper>> GetShippers(ShipperParams shipperParams)
        {
            var shippers = _context.Shippers.Include(o => o.Orders).AsQueryable();
            shippers = shippers.Where(f => (f.CompanyName == shipperParams.CompanyName ||
                                          f.Phone == shipperParams.Phone));


            return await PagedList<Shipper>.CreateAsync(shippers, shipperParams.PageNumber, shipperParams.PageSize);
        }

        public async Task<Supplier> GetSupplier(int id)
        {
            var supplier = await _context.Users.Include(p => p.Products).Include(o=>o.PhotoForSuppliers).FirstOrDefaultAsync(u => u.Id == id);

            return supplier;
        }

        public async Task<PagedList<Supplier>> GetSuppliers(SupplierParams supplierParams)
        {
            var suppliers = _context.Users.Include(p => p.Products).Include(o=>o.PhotoForSuppliers).AsQueryable();
            suppliers = suppliers.Where(f => (f.UserName == supplierParams.FacilityOwnerName ||
                f.Phone == supplierParams.Phone ||
                                           f.IdNumber == supplierParams.IdNumber));


            return await PagedList<Supplier>.CreateAsync(suppliers, supplierParams.PageNumber, supplierParams.PageSize);
        }

        public async Task<OrderDetail> GetOrderDetail(int id)
        {
            var orderDetail = await _context.OrderDetails.Include(p => p.Product).FirstOrDefaultAsync(u => u.OrderDetailId == id);

            return orderDetail;
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetails()
        {
            var orderDetails = await _context.OrderDetails.Include(p => p.Product).ToListAsync();
            var count = orderDetails.Sum(s => s.Price);
            return orderDetails;
        }

        public async Task<PhotoForSupplier> GetPhoto(int id)
        {
            var photo = await _context.PhotoForSuppliers.FirstOrDefaultAsync(u => u.PhotoId == id);

            return photo;
        }

        public async Task<IEnumerable<Section>> GetSections()
        {
            var sections = await _context.Sections.Include(o => o.Products).ToListAsync();
            return sections;
        }

        public async Task<Section> GetSection(int id)
        {
            var section = await _context.Sections.Include(p => p.Products).FirstOrDefaultAsync(u => u.SectionId == id);

            return section;
        }

        public async Task<Discount> GetDiscount(int id)
        {
            var discount = await _context.Discounts.FirstOrDefaultAsync(u => u.DiscountId == id);

            return discount;
        }

        public async Task<PagedList<Discount>> GetDiscounts(DiscountParams discountParams)
        {
            var discounts = _context.Discounts.AsQueryable();
            discounts = discounts.Where(f => (f.DiscountName == discountParams.DiscountName ||
                                           f.CouponCode == discountParams.CouponCode
                                                                                   ));


            return await PagedList<Discount>.CreateAsync(discounts, discountParams.PageNumber, discountParams.PageSize);
        }

        public async Task<int> GetProductCount()
        {
            var products = await _context.Products.ToListAsync();
            var count = products.Count();
            return count;
        }

        public async Task<int> GetOrderCount()
        {
            var orders = await _context.Orders.ToListAsync();
            var count = orders.Count();
            return count;
        }

        public async Task<int> GetOrderNowCount()
        {
            var orders = await _context.OrderDetails.Where(o=>o.BillDate == DateTime.Now).ToListAsync();
            var count = orders.Count();
            return count;
        }

        public async Task<decimal> GetOrderTotalCount()
        {
            var orders = await _context.OrderDetails.SumAsync(o=>o.Total);
            
            return orders;
        }

       

       
        public async Task<IEnumerable<SocialCommunication>> GetSocialCommunications()
        {
            var socialCommunications = await _context.SocialCommunications.ToListAsync();
            return socialCommunications;
        }

        public async Task<SocialCommunication> GetSocialCommunication(int id)
        {
             var socialCommunication = await _context.SocialCommunications.FirstOrDefaultAsync(u => u.SocialCommunicationId == id);

            return socialCommunication;
        }

        public async Task<Section> GetSectionWhereCatgeoryId(int catgeoryId)
        {
            var sectionWhereCatgeoryId = await _context.Sections.FirstOrDefaultAsync(u => u.CategoryId == catgeoryId);

            return sectionWhereCatgeoryId;
        }

        public async Task<PhotoForSupplier> GetPhotoForSupplier(int id)
        {
            var photo = await _context.PhotoForSuppliers.IgnoreQueryFilters().FirstOrDefaultAsync(p=>p.PhotoId==id);
            return photo;
        }

        

        public async Task<PhotoForSupplier> GetMainPhotoForSupplier(int supplierId)
        {
             return await _context.PhotoForSuppliers.Where(u=>u.SupplierId==supplierId).FirstOrDefaultAsync(p=>p.IsMain);
        }

        public async Task<PhotoForProduct> GetPhotoForProduct(int id)
        {
            var photo = await _context.PhotoForProducts.FirstOrDefaultAsync(u => u.PhotoId == id);

            return photo;
        }

        public async Task<PhotoForProduct> GetMainPhotoForProduct(int productId)
        {
            return await _context.PhotoForProducts.Where(u=>u.ProductId==productId).FirstOrDefaultAsync(p=>p.IsMain);
        }

        public Task<Shipper> GetShipper(int id, bool isCurrentUser)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            var products = await _context.Products.Include(o => o.PhotoForProducts).ToListAsync();
            return products;
        }
    }
} 