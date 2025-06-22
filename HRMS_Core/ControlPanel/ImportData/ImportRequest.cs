using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.ControlPanel.ImportData
{
    public class ImportRequest
    {
        public IFormFile File { get; set; }
        public string Type { get; set; }
        public string SheetName { get; set; }
        public int RowFrom { get; set; }
        public int RowTo { get; set; }
    }
}
