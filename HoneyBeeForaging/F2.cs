using System;
using System.Collections.Generic;
using System.Text;

namespace HoneyBeeForaging
{
    class F2 : FitnessFunction
    {
        public F2(int d, double sucessThld)
            : base(d, sucessThld)
        {
            for (int i = 0; i < dimensions; i++)
            {
                searchSpace[i, 0] = 0;
                searchSpace[i, 1] = 1;
            }
            InitializePeaks(F2Peaks);
            ngh = 0.05;//Math.Abs(searchSpace[0, 0] - searchSpace[0, 1]) * 0.1;
            peakError = new double[peaks.GetUpperBound(0) + 1];
            peakFitnessEvaluations = new int[peaks.GetUpperBound(0) + 1];
        }
        static double[,] F2Peaks = {
            {0.0000000000000000,0.1},            
            {0.0829959570000000,0.3},
            {0.2928932190000000,0.5},
            {0.5414979780000000,0.7},
            {0.7500000000000000,0.9}
        };
        public override double Evaluate(Bee b)
        {
            functionEvaluations++;
            double f;
            f = Math.Pow(Math.Sin(5 * Math.PI * b.Position[0]), 6);
            f = f * Math.Exp(-2 * Math.Log(2,Math.E) * Math.Pow((b.Position[0] - 0.1) / 0.8, 2));
            //for (int i = 0; i < 5; i++)
            //    f = Math.Sin(f);
            return 1 - f;
        }

        public override double Evaluate(double[] b)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
