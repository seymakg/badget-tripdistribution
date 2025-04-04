using Microsoft.SolverFoundation.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger.Solvers.Gravity
{
    public interface IGravitySolution : ISolverSolution
    {
        double RMSE { get; }

        double Beta { get; }

        double[] ObservedTLD { get; }

        double[] ModelledTLD { get; }

        double[,] Trips { get; }

        string Warning { get; set; }
    }
}
