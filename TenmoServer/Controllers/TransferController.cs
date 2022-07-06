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

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        private readonly ITransferDAO transferDAO;
        public TransferController()
        {

        }

       
        [HttpGet]
        [Authorize]
        public ActionResult DisplayBalance(string username)
        {
            AccountBalance balance = transferDAO.DisplayBalance(username);

            return Ok(balance);
        }
        



    }
}
