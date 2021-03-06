﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataHelper;
using Models;

namespace Core
{
    public class LogApiProvider : IApiProvider
    {
        private IDataProvider<LogEntry> _dataProvider;

        public LogApiProvider(IDataProvider<LogEntry> dataProvider)
        {
            this._dataProvider = dataProvider;
        }

        public IList<LogEntry> GetLogsByDate(int? year, string month, int? day)
        {
            IEnumerable<LogEntry> logs = _dataProvider.GetData();

            if (year > 0)
            {
                logs = logs
                    .Where(log => log.Year == year);
            }
            if (!string.IsNullOrWhiteSpace(month))
            {
                logs = logs
                        .Where(log => log.Month.IndexOf(month, StringComparison.OrdinalIgnoreCase) >= 0);

            }
            if (day > 0)
            {
                logs = logs
                        .Where(log => log.Day == day);
            }
            //if (!string.IsNullOrWhiteSpace(searchString))
            //{
            //    logs = logs
            //            .Where(log => log.Text.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0);

            //}

            return logs.ToList();
        }

        public IList<LogEntry> GetTodaysLogs()
        {           
            var day = DateTime.Now.Day;
            var month = DateTime.Now.Month;

            IEnumerable<LogEntry> logs = _dataProvider.GetData();

            var todaysLogs = from log in logs
                             let date = log.GetDateTime()
                             where date.Day == day && date.Month == month
                             select log;

            return todaysLogs.ToList();
        }

        public IList<LogEntry> SortLogs(string order)
        {
            IEnumerable<LogEntry> logs = _dataProvider.GetData();
            if(order.Equals("A", StringComparison.OrdinalIgnoreCase))
            {
                return logs.OrderBy(log => log.GetDateTime()).ToList();
            }
            else
            {
                return logs.OrderByDescending(log => log.GetDateTime()).ToList();
            }
        }
    }
}
