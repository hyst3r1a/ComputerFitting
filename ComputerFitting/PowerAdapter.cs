using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComputerFitting
{

    public partial class PowerAdapter : Form
    {
        public Fitting fit;
        public List<AC> data = new List<AC>();
        public PowerAdapter()
        {
            InitializeComponent();
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

        private void PowerAdapter_Load(object sender, EventArgs e)
        {
            LoadPowerAdapter();
            RefreshTable();
        }

        private void LoadPowerAdapter()
        {
            string filename = "C:\\DB\\AC.data";
            using (var file = File.OpenRead(filename))
            {

                var reader = new BinaryFormatter();

                try
                {
                    data = (List<AC>)reader.Deserialize(file); // Reads the entire list.
                }
                catch (System.Runtime.Serialization.SerializationException)
                {
                    Console.WriteLine("File is empty or corrupted");
                }
            }

            for (int i = 0; i < data.Count; i++)
            {
                String compatibility = "";
                ListViewItem temp = listView1.Items.Add(i.ToString());
                temp.SubItems.Add(data[i].name);
              
                
                temp.SubItems.Add(data[i].price);


                temp.SubItems.Add(data[i].note);
                for (int j = 0; j < data[i].notCompatible.Count; j++)
                {

                    compatibility += data[i].notCompatible[j].ToString() + ";";



                }
                temp.SubItems.Add(compatibility);
                temp.SubItems.Add(data[i].powerOutput.ToString());
            }

        }
        private void PowerAdapter_FormClosing(object sender, FormClosingEventArgs e)
        {
            SavePowerAdapter();
        }
        private void RefreshTable()
        {
            listView1.Items.Clear();
            for (int i = 0; i < data.Count; i++)
            {
                String compatibility = "";
                ListViewItem temp = listView1.Items.Add(i.ToString());
                temp.SubItems.Add(data[i].name);
               
              
                temp.SubItems.Add(data[i].price);

                temp.SubItems.Add(data[i].note);



                for (int j = 0; j < data[i].notCompatible.Count; j++)
                {

                    compatibility += data[i].notCompatible[j].ToString() + ";";



                }
                temp.SubItems.Add(compatibility);
                temp.SubItems.Add(data[i].powerOutput.ToString());
            }
        }
        public void SavePowerAdapter()
        {
            string filename = "C:\\DB\\AC.data";
            using (var file = File.OpenWrite(filename))
            {
                var writer = new BinaryFormatter();
                writer.Serialize(file, data); // Writes the entire list.
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            //New/Edit button
            //Edits if ID specified, if not - adds new
            int temp = 0;
            if (textBox2.Text == "" || !int.TryParse(textBox2.Text, out temp))
            {
                //add
                if (textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "" && textBox6.Text != "")
                {
                    AC a = new AC();
                    a.name = textBox3.Text;
                    a.price = textBox5.Text;
                    a.note = textBox4.Text;
                    a.notCompatible = new List<string>();
                    a.powerOutput = textBox8.Text;
                   
                    StringBuilder str = new StringBuilder();
                    for (int i = 0; i < textBox6.Text.Length; i++)
                    {
                        Console.WriteLine(i);
                        if (textBox6.Text[i].Equals(';'))
                        {
                            a.notCompatible.Add(str.ToString());
                            str.Clear();
                        }
                        else
                        {
                            str.Append(textBox6.Text[i]);
                            Console.WriteLine(str.ToString());
                        }
                    }
                    data.Add(a);
                }
                else MessageBox.Show("Please fill up all prompts");
            }
            else
            {
                //edit
                if (temp < data.Count)
                {
                    if (textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "" && textBox6.Text != "")
                    {

                        data[temp].name = textBox3.Text;
                        data[temp].price = textBox5.Text;
                        data[temp].note = textBox4.Text;
                        data[temp].powerOutput = textBox8.Text;
                      
                        data[temp].notCompatible = new List<string>();
                        StringBuilder str = new StringBuilder();
                        for (int i = 0; i < textBox6.Text.Length; i++)
                        {
                            if (textBox6.Text[i].Equals(';'))
                            {
                                data[temp].notCompatible.Add(str.ToString());
                                str = new StringBuilder();
                            }
                            else
                            {
                                str.Append(textBox6.Text[i]);
                            }
                        }
                    }
                    else MessageBox.Show("Please fill up all prompts");

                }
                else MessageBox.Show("Incorrect editing ID");
            }
            RefreshTable();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count != 0)
            {
                var result = MessageBox.Show("Are you sure?", "Dialog", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
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
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count != 0)
            {
                int temp = listView1.SelectedIndices[0];
                fit.data.Add(data[temp]);
                fit.RefreshTable();
                this.Close();
            }
        }

        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count != 0)
            {
                textBox6.Clear();
                int temp = listView1.SelectedIndices[0];
                AC a = data[temp];
                textBox2.Text = temp.ToString();
                textBox3.Text = a.name;
                textBox5.Text = a.price;
                textBox4.Text = a.note;
                textBox8.Text = a.powerOutput;
                
                foreach (String b in a.notCompatible)
                {
                    textBox6.Text += (b + ";");
                }

            }
        }
    }
}


