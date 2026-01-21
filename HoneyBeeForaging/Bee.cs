using System;
using System.Collections.Generic;
using System.Text;

namespace HoneyBeeForaging
{
    class Bee : IComparable
    {
        private static int count = 0;
        private int id;
        private int d;
        private double[] x;
        private double[] v;
        private double f;
        private double[] best_x;
        private double best_f;
        private int fitnessEvalBest;
        private int noChangeCount;
        private Random r;
        private FitnessFunction func;
        //private double[] max_x;
        //private double[] max_v;
        private int eliteCount;

        public int CompareTo(object obj)
        {
            if (obj is Bee)
            {
                Bee temp = (Bee)obj;
                return best_f.CompareTo(temp.best_f);
            }
            throw new ArgumentException("object is not a Bee");
        }
        
        public Bee(FitnessFunction function, Random rand)
        {
            id = count;
            count++;

            func = function;
            d = func.Dimension;

            x = new double[d];
            v = new double[d];

            best_x = new double[d];
            //max_x = new double[d];
            //max_v = new double[d];

            r = rand;

            eliteCount = 0;
            noChangeCount = 0;
            fitnessEvalBest = 0;
        }

        public Bee(Bee b)
        {
            /*
            id = count;
            count++;

            d = b.d;

            x = new double[d];
            v = new double[d];

            best_x = new double[d];

            b.x.CopyTo(x, 0);
            b.v.CopyTo(v, 0);

            b.best_x.CopyTo(best_x, 0);

            f = b.f;
            best_f = b.best_f;
            
            r = b.r;
            func = b.func;

            eliteCount = 0;
            */
            Copy(b);
        }

        public void Copy(Bee b)
        {
            id = count;
            count++;

            d = b.d;

            x = new double[d];
            v = new double[d];

            best_x = new double[d];

            b.x.CopyTo(x, 0);
            b.v.CopyTo(v, 0);

            b.best_x.CopyTo(best_x, 0);

            f = b.f;
            best_f = b.best_f;

            r = b.r;
            func = b.func;

            eliteCount = 0;
            noChangeCount = b.noChangeCount;
            fitnessEvalBest = b.fitnessEvalBest;
        }

        public void Initialize(double[,] max_x, double[,] max_v)
        {
            for (int i = 0; i < d; i++)
            {
                x[i] = (max_x[i, 1] - max_x[i, 0]) * r.NextDouble() + max_x[i, 0];
                v[i] = (max_v[i, 1] - max_v[i, 0]) * r.NextDouble() + max_v[i, 0];
                best_x[i] = x[i];
            }
            f = func.Evaluate(this);
            //best_x = x;
            best_f = f;
            noChangeCount = 0;
        }
        public void InitializeVelocity(double ngh)
        {
            for (int i = 0; i < d; i++)
            {
                v[i] = 2 * ngh * r.NextDouble() + x[i] - ngh;
            }
        }
        public void ResetBest()
        {
            for (int i = 0; i < d; i++)
            {
//                x[i] = loc[i];
//                v[i] = ngh * r.NextDouble() + x[i] - ngh;
                best_x.CopyTo(x, 0);
            }
//            f = func.Evaluate(this);
            //best_x = x;
            f = best_f;
        }

        public void Move(double k, double c1, double c2, Bee g)
        {
            for (int i = 0; i < d; i++)
            {
                v[i] = v[i] + r.NextDouble() * c1 * (best_x[i] - x[i])
                            + r.NextDouble() * c2 * (g.best_x[i] - x[i]);
                v[i] = k * v[i];
                x[i] = x[i] + v[i];
            }
            f = func.Evaluate(this);
            UpdateBest();
            //Console.Write("[{0:F2}({1:F2}),{2:F2}({3:F2})]", x[0], v[0], x[1], v[1]);
        }

        private void UpdateBest()
        {
            if (f < best_f)
            {
                best_f = f;
                x.CopyTo(best_x, 0);
                //                for (int i = 0; i < d; i++)
                //                    best_x[i] = x[i];
                noChangeCount = 0;
                fitnessEvalBest = func.FunctionEvalutions;
            }
            else
                noChangeCount++;
        }

        public void Move(double k, double c1, double c2, Bee g, double[,] max_x)
        {
            for (int i = 0; i < d; i++)
            {
                v[i] = v[i] + r.NextDouble() * c1 * (best_x[i] - x[i])
                            + r.NextDouble() * c2 * (g.best_x[i] - x[i]);
                v[i] = k * v[i];
                x[i] = x[i] + v[i];
            }
            ClipPosition(max_x);
            f = func.Evaluate(this);
            UpdateBest();
/*            if (f < best_f)
            {
                best_f = f;
                x.CopyTo(best_x, 0);
//                for (int i = 0; i < d; i++)
//                    best_x[i] = x[i];
            }*/
        }

        public void Move(double k, double c1)
        {
            for (int i = 0; i < d; i++)
            {
                v[i] = v[i] + r.NextDouble() * c1 * (best_x[i] - x[i]);
                            //+ r.NextDouble() * c2 * (g.best_x[i] - x[i]);
                v[i] = k * v[i];
                x[i] = x[i] + v[i];
            }
            f = func.Evaluate(this);
            UpdateBest();
/*            if (f < best_f)
            {
                best_f = f;
                x.CopyTo(best_x, 0); 
                //for (int i = 0; i < d; i++)
                //    best_x[i] = x[i];
            }*/
            //Console.Write("[{0:F2}({1:F2}),{2:F2}({3:F2})]", x[0], v[0], x[1], v[1]);
        }

        public void Move(double k, double c1, double[,] max_x)
        {
            for (int i = 0; i < d; i++)
            {
                v[i] = v[i] + r.NextDouble() * c1 * (best_x[i] - x[i]);
                            //+ r.NextDouble() * c2 * (g.best_x[i] - x[i]);
                v[i] = k * v[i];
                x[i] = x[i] + v[i];
            }
            ClipPosition(max_x);
            f = func.Evaluate(this);
            UpdateBest();
/*            if (f < best_f)
            {
                best_f = f;
                x.CopyTo(best_x, 0);
                //for (int i = 0; i < d; i++)
                //    best_x[i] = x[i];
            }*/
        }

        private void ClipPosition(double[,] max_x)
        {
            for (int i = 0; i < d; i++)
            {
                if (x[i] < max_x[i, 0])
                {
                    x[i] = max_x[i, 0];
                    //v[i] = 0;
                }
                else if (x[i] > max_x[i, 1])
                {
                    x[i] = max_x[i, 1];
                    //v[i] = 0;
                }
            }
        }

        public bool BestInNeighborhood(double ngh, Bee b)
        {
            for (int i = 0; i < d; i++)
                if (Math.Abs(best_x[i] - b.best_x[i]) > ngh)
                    return false;
            return true;
        }

        //public static bool IsBestPositionEqual(Bee b)*
        public double[] Position
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        public double[] Velocity
        {
            get
            {
                return v;
            }
            set
            {
                v = value;
            }
        }

        public double Fitness
        {
            get
            {
                return f;
            }
            set
            {
                f = value;
            }
        }

        public double[] BestPosition
        {
            get
            {
                return best_x;
            }
            set
            {
                best_x = value;
            }
        }

        public double BestFitness
        {
            get
            {
                return best_f;
            }
            set
            {
                best_f = value;
            }
        }

        public int Dimension
        {
            get 
            {
                return d;
            }
            set
            {
                d = value;
            }
        }

        public FitnessFunction Function
        {
            get
            {
                return func;
            }
            set
            {
                func = value;
            }
        }

        public int EliteCount
        {
            get
            {
                return eliteCount;
            }
            set
            {
                eliteCount = value;
            }
        }
        public int NoChangeCount
        {
            get
            {
                return noChangeCount;
            }
        }
        public int FitnessEvaluationsBest
        {
            get
            {
                return fitnessEvalBest;
            }
        }

        public void ReCalculate()
        {
            f = func.Evaluate(this);
            best_f = f;
            x.CopyTo(best_x, 0);
            //                for (int i = 0; i < d; i++)
            //                    best_x[i] = x[i];
            noChangeCount = 0;
            fitnessEvalBest = func.FunctionEvalutions;

        }
    }
}
