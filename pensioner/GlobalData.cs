﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pensioner
{
    internal class GlobalData
    {
        public static int CurrentNumber { get; set; } = 1;
        public static string TextForChoice { get; set; }
        public static int PointsOfHappiness { get; set; } = 70;
        public static int HomeRoom { get; set; } = 4;
        public static string Picture { get; set; } = "Menu";
        public static bool Batery { get; set;} = false;
        public static int FontText { get; set; } = 20;
        public static bool End { get; set;}=false;

    }
    
}
