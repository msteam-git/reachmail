using System;

namespace KPISearch.Models
{
    public class TownsconCampaign
    {
        public string Account { get; set; }
        public string CampaignName { get; set; }
        public string ListName { get; set; }
        public string Segment { get; set; }
        public DateTime? Schedule { get; set; }
        public string Status { get; set; }
    }
}
