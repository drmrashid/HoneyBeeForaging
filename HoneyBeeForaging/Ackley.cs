using System;
using System.Collections.Generic;
using System.Text;

namespace HoneyBeeForaging
{
    class Ackley : FitnessFunction
    {
        public Ackley(int d, double successThld)
            : base(d, successThld)
        {
            for (int i = 0; i < dimensions; i++)
            {
                searchSpace[i, 0] = -32.768;
                searchSpace[i, 1] = 32.768;
            }
            InitializePeaks(AckleyPeaks);
            ngh = Math.Abs(searchSpace[0, 0] - searchSpace[0, 1]) * 0.01;
            peakError = new double[peaks.GetUpperBound(0) + 1];
            peakFitnessEvaluations = new int[peaks.GetUpperBound(0) + 1];
        }
        static double[,] AckleyPeaks = {
            {0.0000000000000000,0,0},            
            {2.5799275570298694,0,1},
            {2.5799275570298694,0,-1},
            {2.5799275570298694,1,0},
            {2.5799275570298694,-1,0}
        };
        public override double Evaluate(Bee b)
        {
            double sum1 = 0;
            double sum2 = 0;
            double f = 0;
            double xd;
            for (int d = 0; d < dimensions; d++)
            {
                xd = b.Position[d];
                sum1 += xd * xd;
                sum2 += Math.Cos(2 * Math.PI * xd);
            }
            //y = D;
            f = (-20 * Math.Exp(-0.2 * Math.Sqrt(sum1 / dimensions)) - Math.Exp(sum2 / dimensions) + 20 + Math.E);
            return f;
        }
        public override double Evaluate(double[] b)
        {
            throw new Exception("The method or operation is not implemented.");
        }

    }
}
