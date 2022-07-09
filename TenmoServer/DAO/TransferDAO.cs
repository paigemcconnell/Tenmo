using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class TransferDAO : ITransferDAO
    {
        private readonly string connectionString;

        public TransferDAO(string connStr)
        {
            this.connectionString = connStr;
        }


        public AccountBalance DisplayBalance(string username)
        {
            const string sql = "SELECT a.balance FROM accounts a INNER JOIN users u ON u.user_id = a.user_id WHERE username = @username";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand command = new SqlCommand(sql, conn);

                command.Parameters.AddWithValue("@username", username);
                decimal balance = Convert.ToDecimal(command.ExecuteScalar());  // pulling balance from sql
                AccountBalance currentBalance = new AccountBalance(); // new instance of account balance
                currentBalance.Balance = balance; // setting new instances balance to the value from sql

                return currentBalance;
            }
        }
        public List<ReturnUser> DisplayUsers(string username)
        {
            const string sql = "SELECT username, user_id FROM users ";

            List<ReturnUser> userList = new List<ReturnUser>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand command = new SqlCommand(sql, conn);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string returnUsername = Convert.ToString(reader["username"]);
                        int returnUserId = Convert.ToInt32(reader["user_id"]);
                        ReturnUser user = new ReturnUser();
                        user.Username = returnUsername;
                        user.UserId = returnUserId;
                        userList.Add(user);
                    }
                }
                return userList;
            }
        }

        public int GetAccountId(int userId)
        {
            const string sql = "SELECT a.account_id FROM accounts a INNER JOIN users u ON u.user_id = a.user_id WHERE u.user_id = @userId";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand command = new SqlCommand(sql, conn);

                command.Parameters.AddWithValue("@userId", userId);
                int accountId = Convert.ToInt32(command.ExecuteScalar());  // pulling account # from sql

                return accountId;
            }
        }

        public int SendFunds(Transfer transfer)
        {
            int fromAccountId = GetAccountId(transfer.UsersFromId);
            int toAccountId = GetAccountId(transfer.UsersToId);

            const string sql = "INSERT INTO transfers (transfer_type_id, transfer_status_id, account_from, account_to, amount) " +
            "VALUES(1001, 2001, @accountFrom, @accountTo, @amount); SELECT @@IDENTITY";             // 1001 = send, 2001 = approved (these will always be the ids when sending money)

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand command = new SqlCommand(sql, conn);

                command.Parameters.AddWithValue("@accountFrom", fromAccountId);
                command.Parameters.AddWithValue("@accountTo", toAccountId);
                command.Parameters.AddWithValue("@amount", transfer.TransferAmount);

                int transferId = Convert.ToInt32(command.ExecuteScalar());

                return transferId;

            }

        }

        public decimal CreditAccount(int transferId, int userToId)
        {
            int toAccountId = GetAccountId(userToId);

            const string sql = "UPDATE accounts SET balance = (select accounts.balance + transfers.amount as [CurrentBalance] " +
                "from accounts inner join transfers on accounts.account_id = transfers.account_to where transfer_id = @transferId) " +
                "WHERE account_id = @accountTo";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand command = new SqlCommand(sql, conn);

                command.Parameters.AddWithValue("@transferId", transferId);
                command.Parameters.AddWithValue("@accountTo", toAccountId);

                decimal newBalance = Convert.ToDecimal(command.ExecuteScalar());

                return newBalance;
            }
        }

        public decimal DebitAccount(int transferId, int userFromId)
        {
            int fromAccountId = GetAccountId(userFromId);

            const string sql = "UPDATE accounts SET balance = (select accounts.balance - transfers.amount as [CurrentBalance] " +
                "from accounts inner join transfers on accounts.account_id = transfers.account_from where transfer_id = @transferId) " +
                "WHERE account_id = @accountFrom";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand command = new SqlCommand(sql, conn);

                command.Parameters.AddWithValue("@transferId", transferId);
                command.Parameters.AddWithValue("@accountFrom", fromAccountId);

                decimal newBalance = Convert.ToDecimal(command.ExecuteScalar());

                return newBalance;
            }
        }

        
        public List<Transfer> DisplayTransfers(string username)
        {
            const string sql = "SELECT t.transfer_id, t.account_from, uFrom.username AS 'From', t.account_to, uTo.username AS 'To', t.amount " +
                "FROM transfers t INNER JOIN accounts aTo ON aTo.account_id = t.account_to INNER JOIN users uTo ON uTo.user_id = aTo.user_id " +
                "INNER JOIN accounts aFrom ON aFrom.account_id = t.account_from INNER JOIN users uFrom ON uFrom.user_id = aFrom.user_id " +
                "WHERE uFrom.username = @username OR uTo.username = @username";

            List<Transfer> transferList = new List<Transfer>();

            using (SqlConnection conn = new SqlConnection(connectionString))

            {
                conn.Open();

                SqlCommand command = new SqlCommand(sql, conn);

                command.Parameters.AddWithValue("@username", username);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int transferId = Convert.ToInt32(reader["transfer_id"]);
                        string userFrom = Convert.ToString(reader["From"]);
                        string userTo = Convert.ToString(reader["To"]);
                        int amount = Convert.ToInt32(reader["amount"]);
                        Transfer transfer = new Transfer();
                        transfer.TransferId = transferId;
                        transfer.UserFrom = userFrom;
                        transfer.UserTo = userTo;
                        transfer.Amount = amount;
                        transferList.Add(transfer);
                        
                    }
                }

                return transferList;
            }
        }

        public Transfer GetTransferDetails (int transferId)
        {
            const string sql = "SELECT t.transfer_id, t.account_from, uFrom.username AS 'From', t.account_to, uTo.username AS 'To', t.amount " +
                "FROM transfers t INNER JOIN accounts aTo ON aTo.account_id = t.account_to INNER JOIN users uTo ON uTo.user_id = aTo.user_id " +
                "INNER JOIN accounts aFrom ON aFrom.account_id = t.account_from INNER JOIN users uFrom ON uFrom.user_id = aFrom.user_id " +
                "WHERE t.transfer_id = @transferId";

            Transfer transfer = new Transfer();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand command = new SqlCommand(sql, conn);

                
                command.Parameters.AddWithValue("@transferId", transferId);
                
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        transferId = Convert.ToInt32(reader["transfer_id"]);
                        string userFrom = Convert.ToString(reader["from"]);
                        string userTo = Convert.ToString(reader["to"]);
                        int amount = Convert.ToInt32(reader["amount"]);
                        transfer.TransferId = transferId;
                        transfer.UserFrom = userFrom;
                        transfer.UserTo = userTo;
                        transfer.Amount = amount;
                    }
                }
                
            return transfer;
               
            }
        }





    }
}
