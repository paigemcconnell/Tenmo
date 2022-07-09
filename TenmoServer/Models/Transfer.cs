using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Models
{
    public class Transfer //Models
    {
        public int UsersFromId { get; set; } 

        public int UsersToId { get; set; }

        public decimal TransferAmount { get; set; }

        public int TransferId { get; set; }

        public string UserFrom { get; set; }

        public string UserTo { get; set; }

        public decimal Amount { get; set; }
    }
}
