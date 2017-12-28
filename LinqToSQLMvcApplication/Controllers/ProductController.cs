using LinqToSQLMvcApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LinqToSQLMvcApplication.Controllers
{
    public class ProductController : Controller
    {
        private OperationDataContext context;

        public ProductController()
        {
            context = new OperationDataContext();
        }

        private void PrepareCategory(ProductModel model)
        {
            model.Categories = context.categories.AsQueryable<category>().Select(x =>
                    new SelectListItem()
                    {
                        Text = x.cat_name,
                        Value = x.cat_id.ToString()
                    });
        }

        public ActionResult Index(string searchString, double searchString1 = 0, double searchString2 = 0)
        {
            IList<ProductModel> ProductList = new List<ProductModel>();
            var query = from product in context.Products
                        join category in context.categories
                        on product.cat_id equals category.cat_id
                        select new ProductModel
                        {
                            Id = product.product_id,
                            Name = product.product_name,
                            CatId = category.cat_id,
                            CatName = category.cat_name,
                            Price = product.product_price
                        };

            if (!String.IsNullOrEmpty(searchString))
            {
                query = query.Where(s => s.Name.Contains(searchString));
            }

            if (searchString1 < searchString2)
            {
                query = query.Where(s => (s.Price >= searchString1 && s.Price <= searchString2));
            }

            ProductList = query.ToList();
            
            return View(ProductList);
            
        }

        public ActionResult Details(int id)
        {
            ProductModel model = context.Products.Where(x => x.product_id == id).Select(x =>
                                                new ProductModel()
                                                {
                                                    Id = x.product_id,
                                                    Name = x.product_name,
                                                    Price = x.product_price,
                                                    CatId = x.category.cat_id,
                                                    CatName = x.category.cat_name
                                                }).SingleOrDefault();

            return View(model);
        }

        [Authorize]
        public ActionResult Create()
        {
            ProductModel model = new ProductModel();
            PrepareCategory(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(ProductModel model)
        {
            try
            {
                Product product = new Product()
                {
                    product_id = model.Id,
                    product_name = model.Name,
                    product_price = model.Price,
                    cat_id = model.CatId
                };
                context.Products.InsertOnSubmit(product);
                context.SubmitChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            ProductModel model = context.Products.Where(x => x.product_id == id).Select(x =>
                                new ProductModel()
                                {
                                    Id = x.product_id,
                                    Name = x.product_name,
                                    Price = x.product_price,
                                    CatId = x.cat_id
                                }).SingleOrDefault();

            PrepareCategory(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ProductModel model)
        {
            try
            {
                Product product = context.Products.Where(x => x.product_id == model.Id).Single<Product>();
                product.product_id = model.Id;
                product.product_name = model.Name;
                product.product_price = model.Price;
                product.cat_id = model.CatId;

                context.SubmitChanges();
                return RedirectToAction("Index");
            }
            catch {

                
                return View(model);
            }
        }

        [Authorize]
        public ActionResult Delete(int id)
        {

            ProductModel model = context.Products.Where(x => x.product_id == id).Select(x =>
                                  new ProductModel()
                                  {
                                      Id = x.product_id,
                                      Name = x.product_name,
                                      Price = x.product_price,
                                      CatId = x.cat_id
                                  }).SingleOrDefault();
            return View(model);
        }
        [HttpPost]
        public ActionResult Delete(ProductModel model)
        {
            try
            {
                Product product = context.Products.Where(x => x.product_id == model.Id).Single<Product>();
                context.Products.DeleteOnSubmit(product);
                context.SubmitChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }
    }
}