using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 학원_관리_프로그램___A2조
{
    internal class C_AttendanceManageSystem
    {
        E_Attendance student;

        public void saveAttendance(ListViewItem std, ListView stdList)
        {
            B_AttendanceManageUI ui = new B_AttendanceManageUI();

            student = new E_Attendance();

            student.Id = std.SubItems[0].Text;
            student.Name = std.SubItems[1].Text;            
            student.Date = Convert.ToDateTime(std.SubItems[2].Text);

            int attendValue;
            if(std.SubItems[3].Text == "출석") 
                attendValue = 1;
            else 
                attendValue = 0;

            //update data in DB
            string strConn = "Server=127.0.0.1;Port=3306;" +
                "Database=academyManagement;Uid=root;Pwd=012520";
            MySqlConnection conn = new MySqlConnection(strConn);

            conn.Open();
            string sql = string.Format(
                "UPDATE Attendance SET attend = {0} WHERE id = '{1}' AND date = '{2}'",
                attendValue, student.Id, student.Date.ToString("d"));

            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.ExecuteNonQuery();

            conn.Close();

            ui.showStudentListViewUI(stdList);
        }

        public ListView setAttend(ListView attendLV)
        {
            foreach (ListViewItem item in attendLV.CheckedItems)
            {
                if (item.Checked)
                    item.SubItems[3].Text = "출석";
            }
            return attendLV;
        }

        public ListView setAbsent(ListView absentLV)
        {
            foreach (ListViewItem item in absentLV.CheckedItems)
            {
                if (item.Checked)
                    item.SubItems[3].Text = "결석";
            }
            return absentLV;
        }

        public void readDB(ListView attendListView)
        {
            string strConn = "Server=127.0.0.1;Port=3306;Database=academyManagement;Uid=root;Pwd=012520";
            MySqlConnection conn = new MySqlConnection(strConn);

            conn.Open();

            MySqlCommand cmd = new MySqlCommand(
                "SELECT S.id, S.name, A.date, A.attend FROM Student S,Attendance A WHERE S.id=A.id", conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string attendValue;
                if ((bool)reader["attend"]) attendValue = "출석";
                else attendValue = "결석";

                attendListView.Items.Add(new ListViewItem(
                        new string[] { reader["id"].ToString(), (string)reader["name"],
                             ((DateTime)reader["date"]).ToString("d"), attendValue }));
            }
            reader.Close();
            conn.Close();
        }
    }
}
