using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using MauiApp3.Models;


namespace MauiApp3.Services
{
    public class UserService
    {
        private static User? loggedinUser = null;

        private static readonly string UserFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DataStore", "Moneymate.json");


        public List<User> UserAccounts()
        {
            if (!File.Exists(UserFile))
            {
                return new List<User>();

            }
            var jsonFile = File.ReadAllText(UserFile);
            return JsonSerializer.Deserialize<List<User>>(jsonFile) ?? new List<User>();
        }

        public void userAccount(List<User> user_accounts)
        {
            var directoryPath = Path.GetDirectoryName(UserFile);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath!);
            }

            if (!File.Exists(UserFile))
            {
                File.WriteAllText(UserFile, "[]");
            }
            var jsonContent = JsonSerializer.Serialize(user_accounts, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(UserFile, jsonContent);
        }

        public string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var passBytes = Encoding.UTF8.GetBytes(password);
            var Hashpassword = sha256.ComputeHash(passBytes);
            return Convert.ToBase64String(Hashpassword);  
        }

        public bool PasswordVerification(string input_password, string storedPassword)
        {
            var hashedInputPassword = HashPassword(input_password);
            return hashedInputPassword == storedPassword;
        }

        public bool LoginService(string username, string password)
        {
            var Users = UserAccounts();
            var user = Users.FirstOrDefault(u => u.user_name == username);

            if (user != null && PasswordVerification(password, user.pass))
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
      

            var user = UserAccounts();
            var userID = user.FindIndex(u => u.user_name == loggedinUser.user_name);

            if (userID != -1)
            {
                user[userID].Trans.Add(transaction);
                userAccount(user);
                loggedinUser = user[userID];
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

            var existedDebt = loggedinUser.Debts.FirstOrDefault(d => d.debt_id == debt_id);
            if (existedDebt != null)
            {
                existedDebt.Date = receivedDate;
                existedDebt.due_date = dueDate;
                existedDebt.debt_amount = amount;
                existedDebt.source = sourceName;
                existedDebt.debt_tag = debtTag;
                existedDebt.debt_note = debtNote;
            }
            else
            {
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

                loggedinUser.Debts.Add(newTransaction);
            }

            var Users = UserAccounts();
            var Index = Users.FindIndex(u => u.user_name == loggedinUser.user_name);

            if (Index != -1)
            {
                Users[Index] = loggedinUser;
                userAccount(Users);
                return true;
            }

            return false;
        }
        public bool UpdatedTransactions(List<Transaction> trans)
        {
            if (loggedinUser == null) return false;

            var Users = UserAccounts();
            var Index = Users.FindIndex(u => u.user_name == loggedinUser.user_name);

            if (Index != -1)
            {
                Users[Index].Trans = trans;

                userAccount(Users);
                return true;
            }
            return false;
        }


        public bool DebtDetails(Debt debtUpdated)
        {
            if (loggedinUser == null) return false;

            var Users = UserAccounts();
            var Index = Users.FindIndex(u => u.user_name == loggedinUser.user_name);

            if (Index != -1)
            {
                var debtUpdate = Users[Index].Debts.FirstOrDefault(d => d.debt_id == debtUpdated.debt_id);

                if (debtUpdate != null)
                {
                    debtUpdate.debt_status = debtUpdated.debt_status;
                    debtUpdate.cleared_amount = debtUpdated.cleared_amount;
                    debtUpdate.debt_amount = debtUpdated.debt_amount;

                    userAccount(Users);
                    loggedinUser = Users[Index];
                    return true;
                }
            }
            return false;
        }


        public List<Transaction> UserTransactions()
        {
            var Users = UserAccounts();
            var user = Users.FirstOrDefault(u => u.user_name == loggedinUser?.user_name);


            if (user != null)
            {
                return user.Trans ?? new List<Transaction>();
            }

            return new List<Transaction>();
        }

    }
}

