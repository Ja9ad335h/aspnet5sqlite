using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappGroups.Data.Models;
using WhatsappGroups.Data;
using WhatsappGroups.Data.Contexts;

namespace WhatsappGroups.Business.Services
{
    public class ProductService : IProductService
    {
        private WhatsappGroupsDataContext _dataDb;
        public ProductService(WhatsappGroupsDataContext dataDb)
        {
            _dataDb = dataDb;
        }

        public bool Delete(int id)
        {
            var product = _dataDb.Products.SingleOrDefault(p => p.Id == id);
            if(product != null)
            {
                _dataDb.Products.Remove(product);
                return _dataDb.SaveChanges() > 0;
            }
            return false;
        }

        public IEnumerable<Product> GetAll()
        {
            return _dataDb.Products;
        }

        public Product GetByID(int id)
        {
            return _dataDb.Products.SingleOrDefault(p => p.Id == id);
        }

        public bool Insert(Product product)
        {
            _dataDb.Products.Add(product);
            return _dataDb.SaveChanges() > 0;
        }

        public bool Update(Product product)
        {
            _dataDb.Products.Update(product);
            return _dataDb.SaveChanges() > 0;
        }
    }
}
