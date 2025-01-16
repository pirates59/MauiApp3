using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiApp3.Models;

namespace MauiApp3.Models
{
    // Class representing a user in the system
    public class User
    {
        // Unique identifier for the user
        public int userId { get; set; }

        // Username for the user
        public string user_name { get; set; }

        // Password for user authentication
        public string pass { get; set; }

        // Email address of the user
        public string email { get; set; }

        // Currency used by the user (e.g., "USD", "EUR")
        public String Currency { get; set; }

        // List of transactions made by the user
        public List<Transaction> Trans { get; set; } = new List<Transaction>();

        // List of debts associated with the user
        public List<Debt> Debts { get; set; } = new List<Debt>();
    }
}
