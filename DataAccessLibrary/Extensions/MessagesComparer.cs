using System;
using System.Collections.Generic;
using DataAccessLibrary.Models;

namespace DataAccessLibrary.Extensions
{
    public class MessagesComparer : IComparer<Message>
    {
        public int Compare(Message x, Message y)
        {
            var date1 = x.SentOn ?? x.ProvisionalSentOn;
            var date2 = y.SentOn ?? y.ProvisionalSentOn;
            return Nullable.Compare<DateTime>(date1, date2);
        }
    }
}