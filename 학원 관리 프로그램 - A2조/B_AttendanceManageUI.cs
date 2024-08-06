using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 학원_관리_프로그램___A2조
{
    public partial class B_AttendanceManageUI : Form
    {
        public B_AttendanceManageUI()
        {
            InitializeComponent();

            //insert data from DB
            C_AttendanceManageSystem system = new C_AttendanceManageSystem();
            system.readDB(attendListView);

            //set calender
            calender.Value = DateTime.Now;
        }

        private void attendButton_Click(object sender, EventArgs e)
        {
            attendEvent();
        }

        public void attendEvent()
        {
            if (MessageBox.Show("출석으로 변경하시겠습니까?", "확인",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                C_AttendanceManageSystem system = new C_AttendanceManageSystem();
                attendListView = system.setAttend(attendListView);

                showStudentListViewUI(attendListView);
            }
        }

        private void absentButton_Click(object sender, EventArgs e)
        {
            absentEvent();
        }


        public void absentEvent()
        {
            if (MessageBox.Show("결석으로 변경하시겠습니까?", "확인", 
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                C_AttendanceManageSystem system = new C_AttendanceManageSystem();
                attendListView = system.setAbsent(attendListView);
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            var checkedIndexes = attendListView.CheckedIndices;
            foreach (int index in checkedIndexes)
                saveEvent(index);

            MessageBox.Show("저장되었습니다.");
        }

        public void saveEvent(int index)
        {
            C_AttendanceManageSystem system = new C_AttendanceManageSystem();
            system.saveAttendance(attendListView.Items[index], attendListView);
        }

        public void showStudentListViewUI(ListView std)
        {
            attendListView = std;
        }

        private void calender_ValueChanged(object sender, EventArgs e)
        {
            if (calender.Value > DateTime.Now)
            {
                MessageBox.Show("오늘 이후의 출결을 수정할 수 없습니다.", "오류");
                calender.Value = DateTime.Now;
                return;
            }

            //reset listView
            while (attendListView.Items.Count != 0)
                attendListView.Items[0].Remove();

            //insert data from DB with calender's date
            string strConn = "Server=127.0.0.1;Port=3306;Database=academyManagement;Uid=root;Pwd=012520";
            MySqlConnection conn = new MySqlConnection(strConn);

            conn.Open();

            string sql = string.Format(
                "SELECT S.id, S.name, A.date, A.attend FROM Student S,Attendance A WHERE S.id=A.id AND date='{0}'", 
                calender.Value.ToString("d"));

            MySqlCommand cmd = new MySqlCommand(sql, conn);
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
