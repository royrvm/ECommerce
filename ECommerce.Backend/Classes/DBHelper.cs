using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ECommerce.Backend.Models;
using ECommerce.Common.Models;

namespace ECommerce.Backend.Classes
{
    public class DBHelper
    {
        
        //public static Response SaveChabges(ECommerce db)
        //{
        //    try
        //    {
        //        db.SaveChanges();
        //        return new Response { Succeeded = true, };
        //    }
        //    catch (Exception)
        //    {
        //        response.Message=Exception.
        //    }
        //}
        public static int GetState(string description, LocalDataContext db)
        {
            var state = db.States.Where(s => s.Description == description).FirstOrDefault();
            if (state==null)
            {
                state = new State { Description = description, };
                db.States.Add(state);
                db.SaveChanges();
            }

            return state.StateId;
        }
    }
}