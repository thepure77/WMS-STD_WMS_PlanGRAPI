using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PlanGRDataAccess.Models
{
  

    public partial class sy_Config
    {
        [Key]
        public long Config_Index { get; set; }

        public string Config_Search { get; set; }

        public string Config_Key { get; set; }

        public int? IsActive { get; set; }

        public int? IsDelete { get; set; }
    }
}
