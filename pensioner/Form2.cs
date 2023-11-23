using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using pensioner;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;


namespace pensioner2
{
    public partial class Form2 : Form
    {
        int choice1;
        int choice2;
        int choice3;
        int choice4;


        private string connectionString = "server=localhost;port=3306;username=root;password=root;database=pensioner";

        public Form2()
        {
            InitializeComponent();
            SetFullScreen();
            cho1.Click += pictureBox2_Click;
            cho2.Click += pictureBox2_Click;
            cho3.Click += pictureBox2_Click;
            cho4.Click += pictureBox2_Click;
        }

        private void SetFullScreen()
        {
            this.Width = Screen.PrimaryScreen.Bounds.Width;
            this.Height = Screen.PrimaryScreen.Bounds.Height;
            this.WindowState = FormWindowState.Maximized;
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            richTextBox1.Text ="Что бы сегодня сделать?";
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            // Информация из таблицы daily_events
            string dailyEventsQuery = $"SELECT * FROM `daily_events` WHERE `number` = {GlobalData.CurrentNumber}";

            MySqlCommand dailyEventsCommand = new MySqlCommand(dailyEventsQuery, connection);
            MySqlDataReader dailyEventsReader = dailyEventsCommand.ExecuteReader();

            if (dailyEventsReader.Read())
            {
                string var1 = dailyEventsReader["var1"].ToString();
                string var2 = dailyEventsReader["var2"].ToString();
                string var3 = dailyEventsReader["var3"].ToString();
                string var4 = dailyEventsReader["var4"].ToString();

                label1.Text = var1;
                label2.Text = var2;
                label3.Text = var3;
                label4.Text = var4;

                choice1 = int.Parse(dailyEventsReader["var1event"].ToString());
                choice2 = int.Parse(dailyEventsReader["var2event"].ToString());
                choice3 = int.Parse(dailyEventsReader["var3event"].ToString());
                choice4 = int.Parse(dailyEventsReader["var4event"].ToString());

                dailyEventsReader.Close();
            }
        }


        void Daily_Events(int chosen)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT text_pen FROM events WHERE number = @chosenNumber";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@chosenNumber", chosen);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string textPen = reader["text_pen"].ToString();
                            GlobalData.TextForChoice = textPen;
                            GlobalData.PointsOfHappiness += 5;

                            this.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при выполнении запроса: " + ex.Message);
                }
            }
        }

        private int UsersChoice(int lastDigit)
        {
            switch (lastDigit)
            {
                case 1:
                    return choice1;
                   
                case 2:
                    return  choice2;
             
                case 3:
                    return choice3;
  
                case 4:
                    return choice4;
                 
                default:
                    return -1;
            }
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (sender is PictureBox pictureBox)
            {
                int lastDigit = GetLastDigitFromPictureBoxName(pictureBox);
                int chosen = UsersChoice(lastDigit);
                //MessageBox.Show(Convert.ToString(chosen));
                if (chosen == 0||(chosen <= 4 && chosen >= 1)) 
                {
                    GlobalData.PointsOfHappiness -= 10;

                    if (chosen <= 4 && chosen >= 1)
                    { 
                        GlobalData.HomeRoom = chosen;
                    }
                    HomeWalking();


                }
                else
                {
                    GlobalData.PointsOfHappiness += 5;
                    Daily_Events(chosen);
                }
            }
        }

        void HomeWalking()
        {
            int homeRoom = GlobalData.HomeRoom; // Значение из GlobalData.HomeRoom

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM home WHERE locations = @HomeRoom";
                //MessageBox.Show(Convert.ToString(homeRoom));
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@HomeRoom", GlobalData.HomeRoom);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            label1.Text = reader.GetString("var1");
                            label2.Text = reader.GetString("var2");
                            label3.Text = reader.GetString("var3");
                            label4.Text = reader.GetString("var4");

                            choice1 = reader.GetInt32("var1event");
                            choice2 = reader.GetInt32("var2event");
                            choice3 = reader.GetInt32("var3event");
                            choice4 = reader.GetInt32("var4event");
                        }         
                    }
                }
            }
        } 

        int GetLastDigitFromPictureBoxName(PictureBox pictureB)
        {
            string name = pictureB.Name;
            string lastCharacter = name.Substring(name.Length - 1);
            int lastDigit;
            if (int.TryParse(lastCharacter, out lastDigit))
            {
                return lastDigit;
            }
            else
            {
                return -1;
            }
        }
    }
}

