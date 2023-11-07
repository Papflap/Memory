using System.Globalization;

namespace Memory3;

public class SemiNumericComparer: IComparer<string>
{
    //returns true if string parameter can be parsed to an integer value
    private static bool IsNumeric(string value)
    {
        return int.TryParse(value, out _);
    }
    
    //check which one of the two parameters is greater
    public int Compare(string s1, string s2)
    {
        //returns based on which parameter is greater
        const int s1GreaterThanS2 = 1;
        const int s2GreaterThanS1 = -1;

        //establish whether parameters can be parsed to integers
        var isNumeric1 = IsNumeric(s1);
        var isNumeric2 = IsNumeric(s2);

        switch (isNumeric1)
        {
            case true when isNumeric2:
            {
                //actually converting to integers
                var i1 = Convert.ToInt32(s1);
                var i2 = Convert.ToInt32(s2);
                
                if (i1 > i2)
                {
                    return s1GreaterThanS2;
                }
                if (i1 < i2)
                {
                    return s2GreaterThanS1;
                }

                return 0;
            }
            case true:
                return s2GreaterThanS1;
        }

        if (isNumeric2)
        {
            return s1GreaterThanS2;
        }

        return string.Compare(s1, s2, true, CultureInfo.InvariantCulture);
    }
}