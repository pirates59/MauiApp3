using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp3.Models
{
    public class Debt
    {
        public int debt_id { get; set; }
        public string given_Date { get; set; }
        public string due_Date { get; set; }
        public string debt_amount { get; set; }
        public string cleared_amount { get; set; }
        public string debt_Status { get; set; }
        public string source_name { get; set; }
        public string debt_tag { get; set; }
        public string debt_note { get; set; }
    }
}
