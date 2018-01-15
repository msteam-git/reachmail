using System;

namespace KPISearch.Models
{
    public class ListDetail
    {
        public Guid? Id { get; set; }
        public string ListName { get; set; }
        public int Records { get; set; }
        public string ListStatus { get; set; }
    }
}
