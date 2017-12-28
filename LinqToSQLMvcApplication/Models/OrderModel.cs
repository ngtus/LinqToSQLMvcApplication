using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LinqToSQLMvcApplication.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        [DataType(DataType.EmailAddress, ErrorMessage = "Error")]
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber,ErrorMessage = "Error")]
        public string Phone { get; set; } 
        public string Address { get; set; }
        public string Info { get; set; }
        public string Status { get; set; }
    }
}