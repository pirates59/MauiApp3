using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using MauiApp3.Models;

namespace MauiApp3.Services
{
    public class UserService
    {
        // Stores the currently logged-in user
        private static User? loggedinUser = null;

        // Path to the JSON file that stores user data
        private static readonly string UserFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DataStore", "Moneymate.json");

        // Retrieves all user accounts from the JSON file
        public List<User> UserAccounts()
        {
            if (!File.Exists(UserFile))
            {
                return new List<User>(); // Return an empty list if the file doesn't exist
            }
            var jsonFile = File.ReadAllText(UserFile); // Read file content
            return JsonSerializer.Deserialize<List<User>>(jsonFile) ?? new List<User>(); // Deserialize or return an empty list
        }

        // Saves the provided user accounts list to the JSON file
        public void userAccount(List<User> user_accounts)
        {
            var directoryPath = Path.GetDirectoryName(UserFile);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath!); // Create the directory if it doesn't exist
            }

            if (!File.Exists(UserFile))
            {
                File.WriteAllText(UserFile, "[]"); // Initialize the file with an empty array if it doesn't exist
            }
            var jsonContent = JsonSerializer.Serialize(user_accounts, new JsonSerializerOptions { WriteIndented = true }); // Serialize the user accounts
            File.WriteAllText(UserFile, jsonContent); // Save serialized data to the file
        }

        // Hashes the given password using SHA256
        public string HashPassword(string password)
        {
            using var sha256 = SHA256.Create(); // Create an instance of SHA256
            var passBytes = Encoding.UTF8.GetBytes(password); // Convert password to bytes
            var Hashpassword = sha256.ComputeHash(passBytes); // Hash the password
            return Convert.ToBase64String(Hashpassword); // Return the hashed password as a Base64 string
        }

        // Verifies if the input password matches the stored hashed password
        public bool PasswordVerification(string input_password, string storedPassword)
        {
            var InputPassword = HashPassword(input_password); // Hash the input password
            return InputPassword == storedPassword; // Compare hashed input with the stored password
        }

        // Handles user login by verifying credentials
        public bool LoginService(string username, string password)
        {
            var Users = UserAccounts(); // Retrieve all users
            var user = Users.FirstOrDefault(u => u.user_name == username); // Find user by username

            if (user != null && PasswordVerification(password, user.pass))
            {
                loggedinUser = user; // Set the logged-in user
                return true; // Return true for successful login
            }
            return false; // Return false for unsuccessful login
        }

        // Returns the currently logged-in user
        public User? GetLoggedInUser()
        {
            return loggedinUser;
        }

        // Adds a transaction to the logged-in user's account
        public bool addTransaction(Transaction transaction)
        {
            var user = UserAccounts(); // Retrieve all users
            var userID = user.FindIndex(u => u.user_name == loggedinUser.user_name); // Find index of the logged-in user

            if (userID != -1)
            {
                user[userID].Trans.Add(transaction); // Add the transaction to the user's list
                userAccount(user); // Save the updated user list
                loggedinUser = user[userID]; // Update the logged-in user's details
                return true; // Return true for successful addition
            }
            return false; // Return false if user not found
        }

        // Adds or updates a debt transaction for the logged-in user
        public bool AddDebtTransaction(int debt_id, string receivedDate, string dueDate, string amount, string debtTag, string debtNote, string sourceName)
        {
            if (loggedinUser == null)
            {
                return false; // Return false if no user is logged in
            }

            var existed_debt = loggedinUser.Debts.FirstOrDefault(d => d.debt_id == debt_id); // Check if the debt already exists
            if (existed_debt != null)
            {
                // Update existing debt details
                existed_debt.Date = receivedDate;
                existed_debt.due_date = dueDate;
                existed_debt.debt_amount = amount;
                existed_debt.source = sourceName;
                existed_debt.debt_tag = debtTag;
                existed_debt.debt_note = debtNote;
            }
            else
            {
                // Add a new debt transaction
                var newTransaction = new Debt
                {
                    debt_id = debt_id,
                    Date = receivedDate,
                    due_date = dueDate,
                    debt_amount = amount,
                    debt_status = "Pending",
                    source = sourceName,
                    debt_tag = debtTag,
                    debt_note = debtNote,
                };

                loggedinUser.Debts.Add(newTransaction); // Add the new debt to the user's list
            }

            var Users = UserAccounts(); // Retrieve all users
            var Index = Users.FindIndex(u => u.user_name == loggedinUser.user_name); // Find index of the logged-in user

            if (Index != -1)
            {
                Users[Index] = loggedinUser; // Update the user's details in the list
                userAccount(Users); // Save the updated user list
                return true; // Return true for successful addition/update
            }

            return false; // Return false if user not found
        }
        public bool UpdatedTransactions(List<Transaction> trans)
        {
            if (loggedinUser == null) return false; // Return false if no user is logged in

            var Users = UserAccounts(); // Retrieve all user accounts
            var Index = Users.FindIndex(u => u.user_name == loggedinUser.user_name); // Find index of the logged-in user

            if (Index != -1)
            {
                Users[Index].Trans = trans; // Update the transactions for the user

                userAccount(Users); // Save the updated user accounts to the file
                return true; // Return true for successful update
            }
            return false; // Return false if user not found
        }

        public bool DebtDetails(Debt debtUpdated)
        {
            if (loggedinUser == null) return false; // Return false if no user is logged in

            var Users = UserAccounts(); // Retrieve all user accounts
            var Index = Users.FindIndex(u => u.user_name == loggedinUser.user_name); // Find index of the logged-in user

            if (Index != -1)
            {
                var debtUpdate = Users[Index].Debts.FirstOrDefault(d => d.debt_id == debtUpdated.debt_id); // Find the debt by ID

                if (debtUpdate != null)
                {
                    // Update debt details
                    debtUpdate.debt_status = debtUpdated.debt_status;
                    debtUpdate.cleared_amount = debtUpdated.cleared_amount;
                    debtUpdate.debt_amount = debtUpdated.debt_amount;

                    userAccount(Users); // Save the updated user accounts to the file
                    loggedinUser = Users[Index]; // Update the logged-in user details
                    return true; // Return true for successful update
                }
            }
            return false; // Return false if user or debt not found
        }

        public List<Transaction> UserTransactions()
        {
            var Users = UserAccounts(); // Retrieve all user accounts
            var user = Users.FirstOrDefault(u => u.user_name == loggedinUser?.user_name); // Find the logged-in user

            if (user != null)
            {
                return user.Trans ?? new List<Transaction>(); // Return the user's transactions or an empty list if null
            }

            return new List<Transaction>(); // Return an empty list if user not found
        }


    }
}

