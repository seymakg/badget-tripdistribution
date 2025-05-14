using Microsoft.SolverFoundation.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger.Solvers.GameTheory
{
    public class GameTheorySolution: IGameTheorySolution
    {
        private double[] paramaterValues;
        private double solutionValue;
        private NonlinearResult nonlinearResult;

        public GameTheorySolution(NonlinearResult nonlinearResult, double solutionValue, double[] paramaterValues)
        {
            this.paramaterValues = paramaterValues.Select(x => Math.Truncate(x * 1000) / 1000).ToArray();
            this.solutionValue = solutionValue;
            this.nonlinearResult = nonlinearResult;
        }

        public NonlinearResult NonlinearResult => nonlinearResult;

        public double[] ParamaterValues => paramaterValues;

        public double SolutionValue => solutionValue;
    }
}
