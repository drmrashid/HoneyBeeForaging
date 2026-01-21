using System;
using System.Collections.Generic;
using System.Text;

namespace HoneyBeeForaging
{
    class Sphere : FitnessFunction
    {
        public Sphere(int d, double successThld)
            : base(d, successThld)
        {
            for (int i = 0; i < dimensions; i++)
            {
                searchSpace[i, 0] = -5.0;
                searchSpace[i, 1] = 5.0;
            }
            InitializePeaks(SpherePeaks);
            ngh = Math.Abs(searchSpace[0, 0] - searchSpace[0, 1]) * 0.1;
            peakError = new double[peaks.GetUpperBound(0) + 1];
            peakFitnessEvaluations = new int[peaks.GetUpperBound(0) + 1];
        }
        static double[,] SpherePeaks = {
            {0.000000000000000,0,0}
        };
        public override double Evaluate(Bee b)
        {
            double f = 0;
            double p = 0;
            double x = 0;
            for (int i = 0; i < dimensions; i++)
            {
                x = b.Position[i] - p;
                f += x * x;
            }
            functionEvaluations++;
            return f;
        }
        public override double Evaluate(double[] b)
        {
            throw new Exception("The method or operation is not implemented.");
        }

    }
}
