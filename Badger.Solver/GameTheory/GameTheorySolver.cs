using Microsoft.SolverFoundation.Services;
using Microsoft.SolverFoundation.Solvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger.Solvers.GameTheory
{
    public delegate void SolvingCompletedEventHandler(object sender, SolvingCompletedEventArgs e);
    public delegate void CompletedEventHandler(object sender, CompletedEventArgs e);

    public class GameTheorySolver
    {
        private Func<double[], double> objective;
        private int variableCount;

        public GameTheorySolvingStrategy SolvingStrategy { get; set; }

        public GameTheorySolution LastResult { get; private set; }
        public string InstanceUniqueId { get; private set; }
        public int RetryCount { get; private set; }

        public event SolvingCompletedEventHandler OnSolvingCompleted;

        public GameTheorySolver(int variableCount, Func<double[], double> objectiveFunction)
        {
            this.objective = objectiveFunction;
            this.variableCount = variableCount;
            InstanceUniqueId = DateTime.Now.ToString("yyyyMMddHHmmss");
            RetryCount = 0;
        }

        public void Solve(double[] initialValues = null)
        {
            RetryCount++;
            double[] initials;
            if (initialValues == null)
            {
                initials = GetInitials();
            }
            else
            {
                initials = initialValues;
            }

            initials = initials.Select(x => Math.Truncate(x * 1000) / 1000).ToArray();
            var result = SolveInternal(initials);
            double[] variables = new double[variableCount];
            for (int j = 0; j < variableCount; j++)
            {
                variables[j] = result.GetValue(j + 1);
            }

            Tuple<double, double[]> resultset = new Tuple<double, double[]>(result.GetValue(0), variables);

            GameTheorySolution gameTheorySolution = new GameTheorySolution(result.Result, resultset.Item1, resultset.Item2);
            LastResult = gameTheorySolution;
            OnSolvingCompleted?.Invoke(this, new SolvingCompletedEventArgs() { 
                Solution = gameTheorySolution 
            });
        }

        private INonlinearSolution SolveInternal(double[] initals)
        {
            double[] lowers = new double[variableCount];
            double[] uppers = new double[variableCount];

            for (int i = 0; i < variableCount; i++)
            {
                lowers[i] = 0.0;
                uppers[i] = double.PositiveInfinity;
            }

            INonlinearSolution solution = null;
            if (SolvingStrategy == GameTheorySolvingStrategy.NelderMeadSolver)
            {
                solution = NelderMeadSolver.Solve(this.objective, initals, lowers, uppers);
            }

            return solution;
        }

        private double[] GetInitials()
        {
            double[] initals = new double[variableCount];
            for (int i = 0; i < variableCount; i++)
            {
                initals[i] = 0.0;
            }

            return initals;
        }
    }
}
