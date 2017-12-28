using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LinqToSQLMvcApplication.Models
{
    public class ProductModel
    {
        public ProductModel()
        {
            Categories = new List<SelectListItem>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        [DisplayName("Category ID")]
        public int CatId { get; set; }
        public string CatName { get; set; }
        [DisplayName("Category Name")]
        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}