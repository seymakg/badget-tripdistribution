using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger.Algebra.Array
{
    public static class ArrayHelper
    {
        public static double[] GenerateArray(double start, double end, double step)
        {
            if (step == 0)
            {
                throw new NotSupportedException("step cannot be zero");
            }

            if (start > end && step > 0)
            {
                throw new NotSupportedException("step cannot be positive when start bigger than end");
            }
            else if (start < end && step < 0)
            {
                throw new NotSupportedException("step cannot be nagative when end bigger than start");
            }

            var totalValueCount = Math.Floor(Math.Abs(end - start) / Math.Sqrt(step * step)) + 1;

            List<double> betaValues = new List<double>();

            for (int i = 0; i < totalValueCount; i++)
            {
                betaValues.Add(start + (step * i));
            }

            return betaValues.ToArray();
        }

        public static string WriteArray(double[] array, bool vertical)
        {
            string log = string.Empty;

            for (int i = 0; i < array.GetLength(0); i++)
            {
                log += string.Format("{0:0.0000}", array[i]).PadLeft(15, ' ');
                if (vertical)
                {
                    log += Environment.NewLine;
                }
            }

            return log;
        }
    }
}
