using WhatsappGroups.Business.Services;
using WhatsappGroups.Data.Models;
using Microsoft.AspNet.Mvc;

namespace WhatsappGroups.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        public ActionResult Index()
        {
            var products = _productService.GetAll();
            return View(products);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var product = _productService.GetByID(id);
            if (product != null)
            {
                return View(product);
            }
            else
            {
                ModelState.AddModelError("", "Unable to find requested Product.");
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            var success = _productService.Update(product);
            if (success)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Unable to Edit requested Product.");
                return View(product);
            }
        }

        public ActionResult Delete(int id)
        {
            var success = _productService.Delete(id);
            if (success)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Unable to Edit requested Product.");
                return View();
            }
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(Product product)
        {
            var success = _productService.Insert(product);
            if(success)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Unable to Add new Product.");
                return View();
            }
        }
    }
}