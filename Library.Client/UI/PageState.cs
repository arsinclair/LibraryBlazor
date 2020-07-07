using System;
using System.Collections.Generic;

namespace Library.Client.UI
{
    public class PageState
    {
        public Uri Uri { get; set; }
        public Dictionary<string, string> QueryString { get; set; }
        public int ModuleId { get; set; }
        public string Action { get; set; }
        public bool EditMode { get; set; }
        public DateTime LastSyncDate { get; set; }
    }
}
