using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlightsWindowsForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void flightsBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.flightsBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.flightData);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'flightData.flights' table. You can move, or remove it, as needed.
            this.flights2TableAdapter1.Fill(this.flightData.flights2);

            

            FillComboBoxes();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string dayweek = GetDate();
            string depart = cbxDepartFrom.SelectedItem.ToString();
            string arrival = cbxArrival.SelectedItem.ToString();

            //this.flights2TableAdapter1.FillByDayOp(this.flightData.flights2, dayweek);

            this.flights2TableAdapter1.FillBySearch(this.flightData.flights2, depart, arrival, dayweek);

            DateTime time = new DateTime();
            flightData.flights2DataTable test = this.flights2TableAdapter1.GetDataBySearch(depart, arrival, dayweek);
            time = test[0].dep_time;

            DateTime newtime = new DateTime(1899, 12, 30, 18,30,00);

            test = this.flights2TableAdapter1.GetDataByTime(newtime,depart,dayweek);

            for (int i = 0; i < test.Count; i++)
            {
                time = test[i].dep_time;
            }

            //general equation for time zone calcualtions
            // arrival time = departure time + duration + (arrival zone - departure zone)
        }

        public string GetDate()
        {
            DateTime date = new DateTime();
            string dayofweek;
            string ret = null;
            int result;

            date = dateTimePicker.Value;
            dayofweek = date.DayOfWeek.ToString();

            result = String.Compare("Sunday", dayofweek);
            if (result == 0) ret = "1%";
            result = String.Compare("Monday", dayofweek);
            if (result == 0) ret = "%2%";
            result = String.Compare("Tuesday", dayofweek);
            if (result == 0) ret = "%3%";
            result = String.Compare("Wednesday", dayofweek);
            if (result == 0) ret = "%4%";
            result = String.Compare("Thursday", dayofweek);
            if (result == 0) ret = "%5%";
            result = String.Compare("Friday", dayofweek);
            if (result == 0) ret = "%6%";
            result = String.Compare("Saturday", dayofweek);
            if (result == 0) ret = "%7";

            return ret;
        }

        public void FillComboBoxes()
        {
            SqlConnection sqlConnection = new SqlConnection("Data Source=GATEWAY;Initial Catalog=master;User ID=sa;Password=test");
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "Select DISTINCT departure FROM flights2";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection;

            sqlConnection.Open();

            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                cbxDepartFrom.Items.Add(reader[0]);
            }

            reader.Close();

            cmd.CommandText = "SELECT DISTINCT arrival FROM flights2";

            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                cbxArrival.Items.Add(reader[0]);
            }

            reader.Close();

            cbxDepartFrom.SelectedIndex = 0;
            cbxArrival.SelectedIndex = 0;
            cbxDepartTime.SelectedIndex = 0;

            sqlConnection.Close();
        }
    }
}
