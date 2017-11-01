using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    class Direction
    {
        public static readonly Point Left = new Point(-1, 0);
        public static readonly Point Down = new Point(0, -1);
        public static readonly Point Rigth = new Point(1, 0);
        public static readonly Point Top = new Point(0, 1);
        public static readonly Point None = new Point(0, 0);
    }
}
