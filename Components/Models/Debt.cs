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
        public string date { get; set; }
        public string due_date { get; set; }
        public string debt_status { get; set; }
        public string debt_amount { get; set; }
        public string cleared_amount { get; set; } = "0";
        public string debt_tag { get; set; }
        public string source { get; set; }
        public string debt_note { get; set; }
    }
}
