using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappGroups.Data.Models;

namespace WhatsappGroups.Business.Services
{
    public interface IProductService
    {
        IEnumerable<Product> GetAll();

        Product GetByID(int id);

        bool Insert(Product product);

        bool Update(Product product);

        bool Delete(int id);
    }
}
