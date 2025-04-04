using Badger.Algebra.Array;
using Badger.Algebra.Matrix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger.Solvers.Gravity
{
    public class GravitySolver
    {
        public BetaCapabilities BetaCapabilities { get; set; }

        public GravitySolver()
        {
            BetaCapabilities = new BetaCapabilities();
            BetaCapabilities.MinValue = 0.0;
            BetaCapabilities.MaxValue = 4.0;
            BetaCapabilities.Sensitivity = 0.1;
        }

        public IGravitySolution Solve(double[,] observedTripMatrix, double[,] costMatrix)
        {
            Console.Write("Please Enter an Interval Number for Each Bins: ");
            double bin_int = 0.7;

            Console.Write("Please Enter a Value that the Bins End: ");
            double max_bin = 7;

            int num_bins = (int)Math.Floor((max_bin / bin_int) + 1);

            var b = ArrayHelper.GenerateArray(BetaCapabilities.MinValue, BetaCapabilities.MaxValue, BetaCapabilities.Sensitivity);
            var w = b.Length;

            var obs_trips = observedTripMatrix;

            var t_time = costMatrix;

            var rowDataMatrix = PrepareFlatDictionary(t_time, obs_trips);

            double maxi = MatrixHelper.Max(t_time);

            double[,] OTLD = new double[num_bins, 5];

            var intervals = ArrayHelper.GenerateArray(bin_int, max_bin + bin_int, bin_int);
            for (int i = 0; i < num_bins; i++)
            {
                OTLD[i, 0] = i + 1;
                if (i == 0)
                {
                    OTLD[i, 1] = 0;
                }
                else
                {
                    OTLD[i, 1] = intervals[i - 1];
                }
                OTLD[i, 2] = intervals[i];
            }

            for (int i = 0; i < num_bins; i++)
            {
                double startValue = OTLD[i, 1];
                double endValue = OTLD[i, 2];

                double totalValue = rowDataMatrix.FindAll(x => startValue <= x.Item3 && endValue > x.Item3).Select(x => x.Item4).Sum();
                OTLD[i, 3] = totalValue;
            }

            int iter = 50;
            double conv = 0.01;

            double[] prod_tot = MatrixHelper.Sum(obs_trips, 0);

            double[] attr_tot = MatrixHelper.Sum(obs_trips, 1);

            double[] rmse = new double[w];
            var MTLD00 = new double[num_bins, w];
            var MTLD01 = new double[num_bins, w];

            var m = obs_trips.GetLength(0);
            var n = obs_trips.GetLength(1);

            double[,] A0 = new double[m, n];
            double[,] B0 = new double[m, n];
            double[,] Ai = new double[m, iter];
            double[,] Bj = new double[iter, n];
            double[] A = MatrixHelper.Ones(n);
            double[] B = MatrixHelper.Ones(n);

            //rmse, beta, TLD, MTLD
            List<Tuple<double, double, double[], double[], double[,]>> results = new List<Tuple<double, double, double[], double[], double[,]>>();

            for (int g = 0; g < w; g++)
            {
                for (int k = 0; k < iter; k++)
                {
                    //B hesapla
                    for (int i = 0; i < m; i++)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            var exp = Math.Exp(t_time[i, j] * b[g] * -1);
                            B0[i, j] = A[i] * prod_tot[i] * exp;
                        }
                    }

                    B = new double[B0.GetLength(1)];
                    for (int i = 0; i < B0.GetLength(0); i++)
                    {
                        B[i] = 1.0 / MatrixHelper.Sum(B0, 1)[i];
                    }

                    //A hesapla
                    for (int i = 0; i < m; i++)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            var exp = Math.Exp(t_time[i, j] * b[g] * -1);
                            A0[i, j] = B[j] * attr_tot[j] * exp;
                        }
                    }

                    for (int i = 0; i < A0.GetLength(1); i++)
                    {
                        A[i] = 1.0 / MatrixHelper.Sum(A0, 0)[i];
                    }


                    for (int i = 0; i < Ai.GetLength(0); i++)
                    {
                        Ai[i, k] = A[i];
                    }

                    for (int j = 0; j < Bj.GetLength(1); j++)
                    {
                        Bj[k, j] = B[j];
                    }

                    if (k > 0)
                    {
                        //leftSide
                        var leftSumSquare = 0.0;
                        for (int i = 0; i < Ai.GetLength(0); i++)
                        {
                            leftSumSquare += Math.Pow(Ai[i, k] - Ai[i, k - 1], 2);
                        }
                        var leftSideCompare = Math.Sqrt(leftSumSquare / k);

                        //rightside
                        var rightSumSquare = 0.0;
                        for (int j = 0; j < Bj.GetLength(1); j++)
                        {
                            rightSumSquare += Math.Pow(Bj[k, j] - Bj[k - 1, j], 2);
                        }
                        var rightSideCompare = Math.Sqrt(rightSumSquare / k);

                        if (leftSideCompare < conv && rightSideCompare < conv)
                        {
                            Console.WriteLine("Convergence degeri saglandi. Donguyu bitir");
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Convergence degeri saglanmadi. Devam et");
                        }
                    }
                }

                double[,] mod_trips = new double[m, n];
                for (int i = 0; i < mod_trips.GetLength(0); i++)
                {
                    for (int j = 0; j < mod_trips.GetLength(1); j++)
                    {
                        mod_trips[i, j] = A[i] * B[j] * prod_tot[i] * attr_tot[j] * Math.Exp(t_time[i, j] * b[g] * -1);
                    }
                }

                //Add results to rowdata 2
                var v = 0;
                var row_data_2 = new double[rowDataMatrix.Count(), b.GetLength(0)];
                for (int o = 0; o < m; o++)
                {
                    var temp = MatrixHelper.GetSubsetColumn(mod_trips, v);
                    row_data_2[o, g] = temp[o];
                }

                v = v + 1;
                var t = 0;
                var x = 0;

                for (int u = 0; u < n - 1; u++)
                {
                    int endPoint = m * (u + 1);
                    for (int f = t; f < endPoint; f++)
                    {
                        var temp = MatrixHelper.GetSubsetColumn(mod_trips, v);
                        row_data_2[m + f, g] = temp[x + f];
                    }
                    t = t + m;
                    v = v + 1;
                    x = x - m;
                }

                var h = rowDataMatrix.Count;
                for (int i = 0; i < h; i++)
                {
                    for (int k = 0; k < num_bins - 1; k++)
                    {
                        if (OTLD[k, 1] <= rowDataMatrix[i].Item3 && rowDataMatrix[i].Item3 < OTLD[k, 2])
                        {
                            MTLD00[k, g] = MTLD00[k, g] + row_data_2[i, g];
                            break;
                        }
                    }
                }

                //Calculate RMSE
                for (int i = 0; i < num_bins; i++)
                {
                    var sumOfRows1 = MatrixHelper.Sum(OTLD, 1)[3];
                    OTLD[i, 4] = (OTLD[i, 3] / sumOfRows1) * 100.0;

                    var sumOfRows2 = MatrixHelper.Sum(MTLD00, 1)[g];
                    MTLD01[i, g] = (MTLD00[i, g] / sumOfRows2) * 100.0;
                }

                var total = 0.0;
                for (int i = 0; i < OTLD.GetLength(0); i++)
                {
                    total = total + Math.Pow(OTLD[i, 4] - MTLD01[i, g], 2);
                }
                rmse[g] = Math.Sqrt(total) / num_bins;

                //rmse, beta, OTLD, MTLD
                var OTLDResult = MatrixHelper.GetSubsetColumn(OTLD, 4);
                var MTLD01Result = MatrixHelper.GetSubsetColumn(MTLD01, g);

                results.Add(new Tuple<double, double, double[], double[], double[,]>(rmse[g], b[g], OTLDResult, MTLD01Result, mod_trips));
            }

            Tuple<double, double, double[], double[], double[,]> best;
            GravitySolution gravitySolution;

            results.Sort((x, y) =>
            {
                if (x.Item1 > y.Item1)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            });
            best = results.First();
            gravitySolution = new GravitySolution(best.Item1, best.Item2, best.Item3, best.Item4, best.Item5);

            return gravitySolution;
        }

        private List<Tuple<int, int, double, double>> PrepareFlatDictionary(double[,] costMatrix, double[,] timeMatrix)
        {
            var dictionary = new List<Tuple<int, int, double, double>>();

            for (int j = 0; j < costMatrix.GetLength(0); j++)
            {
                for (int i = 0; i < costMatrix.GetLength(1); i++)
                {
                    dictionary.Add(new Tuple<int, int, double, double>(i, j, costMatrix[i, j], timeMatrix[i, j]));
                }
            }
            return dictionary;
        }
    }

    public class BetaCapabilities
    {
        public double MinValue { get; set; }

        public double MaxValue { get; set; }

        public double Sensitivity { get; set; }
    }
}

