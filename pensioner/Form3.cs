﻿using MySql.Data.MySqlClient;
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
        private int currentNumber = 1;

        public Form3()
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

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }
        private void pictureBox7_Click(object sender, EventArgs e)
        {
            connection = new MySqlConnection(connectionString);
            connection.Open();

            // Выполняем SQL-запрос для получения информации из таблицы order
            string query = $"SELECT * FROM `order` WHERE `order_pen` = {currentNumber}";
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader reader = command.ExecuteReader();

            currentNumber++;

            if (reader.Read())
            {
                int number = Convert.ToInt32(reader["number"]);
                int choice = Convert.ToInt32(reader["choice"]);

                // Закрываем первый DataReader
                reader.Close();

                if (choice == 0)
                {
                    // Выполняем SQL-запрос для получения информации из таблицы events
                    string eventsQuery = $"SELECT `text_pen` FROM `events` WHERE `number` = {number}";
                    MySqlCommand eventsCommand = new MySqlCommand(eventsQuery, connection);
                    object result = eventsCommand.ExecuteScalar();

                    if (result != null)
                    {
                        // Записываем значение text_pen в richTextBox1
                        richTextBox1.Text = result.ToString();
                    }
                }
                else if (choice == 1)
                {
                    // Открываем Form2
                    Form2 form2 = new Form2();
                    form2.Show();
                }
            }

            // Закрываем соединение
            connection.Close();
        }
    }



}
