using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComputerFitting
{
    public partial class Fitting : Form
    {
        public List<ComputerPart> data = new List<ComputerPart>();
        public Fitting()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Motherboard a = new Motherboard();
            a.fit = this;
            a.Show();
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            Etcetera a = new Etcetera();
            a.fit = this;
            a.Show();
        }
        public void RefreshTable()
        {
            listView1.Items.Clear();
            for (int i = 0; i < data.Count; i++)
            {
                ListViewItem temp = listView1.Items.Add(i.ToString());
                temp.SubItems.Add(((ComputerPart)data[i]).name);
                temp.SubItems.Add(((ComputerPart)data[i]).price);
                temp.SubItems.Add(((ComputerPart)data[i]).note);
                foreach (String a in ((ComputerPart)data[i]).notCompatible)
                {
                    temp.SubItems.Add(a + ";");
                }


            }

            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].GetType().ToString().Equals("ComputerFitting.Board"))
                {
                    button1.Enabled = false;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    button5.Enabled = true;
                    button6.Enabled = true;
                }
            }
            checkBox1.Checked = true;
            checkBox2.Checked = true;
            checkBox3.Checked = true;
            checkBox4.Checked = true;
            checkBox5.Checked = true;

            //5 VALIDATIONS GO HERE
            //1: All Compatible
            int price = 0;
            for (int k = 0; k < data.Count; k++)
            {
                price += int.Parse(data[k].price);
                for (int j = 0; j < data.Count; j++)
                {
                    foreach (String l in ((ComputerPart)data[j]).notCompatible)
                    {
                        if (((ComputerPart)data[k]).name.Contains(l))
                        {
                            MessageBox.Show(((ComputerPart)data[k]).name + " is not compatible with " + ((ComputerPart)data[j]).name);
                            checkBox1.Checked = false;
                        }
                    }
                }
            }
            label7.Text = "Est. Price: " + price;
            //2:Sockets
            Board mthr = new Board();
            label1.Text = "Processor Freq: ";
            for (int k = 0; k < data.Count; k++)
            {
                if (data[k].GetType().ToString().Equals("ComputerFitting.Board"))
                {
                    mthr = (Board)data[k];
                    label8.Text = "Socket: " + mthr.socket;
                }
            }
            for (int k = 0; k < data.Count; k++)
            {
                if (data[k].GetType().ToString().Equals("ComputerFitting.Proc"))
                {
                    if (!((Proc)data[k]).socket.Equals(mthr.socket))
                    {
                        label1.Text += ((Proc)data[k]).freq + " ";
                        MessageBox.Show(mthr.name + " is not compatible with " + ((ComputerPart)data[k]).name);
                        //MessageBox.Show(((Proc)data[k]).socket + " " + mthr.socket);
                        checkBox1.Checked = false;
                    }
                }
            }
            //3:Slots
            toolStripStatusLabel1.Text = "Motherboard: " + mthr.name;
            int proc = 0, pci = 0, memory=0, totalRAM = 0, totalVRAM = 0;
            for (int k = 0; k < data.Count; k++)
            {
                if (data[k].GetType().ToString().Equals("ComputerFitting.Proc"))
                {
                    proc++;
                }
                if (data[k].GetType().ToString().Equals("ComputerFitting.Graphics"))
                {
                    pci++;
                    int ou = 0;
                    int.TryParse(((Graphics)data[k]).memorySize, out ou);
                    totalVRAM += ou;
                }
                if (data[k].GetType().ToString().Equals("ComputerFitting.RAM"))
                {
                    int ou = 0;
                    int.TryParse(((RAM)data[k]).size, out ou);
                    totalRAM += ou;
                    memory++;
                }
            }
            label3.Text = "Total RAM: " + totalRAM.ToString();
            label5.Text = "Total VRAM: " + totalVRAM.ToString();
            toolStripStatusLabel2.Text = "PCI-E Slots: " + pci.ToString() + "\\" +mthr.maxPci;
            toolStripStatusLabel3.Text = "Memory Slots: " + memory.ToString() + "\\" +mthr.maxMemory;
            toolStripStatusLabel4.Text = "Processors: " + proc.ToString() + "\\" + mthr.maxSocket;

            if (data.Count > 0)
            {
                try
                {
                    if (!(int.Parse(mthr.maxPci) >= pci) || !(int.Parse(mthr.maxMemory) >= memory) || !(int.Parse(mthr.maxSocket) >= proc))
                    {
                        checkBox3.Checked = false;
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Motherboard slots invalid format. Please fix it in DB view");
                }
            }
            //4:Memory type
            for (int k = 0; k < data.Count; k++)
            {
                if (data[k].GetType().ToString().Equals("ComputerFitting.RAM"))
                {
                    for (int j = 0; j < data.Count; j++)
                    {
                        if (data[j].GetType().ToString().Equals("ComputerFitting.RAM"))
                        {
                            label2.Text = "Memory type: " + ((RAM)data[j]).type;
                            if (!((RAM)data[j]).type.Equals(((RAM)data[k]).type))
                            {
                                checkBox5.Checked = false;
                                break;
                            }
                        }
                    }
                }
            }
            //5:Overall
            if(!checkBox1.Checked || !checkBox2.Checked || !checkBox3.Checked || !checkBox5.Checked)
            {
                checkBox4.Checked = false;
            }
                }
        private void Fitting_Load(object sender, EventArgs e)
        {

        }
        // The column we are currently using for sorting.
        private ColumnHeader SortingColumn = null;

        // Sort on this column.
        private void ListView1_ColumnClick(object sender,
            ColumnClickEventArgs e)
        {
            // Get the new sorting column.
            ColumnHeader new_sorting_column = listView1.Columns[e.Column];

            // Figure out the new sorting order.
            System.Windows.Forms.SortOrder sort_order;
            if (SortingColumn == null)
            {
                // New column. Sort ascending.
                sort_order = SortOrder.Ascending;
            }
            else
            {
                // See if this is the same column.
                if (new_sorting_column == SortingColumn)
                {
                    // Same column. Switch the sort order.
                    if (SortingColumn.Text.StartsWith("> "))
                    {
                        sort_order = SortOrder.Descending;
                    }
                    else
                    {
                        sort_order = SortOrder.Ascending;
                    }
                }
                else
                {
                    // New column. Sort ascending.
                    sort_order = SortOrder.Ascending;
                }

                // Remove the old sort indicator.
                SortingColumn.Text = SortingColumn.Text.Substring(2);
            }

            // Display the new sort order.
            SortingColumn = new_sorting_column;
            if (sort_order == SortOrder.Ascending)
            {
                SortingColumn.Text = "> " + SortingColumn.Text;
            }
            else
            {
                SortingColumn.Text = "< " + SortingColumn.Text;
            }

            // Create a comparer.
            listView1.ListViewItemSorter =
                new ListViewComparer(e.Column, sort_order);

            // Sort.
            listView1.Sort();
        }


        private void Button5_Click(object sender, EventArgs e)
        {
            Memory a = new Memory();
            a.fit = this;
            a.Show();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            PowerAdapter a = new PowerAdapter();
            a.fit = this;
            a.Show();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Processor a = new Processor();
            a.fit = this;
            a.Show();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            GraphicCard a = new GraphicCard();
            a.fit = this;
            a.Show();
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count != 0)
            {
                int index = listView1.SelectedIndices[0];



                if (index != -1 && index < data.Count)
                {
                    data.RemoveAt(index);
                    RefreshTable();

                }
                else
                {
                    MessageBox.Show("Invalid ID");
                }
            }
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            data.Clear();
            RefreshTable();
        }
    }
}
