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

        public static Response NewCollectionTmp(OpenDay view, string userName)
        {
            using (var transacction = db.Database.BeginTransaction())
            {
                try
                {
                    var user = db.Users.Where(u => u.UserName == userName).FirstOrDefault();
                    var mainWareHouses = db.MainWarehouses.Where(w => w.CompanyId == user.CompanyId).ToList();
                    var wareHouses = db.Warehouses.Where(w => w.CompanyId == view.CompanyId).ToList();

                    var disbursedLoans = db.DisbursedLoans.Where(wH => wH.CompanyId == view.CompanyId).Where(cA=>cA.StateId==2).ToList();

                    //var lastOpenDay = db.OpenDays.Where(od => od.CompanyId == view.CompanyId).Where(of => of.OnOff == true).FirstOrDefault();
                    //lastOpenDay.OnOff = false;
                    //db.Entry(lastOpenDay).State = EntityState.Modified;

                    var openDay = new OpenDay
                    {
                        OpenDate=DateTime.Today,
                        CompanyId=user.CompanyId,
                        UserName=user.UserName,
                        OnOff=true,
                    };
                    db.OpenDays.Add(openDay);
                    db.SaveChanges();                    

                    foreach (MainWarehouse mainWarehouse in mainWareHouses)
                    {

                        var mainInventory = new MainInventory
                        {
                            MainWarehouseId = mainWarehouse.MainWarehouseId,
                            CompanyId = view.CompanyId,
                            Date = openDay.OpenDate,
                            Collection = 0,
                            Pettycash = 0,
                            Administrativeexpense = 0,
                            Subtotal = 0,
                            Renewedcredit = 0,
                            Total = 0,
                            WareHouseInitialLoan = 0,
                            WareHouseTotalLoan = 0,
                            WareHouseTotalBalance = 0,
                        };
                        db.MainInventories.Add(mainInventory);                        
                    }                 
                    
                    foreach (Warehouse warehouse in wareHouses)
                    {

                        var inventory = new Inventory
                        {
                            WarehouseId= warehouse.WarehouseId,
                            CompanyId=view.CompanyId,
                            Date= openDay.OpenDate,
                            Collection =0,
                            Pettycash=0,
                            Administrativeexpense=0,
                            Subtotal=0,
                            Renewedcredit=0,
                            Total=0,
                            InitialLoan=0,
                            TotalLoan=0,
                            TotalBalance=0,
                        };
                        db.Inventories.Add(inventory);                        
                    }               

                    foreach (DisbursedLoan detailDL in disbursedLoans)
                    {
                        var collectionTmp = new CollectionTmp
                        {
                            CompanyId = detailDL.CompanyId,
                            WarehouseId = detailDL.WarehouseId,
                            DisbursedLoanId = detailDL.DisbursedLoanId,
                            UserName = userName,
                            LoanState = detailDL.LoanState,
                            CollectionDate= openDay.OpenDate,
                            Payment =0,
                            CurrentBalance=detailDL.Balance,                            
                        };
                        db.CollectionTmps.Add(collectionTmp);                        
                    }

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

        public static Response NewCollection(string userName)
        {
            using (var transacction = db.Database.BeginTransaction())
            {
                try
                {
                    var user = db.Users.Where(u => u.UserName == userName).FirstOrDefault();
                    var openDay = db.OpenDays.Where(opd => opd.CompanyId == user.CompanyId).Where(onOff => onOff.OnOff.Equals(true)).FirstOrDefault();
                    var wareHouse = db.Warehouses.Where(w => w.UserId == user.UserId).FirstOrDefault();
                    var inventories = db.Inventories.Where(i => i.WarehouseId == wareHouse.WarehouseId)
                        .Where(idate => idate.Date.Year.Equals(openDay.OpenDate.Year))
                        .Where(idate => idate.Date.Month.Equals(openDay.OpenDate.Month))
                        .Where(idate => idate.Date.Day.Equals(openDay.OpenDate.Day)).FirstOrDefault();
                    //var mainInventories = db.MainInventories.Where(mi => mi.MainWarehouseId == user.MainWarehouseId).Where(d => d.Date == openDay.OpenDate).FirstOrDefault();
                    var collectionTmps = db.CollectionTmps.Where(wH => wH.WarehouseId == wareHouse.WarehouseId).ToList();
                                       
                    foreach (CollectionTmp detailcTmp in collectionTmps)
                    {
                        var collection = new Collection
                        {
                            InventoryId = inventories.InventoryId,
                            CompanyId = detailcTmp.CompanyId,
                            WarehouseId = detailcTmp.WarehouseId,
                            DisbursedLoanId = detailcTmp.DisbursedLoanId,
                            UserName = userName,
                            CollectionDate = detailcTmp.CollectionDate,
                            Payment = detailcTmp.Payment,
                            CurrentBalance = detailcTmp.CurrentBalance,
                            LoanState = detailcTmp.LoanState,
                        };

                        db.Collections.Add(collection);
                        db.CollectionTmps.Remove(detailcTmp);

                    }

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

        public static Response UpdateInventories(string userName)
        {
            using (var transacction = db.Database.BeginTransaction())
            {
                try
                {
                    var user = db.Users.Where(u => u.UserName == userName).FirstOrDefault();
                    var openDay = db.OpenDays.Where(opd => opd.CompanyId == user.CompanyId).Where(onOff=>onOff.OnOff.Equals(true)).FirstOrDefault();
                    var wareHouse = db.Warehouses.Where(w => w.UserId == user.UserId).FirstOrDefault();
                    var inventories = db.Inventories.Where(i => i.WarehouseId == wareHouse.WarehouseId)
                        .Where(idate => idate.Date.Year.Equals(openDay.OpenDate.Year))
                        .Where(idate => idate.Date.Month.Equals(openDay.OpenDate.Month))
                        .Where(idate => idate.Date.Day.Equals(openDay.OpenDate.Day)).FirstOrDefault();

                    var inventoryList = db.Inventories.Where(i => i.CompanyId == wareHouse.CompanyId).ToList();
                    var mainInventories = db.MainInventories.Where(mi => mi.MainWarehouseId == user.MainWarehouseId)
                        .Where(date => date.Date.Year.Equals(openDay.OpenDate.Year))
                        .Where(date => date.Date.Month.Equals(openDay.OpenDate.Month))
                        .Where(date => date.Date.Day.Equals(openDay.OpenDate.Day)).FirstOrDefault();

                    var collection = db.Collections.Where(coll => coll.WarehouseId == wareHouse.WarehouseId)
                        .Where(date => date.CollectionDate.Year.Equals(openDay.OpenDate.Year))
                        .Where(date => date.CollectionDate.Month.Equals(openDay.OpenDate.Month))
                        .Where(date => date.CollectionDate.Day.Equals(openDay.OpenDate.Day)).ToList();

                    

                    var disbursedloan = db.DisbursedLoans.Where(dl => dl.WarehouseId == wareHouse.WarehouseId).ToList();


                    //

                    foreach (Collection detailcTmp in collection)
                    {
                        var disbursedloans = db.DisbursedLoans.Where(l => l.DisbursedLoanId == detailcTmp.DisbursedLoanId).FirstOrDefault();

                        disbursedloans.DisbursedLoanId = detailcTmp.DisbursedLoanId;
                        //disbursedloans.CustomerId = detailcTmp.CustomerId;
                        //disbursedloans.WarehouseId = detailcTmp.WarehouseId;
                        //disbursedloans.OrderId = detailcTmp.OrderId;
                        if (detailcTmp.CurrentBalance == 0)
                        {
                            disbursedloans.StateId = DBHelper.GetState("Cancelled", db);
                        }
                        else
                        {
                            disbursedloans.StateId = DBHelper.GetState("Disbursed", db);
                        }
                        //disbursedloans.TypeLoanId = DBHelper.GetTypeLoan("Renewed", db);
                        //disbursedloans.LoanStateId = DBHelper.GetLoanState("Common", db);
                        //disbursedloans.StartDate = detailcTmp.StartDate;
                        //disbursedloans.EndDate = detailcTmp.EndDate;
                        //disbursedloans.Period = detailcTmp.Period;
                        //disbursedloans.UserName = detailcTmp.UserName;
                        //disbursedloans.Remarks = detailcTmp.Remarks;
                        //disbursedloans.BorrowedCapital = detailcTmp.BorrowedCapital;
                        //disbursedloans.Interest = detailcTmp.Interest;
                        //disbursedloans.Total = detailcTmp.Total;
                        disbursedloans.Balance = detailcTmp.CurrentBalance;
                        //disbursedloans.DailyPayment = detailcTmp.DailyPayment;
                        //disbursedloans.OperatingExpenses = detailcTmp.OperatingExpenses;

                        db.Entry(detailcTmp).State = EntityState.Modified;
                    }
                    db.SaveChanges();

                    var totaldisbursedloans = db.DisbursedLoans.Where(l => l.WarehouseId == wareHouse.WarehouseId)
                        .Where(l => l.StateId == 2).ToList();
                    var tdlxday = db.DisbursedLoans.Where(l => l.WarehouseId == wareHouse.WarehouseId)
                        .Where(date => date.StartDate.Year.Equals(openDay.OpenDate.Year))
                        .Where(date => date.StartDate.Month.Equals(openDay.OpenDate.Month))
                        .Where(date => date.StartDate.Day.Equals(openDay.OpenDate.Day)).ToList();

                    var inventory = db.Inventories.Where(l => l.InventoryId == inventories.InventoryId).FirstOrDefault();

                    inventory.InventoryId = inventories.InventoryId;
                    inventory.WarehouseId = wareHouse.WarehouseId;
                    inventory.CompanyId = user.CompanyId;
                    inventory.Date = inventories.Date;
                    inventory.Collection = collection.Sum(col => Convert.ToDouble(col.Payment));
                    inventory.Pettycash = inventories.Pettycash;
                    inventory.Administrativeexpense = tdlxday.Sum(col => Convert.ToDouble(col.OperatingExpenses));
                    inventory.Subtotal = Convert.ToDouble(inventory.Collection) + Convert.ToDouble(inventory.Pettycash) + Convert.ToDouble(inventory.Administrativeexpense);
                    inventory.Renewedcredit = tdlxday.Sum(col => Convert.ToDouble(col.BorrowedCapital)); ;
                    inventory.Total = inventories.Total;

                    inventory.InitialLoan = totaldisbursedloans.Sum(col => Convert.ToDouble(col.BorrowedCapital));
                    inventory.TotalLoan = totaldisbursedloans.Sum(col => Convert.ToDouble(col.Total));
                    inventory.TotalBalance = collection.Sum(col => Convert.ToDouble(col.CurrentBalance));

                    db.Entry(inventory).State = EntityState.Modified;

                    var mainInventory = db.MainInventories.Where(l => l.MainInventoryId == mainInventories.MainInventoryId).FirstOrDefault();
                    mainInventory.MainInventoryId = mainInventories.MainInventoryId;
                    mainInventory.MainWarehouseId = mainInventories.MainWarehouseId;
                    mainInventory.CompanyId = mainInventories.CompanyId;
                    mainInventory.Date = mainInventories.Date;
                    mainInventory.Collection = collection.Sum(col => Convert.ToDouble(col.CurrentBalance));
                    mainInventory.Pettycash = mainInventories.Pettycash;
                    mainInventory.Administrativeexpense = mainInventories.Administrativeexpense;
                    mainInventory.Subtotal = mainInventories.Subtotal;
                    mainInventory.Renewedcredit = mainInventories.Renewedcredit;
                    mainInventory.Total = mainInventories.Total;
                    mainInventory.WareHouseInitialLoan = mainInventories.WareHouseInitialLoan;
                    mainInventory.WareHouseTotalLoan = mainInventories.WareHouseTotalLoan;
                    mainInventory.WareHouseTotalBalance = mainInventories.WareHouseTotalBalance;

                    db.Entry(mainInventory).State = EntityState.Modified;

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