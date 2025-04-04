using Microsoft.SolverFoundation.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badget.Solvers.GameTheory
{
    public interface ICompactQuasiNewtonSolution: ISolverSolution
    {
        double SolutionValue { get; }

        double[] ParamaterValues { get; }

        NonlinearResult NonlinearResult { get;  }
    }
}
