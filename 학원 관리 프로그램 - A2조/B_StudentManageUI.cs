using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 학원_관리_프로그램___A2조
{
    public partial class B_StudentManageUI : Form
    {
        //constructor
        public B_StudentManageUI()
        {
            InitializeComponent();

            //insert data from DB
            C_StudentManageSystem system = new C_StudentManageSystem();
            system.readDB(studentListView);
        }

        //update student
        public void modifyStudentEvent()
        {
            E_Student std = new E_Student();
            std.Id = idText.Text;
            std.Name = nameText.Text;
            std.IdentificationNum = identityText.Text;
            std.Address = addressText.Text;
            std.PhoneNum = phoneText.Text;
            std.Email = emailText.Text;

            C_StudentManageSystem system = new C_StudentManageSystem();
            var tmp = system.modifyStudent(studentListView, std);

            if (tmp != null)
                studentListView = tmp;
        }

        public void deleteStudentEvent()
        {
            C_StudentManageSystem system = new C_StudentManageSystem();
            studentListView = system.deleteStudent(studentListView);
        }

        public void warningIdentificationNumOverlap()
        {
            MessageBox.Show("주민등록번호가 중복됩니다", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        //수정
        private void modifyButton_Click(object sender, EventArgs e)
        {
            modifyStudentEvent();
            formClear();
        }

        //삭제
        private void deleteButton_Click(object sender, EventArgs e)
        {
            deleteStudentEvent();
            formClear();
        }

        public void formClear()
        {
            idText.Clear(); nameText.Clear();
            identityText.Clear(); addressText.Clear();
            phoneText.Clear(); emailText.Clear();
            nameText.Focus();
        }

        private void studentListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (studentListView.SelectedItems.Count > 0)
            {
                idText.Text = studentListView.SelectedItems[0].SubItems[0].Text;
                nameText.Text = studentListView.SelectedItems[0].SubItems[1].Text;
                identityText.Text = studentListView.SelectedItems[0].SubItems[2].Text;
                addressText.Text = studentListView.SelectedItems[0].SubItems[3].Text;
                phoneText.Text = studentListView.SelectedItems[0].SubItems[4].Text;
                emailText.Text = studentListView.SelectedItems[0].SubItems[5].Text;
            }
        }
    }
}
