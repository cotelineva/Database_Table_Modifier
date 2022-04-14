using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Tema1_v2
{
    public partial class Form1 : Form
    {      
        public Form1()
        {
            InitializeComponent();
        }

        SqlDataAdapter dAdapter;
        DataSet dSet;
        private string conStr = "Integrated Security=true;" +
                                "Initial Catalog=Florarie;" +
                                "Data Source=LAPTOP-N16I6V2I\\SQLEXPRESS;";
        SqlConnection con = new SqlConnection("Integrated Security=true;" +
                                "Initial Catalog=Florarie;" +
                                "Data Source=LAPTOP-N16I6V2I\\SQLEXPRESS;");

        private void button2_Click(object sender, EventArgs e)
        {
            dAdapter = new SqlDataAdapter("select * from Adresse", conStr);
            dSet = new System.Data.DataSet();
            dAdapter.Fill(dSet);
            dataGridView1.DataSource = dSet.Tables[0].DefaultView;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //exception handler for not binding the second pressed cell with the first one
            if (textBox2.DataBindings.Count > 0)
                textBox2.DataBindings.RemoveAt(0);

            //binds cell to text box (zeigt IdA in einem TextBox wenn ein cell gedruckt wird)
            Binding b1 = new Binding("Text", dataGridView1[0, e.RowIndex], "Value", false);
            textBox2.DataBindings.Add(b1);
            textBox2.ReadOnly = true;

            //zeigt die Verkaufer fur IdA der gedruckt wurde

            //wir nehmen die Werte aus der Verkaufer Tabelle mit dem IdA = @p
            dAdapter = new SqlDataAdapter("select * from Verkaufer where IdA = @p", conStr);    
            dAdapter.SelectCommand.Parameters.Add("@p", SqlDbType.Int, 10).Value = dataGridView1.Rows[e.RowIndex].Cells[0].Value;

            //wir stellen die Tupeln in einem data set
            dSet = new System.Data.DataSet();
            dAdapter.Fill(dSet);

            //stellt die Werte in den zweiten data grid
            dataGridView3.DataSource = dSet.Tables[0].DefaultView;  
        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //exception handler for not binding the second pressed cell with the first one
            if (textBox3.DataBindings.Count > 0)
                textBox3.DataBindings.RemoveAt(0);

            //binds cell to text box (zeigt NameV)
            Binding b2 = new Binding("Text", dataGridView3[0, e.RowIndex], "Value", false);
            textBox3.DataBindings.Add(b2);

            //zeigt VornameV
            if (textBox4.DataBindings.Count > 0)
                textBox4.DataBindings.RemoveAt(0);
            Binding b3 = new Binding("Text", dataGridView3[1, e.RowIndex], "Value", false);
            textBox4.DataBindings.Add(b3);

            //zeigt IdV
            if (textBox5.DataBindings.Count > 0)
                textBox5.DataBindings.RemoveAt(0);
            Binding b4 = new Binding("Text", dataGridView3[2, e.RowIndex], "Value", false);
            textBox5.DataBindings.Add(b4);

            //zeigt Gehalt
            if (textBox7.DataBindings.Count > 0)
                textBox7.DataBindings.RemoveAt(0);
            Binding b5 = new Binding("Text", dataGridView3[4, e.RowIndex], "Value", false);
            textBox7.DataBindings.Add(b5);
        }

        private void button3_Click(object sender, EventArgs e)
        {                      
            //insert
            
            if (textBox5.Text.All(Char.IsDigit) && textBox7.Text.All(Char.IsDigit))
            {
                try { 
                    SqlDataAdapter cmd = new SqlDataAdapter();
                    cmd.InsertCommand = new SqlCommand("insert into Verkaufer values (@name,@vorname,@idV,@idA,@gehalt)", con);

                    cmd.InsertCommand.Parameters.AddWithValue("@idV", textBox5.Text);
                    cmd.InsertCommand.Parameters.AddWithValue("@name", textBox3.Text);
                    cmd.InsertCommand.Parameters.AddWithValue("@vorname", textBox4.Text);
                    cmd.InsertCommand.Parameters.AddWithValue("@idA", textBox2.Text);
                    cmd.InsertCommand.Parameters.AddWithValue("@gehalt", textBox7.Text);

                    con.Open();
                    cmd.InsertCommand.ExecuteNonQuery();
                    con.Close();

                    textBox6.Text = "Inserted!";
                }
                catch(Exception exception){
                    MessageBox.Show(exception.ToString(),"Error");
                    con.Close();
                }
            }
            else
            {
                textBox6.Text = "Error: Wrong DataType for IdV or Gehalt!";
                MessageBox.Show("This field only for integers!", "Error");
            }           
           
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //delete

            SqlCommand cmd = new SqlCommand("delete from Verkaufer where IdV = @idV", con);

            cmd.Parameters.AddWithValue("@idV", textBox5.Text);
            
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            
            textBox6.Text = "Deleted!";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //update

            if (textBox5.Text.All(Char.IsDigit) && textBox7.Text.All(Char.IsDigit))
            {
                SqlCommand cmd = new SqlCommand("update Verkaufer set NameV = @name, VornameV = @vorname, Gehalt = @gehalt where IdV = @idV", con);

                cmd.Parameters.AddWithValue("@idV", textBox5.Text);
                cmd.Parameters.AddWithValue("@name", textBox3.Text);
                cmd.Parameters.AddWithValue("@vorname", textBox4.Text);
                cmd.Parameters.AddWithValue("@gehalt", textBox7.Text);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                textBox6.Text = "Updated!";
            }
            else
            {
                textBox6.Text = "Error: Wrong DataType for IdV or Gehalt!";
                MessageBox.Show("This field only for integers!", "Error");
            }
        }
    }
}
