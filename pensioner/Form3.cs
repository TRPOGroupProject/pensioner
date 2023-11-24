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
            GlobalData.TextForChoice = "Добро пожаловать в симулятор пенсионера!";
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
            Form2 form2 = new Form2();
            form2.Close();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
           /* using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string sqlQuery = "SELECT number, points FROM гы";
                    MySqlCommand command = new MySqlCommand(sqlQuery, connection);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int currentNumber = reader.GetInt32("number");
                            GlobalData.PointsOfHappiness = reader.GetInt32("points");
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Обработка ошибок подключения или выполнения запроса
                    Console.WriteLine("Ошибка: " + ex.Message);
                }
            }*/
        }
    

        void restart(int number, int happiness)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open(); 

                string query = "UPDATE гы SET number = @number, points = @happiness";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
              
                    command.Parameters.AddWithValue("@number", number);
                    command.Parameters.AddWithValue("@happiness", happiness);

                    command.ExecuteNonQuery();
                }
            }
        }
        void EndGame()
        {
            int pointsOfHappiness = GlobalData.PointsOfHappiness;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                restart(1, 70);
                connection.Open();

                if (GlobalData.Batery)
                {
                    DisplayEvent(connection, 800);
                }
                else if (pointsOfHappiness < 50)
                {
                    DisplayEvent(connection, 711);
                }
                else if (pointsOfHappiness >= 50 && pointsOfHappiness <= 75)
                {
                    DisplayEvent(connection, 712);
                }
                else if (pointsOfHappiness > 75)
                {
                    string number = GetEndEventNumber(connection);
                    if (!string.IsNullOrEmpty(number))
                    {
                        DisplayEvent(connection, int.Parse(number));
                        currentNum++;
                    }
                }

                connection.Close();
            }
        }

        void DisplayEvent(MySqlConnection connection, int eventNumber)
        {
            string query = $"SELECT `text_pen`, `pictures` FROM `events` WHERE `number` = @eventNumber";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@eventNumber", eventNumber);

            using (MySqlDataReader dataReader = command.ExecuteReader())
            {
                if (dataReader.Read())
                {
                    string textPen = dataReader["text_pen"].ToString();
                    string imageName = dataReader["pictures"].ToString();

                    ImageSet(imageName);
                    richTextBox1.Text = textPen;
                }
            }
        }

        string GetEndEventNumber(MySqlConnection connection)
        {
            string query = "SELECT number FROM end WHERE order_pen = @currentNum";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@currentNum", currentNum);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    string number = reader["number"].ToString();
                    return number;
                }
            }

            return null;
        }

        void ImageSet(string imageName)
        {
            string directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "pensioner", "pensioner", "images");
            this.BackgroundImage = System.Drawing.Image.FromFile(Path.Combine(directoryPath, imageName + ".png"));
        }


        private void pictureBox7_Click(object sender, EventArgs e)
        {
            if (GlobalData.CurrentNumber == 630||GlobalData.Batery)
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
            ImageSet(GlobalData.Picture);
            if (GlobalData.Picture == "батареи") GlobalData.Batery = true;
            Font currentFont = richTextBox1.Font;
            float newSize = GlobalData.FontText;
            Font newFont = new Font(currentFont.FontFamily, newSize, currentFont.Style);
            richTextBox1.Font = newFont;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            form4.ShowDialog();
        }
    }
}

