using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnerGoConsult
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int size = -1;
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string fileName = openFileDialog1.FileName;
                textBox1.Text = fileName;
                string text="";
                try
                {
                    text = File.ReadAllText(fileName);
                    size = text.Length;
                }
                catch (IOException)
                {
                    string message = "Some error occured :(";
                    string caption = "Error Detected";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult MBresult;

                    // Displays the MessageBox.

                    MBresult = MessageBox.Show(message, caption, buttons);

                    if (MBresult == System.Windows.Forms.DialogResult.OK)
                    {

                        // Closes the parent form.

                        this.Close();

                    }

                }
                SourceCode.FileHandler fileHandler = new SourceCode.FileHandler();
				fileHandler.ParseFile(text);

				List<string[]> headerArray = fileHandler.GetHeaderArray();
				foreach (string[] item in headerArray)
				{
					listView1.Items.Add(new ListViewItem(item));
				}
				listView1.Columns[0].Width = -2;
				listView1.Columns[1].Width = -2;

				List<List<string>> qualityArray = fileHandler.GetQualityArray();
				dataGridView1.ColumnCount = qualityArray[0].Count;
				for (int i = 0; i < qualityArray[0].Count; i++)
				{
					dataGridView1.Columns[i].Name = qualityArray[0][i];
				}
				for (int i = 1; i < qualityArray.Count; i++)
				{
					dataGridView1.Rows.Add(qualityArray[i].ToArray());
				}
				label1.Text = "Start date: dd/mm/yyyy";
				label2.Text = "End date: dd/mm/yyyy";
				label3.Text = "Max: value";
				label4.Text = "Min: value";
			}
		}
    }
}
