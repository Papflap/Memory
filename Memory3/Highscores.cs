using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memory3
{
    public class Highscores
    {
        string TimeString;
        double TimeDouble;
            
        public Highscores(string _timeString, double _timeDouble)
        {
            TimeString = _timeString;
            TimeDouble = _timeDouble;
        }
    }
}
