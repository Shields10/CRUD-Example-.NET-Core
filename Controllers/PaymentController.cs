﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PaymenrOrderAPI.Data;
using PaymenrOrderAPI.Models;

namespace PaymenrOrderAPI.Controllers
{
    public class PaymentController : Controller
    {
        private PaymenrOrderAPIContext db = new PaymenrOrderAPIContext();

        // GET: Payment
        public ActionResult Index()
        {
            return View(db.PaymentOrders.ToList());
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

