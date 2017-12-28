using LinqToSQLMvcApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LinqToSQLMvcApplication.Controllers
{
    public class CategoryController : Controller
    {
        private OperationDataContext context;
        public CategoryController()
        {
            context = new OperationDataContext();
        }

        public ActionResult Index()
        {
            IList<CategoryModel> categoryList = new List<CategoryModel>();
            var query = from category in context.categories
                        select category;

            var categories = query.ToList();
            foreach (var categoryData in categories)
            {
                categoryList.Add(new CategoryModel()
                {
                    Id = categoryData.cat_id,
                    Name = categoryData.cat_name
                });
            }

            return View(categoryList);
            
        }

        public ActionResult Details(int id)
        {
            CategoryModel model = context.categories.Where(x => x.cat_id == id).Select(x =>
                                                new CategoryModel()
                                                {
                                                    Id = x.cat_id,
                                                    Name = x.cat_name
                                                    
                                                }).SingleOrDefault();

            return View(model);
        }

        [Authorize]
        public ActionResult Create()
        {
            CategoryModel model = new CategoryModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CategoryModel model)
        {
            try
            {
                category category = new category()
                {
                    cat_id = model.Id,
                    cat_name = model.Name
                };
                context.categories.InsertOnSubmit(category);
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
            CategoryModel model = context.categories.Where(x => x.cat_id == id).Select(x =>
                                new CategoryModel()
                                {
                                    Id = x.cat_id,
                                    Name = x.cat_name
                                }).SingleOrDefault();

            
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(CategoryModel model)
        {
            try
            {

                category category = context.categories.Where(x => x.cat_id == model.Id).Single<category>();
                category.cat_id = model.Id;
                category.cat_name = model.Name;
                context.SubmitChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }

        [Authorize]
        public ActionResult Delete(int id)
        {

            CategoryModel model = context.categories.Where(x => x.cat_id == id).Select(x =>
                                  new CategoryModel()
                                  {
                                      Id = x.cat_id,
                                      Name = x.cat_name
                           
                                  }).SingleOrDefault();
            return View(model);
        }
        [HttpPost]
        public ActionResult Delete(CategoryModel model)
        {
            try
            {
                category category = context.categories.Where(x => x.cat_id == model.Id).Single<category>();
                context.categories.DeleteOnSubmit(category);
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