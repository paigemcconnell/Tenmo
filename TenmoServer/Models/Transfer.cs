using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Models
{
    public class Transfer
    {
        public int AccountFromId { get; set; }

        public int AccountToId { get; set; }

        public decimal TransferAmount { get; set; }

    }
}
