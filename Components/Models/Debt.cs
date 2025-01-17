using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp3.Models
{
    // Class representing a Debt record with associated details
    public class Debt
    {
        // Unique identifier for each debt record
        public int debt_id { get; set; }

        // Date when the debt was recorded
        public string Date { get; set; }

        // The due date for the debt repayment
        public string due_date { get; set; }

        // Status of the debt (e.g., "Pending", "Cleared", etc.)
        public string debt_status { get; set; }

        // Total amount of the debt
        public string debt_amount { get; set; }

        // Amount that has already been cleared (default is 0)
        public string cleared_amount { get; set; } = "0";

        // Tag for categorizing the debt (e.g., "Loan", "Credit")
        public string debt_tag { get; set; }

        // The source from which the debt originated (e.g., "Bank", "Individual")
        public string source { get; set; }

        // Additional notes regarding the debt 
        public string debt_note { get; set; }
    }
}
