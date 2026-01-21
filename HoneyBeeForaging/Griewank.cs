using System;
using System.Collections.Generic;
using System.Text;

namespace HoneyBeeForaging
{
    class Griewank : FitnessFunction
    {
        public Griewank(int d, double successThld)
            : base(d, successThld)
        {
            for (int i = 0; i < dimensions; i++)
            {
                searchSpace[i, 0] = -600;
                searchSpace[i, 1] = 600;
            }
            InitializePeaks(GriewankPeaks);
            ngh = Math.Abs(searchSpace[0, 0] - searchSpace[0, 1]) * 0.0025;
            peakError = new double[peaks.GetUpperBound(0) + 1];
            peakFitnessEvaluations = new int[peaks.GetUpperBound(0) + 1];
        }
        static double[,] GriewankPeaks = {
            {0.0000000000000000000,0,0},
            {0.0073960403341150061,3.140,4.438},
            {0.0073960403341150061,-3.140,4.438},
            {0.0073960403341150061,3.140,-4.438},
            {0.0073960403341150061,-3.140,-4.438},
            {0.0098646720610061633,6.280,0.000},
            {0.0098646720610061633,-6.280,0.000}/*,
            {0.019719489248184785,0.000,8.876},
            {0.019719489248184785,0.000,-8.876},
            {0.027125384395415564,9.420,4.438},
            {0.027125384395415564,-9.420,4.438},
            {0.027125384395415564,9.420,-4.438},
            {0.027125384395415564,-9.420,-4.438},
            {0.029584161212069859,6.280,8.876},
            {0.029584161212069859,-6.280,8.876},
            {0.029584161212069859,6.280,-8.876},
            {0.029584161212069859,-6.280,-8.876}*/
        };
        public override double Evaluate(Bee b)
        {
            double f = 0;
            double p = 1;
            double xd;
            for (int d = 0; d < dimensions; d++)
            {
                xd = b.Position[d];
                f = f + xd * xd;
                p = p * Math.Cos(xd / Math.Sqrt(d + 1));
            }
            f = f / 4000 - p + 1;
            return f;
        }
        public override double Evaluate(double[] b)
        {
            throw new Exception("The method or operation is not implemented.");
        }

    }
}
