using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Data
{
    public class Transfer //Data
    {
        public int UserFromId { get; set; }

        public int UsersToId { get; set; }

        public decimal TransferAmount { get; set; }

    }
}
