using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PaymenrOrderAPI.Data;
using PaymenrOrderAPI.Models;
using System.Linq.Dynamic.Core;

namespace PaymenrOrderAPI.Controllers
{
    public class PaymentController : Controller
    {
        private PaymenrOrderAPIContext db = new PaymenrOrderAPIContext();

        // GET: Payment
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetData()
        {
            
            //Server Side Parameter
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            List<PaymentOrder> orderList = new List<PaymentOrder>();

            orderList = db.PaymentOrders.ToList();

            int totalrows = orderList.Count;
            if (!string.IsNullOrEmpty(searchValue))//filter
            { 
     
                orderList = orderList.
                    Where(x => x.RouteId.ToString().Contains(searchValue.ToLower()) || x.RemitterName.ToLower().Contains(searchValue.ToLower()) || x.RecepientName.ToLower().Contains(searchValue.ToLower()) 
                    || x.PrimaryAccountNumber.ToLower().Contains(searchValue.ToLower()) || x.Amount.ToString().Contains(searchValue.ToLower()) || x.Reference.ToLower().Contains(searchValue.ToLower()) || x.SystemTraceAuditNumber.ToString().Contains(searchValue.ToLower())).ToList<PaymentOrder>();
            }

            int totalrowsafterfiltering = orderList.Count;
            //sorting
            var queryable = orderList.AsQueryable();
            orderList = queryable.OrderBy(sortColumnName + " " + sortDirection).ToList<PaymentOrder>();

            //paging
            orderList = orderList.Skip(start).Take(length).ToList<PaymentOrder>();


            return Json(new { data = orderList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);

        }

        // GET: Payment/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentOrder paymentOrder = db.PaymentOrders.Find(id);
            if (paymentOrder == null)
            {
                return HttpNotFound();
            }
            return View(paymentOrder);
        }

        // GET: Payment/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Payment/Pay
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OriginatorConversationId,RemitterName,RemitterAddress,RemitterPhoneNumber,RemitterIdType,RemitterIdNumber,Country,Ccy,FinancialInstituion,SourceOfFunds,PrincipalActivity,RecepientName,RecepientPhoneNumber,PrimaryAccountNumber,Amount,RouteId,SystemTraceAuditNumber,Reference")] PaymentOrder paymentOrder)
        {
            if (ModelState.IsValid)
            {
                paymentOrder.OriginatorConversationId = Guid.NewGuid();
                paymentOrder.RouteId = Guid.NewGuid();
                paymentOrder.SystemTraceAuditNumber = Guid.NewGuid();
                paymentOrder.Reference = "STM_" + RandomString(3) + "_" + RandomString(4);
                paymentOrder.PrimaryAccountNumber = paymentOrder.RecepientPhoneNumber.ToString();

                db.PaymentOrders.Add(paymentOrder);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(paymentOrder);
        }

        // GET: Payment/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentOrder paymentOrder = db.PaymentOrders.Find(id);
            if (paymentOrder == null)
            {
                return HttpNotFound();
            }
            return View(paymentOrder);
        }

        // POST: Payment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OriginatorConversationId,RemitterName,RemitterAddress,RemitterPhoneNumber,RemitterIdType,RemitterIdNumber," +
            "Country,Ccy,FinancialInstituion,SourceOfFunds,PrincipalActivity,RecepientName,RecepientPhoneNumber,PrimaryAccountNumbe" +
            "r,Amount,RouteId,SystemTraceAuditNumber,Reference")] PaymentOrder paymentOrder)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paymentOrder).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(paymentOrder);
        }

        // GET: Payment/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentOrder paymentOrder = db.PaymentOrders.Find(id);
            if (paymentOrder == null)
            {
                return HttpNotFound();
            }
            return View(paymentOrder);
        }

        // POST: Payment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            PaymentOrder paymentOrder = db.PaymentOrders.Find(id);
            db.PaymentOrders.Remove(paymentOrder);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



        private static Random random = new Random();
        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }


    }
}

