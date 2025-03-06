using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOPASSIGNMENT
{
    internal class CustomerMenu
    {
        private decimal itemPrice = 0;
        public CustomerMenu(decimal itemPrice)
        {
            this.itemPrice = itemPrice; 
        }

        public decimal calcItemPrice(decimal itemPrice, int qty)
        {
            return itemPrice * qty;
        }
    }
}
