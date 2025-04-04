using Microsoft.SolverFoundation.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger.Solvers.GameTheory
{
    public interface IGameTheorySolution: ISolverSolution
    {
        double SolutionValue { get; }

        double[] ParamaterValues { get; }

        NonlinearResult NonlinearResult { get;  }
    }
}
