using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public struct Range
{
    public int Min { get; set; }
    public int Max { get; set; }

    public Range(int min, int max)
    {
        if (Max >= Min)
        {
            Min = min;
            Max = max;
        }
        else
            throw new ArgumentException("The max value must be bigger than the min value");
    }

    public bool IsInRange(int value)
    {
        return (value - Min) >= 0 && (value - Min) <= (Max - Min);
    }

    public int GetNumberOfItems()
    {
        return Max - Min;
    }
}

