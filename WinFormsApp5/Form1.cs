using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent(); 
            listBox1.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            textBox1.Visible = false;
            textBox4.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2($"{textBox2.Text}|{textBox3.Text}|{(checkBox1.Checked==true?"True":"False")}");
            f2.ShowDialog();
        }

        int SelectedIDX;
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                textBox1.Text = texts[listBox1.SelectedIndex];
                textBox4.Text = dates[listBox1.SelectedIndex];
                SelectedIDX = listBox1.SelectedIndex;
            }
            else
            {
                textBox1.Text = string.Empty;
            }
        }

        private void Show()
        {
                connection.Open();
            if (deleteMode == false)
            {
                oString = "Select * FROM [NOTE]";
                oCmd = new SqlCommand(oString, connection);
                oReader = oCmd.ExecuteReader();
                listBox1.Items.Clear();
                texts.Clear();
                while (oReader.Read())
                {
                    listBox1.Items.Add(oReader["NAME"]);
                    texts.Add(oReader["TEXT"].ToString());
                    dates.Add(oReader["CREATION_TIME"].ToString());
                }
                
                if(listBox1.Items.Count>0)
                listBox1.SelectedIndex = SelectedIDX;
            }
            else
            {
                //oString = $"Delete FROM [NOTE] WHERE ROW_NUMBER={listBox1.SelectedIndex + 1}";
                oString = $"Delete [NOTE] * From( Select Row_Number() Over(Order By ID) As RowNum, * From [NOTE]) [NOTE] Where RowNum = {listBox1.SelectedIndex + 1}";
                oCmd = new SqlCommand(oString, connection);
                oReader = oCmd.ExecuteReader();

               // MessageBox.Show($"Delete FROM [NOTE] WHERE ROW_NUMBER={listBox1.SelectedIndex + 1}");

                deleteMode = false;
            }
                connection.Close();
        }


        private async void Update()
        {

            Action action = () =>
            {
                Show();
            };

            await Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(500);
                    Invoke(action);
                }
            });
        }
        SqlConnection connection;
        string connectionString;
        string oString;
        SqlCommand oCmd;
        SqlDataReader oReader;
        List<string> texts = new List<string>();
        List<string> dates = new List<string>();
        private void button3_Click(object sender, EventArgs e)
        {
            connectionString = $"Data Source={textBox2.Text};Initial Catalog={textBox3.Text};Integrated Security=True;";
            connection = new SqlConnection(connectionString);
            listBox1.Visible = true;
            button1.Visible = true;
            button2.Visible = true;
            textBox1.Visible = true;
            textBox4.Visible = true;
            Update();
        }
        bool deleteMode = false;

        private void button2_Click(object sender, EventArgs e)
        {
            deleteMode = true;
        }
    }
}
