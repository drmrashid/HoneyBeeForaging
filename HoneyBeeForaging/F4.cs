using System;
using System.Collections.Generic;
using System.Text;

namespace HoneyBeeForaging
{
    class F4 : FitnessFunction
    {
        public F4(int d, double sucessThld)
            : base(d, sucessThld)
        {
            for (int i = 0; i < dimensions; i++)
            {
                searchSpace[i, 0] = 0;
                searchSpace[i, 1] = 1;
            }
            InitializePeaks(F4Peaks);
            ngh = 0.05;//Math.Abs(searchSpace[0, 0] - searchSpace[0, 1]) * 0.1;
            peakError = new double[peaks.GetUpperBound(0) + 1];
            peakFitnessEvaluations = new int[peaks.GetUpperBound(0) + 1];
        }
        static double[,] F4Peaks = {
            {0.0000001715600000,0.07970},            
            {0.0513110000000000,0.24628},
            {0.2291800000000000,0.44950},
            {0.4958900000000000,0.67917},
            {0.7483900000000000,0.93015}
        };
        public override double Evaluate(Bee b)
        {
            functionEvaluations++;
            double f;
            f = 5 * Math.PI * (Math.Pow(b.Position[0], 3.0 / 4.0) - 0.05);
            f = Math.Pow(Math.Sin(f), 6);
            f = f * Math.Exp(-2 * Math.Log(2, Math.E) * Math.Pow((b.Position[0] - 0.08) / 0.854, 2));
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
