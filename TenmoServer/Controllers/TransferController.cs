﻿using Microsoft.AspNetCore.Authorization;
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

        public TransferController(ITransferDAO transferDAO)
        {
            this.transferDAO = transferDAO;
        }

       
        [HttpGet]
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
        



    }
}
