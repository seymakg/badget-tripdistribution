using Microsoft.SolverFoundation.Services;
using Microsoft.SolverFoundation.Solvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badget.Solvers.GameTheory
{
    public class CompactQuasiNewtonSolutionSolver
    {
        private Func<double[], double> objective;
        private int variableCount;
        private int iterationCount = 0;

        public CompactQuasiNewtonSolutionSolver(int variableCount, Func<double[], double> objectiveFunction)
        {
            this.objective = objectiveFunction;
            this.variableCount = variableCount;
        }

        public ICompactQuasiNewtonSolution Solve()
        {
            double[] initals = GetInitials();

            var result = SolveRecursive(null, initals, 0);
            double[] variables = new double[variableCount];
            for (int j = 0; j < variableCount; j++)
            {
                variables[j] = result.GetValue(j + 1);
            }

            Tuple<double, double[]> resultset = new Tuple<double, double[]>(result.GetValue(0), variables);

            GameTheorySolution gameTheorySolution = new GameTheorySolution(result.Result, resultset.Item1, resultset.Item2);
            return gameTheorySolution;
        }


        private INonlinearSolution SolveRecursive(INonlinearSolution previousResult, double[] initals, int iterationIndex)
        {
            if (iterationIndex > iterationCount)
            {
                return previousResult;
            }

            var result = Solve(initals);
            double[] variables = new double[variableCount];
            for (int j = 0; j < variableCount; j++)
            {
                variables[j] = result.GetValue(j + 1);
            }

            iterationIndex++;
            return SolveRecursive(result, variables, iterationIndex);
        }

        private INonlinearSolution Solve(double[] initals)
        {
            var function = new NonlinearObjectiveFunction(variableCount, x => this.objective(x));
            double[] lowers = new double[variableCount];
            double[] uppers = new double[variableCount];

            for (int i = 0; i < variableCount; i++)
            {
                lowers[i] = 0.0;
                uppers[i] = double.PositiveInfinity;
            }

            var solution = NelderMeadSolver.Solve(function.Function, initals, lowers, uppers);

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
