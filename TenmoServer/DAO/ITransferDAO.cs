using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
   public interface ITransferDAO
    {
        AccountBalance DisplayBalance(string username);

        List<ReturnUser> DisplayUsers(string username);

        int GetAccountId(int userId);

        int SendFunds(Transfer transfer);

        decimal CreditAccount(int transferId, int userToId);
    }
    
}
