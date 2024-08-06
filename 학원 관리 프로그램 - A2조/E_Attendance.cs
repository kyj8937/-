using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 학원_관리_프로그램___A2조
{
    internal class E_Attendance
    {
        DateTime date;
        string name;
        string id;
        bool attend;

        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public bool Attend { get; set; }
    }
}
