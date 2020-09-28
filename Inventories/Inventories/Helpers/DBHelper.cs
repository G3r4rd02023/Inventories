using Inventories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventories.Helpers
{
    public class DBHelper
    {
        public static int GetState(string description, InventoriesContext db)
        {
            var state = db.States.Where(s => s.Description == description).FirstOrDefault();
            if (state == null)
            {
                state = new State { Description = description, };
                db.States.Add(state);
                db.SaveChanges();              
            }
            return state.StateId;
        }

        public static Response SaveChanges(InventoriesContext db)
        {
            try
            {
                db.SaveChanges();
                return new Response { Succeeded = true, };
            }
            catch (Exception ex)
            {
                var response = new Response { Succeeded = false, };
                if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("_Index"))
                {
                    response.Message = "There is a record with the same value";
                }
                else if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("REFERENCE"))
                {
                    response.Message = "The record can't be delete because it has related records";
                }
                else
                {
                    response.Message = ex.Message;
                }

                return response;
            }
        }

    }
}