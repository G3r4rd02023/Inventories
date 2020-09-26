using Inventories.Models;
using Inventories.Models.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventories.Helpers
{
    public class MovementsHelper : IDisposable
    {
        public static InventoriesContext db = new InventoriesContext();

        public void Dispose()
        {
            db.Dispose();
        }

        public static Response NewOrder(NewOrderView view, string userName)
        {
           using (var transaccion = db.Database.BeginTransaction())
            {
               try
                {
                    var user = db.Users.Where(u => u.UserName == userName).FirstOrDefault();
                    var order = new Order
                    {
                        CompanyID = user.CompanyID,
                        CustomerId = view.CustomerId,
                        Date = view.Date,
                        Remarks = view.Remarks,
                        StateId = DBHelper.GetState("Created", db),

                    };
                    db.Orders.Add(order);
                    db.SaveChanges();
                    var details = db.OrderDetailTmps.Where(odt => odt.UserName == userName).ToList();

                    foreach (var detail in details)
                    {
                        var orderDetail = new OrderDetail
                        {
                            Description = detail.Description,
                            OrderId = order.OrderId,
                            Price = detail.Price,
                            ProductId = detail.ProductId,
                            Quantity = detail.Quantity,
                            TaxRate = detail.TaxRate,
                        };
                        db.OrderDetails.Add(orderDetail);
                        db.OrderDetailTmps.Remove(detail);
                    }
                    db.SaveChanges();
                    transaccion.Commit();
                    return new Response { Succeeded = true, };
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    return new Response
                    {
                        Message = ex.Message,
                        Succeeded = false,
                    };
                }
           }                          
        }
    }
}