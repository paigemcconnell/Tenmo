using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Security;
using TenmoServer.DAO;
using TenmoServer.Models;
using System.Security.Claims;

namespace TenmoServer.Controllers
{
    [Route("/")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        private readonly ITransferDAO transferDAO;

        public TransferController(ITransferDAO transferDAO)
        {
            this.transferDAO = transferDAO;
        }


        [HttpGet("transfer/balance")]
        [Authorize]
        public ActionResult DisplayBalance()
        {
            string username = User.Identity.Name; // username of the currently logged in user

            AccountBalance balance = transferDAO.DisplayBalance(username);  // balance for ONLY the currently logged in user

            if (balance == null)
            {
                return NotFound("Could not find balance");
            }

            return Ok(balance);
        }

        [HttpGet("transfer/users")]
        [Authorize]
        public ActionResult DisplayAllUsers()
        {
            string username = User.Identity.Name;

            List<ReturnUser> users = transferDAO.DisplayUsers(username);

            if (users == null)
            {
                return NotFound("Could not find users");
            }

            return Ok(users);
        }

        [HttpPost("transfer/sendfunds")]
        [Authorize]
        public ActionResult SendFunds(Transfer transfer) 
        {
            transfer.UsersFromId = LoggedInUserId;
            AccountBalance balance = transferDAO.DisplayBalance(User.Identity.Name);

            if (balance.Balance - transfer.TransferAmount >= 0)
            {
                int transferId = transferDAO.SendFunds(transfer);
                //return Ok(transferId);
                decimal currentToBalance = transferDAO.CreditAccount(transferId, transfer.UsersToId);
                return Ok(currentToBalance);
            }

            else
            {
                return BadRequest();
            }

        }

        private int LoggedInUserId
        {
            get
            {
                Claim idClaim = User.FindFirst("sub");
                if (idClaim == null)
                {
                    // User is not logged in
                    return -1;
                }
                else
                {
                    // User is logged in. Their subject (sub) claim is their ID
                    return int.Parse(idClaim.Value);
                }
            }
        }

       
    }
}
