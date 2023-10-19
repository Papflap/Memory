using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memory3
{
    public class Highscores
    {
        public readonly string TimeString;
        public readonly double TimeDouble;
            
        public Highscores(string timeString, double timeDouble)
        {
            this.TimeString = timeString;
            this.TimeDouble = timeDouble;
        }
    }
}
