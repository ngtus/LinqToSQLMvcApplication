using LinqToSQLMvcApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LinqToSQLMvcApplication.Controllers
{
    public class OrderController : Controller
    {
        private OperationDataContext context;
        public OrderController()
        {
            context = new OperationDataContext();
        }

        public ActionResult Index(string searchString)
        {
            IList<OrderModel> orderList = new List<OrderModel>();
            var query = from order in context.orders
                        select order;
            if (!String.IsNullOrEmpty(searchString))
            {
                query = query.Where(s => s.order_status.Contains(searchString));
            }

            var orders = query.ToList();
            foreach (var orderData in orders)
            {
                orderList.Add(new OrderModel()
                {
                    Id = orderData.order_id,
                    Email = orderData.cus_email,
                    Phone = orderData.cus_phone,
                    Address = orderData.cus_address,
                    Info = orderData.order_info,
                    Status = orderData.order_status
                });
            }

            

            return View(orderList);
        }

        public ActionResult Details(int id)
        {
            OrderModel model = context.orders.Where(x => x.order_id == id).Select(x =>
                                                new OrderModel()
                                                {
                                                    Id = x.order_id,
                                                    Email = x.cus_email,
                                                    Phone = x.cus_phone,
                                                    Address = x.cus_address,
                                                    Info = x.order_info,
                                                    Status = x.order_status

                                                }).SingleOrDefault();

            return View(model);
        }

        
        public ActionResult Create()
        {
            OrderModel model = new OrderModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(OrderModel model)
        {
            try
            {
                order order = new order()
                {
                    order_id = model.Id,
                    cus_email = model.Email,
                    cus_phone = model.Phone,
                    cus_address = model.Address,
                    order_info = model.Info,
                    order_status = model.Status
                };
                context.orders.InsertOnSubmit(order);
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
            OrderModel model = context.orders.Where(x => x.order_id == id).Select(x =>
                                new OrderModel()
                                {
                                    Id = x.order_id,
                                    Email = x.cus_email,
                                    Phone = x.cus_phone,
                                    Address = x.cus_address,
                                    Info = x.order_info,
                                    Status = x.order_status
                                }).SingleOrDefault();


            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(OrderModel model)
        {
            try
            {

                order order = context.orders.Where(x => x.order_id == model.Id).Single<order>();
                order.order_id = model.Id;
                order.cus_email = model.Email;
                order.cus_phone = model.Phone;
                order.cus_address = model.Address;
                order.order_info = model.Info;
                order.order_status = model.Status;
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

            OrderModel model = context.orders.Where(x => x.order_id == id).Select(x =>
                                  new OrderModel()
                                  {
                                      Id = x.order_id,
                                      Email = x.cus_email,
                                      Phone = x.cus_phone,
                                      Address = x.cus_address,
                                      Info = x.order_info,
                                      Status = x.order_status
                                  }).SingleOrDefault();
            return View(model);
        }
        [HttpPost]
        public ActionResult Delete(OrderModel model)
        {
            try
            {
                order order = context.orders.Where(x => x.order_id == model.Id).Single<order>();
                context.orders.DeleteOnSubmit(order);
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