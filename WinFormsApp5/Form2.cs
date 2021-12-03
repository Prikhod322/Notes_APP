using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WinFormsApp5
{
    public partial class Form2 : Form
    {
        string DataSourse;
        string InitialCatalog;
        string IntegratedSecurity;
        public Form2(string connection)
        {
            InitializeComponent();

            DataSourse = connection.Split('|')[0];
            InitialCatalog = connection.Split('|')[1];
            IntegratedSecurity = connection.Split('|')[2];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection($"Data Source={DataSourse};Initial Catalog={InitialCatalog};Integrated Security={IntegratedSecurity};"))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(@$"INSERT INTO NOTE(NAME,TEXT,CREATION_TIME) VALUES('{textBox1.Text}','{textBox2.Text}','{DateTime.Now}')", connection))
                {
                    int rows = command.ExecuteNonQuery();
                    if (rows > 0)
                        MessageBox.Show("Note Added!");
                    else
                        Console.WriteLine("ERROR!");

                }
                connection.Close();
                    this.Close();
            }
        }
    }
}
