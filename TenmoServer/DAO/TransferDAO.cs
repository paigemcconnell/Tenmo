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

    }
}
