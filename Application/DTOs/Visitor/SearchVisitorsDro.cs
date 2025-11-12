using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Visitor
{
    public class SearchVisitorsDro
    {
        public bool IsInside { get; set; }
        public string searchQuery { get; set; }
        public DateTime? EnterTime { get; set; }
        public DateTime? ExitTime { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }

    }
}
