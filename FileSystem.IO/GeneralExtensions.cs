using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class GeneralExtensions
{
    public static string ToStringOrEmpty(this object value)
    {
        if (value == null)
        {
            return string.Empty;
        }

        return value.ToString();
    }

    public static int ToInt(this object value)
    {
        if (value == null)
        {
            return 0;
        }

        int result = 0;
        int.TryParse(value.ToStringOrEmpty(), out result);

        return result;
    }

    public static string[,] ToStringMatrix(this object[,] data)
    {
        string[,] strings = new string[data.GetLength(0), data.GetLength(1)];

        for (int i = 0; i < data.GetLength(0); i++)
        {
            for (int j = 0; j < data.GetLength(1); j++)
            {
                strings[i, j] = data[i, j].ToStringOrEmpty();
            }
        }

        return strings;
    }

    public static string[,] ToStringMatrix(this double[,] data)
    {
        string[,] strings = new string[data.GetLength(0), data.GetLength(1)];

        for (int i = 0; i < data.GetLength(0); i++)
        {
            for (int j = 0; j < data.GetLength(1); j++)
            {
                strings[i, j] = data[i, j].ToStringOrEmpty();
            }
        }

        return strings;
    }

    public static string[] ToStringArray(this double[] data)
    {
        string[] strings = new string[data.Length];

        for (int i = 0; i < data.Length; i++)
        {
            strings[i] = data[i].ToStringOrEmpty();
        }

        return strings;
    }
}