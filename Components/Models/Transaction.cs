using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp3.Models
{
    public class Transaction
    {
        public int transaction_id { get; set; }
        public string transaction_Date { get; set; }
        public string transaction_amount { get; set; }
        public string transaction_type { get; set; }
        public string transaction_tag { get; set; }
        public string transaction_note { get; set; }


    }
}
