using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp3.Models
{
    // Class representing a financial transaction
    public class Transaction
    {
        // Amount involved in the transaction (e.g., 100, "50.5")
        public string trans_amount { get; set; }

        // Date when the transaction took place (e.g., "2025-01-16")
        public string trans_date { get; set; }

        // Type of transaction (e.g., "Income", "Expense")
        public string trans_type { get; set; }

        // Additional notes related to the transaction (e.g., "Payment for service")
        public string trans_note { get; set; }

        // Tag for categorizing the transaction (e.g., "Rent", "Salary")
        public string trans_tag { get; set; }
    }
}
