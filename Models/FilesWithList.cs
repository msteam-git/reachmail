using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KPISearch.Models
{
    public class FilesWithList
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string ListName { get; set; }
        public int Records { get; set; }
    }
}
