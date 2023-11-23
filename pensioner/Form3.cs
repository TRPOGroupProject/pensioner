using MySql.Data.MySqlClient;
using pensioner;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pensioner2
{
    public partial class Form3 : Form
    {
        private MySqlConnection connection;
        private string connectionString = "server=localhost;port=3306;username=root;password=root;database=pensioner";
        private int currentNumber;
       
        

        public Form3()
        {
            InitializeComponent();
            SetFullScreen();
            GlobalData.TextForChoice = "Добро пожаловать в смулятор пенсионера!";
        }

        private void SetFullScreen()
        {
            this.Width = Screen.PrimaryScreen.Bounds.Width;
            this.Height = Screen.PrimaryScreen.Bounds.Height;
            this.WindowState = FormWindowState.Maximized;
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            richTextBox1.Text = GlobalData.TextForChoice;
        }
        private void pictureBox7_Click(object sender, EventArgs e)
        {
           

            connection = new MySqlConnection(connectionString);
            connection.Open();

            //инфа из таблицы order
            string query = $"SELECT * FROM `order` WHERE `order_pen` = {currentNumber}";
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader reader = command.ExecuteReader();

            currentNumber++;

            if (reader.Read())
            {
                int number = Convert.ToInt32(reader["number"]);
                int choice = Convert.ToInt32(reader["choice"]);
                GlobalData.CurrentNumber = number;
                reader.Close();

                if (choice == 0)
                {
                    //информация из таблицы events
                    string eventsQuery = $"SELECT `text_pen` FROM `events` WHERE `number` = {number}";
                    MySqlCommand eventsCommand = new MySqlCommand(eventsQuery, connection);
                    object result = eventsCommand.ExecuteScalar();

                    if (result != null)
                    {
                        //текст
                        richTextBox1.Text = result.ToString();
                    }
                }
                else if (choice == 1)
                {
                    // работа с событиями
                    Form2 form2 = new Form2();
                    Form3 form3 = new Form3();
                    form3.Close();
                    form2.Show();    
                }
            }

            connection.Close();
        }
    }






}

