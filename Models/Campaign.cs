using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPISearch.Models
{
    public class Campaign
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public Guid ListId { get; set; }
        public string ListName { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
    }
}
