using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pensioner
{
    internal class GlobalData
    {
        public static int CurrentNumber { get; set; }
        public static string TextForChoice { get; set; }
        public static int PointsOfHappiness { get; set; } = 70;
        public static int HomeRoom { get; set; } = 4;
        public static bool InHome { get; set; } = false;
    }
}
