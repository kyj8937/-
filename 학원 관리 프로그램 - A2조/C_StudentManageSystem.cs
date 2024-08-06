using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 학원_관리_프로그램___A2조
{
    internal class C_StudentManageSystem
    {
        public ListView modifyStudent(ListView studentListView, E_Student std)
        {
            if (studentListView.SelectedItems.Count > 0)
            {
                //check identificationNum Overlap
                foreach (ListViewItem item in studentListView.Items)
                {
                    if (item.SubItems[0].Text == studentListView.SelectedItems[0].SubItems[0].Text)
                        continue;
                    if (item.SubItems[2].Text == std.IdentificationNum)
                    {
                        B_StudentManageUI ui = new B_StudentManageUI();
                        ui.warningIdentificationNumOverlap();
                        return null;
                    }
                }

                //update data in listView
                studentListView.SelectedItems[0].SubItems[0].Text = std.Id;
                studentListView.SelectedItems[0].SubItems[1].Text = std.Name;
                studentListView.SelectedItems[0].SubItems[2].Text = std.IdentificationNum;
                studentListView.SelectedItems[0].SubItems[3].Text = std.Address;
                studentListView.SelectedItems[0].SubItems[4].Text = std.PhoneNum;
                studentListView.SelectedItems[0].SubItems[5].Text = std.Email;
            }

            //update data in DB
            string strConn = "Server=127.0.0.1;Port=3306;Database=academyManagement;Uid=root;Pwd=012520";
            MySqlConnection conn = new MySqlConnection(strConn);

            conn.Open();
            string sql = string.Format(
                            "UPDATE Student SET name = '{0}', identificationNum = '{1}' " +
                            ", address = '{2}', phoneNum = '{3}' " +
                            ", email = '{4}' WHERE id = '{5}'",
                            std.Name, std.IdentificationNum, std.Address, std.PhoneNum, std.Email, std.Id);

            MySqlCommand cmd = new MySqlCommand(sql, conn);

            cmd.ExecuteNonQuery();
            conn.Close();

            return studentListView;
        }

        public ListView deleteStudent(ListView stdListView)
        {
            if (stdListView.SelectedItems.Count > 0)
            {
                stdListView.Items.Remove(stdListView.SelectedItems[0]);
            }
            return stdListView;
        }

        public void readDB(ListView studentListView)
        {
            string strConn = "Server=127.0.0.1;Port=3306;Database=academyManagement;Uid=root;Pwd=012520";
            MySqlConnection conn = new MySqlConnection(strConn);

            conn.Open();

            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Student", conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                studentListView.Items.Add(new ListViewItem(
                        new string[] { (string)reader["id"], (string)reader["name"],
                            (string)reader["identificationNum"], (string)reader["address"],
                            (string)reader["phoneNum"], (string)reader["email"]}));
            }
            reader.Close();
            conn.Close();
        }
    }
}
