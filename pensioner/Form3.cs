using Microsoft.Win32;
using MySql.Data.MySqlClient;
using pensioner;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using Application = System.Net.Mime.MediaTypeNames.Application;

namespace pensioner2
{
    public partial class Form3 : Form
    {
        private MySqlConnection connection;
        private string connectionString = "server=localhost;port=3306;username=root;password=root;database=pensioner";
        private int currentNumber;
        private int currentNum=1;

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

        void EndGame()
        {
            int pointsOfHappiness = GlobalData.PointsOfHappiness;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query;
                MySqlCommand command;
                MySqlDataReader reader;

                if (pointsOfHappiness < 50)
                {
                    query = "SELECT text_pen FROM events WHERE number = 711";
                    command = new MySqlCommand(query, connection);
                    reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        string textPen = reader["text_pen"].ToString();
                        richTextBox1.Text = textPen;
                    }

                    reader.Close();
                }
                else if (pointsOfHappiness >= 50 && pointsOfHappiness <= 75)
                {
                    query = "SELECT text_pen FROM events WHERE number = 712";
                    command = new MySqlCommand(query, connection);
                    reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        string textPen = reader["text_pen"].ToString();
 

                        richTextBox1.Text = textPen;
                    }

                    reader.Close();
                }
                else if (pointsOfHappiness > 75)
                {
                    query = "SELECT number FROM end WHERE order_pen = @currentNum";
                    command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@currentNum", currentNum);
                    reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        string number = reader["number"].ToString();
                        reader.Close();

                        query = "SELECT text_pen FROM events WHERE number = @number";
                        command = new MySqlCommand(query, connection);
                        command.Parameters.AddWithValue("@number", number);
                        reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            string textPen = reader["text_pen"].ToString();
                            richTextBox1.Text = textPen;
                        }
                        currentNum++;
                    }

                    reader.Close();
                }

                connection.Close();
            }
        }

        void ImageSet(string imageName)
        {
            string directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "pensioner", "pensioner", "images");
            this.BackgroundImage = System.Drawing.Image.FromFile(Path.Combine(directoryPath, imageName + ".png"));
        }


        private void pictureBox7_Click(object sender, EventArgs e)
        {
            if (GlobalData.CurrentNumber == 630)
            {
                EndGame();
            }
            else
            {

                connection = new MySqlConnection(connectionString);
                connection.Open();

                //инфа из таблицы order
                string query = $"SELECT * FROM `order` WHERE `order_pen` = {currentNumber}";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();
                {
                    currentNumber++;

                    if (reader.Read())
                    {
                        int number = Convert.ToInt32(reader["number"]);
                        int choice = Convert.ToInt32(reader["choice"]);
                        GlobalData.CurrentNumber = number;

                        reader.Close();

                        if (choice == 0)
                        {
                            string eventsQuery = $"SELECT `text_pen`, `pictures` FROM `events` WHERE `number` = {number}";
                            MySqlCommand eventsCommand = new MySqlCommand(eventsQuery, connection);

                            using (MySqlDataReader dataReader = eventsCommand.ExecuteReader())
                            {
                                if (dataReader.Read())
                                {
                                    richTextBox1.Text = dataReader["text_pen"].ToString();
                                    string imageName = dataReader["pictures"].ToString();
                                    ImageSet(imageName);
                                }
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
                }
                connection.Close();
            }
        }

        private void Form3_Activated(object sender, EventArgs e)
        {
            richTextBox1.Text = GlobalData.TextForChoice;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}

