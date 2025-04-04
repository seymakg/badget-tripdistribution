using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Badger.Solvers.Gravity
{
    internal class GravitySolution : IGravitySolution
    {
        private double rmse;
        private double beta;
        private double[] observedTLD;
        private double[] modelledTLD;
        private double[,] trips;

        public GravitySolution(double rmse, double beta, double[] observedTLD, double[] modelledTLD, double[,] trips)
        {
            this.rmse = rmse;
            this.beta = beta;
            this.observedTLD = observedTLD;
            this.modelledTLD = modelledTLD;
            this.trips = trips;
        }

        public double RMSE => rmse;

        public double Beta => beta;

        public double[] ObservedTLD => observedTLD;

        public double[] ModelledTLD => modelledTLD;

        public double[,] Trips => trips;

        public string Warning { get; set; }

    }
}
