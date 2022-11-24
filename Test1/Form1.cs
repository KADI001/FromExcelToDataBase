using System.ComponentModel;
using MySql.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using MySql.Data.MySqlClient;

namespace Test1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (var stream = new StreamReader(openFileDialog1.FileName))
                {
                    string str = stream.ReadToEnd();
                    string[] content = str.Split(',', '\n').Where(s => s.Length > 0).ToArray();
                    
                    string connectionToDB = "server=localhost;user=root;database=people;password=root;";

                    int numberColumns = 4;
                    int amount = (content.Length - numberColumns) / numberColumns;
                    
                    MySqlConnection connection = new MySqlConnection(connectionToDB);
                    connection.Open();

                    for (int i = numberColumns, a = 0; a < amount; i += numberColumns, a++)
                    {
                        string req = $"INSERT people.info (firstname, lastname, phone, city) VALUES " +
                                     $"({content[i]}, {content[i + 1]}, {content[i + 2]}, {content[i + 3]});";
                        
                        MySqlCommand command = new MySqlCommand(req, connection);
                        
                        command.ExecuteScalar();
                    }
                    
                    connection.Close();
                }
            }
        }
    }
}