using System;
using System.Collections.Generic;
using System.Text;

namespace HoneyBeeForaging
{
    class F5 : FitnessFunction
    {
        public F5(int d, double sucessThld)//, double neighborhood)
            : base(d, sucessThld)
        {
            for (int i = 0; i < dimensions; i++)
            {
                searchSpace[i, 0] = -6;
                searchSpace[i, 1] = 6;
            }
            InitializePeaks(F5Peaks);
            ngh = 0.05;// neighborhood;//Math.Abs(searchSpace[0, 0] - searchSpace[0, 1]) * 0.1;
            peakError = new double[peaks.GetUpperBound(0) + 1];
            peakFitnessEvaluations = new int[peaks.GetUpperBound(0) + 1];
        }
        static double[,] F5Peaks = {
            {0.0000000000000000,3.58,-1.86},            
            {0.0000000000000000,3.0,2.0},
            {0.0000000000000000,-2.815,3.125},
            {0.0000000000000000,-3.78,-3.28}
        };
        public override double Evaluate(Bee b)
        {
            functionEvaluations++;
            double f, term1, term2;
            term1 = b.Position[0] * b.Position[0] + b.Position[1] - 11;
            term1 *= term1;
            term2 = b.Position[0] + b.Position[1] * b.Position[1] - 7;
            term2 *= term2;
            f = term1 + term2;
            return f;
        }

        public override double Evaluate(double[] b)
        {
            throw new Exception("The method or operation is not implemented.");
        }
}
}
