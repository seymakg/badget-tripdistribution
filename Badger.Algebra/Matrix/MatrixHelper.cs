using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger.Algebra.Matrix
{
    public static class MatrixHelper
    {
        public static double Max(double[,] matrix)
        {
            double max = -99999.0;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (max < matrix[i, j])
                    {
                        max = matrix[i, j];
                    }
                }
            }

            return max;
        }

        public static double[] Sum(double[,] matrix, int dimension = 0)
        {
            double[] sum = new double[matrix.GetLength(1)];
            int rowDimension = dimension;
            int columnDimension = rowDimension == 1 ? 0 : 1;

            for (int i = 0; i < matrix.GetLength(rowDimension); i++)
            {
                double total = 0;
                for (int j = 0; j < matrix.GetLength(columnDimension); j++)
                {
                    if (dimension == 0)
                    {
                        total += matrix[i, j];
                    }
                    else
                    {
                        total += matrix[j, i];
                    }

                }

                sum[i] = total;
            }

            return sum;
        }

        public static string WriteMatrix(double[,] matrix)
        {
            string log = string.Empty;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    log += string.Format("{0:0.0000}", matrix[i, j]).PadLeft(15, ' ');
                }
                log +=Environment.NewLine;
            }

            return log;
        }

        public static double[] Ones(int m)
        {
            double[] onesArray = new double[m];
            for (int i = 0; i < m; i++)
            {
                onesArray[i] = 1;
            }

            return onesArray;
        }

        public static double[,] Ones(int m, int n)
        {
            double[,] onesArray = new double[m, n];
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    onesArray[i, j] = 1;

                }
            }

            return onesArray;
        }

        public static double[,] GetSubset(double[,] matrix, Tuple<int, int> start, Tuple<int, int> count)
        {

            double[,] temp = new double[count.Item1, count.Item2];
            for (int i = start.Item1; i < (start.Item1 + count.Item1); i++)
            {
                for (int j = start.Item2; j < (start.Item2 + count.Item2); j++)
                {
                    temp[i - start.Item1, j - start.Item2] = matrix[i, j];
                }
            }

            return temp;
        }

        public static double[] GetSubsetColumn(double[,] matrix, int columnIndex)
        {
            double[] temp = new double[matrix.GetLength(0)];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                temp[i] = matrix[i, columnIndex];
            }

            return temp;
        }

        public static double[,] InsertSubset(double[,] matrix, double[,] subset, Tuple<int, int> start)
        {
            for (int i = start.Item1; i < (start.Item1 + subset.GetLength(0)); i++)
            {
                for (int j = start.Item2; j < (start.Item2 + subset.GetLength(1)); j++)
                {
                    matrix[i, j] = subset[i - start.Item1, j - start.Item2];
                }
            }

            return matrix;
        }
    }
}
