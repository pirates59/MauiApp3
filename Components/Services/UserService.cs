using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using MauiApp3.Models;


namespace MauiApp3.Services
{
    public class UserService
    {
        private static User? loggedinUser = null;

        private static readonly string UserFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DataStore", "Moneymate.json");


        public List<User> LoadUserAccounts()
        {
            if (!File.Exists(UserFilePath))
            {
                return new List<User>();  // Return an empty list if no users exist

            }
            var jsonContent = File.ReadAllText(UserFilePath);
            return JsonSerializer.Deserialize<List<User>>(jsonContent) ?? new List<User>();
        }

        public void SaveUserAccounts(List<User> userAccounts)
        {
            var directoryPath = Path.GetDirectoryName(UserFilePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath!);
            }

            if (!File.Exists(UserFilePath))
            {
                File.WriteAllText(UserFilePath, "[]"); // Write an empty JSON array
            }
            var jsonContent = JsonSerializer.Serialize(userAccounts, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(UserFilePath, jsonContent);
        }

        public string GeneratePasswordHash(string password)
        {
            using var sha256 = SHA256.Create();
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var passwordHash = sha256.ComputeHash(passwordBytes);
            return Convert.ToBase64String(passwordHash);  // Return hashed password
        }

        public bool VerifyPassword(string inputPassword, string storedPasswordHash)
        {
            var hashedInputPassword = GeneratePasswordHash(inputPassword);
            return hashedInputPassword == storedPasswordHash;
        }

        public bool LoginService(string username, string password)
        {
            var users = LoadUserAccounts();
            var user = users.FirstOrDefault(u => u.Username == username);

            if (user != null && VerifyPassword(password, user.Password))
            {

                loggedinUser = user;
                return true;
            }
            return false;
        }

        public User? GetLoggedInUser()
        {
            return loggedinUser;
        }

        public bool addTransaction(Transaction transaction)
        {
            loggedinUser.Trans.Add(transaction);

            var user = LoadUserAccounts();
            var userId = user.FindIndex(u => u.Username == loggedinUser.Username);

            if (userId != -1)
            {
                user[userId] = loggedinUser;
                SaveUserAccounts(user);
                return true;

            }
            return false;

        }

        public bool AddDebtTransaction(int debt_id, string receivedDate, string dueDate, string amount, string debtTag, string debtNote, string sourceName)
        {
            if (loggedinUser == null)
            {
                return false;
            }

            var existingDebt = loggedinUser.debts.FirstOrDefault(d => d.debt_id == debt_id);
            if (existingDebt != null)
            {
                existingDebt.given_Date = receivedDate;
                existingDebt.due_Date = dueDate;
                existingDebt.debt_amount = amount;
                existingDebt.source_name = sourceName;
                existingDebt.debt_tag = debtTag;
                existingDebt.debt_note = debtNote;
            }
            else
            {
                var newDebtTransaction = new Debt
                {
                    debt_id = debt_id,
                    given_Date = receivedDate,
                    due_Date = dueDate,
                    debt_amount = amount,
                    debt_Status = "Pending",
                    source_name = sourceName,
                    debt_tag = debtTag,
                    debt_note = debtNote,
                };

                loggedinUser.debts.Add(newDebtTransaction);
            }

            var users = LoadUserAccounts();
            var userIndex = users.FindIndex(u => u.Username == loggedinUser.Username);

            if (userIndex != -1)
            {
                users[userIndex] = loggedinUser;
                SaveUserAccounts(users);
                return true;
            }

            return false;
        }
        public bool SaveUpdatedTransactions(List<Transaction> transactions)
        {
            if (loggedinUser == null) return false;

            var users = LoadUserAccounts();
            var userIndex = users.FindIndex(u => u.Username == loggedinUser.Username);

            if (userIndex != -1)
            {
                users[userIndex].Trans = transactions;

                SaveUserAccounts(users);
                return true;
            }
            return false;
        }


        public bool UpdateDebtDetails(Debt updatedDebt)
        {
            if (loggedinUser == null) return false;

            var users = LoadUserAccounts();
            var userIndex = users.FindIndex(u => u.Username == loggedinUser.Username);

            if (userIndex != -1)
            {
                var debtToUpdate = users[userIndex].debts.FirstOrDefault(d => d.debt_id == updatedDebt.debt_id);

                if (debtToUpdate != null)
                {
                    debtToUpdate.debt_Status = updatedDebt.debt_Status;
                    debtToUpdate.cleared_amount = updatedDebt.cleared_amount;
                    debtToUpdate.debt_amount = updatedDebt.debt_amount;

                    SaveUserAccounts(users);
                    return true;
                }
            }
            return false;
        }





    }


}
