using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPISearch.Models
{
    public class KPSACampaign
    {
        public string Account { get; set; }
        public string CampaignName { get; set; }
        public string ListName { get; set; }
        public DateTime? Schedule1 { get; set; }
        public DateTime? Schedule2 { get; set; }
        public DateTime? Schedule3 { get; set; }
        public DateTime? Schedule4 { get; set; }
        public string Status { get; set; }
    }
}
