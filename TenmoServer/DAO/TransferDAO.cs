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

        public void SendFunds(Transfer transfer)
        {
            int fromAccountId = GetAccountId(transfer.UsersFromId);
            int toAccountId = GetAccountId(transfer.UsersToId);

            const string sql = "INSERT INTO transfers (transfer_type_id, transfer_status_id, account_from, account_to, amount) " +
            "VALUES(1001, 2001, @accountFrom, @accountTo, @amount)";             // 1001 = send, 2001 = approved (these will always be the ids when sending money)

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand command = new SqlCommand(sql, conn);

                command.Parameters.AddWithValue("@accountFrom", fromAccountId);
                command.Parameters.AddWithValue("@accountTo", toAccountId);
                command.Parameters.AddWithValue("@amount", transfer.TransferAmount);

                command.ExecuteNonQuery();

            }


        }



    }
}
