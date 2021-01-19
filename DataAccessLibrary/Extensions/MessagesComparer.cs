using System;
using System.Collections.Generic;
using DataAccessLibrary.Models;

namespace DataAccessLibrary.Extensions
{
    public class MessagesComparer : IComparer<Entity>
    {
        public int Compare(Entity x, Entity y)
        {
            var date1 = (DateTime?)(x["SentOn"] ?? x["ProvisionalSentOn"]);
            var date2 = (DateTime?)(y["SentOn"] ?? y["ProvisionalSentOn"]);
            return Nullable.Compare(date1, date2);
        }
    }
}