using Badger.Algebra.Matrix;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Badger.Win.Temp
{
    internal static class ObjectiveFunction
    {
        public static string CreateFunctionText(int zoneCount, double[,] costs, double[] tripRowSums, double[] tripColumnSums, int partNumber)
        {
            StringBuilder stringBuilder = new StringBuilder();

            double[,] coef = MatrixHelper.Ones(zoneCount, zoneCount);
            for (int i = 0; i < coef.GetLength(0); i++)
            {
                for (int j = 0; j < coef.GetLength(1); j++)
                {
                    if (i == j)
                    {
                        coef[i, j] = 2.0;
                    }
                    
                }
            }

            int border = zoneCount * zoneCount;
            int border2 = 2 * border;

            int startPoint = partNumber * zoneCount;
            int endPoint = startPoint + zoneCount;

            int constraintCount = border2 + (2 * zoneCount);
            for (int constraintIndex = 0; constraintIndex < constraintCount; constraintIndex++)
            {
                if (constraintIndex >= startPoint && constraintIndex < endPoint)
                {
                    if (constraintIndex < border2)
                    {
                        if (constraintIndex < border)
                        {
                            stringBuilder.Append($"Math.Pow(");
                            int ai = 0;
                            stringBuilder.Append($"x[{ai}]");

                            for (int coj = 0; coj < zoneCount; coj++)
                            {
                                int coi = constraintIndex % zoneCount;
                                int bi = 1 + coj;
                                int qij = bi + zoneCount;

                                stringBuilder.Append($" - {coef[coi, coj]} * x[{bi}] * x[{qij}]");
                            }

                            int ci = -1;
                            int cj = -1;
                            if (constraintIndex < border)
                            {
                                ci = constraintIndex % zoneCount;
                                cj = (partNumber % zoneCount);
                            }
                            else
                            {
                                int value = int.Parse(Math.Floor(double.Parse((constraintIndex / zoneCount).ToString())).ToString());
                                ci = (value % zoneCount);
                                cj = constraintIndex % zoneCount;
                            }

                            stringBuilder.Append($" - {costs[ci, cj]}");
                            stringBuilder.Append($", 2)");
                            if (constraintIndex + 1 != constraintCount && constraintIndex + 1 != endPoint)
                            {
                                stringBuilder.Append(" + ");
                            }
                            stringBuilder.Append(Environment.NewLine);
                        }
                    }
                    else if (constraintIndex < (border2 + zoneCount))
                    {
                        stringBuilder.Append($"Math.Pow(");
                        for (int coj = 0; coj < zoneCount; coj++)
                        {
                            if (coj != 0)
                            {
                                stringBuilder.Append(" + ");
                            }
                            int qij = FindQijIndex(constraintIndex, zoneCount, constraintCount, coj);
                            stringBuilder.Append($"x[{qij}]");
                        }

                        stringBuilder.Append($" - {tripRowSums[(constraintIndex % zoneCount)]}");
                        stringBuilder.Append($", 2)");
                        if (constraintIndex + 1 != constraintCount && constraintIndex + 1 != endPoint)
                        {
                            stringBuilder.Append(" + ");
                        }
                        stringBuilder.Append(Environment.NewLine);
                    }
                    else
                    {
                        stringBuilder.Append($"Math.Pow(");
                        for (int coj = 0; coj < zoneCount; coj++)
                        {
                            if (coj != 0)
                            {
                                stringBuilder.Append(" + ");
                            }

                            int qij = FindQijIndex(constraintIndex, zoneCount, constraintCount, coj);
                            stringBuilder.Append($"x[{qij}]");
                        }

                        stringBuilder.Append($" - {tripColumnSums[(constraintIndex % zoneCount)]}");
                        stringBuilder.Append($", 2)");
                        if (constraintIndex + 1 != constraintCount && constraintIndex + 1 != endPoint)
                        {
                            stringBuilder.Append(" + ");
                        }
                        stringBuilder.Append(Environment.NewLine);
                    }
                }
            }

            var constraints = stringBuilder.ToString();

            return $"Math.Sqrt({constraints})";
        }

        private static int FindQijIndex(int constraintIndex, int zoneCount, int constraintCount, int coefIndex) {
            int border = zoneCount * zoneCount;
            int border2 = 2 * border;

            if (constraintIndex >= 0 && constraintIndex < border)
            {
                int i = FindMatrixRowIndex(constraintIndex, zoneCount, constraintCount, coefIndex);
                int j = FindMatrixColumnIndex(constraintIndex, zoneCount, constraintCount, coefIndex);
                return (2 * zoneCount) + (zoneCount * i) + j;
            }
            else if (constraintIndex >= border && constraintIndex < border2)
            {
                int i = FindMatrixRowIndex(constraintIndex, zoneCount, constraintCount, coefIndex);
                int j = FindMatrixColumnIndex(constraintIndex, zoneCount, constraintCount, coefIndex);
                return (2 * zoneCount) + (zoneCount * i) + j;
            }
            else if (constraintIndex >= border2 && constraintIndex < (border2 + zoneCount))
            {
                int i = FindMatrixRowIndex(constraintIndex, zoneCount, constraintCount, coefIndex);
                int j = FindMatrixColumnIndex(constraintIndex, zoneCount, constraintCount, coefIndex);
                return (2 * zoneCount) + (zoneCount * i) + coefIndex;
            }
            else
            {
                int i = FindMatrixRowIndex(constraintIndex, zoneCount, constraintCount, coefIndex);
                int j = FindMatrixColumnIndex(constraintIndex, zoneCount, constraintCount, coefIndex);
                return (2 * zoneCount) + (zoneCount * coefIndex) + (constraintIndex % zoneCount);
            }
        }

        private static int FindMatrixColumnIndex(int constraintIndex, int zoneCount, int constraintCount, int coefIndex)
        {
            int border = zoneCount * zoneCount;
            int border2 = 2 * border;

            if (constraintIndex >= 0 && constraintIndex < border)
            {
                int value = int.Parse(Math.Floor(double.Parse((constraintIndex / zoneCount).ToString())).ToString());
                return value;
            }
            else if (constraintIndex >= border && constraintIndex < border2)
            {
                return coefIndex;
            }
            else
            {
                return -1;
            }
        }

        private static int FindMatrixRowIndex(int constraintIndex, int zoneCount, int constraintCount, int coefIndex)
        {
            int border = zoneCount * zoneCount;
            int border2 = 2 * border;

            if (constraintIndex >= 0 && constraintIndex < border)
            {
                return coefIndex;
            }
            else if (constraintIndex >= border && constraintIndex < border2)
            {
                int value = int.Parse(Math.Floor(double.Parse((constraintIndex / zoneCount).ToString())).ToString());
                return (value % zoneCount);
            }
            else
            {
                return (constraintIndex % zoneCount);
            }
        }

        private static int FindArrayAIndex(int constraintIndex, int zoneCount, int constraintCount)
        {
            int border = zoneCount * zoneCount;
            int border2 = 2 * border;

            if (constraintIndex >= 0 && constraintIndex < border)
            {
                int value = int.Parse(Math.Floor(double.Parse((constraintIndex / zoneCount).ToString())).ToString());
                return value;
            }
            else if (constraintIndex >= border && constraintIndex < border2)
            {
                return constraintIndex % zoneCount;
            }
            else
            {
                return -1;
            }
        }

        private static int FindCoefColumnIndex(int constraintIndex, int zoneCount, int constraintCount)
        {
            int value = int.Parse(Math.Floor(double.Parse((constraintIndex / zoneCount).ToString())).ToString());
            return value;
        }

        private static int FindArrayBIndex(int constraintIndex, int zoneCount, int constraintCount, int coefIndex)
        {
            int border = zoneCount * zoneCount;
            int border2 = 2 * border;

            if (constraintIndex >= 0 && constraintIndex < border)
            {
                return zoneCount + coefIndex;
            }
            else if (constraintIndex >= border && constraintIndex < border2)
            {
                int value = int.Parse(Math.Floor(double.Parse((constraintIndex / zoneCount).ToString())).ToString());
                return zoneCount + (value % zoneCount);
            }
            else
            {
                return -1;
            }

        }


        public static string ExpressionText = @"Math.Sqrt(
                Math.Pow(x[0] - 2 * x[5] * x[10] - x[6] * x[15] - x[7] * x[20] - x[8] * x[25] - x[9] * x[30] - 0, 2) +
                Math.Pow(x[0] - x[5] * x[10] - 2 * x[6] * x[15] - x[7] * x[20] - x[8] * x[25] - x[9] * x[30] - 1.99, 2) +
                Math.Pow(x[0] - x[5] * x[10] - x[6] * x[15] - 2 * x[7] * x[20] - x[8] * x[25] - x[9] * x[30] - 3.75, 2) +
                Math.Pow(x[0] - x[5] * x[10] - x[6] * x[15] - x[7] * x[20] - 2 * x[8] * x[25] - x[9] * x[30] - 1.57, 2) +
                Math.Pow(x[0] - x[5] * x[10] - x[6] * x[15] - x[7] * x[20] - x[8] * x[25] - 2 * x[9] * x[30] - 0.65, 2) +
                Math.Pow(x[1] - 2 * x[5] * x[11] - x[6] * x[16] - x[7] * x[21] - x[8] * x[26] - x[9] * x[31] - 1.86, 2) +
                Math.Pow(x[1] - x[5] * x[11] - 2 * x[6] * x[16] - x[7] * x[21] - x[8] * x[26] - x[9] * x[31] - 0, 2) +
                Math.Pow(x[1] - x[5] * x[11] - x[6] * x[16] - 2 * x[7] * x[21] - x[8] * x[26] - x[9] * x[31] - 6.09, 2) +
                Math.Pow(x[1] - x[5] * x[11] - x[6] * x[16] - x[7] * x[21] - 2 * x[8] * x[26] - x[9] * x[31] - 1.74, 2) +
                Math.Pow(x[1] - x[5] * x[11] - x[6] * x[16] - x[7] * x[21] - x[8] * x[26] - 2 * x[9] * x[31] - 3.63, 2) +
                Math.Pow(x[2] - 2 * x[5] * x[12] - x[6] * x[17] - x[7] * x[22] - x[8] * x[27] - x[9] * x[32] - 3.81, 2) +
                Math.Pow(x[2] - x[5] * x[12] - 2 * x[6] * x[17] - x[7] * x[22] - x[8] * x[27] - x[9] * x[32] - 6.56, 2) +
                Math.Pow(x[2] - x[5] * x[12] - x[6] * x[17] - 2 * x[7] * x[22] - x[8] * x[27] - x[9] * x[32] - 0, 2) +
                Math.Pow(x[2] - x[5] * x[12] - x[6] * x[17] - x[7] * x[22] - 2 * x[8] * x[27] - x[9] * x[32] - 5.37, 2) +
                Math.Pow(x[2] - x[5] * x[12] - x[6] * x[17] - x[7] * x[22] - x[8] * x[27] - 2 * x[9] * x[32] - 3.44, 2) +
                Math.Pow(x[3] - 2 * x[5] * x[13] - x[6] * x[18] - x[7] * x[23] - x[8] * x[28] - x[9] * x[33] - 1.52, 2) +
                Math.Pow(x[3] - x[5] * x[13] - 2 * x[6] * x[18] - x[7] * x[23] - x[8] * x[28] - x[9] * x[33] - 1.31, 2) +
                Math.Pow(x[3] - x[5] * x[13] - x[6] * x[18] - 2 * x[7] * x[23] - x[8] * x[28] - x[9] * x[33] - 5.09, 2) +
                Math.Pow(x[3] - x[5] * x[13] - x[6] * x[18] - x[7] * x[23] - 2 * x[8] * x[28] - x[9] * x[33] - 0, 2) +
                Math.Pow(x[3] - x[5] * x[13] - x[6] * x[18] - x[7] * x[23] - x[8] * x[28] - 2 * x[9] * x[33] - 2, 2) +
                Math.Pow(x[4] - 2 * x[5] * x[14] - x[6] * x[19] - x[7] * x[24] - x[8] * x[29] - x[9] * x[34] - 0.65, 2) +
                Math.Pow(x[4] - x[5] * x[14] - 2 * x[6] * x[19] - x[7] * x[24] - x[8] * x[29] - x[9] * x[34] - 3.59, 2) +
                Math.Pow(x[4] - x[5] * x[14] - x[6] * x[19] - 2 * x[7] * x[24] - x[8] * x[29] - x[9] * x[34] - 3.26, 2) +
                Math.Pow(x[4] - x[5] * x[14] - x[6] * x[19] - x[7] * x[24] - 2 * x[8] * x[29] - x[9] * x[34] - 1.73, 2) +
                Math.Pow(x[4] - x[5] * x[14] - x[6] * x[19] - x[7] * x[24] - x[8] * x[29] - 2 * x[9] * x[34] - 0, 2) +
                Math.Pow(x[0] - 2 * x[5] * x[10] - x[5] * x[11] - x[5] * x[12] - x[5] * x[13] - x[5] * x[14] - 0, 2) +
                Math.Pow(x[1] - x[5] * x[10] - 2 * x[5] * x[11] - x[5] * x[12] - x[5] * x[13] - x[5] * x[14] - 1.86, 2) +
                Math.Pow(x[2] - x[5] * x[10] - x[5] * x[11] - 2 * x[5] * x[12] - x[5] * x[13] - x[5] * x[14] - 3.81, 2) +
                Math.Pow(x[3] - x[5] * x[10] - x[5] * x[11] - x[5] * x[12] - 2 * x[5] * x[13] - x[5] * x[14] - 1.52, 2) +
                Math.Pow(x[4] - x[5] * x[10] - x[5] * x[11] - x[5] * x[12] - x[5] * x[13] - 2 * x[5] * x[14] - 0.65, 2) +
                Math.Pow(x[0] - 2 * x[6] * x[15] - x[6] * x[16] - x[6] * x[17] - x[6] * x[18] - x[6] * x[19] - 1.99, 2) +
                Math.Pow(x[1] - x[6] * x[15] - 2 * x[6] * x[16] - x[6] * x[17] - x[6] * x[18] - x[6] * x[19] - 0, 2) +
                Math.Pow(x[2] - x[6] * x[15] - x[6] * x[16] - 2 * x[6] * x[17] - x[6] * x[18] - x[6] * x[19] - 6.56, 2) +
                Math.Pow(x[3] - x[6] * x[15] - x[6] * x[16] - x[6] * x[17] - 2 * x[6] * x[18] - x[6] * x[19] - 1.31, 2) +
                Math.Pow(x[4] - x[6] * x[15] - x[6] * x[16] - x[6] * x[17] - x[6] * x[18] - 2 * x[6] * x[19] - 3.59, 2) +
                Math.Pow(x[0] - 2 * x[7] * x[20] - x[7] * x[21] - x[7] * x[22] - x[7] * x[23] - x[7] * x[24] - 3.75, 2) +
                Math.Pow(x[1] - x[7] * x[20] - 2 * x[7] * x[21] - x[7] * x[22] - x[7] * x[23] - x[7] * x[24] - 6.09, 2) +
                Math.Pow(x[2] - x[7] * x[20] - x[7] * x[21] - 2 * x[7] * x[22] - x[7] * x[23] - x[7] * x[24] - 0, 2) +
                Math.Pow(x[3] - x[7] * x[20] - x[7] * x[21] - x[7] * x[22] - 2 * x[7] * x[23] - x[7] * x[24] - 5.09, 2) +
                Math.Pow(x[4] - x[7] * x[20] - x[7] * x[21] - x[7] * x[22] - x[7] * x[23] - 2 * x[7] * x[24] - 3.26, 2) +
                Math.Pow(x[0] - 2 * x[8] * x[25] - x[8] * x[26] - x[8] * x[27] - x[8] * x[28] - x[8] * x[29] - 1.57, 2) +
                Math.Pow(x[1] - x[8] * x[25] - 2 * x[8] * x[26] - x[8] * x[27] - x[8] * x[28] - x[8] * x[29] - 1.74, 2) +
                Math.Pow(x[2] - x[8] * x[25] - x[8] * x[26] - 2 * x[8] * x[27] - x[8] * x[28] - x[8] * x[29] - 5.37, 2) +
                Math.Pow(x[3] - x[8] * x[25] - x[8] * x[26] - x[8] * x[27] - 2 * x[8] * x[28] - x[8] * x[29] - 0, 2) +
                Math.Pow(x[4] - x[8] * x[25] - x[8] * x[26] - x[8] * x[27] - x[8] * x[28] - 2 * x[8] * x[29] - 1.73, 2) +
                Math.Pow(x[0] - 2 * x[9] * x[30] - x[9] * x[31] - x[9] * x[32] - x[9] * x[33] - x[9] * x[34] - 0.65, 2) +
                Math.Pow(x[1] - x[9] * x[30] - 2 * x[9] * x[31] - x[9] * x[32] - x[9] * x[33] - x[9] * x[34] - 3.63, 2) +
                Math.Pow(x[2] - x[9] * x[30] - x[9] * x[31] - 2 * x[9] * x[32] - x[9] * x[33] - x[9] * x[34] - 3.44, 2) +
                Math.Pow(x[3] - x[9] * x[30] - x[9] * x[31] - x[9] * x[32] - 2 * x[9] * x[33] - x[9] * x[34] - 2, 2) +
                Math.Pow(x[4] - x[9] * x[30] - x[9] * x[31] - x[9] * x[32] - x[9] * x[33] - 2 * x[9] * x[34] - 0, 2) +
                Math.Pow(x[10] + x[11] + x[12] + x[13] + x[14] - 102, 2) +
                Math.Pow(x[10] + x[15] + x[20] + x[25] + x[30] - 138, 2) +
                Math.Pow(x[15] + x[16] + x[17] + x[18] + x[19] - 88, 2) +
                Math.Pow(x[11] + x[16] + x[21] + x[26] + x[31] - 112, 2) +
                Math.Pow(x[20] + x[21] + x[22] + x[23] + x[24] - 279, 2) +
                Math.Pow(x[12] + x[17] + x[22] + x[27] + x[32] - 287, 2) +
                Math.Pow(x[25] + x[26] + x[27] + x[28] + x[29] - 226, 2) +
                Math.Pow(x[13] + x[18] + x[23] + x[28] + x[33] - 190, 2) +
                Math.Pow(x[30] + x[31] + x[32] + x[33] + x[34] - 331, 2) +
                Math.Pow(x[14] + x[19] + x[24] + x[29] + x[34] - 299, 2)
            )";

        public static double Function(double[] x)
        {
            return Math.Sqrt(
                Math.Pow(x[0] - 2 * x[5] * x[10] - x[6] * x[15] - x[7] * x[20] - x[8] * x[25] - x[9] * x[30] - 0, 2) +
                Math.Pow(x[0] - x[5] * x[10] - 2 * x[6] * x[15] - x[7] * x[20] - x[8] * x[25] - x[9] * x[30] - 1.99, 2) +
                Math.Pow(x[0] - x[5] * x[10] - x[6] * x[15] - 2 * x[7] * x[20] - x[8] * x[25] - x[9] * x[30] - 3.75, 2) +
                Math.Pow(x[0] - x[5] * x[10] - x[6] * x[15] - x[7] * x[20] - 2 * x[8] * x[25] - x[9] * x[30] - 1.57, 2) +
                Math.Pow(x[0] - x[5] * x[10] - x[6] * x[15] - x[7] * x[20] - x[8] * x[25] - 2 * x[9] * x[30] - 0.65, 2) +
                Math.Pow(x[1] - 2 * x[5] * x[11] - x[6] * x[16] - x[7] * x[21] - x[8] * x[26] - x[9] * x[31] - 1.86, 2) +
                Math.Pow(x[1] - x[5] * x[11] - 2 * x[6] * x[16] - x[7] * x[21] - x[8] * x[26] - x[9] * x[31] - 0, 2) +
                Math.Pow(x[1] - x[5] * x[11] - x[6] * x[16] - 2 * x[7] * x[21] - x[8] * x[26] - x[9] * x[31] - 6.09, 2) +
                Math.Pow(x[1] - x[5] * x[11] - x[6] * x[16] - x[7] * x[21] - 2 * x[8] * x[26] - x[9] * x[31] - 1.74, 2) +
                Math.Pow(x[1] - x[5] * x[11] - x[6] * x[16] - x[7] * x[21] - x[8] * x[26] - 2 * x[9] * x[31] - 3.63, 2) +
                Math.Pow(x[2] - 2 * x[5] * x[12] - x[6] * x[17] - x[7] * x[22] - x[8] * x[27] - x[9] * x[32] - 3.81, 2) +
                Math.Pow(x[2] - x[5] * x[12] - 2 * x[6] * x[17] - x[7] * x[22] - x[8] * x[27] - x[9] * x[32] - 6.56, 2) +
                Math.Pow(x[2] - x[5] * x[12] - x[6] * x[17] - 2 * x[7] * x[22] - x[8] * x[27] - x[9] * x[32] - 0, 2) +
                Math.Pow(x[2] - x[5] * x[12] - x[6] * x[17] - x[7] * x[22] - 2 * x[8] * x[27] - x[9] * x[32] - 5.37, 2) +
                Math.Pow(x[2] - x[5] * x[12] - x[6] * x[17] - x[7] * x[22] - x[8] * x[27] - 2 * x[9] * x[32] - 3.44, 2) +
                Math.Pow(x[3] - 2 * x[5] * x[13] - x[6] * x[18] - x[7] * x[23] - x[8] * x[28] - x[9] * x[33] - 1.52, 2) +
                Math.Pow(x[3] - x[5] * x[13] - 2 * x[6] * x[18] - x[7] * x[23] - x[8] * x[28] - x[9] * x[33] - 1.31, 2) +
                Math.Pow(x[3] - x[5] * x[13] - x[6] * x[18] - 2 * x[7] * x[23] - x[8] * x[28] - x[9] * x[33] - 5.09, 2) +
                Math.Pow(x[3] - x[5] * x[13] - x[6] * x[18] - x[7] * x[23] - 2 * x[8] * x[28] - x[9] * x[33] - 0, 2) +
                Math.Pow(x[3] - x[5] * x[13] - x[6] * x[18] - x[7] * x[23] - x[8] * x[28] - 2 * x[9] * x[33] - 2, 2) +
                Math.Pow(x[4] - 2 * x[5] * x[14] - x[6] * x[19] - x[7] * x[24] - x[8] * x[29] - x[9] * x[34] - 0.65, 2) +
                Math.Pow(x[4] - x[5] * x[14] - 2 * x[6] * x[19] - x[7] * x[24] - x[8] * x[29] - x[9] * x[34] - 3.59, 2) +
                Math.Pow(x[4] - x[5] * x[14] - x[6] * x[19] - 2 * x[7] * x[24] - x[8] * x[29] - x[9] * x[34] - 3.26, 2) +
                Math.Pow(x[4] - x[5] * x[14] - x[6] * x[19] - x[7] * x[24] - 2 * x[8] * x[29] - x[9] * x[34] - 1.73, 2) +
                Math.Pow(x[4] - x[5] * x[14] - x[6] * x[19] - x[7] * x[24] - x[8] * x[29] - 2 * x[9] * x[34] - 0, 2) +
                Math.Pow(x[0] - 2 * x[5] * x[10] - x[5] * x[11] - x[5] * x[12] - x[5] * x[13] - x[5] * x[14] - 0, 2) +
                Math.Pow(x[1] - x[5] * x[10] - 2 * x[5] * x[11] - x[5] * x[12] - x[5] * x[13] - x[5] * x[14] - 1.86, 2) +
                Math.Pow(x[2] - x[5] * x[10] - x[5] * x[11] - 2 * x[5] * x[12] - x[5] * x[13] - x[5] * x[14] - 3.81, 2) +
                Math.Pow(x[3] - x[5] * x[10] - x[5] * x[11] - x[5] * x[12] - 2 * x[5] * x[13] - x[5] * x[14] - 1.52, 2) +
                Math.Pow(x[4] - x[5] * x[10] - x[5] * x[11] - x[5] * x[12] - x[5] * x[13] - 2 * x[5] * x[14] - 0.65, 2) +
                Math.Pow(x[0] - 2 * x[6] * x[15] - x[6] * x[16] - x[6] * x[17] - x[6] * x[18] - x[6] * x[19] - 1.99, 2) +
                Math.Pow(x[1] - x[6] * x[15] - 2 * x[6] * x[16] - x[6] * x[17] - x[6] * x[18] - x[6] * x[19] - 0, 2) +
                Math.Pow(x[2] - x[6] * x[15] - x[6] * x[16] - 2 * x[6] * x[17] - x[6] * x[18] - x[6] * x[19] - 6.56, 2) +
                Math.Pow(x[3] - x[6] * x[15] - x[6] * x[16] - x[6] * x[17] - 2 * x[6] * x[18] - x[6] * x[19] - 1.31, 2) +
                Math.Pow(x[4] - x[6] * x[15] - x[6] * x[16] - x[6] * x[17] - x[6] * x[18] - 2 * x[6] * x[19] - 3.59, 2) +
                Math.Pow(x[0] - 2 * x[7] * x[20] - x[7] * x[21] - x[7] * x[22] - x[7] * x[23] - x[7] * x[24] - 3.75, 2) +
                Math.Pow(x[1] - x[7] * x[20] - 2 * x[7] * x[21] - x[7] * x[22] - x[7] * x[23] - x[7] * x[24] - 6.09, 2) +
                Math.Pow(x[2] - x[7] * x[20] - x[7] * x[21] - 2 * x[7] * x[22] - x[7] * x[23] - x[7] * x[24] - 0, 2) +
                Math.Pow(x[3] - x[7] * x[20] - x[7] * x[21] - x[7] * x[22] - 2 * x[7] * x[23] - x[7] * x[24] - 5.09, 2) +
                Math.Pow(x[4] - x[7] * x[20] - x[7] * x[21] - x[7] * x[22] - x[7] * x[23] - 2 * x[7] * x[24] - 3.26, 2) +
                Math.Pow(x[0] - 2 * x[8] * x[25] - x[8] * x[26] - x[8] * x[27] - x[8] * x[28] - x[8] * x[29] - 1.57, 2) +
                Math.Pow(x[1] - x[8] * x[25] - 2 * x[8] * x[26] - x[8] * x[27] - x[8] * x[28] - x[8] * x[29] - 1.74, 2) +
                Math.Pow(x[2] - x[8] * x[25] - x[8] * x[26] - 2 * x[8] * x[27] - x[8] * x[28] - x[8] * x[29] - 5.37, 2) +
                Math.Pow(x[3] - x[8] * x[25] - x[8] * x[26] - x[8] * x[27] - 2 * x[8] * x[28] - x[8] * x[29] - 0, 2) +
                Math.Pow(x[4] - x[8] * x[25] - x[8] * x[26] - x[8] * x[27] - x[8] * x[28] - 2 * x[8] * x[29] - 1.73, 2) +
                Math.Pow(x[0] - 2 * x[9] * x[30] - x[9] * x[31] - x[9] * x[32] - x[9] * x[33] - x[9] * x[34] - 0.65, 2) +
                Math.Pow(x[1] - x[9] * x[30] - 2 * x[9] * x[31] - x[9] * x[32] - x[9] * x[33] - x[9] * x[34] - 3.63, 2) +
                Math.Pow(x[2] - x[9] * x[30] - x[9] * x[31] - 2 * x[9] * x[32] - x[9] * x[33] - x[9] * x[34] - 3.44, 2) +
                Math.Pow(x[3] - x[9] * x[30] - x[9] * x[31] - x[9] * x[32] - 2 * x[9] * x[33] - x[9] * x[34] - 2, 2) +
                Math.Pow(x[4] - x[9] * x[30] - x[9] * x[31] - x[9] * x[32] - x[9] * x[33] - 2 * x[9] * x[34] - 0, 2) +
                Math.Pow(x[10] + x[11] + x[12] + x[13] + x[14] - 102, 2) +
                Math.Pow(x[10] + x[15] + x[20] + x[25] + x[30] - 138, 2) +
                Math.Pow(x[15] + x[16] + x[17] + x[18] + x[19] - 88, 2) +
                Math.Pow(x[11] + x[16] + x[21] + x[26] + x[31] - 112, 2) +
                Math.Pow(x[20] + x[21] + x[22] + x[23] + x[24] - 279, 2) +
                Math.Pow(x[12] + x[17] + x[22] + x[27] + x[32] - 287, 2) +
                Math.Pow(x[25] + x[26] + x[27] + x[28] + x[29] - 226, 2) +
                Math.Pow(x[13] + x[18] + x[23] + x[28] + x[33] - 190, 2) +
                Math.Pow(x[30] + x[31] + x[32] + x[33] + x[34] - 331, 2) +
                Math.Pow(x[14] + x[19] + x[24] + x[29] + x[34] - 299, 2)
            );
        }
    }
}
