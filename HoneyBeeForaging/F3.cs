using System;
using System.Collections.Generic;
using System.Text;

namespace HoneyBeeForaging
{
    class F3 : FitnessFunction
    {
        public F3(int d, double sucessThld)
            : base(d, sucessThld)
        {
            for (int i = 0; i < dimensions; i++)
            {
                searchSpace[i, 0] = 0;
                searchSpace[i, 1] = 1;
            }
            InitializePeaks(F3Peaks);
            ngh = 0.05;//Math.Abs(searchSpace[0, 0] - searchSpace[0, 1]) * 0.1;
            peakError = new double[peaks.GetUpperBound(0) + 1];
            peakFitnessEvaluations = new int[peaks.GetUpperBound(0) + 1];
        }
        static double[,] F3Peaks = {
            {0.0000000000000000,0.079699},            
            {0.0000000000000000,0.246660},
            {0.0000000000000000,0.450630},
            {0.0000000000000000,0.681430},
            {0.0000000000000000,0.933900}
        };
        public override double Evaluate(Bee b)
        {
            functionEvaluations++;
            double f;
            f = 5 * Math.PI * (Math.Pow(b.Position[0], 3.0 / 4.0) - 0.05);
            f = Math.Pow(Math.Sin(f), 6);
            //for (int i = 0; i < 5; i++)
            //    f = Math.Sin(f);
            return 1 - f;
        }

        public override double Evaluate(double[] b)
        {
            throw new Exception("The method or operation is not implemented.");
        }    }
}
