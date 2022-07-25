using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TapeCalc
{
    public static class Globals
    {
        public static bool DocumentLoaded { get; set; } = false;
        public static string DocumentName { get; set; } = "Untitled.tape";
        public static string DocumentPath { get; set; } = "";
    }
}
