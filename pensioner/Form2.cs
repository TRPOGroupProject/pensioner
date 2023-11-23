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


namespace pensioner2
{
    public partial class Form2 : Form
    {
        int choice1;
        int choice2;
        int choice3;    
        int choice4;
        private MySqlConnection connection;
        private string connectionString = "server=localhost;port=3306;username=root;password=root;database=pensioner";

        public Form2()
        {
            InitializeComponent();
            SetFullScreen();
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
            
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            // Информация из таблицы daily_events
            string dailyEventsQuery = $"SELECT * FROM `daily_events` WHERE `number` = {GlobalData.CurrentNumber}";
            MessageBox.Show(Convert.ToString(GlobalData.CurrentNumber));
            MySqlCommand dailyEventsCommand = new MySqlCommand(dailyEventsQuery, connection);
            MySqlDataReader dailyEventsReader = dailyEventsCommand.ExecuteReader();
        
            if (dailyEventsReader.Read())
            {

                // Чтение значений полей
                string var1 = dailyEventsReader["var1"].ToString();
                string var2 = dailyEventsReader["var2"].ToString();
                string var3 = dailyEventsReader["var3"].ToString();
                string var4 = dailyEventsReader["var4"].ToString();
        
             

                // Заполнение значений в соответствующих элементах управления
    
                label1.Text = var1;
                label2.Text = var2;
                label3.Text = var3;
                label4.Text = var4;

                // Запись значений в глобальные переменные
                choice1 = int.Parse(dailyEventsReader["var1event"].ToString());
                choice2 = int.Parse(dailyEventsReader["var2event"].ToString());
                choice3 = int.Parse(dailyEventsReader["var3event"].ToString());
                choice4 = int.Parse(dailyEventsReader["var4event"].ToString());

                dailyEventsReader.Close();

            }
        }
    }
}
