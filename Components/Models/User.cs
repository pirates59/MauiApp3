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
        public int userID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } // This will store the hashed password
        public string Email { get; set; }
        public String Currency { get; set; }

        public List<Transaction> Trans { get; set; } = new List<Transaction>();

        public List<Debt> debts { get; set; } = new List<Debt>();
    }
}
