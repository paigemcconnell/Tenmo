using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Data
{
    public class Transfer
    {
        public int AccountFromId { get; set; }

        public int AccountToId { get; set; }

        public decimal TransferAmount { get; set; }

    }
}
