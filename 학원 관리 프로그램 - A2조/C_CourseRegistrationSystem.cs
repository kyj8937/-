using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 학원_관리_프로그램___A2조
{
    internal class C_CourseRegistrationSystem
    {
        E_Course selected;

        public int register(ListViewItem selectedLvi, ListView registeredLV, ListView timetable)
        {
            B_CourseRegistrationUI ui = new B_CourseRegistrationUI();
            int isErrored = -1;

            selected = new E_Course();
            selected.CourseNum = selectedLvi.SubItems[0].Text;
            selected.CourseName = selectedLvi.SubItems[1].Text;
            selected.ClassTime = selectedLvi.SubItems[2].Text;
            selected.Teacher = selectedLvi.SubItems[3].Text;

            //checkCourseOverlap
            for (int i = 0; i < registeredLV.Items.Count; i++)
            {
                ListViewItem tmp = registeredLV.Items[i];
                if (tmp.SubItems[0].Text == selected.CourseNum)
                    return 0;
            }

            string[] courseData = new string[] {
                selected.CourseNum,
                selected.CourseName,
                selected.ClassTime,
                selected.Teacher                
            };

            ListViewItem lvi = new ListViewItem(courseData);
            registeredLV.Items.Add(lvi);
            ui.showRegisteredCoursesUI(registeredLV);

            //checkTimeOverlap
            string[] times = selected.ClassTime.Split(',');

            for (int i = 0; i < times.Length; i++)
            {
                int week = 0;
                
                switch (times[i][0] + "" + times[i][1])
                {
                    case "Mo": week = 0; break;
                    case "Tu": week = 1; break;
                    case "We": week = 2; break;
                    case "Th": week = 3; break;
                    case "Fr": week = 4; break;
                }

                for (int j = 2; j < times[i].Length; j++)
                {
                    if (timetable.Items[times[i][j] - '1'].SubItems[week + 1].Text != "")
                    {
                        timetable.Items[times[i][j] - '1'].SubItems[week + 1].Text = selected.CourseNum;
                        timetable.Items[times[i][j] - '1'].SubItems[week + 1].BackColor = Color.Red;
                        isErrored = 1;
                    }
                    else
                    {
                        timetable.Items[times[i][j] - '1'].SubItems[week + 1].Text = selected.CourseNum;
                    }
                }
            }

            //insert registered data to DB if not error
            if(isErrored == -1)
            {
                string strConn = "Server=127.0.0.1;Port=3306;" +
                    "Database=academyManagement;Uid=root;Pwd=012520";
                MySqlConnection conn = new MySqlConnection(strConn);

                conn.Open();

                string sql = string.Format(
                    "INSERT INTO Register (id, courseNum, registerDate) " +
                    "VALUES ('{0}', '{1}', '{2}');",
                    B_CourseRegistrationUI.userId, selected.CourseNum, 
                    DateTime.Now.ToString("d"));

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            return isErrored;
        }

        public void readDB(ListView cListView, ListView rListView, string userId)
        {
            string strConn = "Server=127.0.0.1;Port=3306;Database=academyManagement;Uid=root;Pwd=012520";
            MySqlConnection conn = new MySqlConnection(strConn);

            conn.Open();

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Course", conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                cListView.Items.Add(new ListViewItem(
                        new string[] { (string)reader["courseNum"], (string)reader["courseName"],
                            (string)reader["classTime"], (string)reader["teacher"]}));
            }
            reader.Close();

            cmd.CommandText = String.Format(
                "SELECT * FROM Register,Course WHERE Course.courseNum=Register.CourseNum AND id = '{0}'", userId);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                rListView.Items.Add(new ListViewItem(
                        new string[] { (string)reader["courseNum"], (string)reader["courseName"],
                            (string)reader["classTime"], (string)reader["teacher"]}));
            }
            reader.Close();
            conn.Close();
        }
    }
}
