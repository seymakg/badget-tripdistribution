using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

public static class ExcelExtensions
{
    public static Range GetVerticalLastCellRange(this Range range, int zoneCount)
    {
        Range lastCell = range.Offset[zoneCount, 0];
        return lastCell;
    }

    public static Range GetHorizontalLastCellRange(this Range range, int zoneCount)
    {
        Range lastCell = range.Offset[0, zoneCount];
        return lastCell;
    }
    
    public static T[] GetRow<T>(this T[,] array, int row)
    {
        if (!typeof(T).IsPrimitive)
            throw new InvalidOperationException("Not supported for managed types.");

        if (array == null)
            throw new ArgumentNullException("array");

        int cols = array.GetUpperBound(1) + 1;
        T[] result = new T[cols];
        int size = Marshal.SizeOf<T>();

        Buffer.BlockCopy(array, row * cols * size, result, 0, cols * size);

        return result;
    }

    public static T[] GetColumn<T>(this T[,] array, int column)
    {
        if (!typeof(T).IsPrimitive)
            throw new InvalidOperationException("Not supported for managed types.");

        if (array == null)
            throw new ArgumentNullException("array");
        
        int cols = array.GetUpperBound(0) + 1;

        T[] result = new T[cols];

        for (int i = 0; i < cols; i++)
        {
            result[i] = array[i, column];
        }

        return result;
    }
}
