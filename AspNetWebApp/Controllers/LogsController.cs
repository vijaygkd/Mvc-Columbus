﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Core;
using DataHelper;
using Models;

namespace AspNetWebApp.Controllers
{
    public class LogsController : Controller
    {
        private IApiProvider _apiProvider;

        public LogsController() : base()
        {
            _apiProvider = new LogApiProvider(new LogDataProvider());
        }

        // GET: Logs
        [Route("logs/{year=0}/{month=}/{day=0}")]
        public ActionResult Index(string searchString, int? year, string month, int? day)
        {
            //data validation of input varibles

            var logs = _apiProvider.GetLogsByDate(year, month, day);

            ViewBag.Note = "Diary of Columbus";
            return View(logs.ToList());
        }

        [Route("logs/sort/{order=a}")]
        public ActionResult Sort(string order)
        {
            //Data verification
            if (!order.Equals("a", StringComparison.OrdinalIgnoreCase) &&
               !order.Equals("d", StringComparison.OrdinalIgnoreCase))
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return View("Error");
            }

            var logs = _apiProvider.SortLogs(order);

            var sortOrder = order.Equals("a", StringComparison.OrdinalIgnoreCase) ? "Ascending" : "Descending";
            ViewBag.Note = $"Logs Sorted {sortOrder}";
            return View("Index", logs);

        }

        [Route("logs/today")]
        public ActionResult Today()
        {
            var logs = _apiProvider.GetTodaysLogs();
            if (logs.Any())
            {
                ViewBag.Note = "Today in history of Columbus";
            }
            else
            {
                ViewBag.Note = "No history today!";
            }
            
            return View("Index", logs);
        }
    }
}