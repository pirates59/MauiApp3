using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiApp3.Models;

namespace MauiApp3.Models
{
    public class User
    {
        public int userId { get; set; }
        public string user_name { get; set; }
        public string pass { get; set; }
        public string email { get; set; }
        public String Currency { get; set; }

        public List<Transaction> Trans { get; set; } = new List<Transaction>();

        public List<Debt> Debts { get; set; } = new List<Debt>();
    }
}
