using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using OOPASSIGNMENT.UserControl_Customer;

namespace OOPASSIGNMENT
{
    public partial class CustomerHome : Form
    {
        private MenuControl menuControl;
        public CustomerHome()
        {
            InitializeComponent();

            menuControl = new MenuControl();
            {
                Dock = DockStyle.Fill;
            }
        }

        // Side Bar highlight 
        private void btnMenu_Click(object sender, EventArgs e)
        {
            panelSidebar.Height = btnMenu.Height;
            panelSidebar.Top = btnMenu.Top;
        }

        private void btnViewMyOrders_Click(object sender, EventArgs e)
        {
            panelSidebar.Height = btnViewMyOrders.Height;
            panelSidebar.Top = btnViewMyOrders.Top;
        }

        private void btnReservation_Click(object sender, EventArgs e)
        {
            panelSidebar.Height = btnReservation.Height;
            panelSidebar.Top = btnReservation.Top;
        }

        private void btnFeedback_Click(object sender, EventArgs e)
        {
            panelSidebar.Height = btnFeedback.Height;
            panelSidebar.Top = btnFeedback.Top;
        }
    }
}
