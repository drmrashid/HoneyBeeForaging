using System;
using System.Collections.Generic;
using System.Text;

namespace HoneyBeeForaging
{
    class Rastrigin : FitnessFunction
    {
        public Rastrigin(int d, double successThld)
            : base(d, successThld)
        {
            for (int i = 0; i < dimensions; i++)
            {
                searchSpace[i, 0] = -5.12;
                searchSpace[i, 1] = 5.12;
            }
            InitializePeaks(RastriginPeaks);
            ngh = Math.Abs(searchSpace[0, 0] - searchSpace[0, 1]) * 0.02;
            peakError = new double[peaks.GetUpperBound(0) + 1];
            peakFitnessEvaluations = new int[peaks.GetUpperBound(0) + 1];
        }
        static double[,] RastriginPeaks = {
            {0.000000000000000,0,0,0,0,0,0,0,0,0,0,0,0},
            {0.9949590570932898,-1,0,0,0,0,0,0,0,0,0,0,0},
            {0.9949590570932898,0,1,0,0,0,0,0,0,0,0,0,0},
            {0.9949590570932898,1,0,0,0,0,0,0,0,0,0,0,0},
            {0.9949590570932898,0,-1,0,0,0,0,0,0,0,0,0,0},
            {1.9899181141865796,1,1,0,0,0,0,0,0,0,0,0,0},
            {1.9899181141865796,1,-1,0,0,0,0,0,0,0,0,0,0},
            {1.9899181141865796,-1,1,0,0,0,0,0,0,0,0,0,0},
            {1.9899181141865796,-1,-1,0,0,0,0,0,0,0,0,0,0}
        };
        public override double Evaluate(Bee b)
        {
            double k = 10;
            double f = 0;
            double xd;
            for (int d = 0; d < dimensions; d++)
            {
                xd = b.Position[d];
                f += xd * xd - k * Math.Cos(2 * Math.PI * xd);
            }
            f += dimensions * k;
            functionEvaluations++;
            return f;
        }
        public override double Evaluate(double[] b)
        {
            throw new Exception("The method or operation is not implemented.");
        }

    }
}
