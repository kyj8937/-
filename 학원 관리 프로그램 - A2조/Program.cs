using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 학원_관리_프로그램___A2조
{
    internal static class Program
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            //수강 신청
            //Application.Run(new B_CourseRegistrationUI());

            //학원생 정보 수정
            //Application.Run(new B_StudentManageUI());
            
            //출결 정보 수정
            Application.Run(new B_AttendanceManageUI());
        }
    }
}
