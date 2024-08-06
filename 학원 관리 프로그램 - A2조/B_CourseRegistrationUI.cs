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

namespace 학원_관리_프로그램___A2조
{
    public partial class B_CourseRegistrationUI : Form
    {
        int isErrored;
        public static string userId = "user005";
        
        //constructor
        public B_CourseRegistrationUI()
        {
            InitializeComponent();

            //set timetable options
            ImageList imgList = new ImageList();
            imgList.ImageSize = new Size(1, 35);
            timetable.SmallImageList = imgList;

            //insert data from DB
            C_CourseRegistrationSystem system = new C_CourseRegistrationSystem();
            system.readDB(cListView, rListView, userId);

            //create button
            createButtons();
        }

        private void button_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            int pos = b.Location.Y;
            pos = (pos - 170) / 23;
            registerEvent(pos);
        }

        public void registerEvent(int idx)
        {
            C_CourseRegistrationSystem system = new C_CourseRegistrationSystem();
            isErrored = system.register(cListView.Items[idx], rListView, timetable);

            if (isErrored == 0)
                warningCourseOverlap();
            else if (isErrored == 1)
                warningTimeOverlap();
        }

        public void showRegisteredCoursesUI(ListView rLV)
        {
            rListView = rLV;
        }

        public void warningTimeOverlap()
        {
            MessageBox.Show("수업 시간이 중복됩니다.", "경고 메시지", MessageBoxButtons.OK);
        }

        public void warningCourseOverlap()
        {
            MessageBox.Show("이미 신청한 과목입니다.", "경고 메시지", MessageBoxButtons.OK);
        }

        //버튼 생성
        public void createButtons()
        {
            Button[] cbtn = new Button[cListView.Items.Count];
            int btnY = 170;
            for (int i = 0; i < cbtn.Length; i++)
            {
                cbtn[i] = new Button();
                cbtn[i].Text = "신청";
                cbtn[i].Size = new Size(48, 20);
                cbtn[i].Location = new Point(703, btnY);
                btnY += 23;
                cbtn[i].Click += button_Click;

                Controls.Add(cbtn[i]);
            }

            Button[] rbtn = new Button[rListView.Items.Count];
            btnY = 632;
            for (int i = 0; i < rbtn.Length; i++)
            {
                rbtn[i] = new Button();
                rbtn[i].Text = "삭제";
                rbtn[i].Size = new Size(48, 20);
                rbtn[i].Location = new Point(703, btnY);
                btnY += 23;
                rbtn[i].Click += button_Click;

                Controls.Add(rbtn[i]);
            }

            rListView.SendToBack();
            cListView.SendToBack();
        }
    }
}

