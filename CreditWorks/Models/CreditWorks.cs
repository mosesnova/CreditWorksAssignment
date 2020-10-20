using System;
using System.Collections.Generic;

namespace CreditWorksAssignment.Models
{
    public partial class CreditWorks
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public string Year { get; set; }
        public double? Weight { get; set; }
    }
}
