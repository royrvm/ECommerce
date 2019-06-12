using ECommerce.Backend.Models;
using ECommerce.Common.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ECommerce.Backend.Classes
{
    public class MovementsHelper : IDisposable
    {
        private static LocalDataContext db = new LocalDataContext();

        public void Dispose()
        {
            db.Dispose();
        }

        public static Response NewLoan(DisbursedLoan view, string userName)
        {
            using (var transacction= db.Database.BeginTransaction())
            {
                try
                {
                    var user = db.Users.Where(u => u.UserName == userName).FirstOrDefault();

                    var lastOrder = db.Orders.Where(l => l.OrderId == view.OrderId).FirstOrDefault();


                    lastOrder.CompanyId = view.CompanyId;
                    lastOrder.CustomerId = view.CustomerId;
                    lastOrder.WarehouseId = view.WarehouseId;
                    lastOrder.OrderId = view.OrderId;
                    lastOrder.StateId = DBHelper.GetState("Disbursed", db);
                    lastOrder.StartDate = view.StartDate;
                    lastOrder.EndDate = view.EndDate;
                    lastOrder.Period = view.Period;
                    lastOrder.UserName = view.UserName;
                    lastOrder.Remarks = lastOrder.Remarks;
                    lastOrder.BorrowedCapital = lastOrder.BorrowedCapital;
                    lastOrder.Interest = lastOrder.Interest;
                    lastOrder.Total = lastOrder.Total;
                    lastOrder.Balance = lastOrder.Balance;
                    lastOrder.DailyPayment = lastOrder.DailyPayment;
                    lastOrder.OperatingExpenses = lastOrder.OperatingExpenses;

                    db.Entry(lastOrder).State = EntityState.Modified;
                    db.SaveChanges();

                    var disbursedLoan = new DisbursedLoan
                    {
                        CompanyId = view.CompanyId,
                        CustomerId = view.CustomerId,
                        WarehouseId = view.WarehouseId,
                        OrderId = view.OrderId,
                        StateId = DBHelper.GetState("Disbursed", db),
                        TypeLoanId = DBHelper.GetTypeLoan("Renewed", db),
                        LoanStateId = DBHelper.GetLoanState("Common", db),
                        StartDate = view.StartDate,
                        EndDate = view.EndDate,
                        Period = view.Period,
                        UserName = view.UserName,
                        Remarks = view.Remarks,
                        BorrowedCapital = view.BorrowedCapital,
                        Interest = view.Interest,
                        Total = view.Total,
                        Balance = view.Balance,
                        DailyPayment = view.DailyPayment,
                        OperatingExpenses = view.OperatingExpenses,
                    };
                    db.DisbursedLoans.Add(disbursedLoan);
                    db.SaveChanges();
                    transacction.Commit();
                    return new Response { Succeeded = true, };
                }
                catch (Exception ex)
                {
                    transacction.Rollback();
                    return new Response
                    {
                        Message=ex.Message,
                        Succeeded= false,
                    };
                }
            }
            
        }

        public static Response NewCollection(DisbursedLoan view, string userName)
        {
            using (var transacction = db.Database.BeginTransaction())
            {
                try
                {
                    var user = db.Users.Where(u => u.UserName == userName).FirstOrDefault();

                    var lastOrder = db.Orders.Where(l => l.OrderId == view.OrderId).FirstOrDefault();


                    lastOrder.CompanyId = view.CompanyId;
                    lastOrder.CustomerId = view.CustomerId;
                    lastOrder.WarehouseId = view.WarehouseId;
                    lastOrder.OrderId = view.OrderId;
                    lastOrder.StateId = DBHelper.GetState("Disbursed", db);
                    lastOrder.StartDate = view.StartDate;
                    lastOrder.EndDate = view.EndDate;
                    lastOrder.Period = view.Period;
                    lastOrder.UserName = view.UserName;
                    lastOrder.Remarks = lastOrder.Remarks;
                    lastOrder.BorrowedCapital = lastOrder.BorrowedCapital;
                    lastOrder.Interest = lastOrder.Interest;
                    lastOrder.Total = lastOrder.Total;
                    lastOrder.Balance = lastOrder.Balance;
                    lastOrder.DailyPayment = lastOrder.DailyPayment;
                    lastOrder.OperatingExpenses = lastOrder.OperatingExpenses;

                    db.Entry(lastOrder).State = EntityState.Modified;
                    db.SaveChanges();

                    var disbursedLoan = new DisbursedLoan
                    {
                        CompanyId = view.CompanyId,
                        CustomerId = view.CustomerId,
                        WarehouseId = view.WarehouseId,
                        OrderId = view.OrderId,
                        StateId = DBHelper.GetState("Disbursed", db),
                        TypeLoanId = DBHelper.GetTypeLoan("Renewed", db),
                        LoanStateId = DBHelper.GetLoanState("Common", db),
                        StartDate = view.StartDate,
                        EndDate = view.EndDate,
                        Period = view.Period,
                        UserName = view.UserName,
                        Remarks = view.Remarks,
                        BorrowedCapital = view.BorrowedCapital,
                        Interest = view.Interest,
                        Total = view.Total,
                        Balance = view.Balance,
                        DailyPayment = view.DailyPayment,
                        OperatingExpenses = view.OperatingExpenses,
                    };
                    db.DisbursedLoans.Add(disbursedLoan);
                    db.SaveChanges();
                    transacction.Commit();
                    return new Response { Succeeded = true, };
                }
                catch (Exception ex)
                {
                    transacction.Rollback();
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