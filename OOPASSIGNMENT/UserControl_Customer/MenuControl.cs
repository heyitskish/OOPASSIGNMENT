using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOPASSIGNMENT.UserControl_Customer
{
    public partial class MenuControl : UserControl
    {
        public FlowLayoutPanel OrderPanel { get; set; }  // Reference to flpOrders in MainForm
        private decimal previousPanelQtyValue = 0;

        public MenuControl()
        {
            InitializeComponent();
            InitializeMenuButtons();
        }

        private void InitializeMenuButtons()
        {
            foreach (Control ctrl in flpMenu.Controls) //Control every panels inside the flow layout panel menu
            {
                if (ctrl is Panel panel)
                {
                    foreach (Control Subctrl in panel.Controls)
                    {
                        if (Subctrl is Button btn)
                        {
                            btn.Tag = false;  //Set the defualt state to false
                            btn.Click += btnSelect_Click;
                        }
                    }
                }
            }

            foreach (Control ctrl in flpOrders.Controls) //Control every panels inside the flow layout panel orders
            {
                if (ctrl is Panel panel)
                {
                    foreach (Control Subctrl in panel.Controls)
                    {
                        if (Subctrl is Button btn)
                        {
                            flpOrders.Controls.Remove(ctrl);
                        }
                    }
                }
            }
        }

        // Create panels for each ordered item inside the order panel using AddPanelOrder method
        private void btnSelect_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            Panel parentPanel = clickedButton.Parent as Panel; // Get the parent panel of the button

            if (clickedButton != null)
            {
                bool isSelected = (bool)clickedButton.Tag;  //Unchecked the button state
                if (!isSelected)
                {
                    clickedButton.BackColor = Color.LawnGreen;
                    if (parentPanel != null)
                    {
                        AddPanelOrder(parentPanel);
                    }
                }

                clickedButton.Tag = !isSelected; // Toggle state
            }

        }

        // Method to create the ordered panels
        private void AddPanelOrder(Panel itemPanel)
        {
            List<string> labelTexts = new List<string>();
            Button sourceButton = null;  // Store the button Reference

            foreach (Control ctrl in itemPanel.Controls) // Loop through all labels inside the Menu Item panel
            {
                if (ctrl is Label lbl)
                {
                    labelTexts.Add(lbl.Text);
                }
                else if (ctrl is Button btn)
                {
                    sourceButton = btn;
                }
            }

            Match pattern = Regex.Match(labelTexts[0], @"\$(\d+)"); // Grep the itemPrice from the label
            decimal labelPrice = Convert.ToDecimal(pattern.Groups[1].Value); // Convert the price to decimal

            //Create a Panel for Order items
            Panel panel = new Panel
            {
                Size = new Size(359, 51),
                BackColor = SystemColors.Control,
                BorderStyle = BorderStyle.FixedSingle,
                Tag = Tuple.Create(sourceButton, labelPrice) // Store the unit price in the Tag property
            };

            // Create label for Menu Item Name
            Label orderLabel = new Label
            {
                Text = labelTexts[1],
                Location = new Point(40, 16),
                AutoSize = true
            };

            // Create label for price
            Label priceLabel = new Label
            {
                Text = $"{labelPrice:C}",
                Location = new Point(280, 16),
                AutoSize = true
            };

            // Create numbericUpDown for quantity
            NumericUpDown qty_box = new NumericUpDown
            {
                Size = new Size(15, 18),
                Location = new Point(235, 13),
                AutoSize = true,
                Value = 1
            };

            // Create Remove Button
            Button rm_btn = new Button
            {
                Size = new Size(20, 20),
                Location = new Point(10, 14),
                AutoSize = true,
                Image = Properties.Resources.bin
            };

            // Disable the button light if user delete the item from the order panel
            rm_btn.Click += (s, e) =>
            {
                // Find the parent panel of the button
                Panel parentPanel = rm_btn.Parent as Panel;

                if (parentPanel != null)
                {
                    // Remove the panel from flpOrders
                    flpOrders.Controls.Remove(parentPanel);

                    if (parentPanel.Tag is Tuple<Button, decimal> tag)
                    {
                        tag.Item1.BackColor = SystemColors.Control; // Reset color
                        tag.Item1.Tag = false; // Reset selection state
                    }

                    // Update the total price
                    UpdateTotalPrice();
                }
            };

            // Change the item price based on the quantity
            qty_box.ValueChanged += (s, e) =>
            {
                NumericUpDown qty = s as NumericUpDown;
                Panel qtyParentPanel = qty.Parent as Panel; // Call the parent panel

                if (qtyParentPanel != null)
                {
                    Label priceLable = qtyParentPanel.Controls.OfType<Label>().FirstOrDefault(lbl => lbl.Location == new Point(280, 16));  // locate the item price based on the position
                    Match match = Regex.Match(priceLable.ToString(), @"\$(\d+)"); // Grep the price number using Regex

                    if (qtyParentPanel.Tag is Tuple<Button, decimal> tag) // Calculate the item price based on the quantity
                    {
                        decimal unitPrice = tag.Item2;
                        CustomerMenu itemPrice = new CustomerMenu(unitPrice);

                        if (qty.Value > previousPanelQtyValue)  // if the quantity is increased
                        {
                            decimal totalPrice = itemPrice.calcItemPrice(unitPrice, Convert.ToInt32(qty.Value)); // Calculate the total price based on the quantity
                            priceLabel.Text = $"{totalPrice:C}";
                        }
                        else if (qty.Value < previousPanelQtyValue) // if the quantity is decreased
                        {
                            decimal currentPrice = Convert.ToDecimal(match.Groups[1].Value); // Assign the current price of the item
                            decimal totalPrice = itemPrice.calcItemPrice(unitPrice, Convert.ToInt32(qty.Value));
                            priceLabel.Text = $"{totalPrice:C}";
                        }
                        qty.Tag = qty.Value;

                        // Update the total price
                        UpdateTotalPrice();
                    }
                }
            };

            panel.Controls.Add(rm_btn);
            panel.Controls.Add(qty_box);
            panel.Controls.Add(orderLabel);
            panel.Controls.Add(priceLabel);
            flpOrders.Controls.Add(panel);

            // Update the total price
            UpdateTotalPrice();
        }

        private void UpdateTotalPrice()
        {
            decimal totalPrice = 0;

            foreach (Control ctrl in flpOrders.Controls)
            {
                if (ctrl is Panel panel)
                {
                    Label priceLabel = panel.Controls.OfType<Label>().FirstOrDefault(lbl => lbl.Location == new Point(280, 16));
                    if (priceLabel != null)
                    {
                        Match match = Regex.Match(priceLabel.Text, @"\$(\d+(\.\d{1,2})?)");
                        if(match.Success)
                        {
                            totalPrice += Convert.ToDecimal(match.Groups[1].Value);
                        }
                    }
                }
            }
            lblTotalPriceDisplay.Text = $"{totalPrice:C}";
        }
    }
}
