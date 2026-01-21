using System;
using System.Collections.Generic;
using System.Text;

namespace HoneyBeeForaging
{
    class Swarm : IComparable
    {
        private static int count = 0;
        private int id;

        public enum TerminationCriteria
        {
            Iterations,
            FunctionEvaluaions,
            Error
        };
        private Bee[] bees;
        private Bee bestBee;
        private int maxIteration;
        private int maxEvaluations;
        private double maxError;
        private double k;
        private double c1;
        private double c2;
        private bool global;
        private bool neighborhood;
        private double[,] max_x;
        private TerminationCriteria term;
        private FitnessFunction func;

        private int a;

        public int CompareTo(object obj)
        {
            if (obj is Swarm)
            {
                Swarm temp = (Swarm)obj;
                return bestBee.BestFitness.CompareTo(temp.bestBee.BestFitness);
            }
            throw new ArgumentException("object is not a Swarm");
        }

        public Swarm(FitnessFunction f, Bee[] b, TerminationCriteria t, bool g, bool ngh)
        {
            id = count;
            count++;

            func = f;
            bees = b;
            term = t;
            global = g;
            neighborhood = ngh;
            FindBest();
            a = 0;
        }

        public Swarm(Swarm s)
        {
            id = s.id;

            if (s.bees != null)
            {
                bees = new Bee[s.bees.Length];
                for (int i = 0; i < bees.Length; i++)
                    bees[i] = new Bee(s.bees[i]);
            }
            FindBest();
            maxIteration = s.maxIteration;
            maxEvaluations = s.maxEvaluations;
            maxError = s.maxError;
            k = s.k;
            c1 = s.c1;
            c2 = s.c2;
            global = s.global;
            neighborhood = s.neighborhood;
            max_x = s.max_x;
            term = s.term;
            func = s.func;
            a = s.a;
        }

        public void ReCalculate()
        {
            for (int i = 0; i < bees.Length; i++)
                bees[i].ReCalculate();
            FindBest();
        }

        public void AddBees(Bee[] b)
        {
            Bee[] temp = new Bee[bees.Length + b.Length];
            bees.CopyTo(temp, 0);
            b.CopyTo(temp, bees.Length);
            bees = temp;
            FindBest();
        }

        public void Step()
        {
            a++;
            if (bestBee == null)
            {
                FindBest();
            }
            for (int i = 0; i < bees.Length; i++)
            {
                Move(i);
                if (bees[i].BestFitness < bestBee.BestFitness)
                    bestBee = bees[i];
            }
            //Console.Write("{0}\t{1}\t{2}",id,a,BestBee.BestFitness);
            //for (int i = 0; i < bestBee.Dimension; i++)
            //    Console.Write("\t{0}", bestBee.Position[i]);
            //Console.WriteLine();
        }
        private void Move(int i)
        {
            if (global)
            {
                if (neighborhood)
                {
                    bees[i].Move(k, c1, c2, bestBee, max_x);
                }
                else
                {
                    bees[i].Move(k, c1, c2, bestBee);
                }
            }
            else
            {
                if (neighborhood)
                {
                    bees[i].Move(k, c1, max_x);
                }
                else
                {
                    bees[i].Move(k, c1);
                }
            }
        }
        private void FindBest()
        {
            bestBee = bees[0];
            for (int i = 0; i < bees.Length; i++)
                if (bees[i].BestFitness < bestBee.BestFitness)
                    bestBee = bees[i];
        }
        public void Fly()
        {
            switch (term)
            {
                case TerminationCriteria.Iterations:
                    for (int i = 0; i < maxIteration; i++)
                        Step();
                    break;
                case TerminationCriteria.FunctionEvaluaions:
                    while (func.FunctionEvalutions < maxEvaluations)
                        Step();
                    break;
                case TerminationCriteria.Error:
                    int j = 0;
                    while (bestBee.BestFitness > maxError && j++ < maxIteration)
                        Step();
                    break;
            }
        }
        public double K
        {
            get
            {
                return k;
            }
            set
            {
                k = value;
            }
        }

        public double C1
        {
            get
            {
                return c1;
            }
            set
            {
                c1 = value;
            }
        }

        public double C2
        {
            get
            {
                return c2;
            }
            set
            {
                c2 = value;
            }
        }

        public Bee BestBee
        {
            get
            {
                return bestBee;
            }
        }
        public int MaxIterations
        {
            get
            {
                return maxIteration;
            }
            set
            {
                maxIteration = value;
            }
        }
        public int MaxEvaluations
        {
            get
            {
                return maxEvaluations;
            }
            set
            {
                maxEvaluations = value;
            }
        }
        public double MaxError
        {
            get
            {
                return maxError;
            }
            set
            {
                maxError = value;
            }
        }
        public TerminationCriteria Termination
        {
            get
            {
                return term;
            }
            set
            {
                term = value;
            }
        }
        public bool UseGlobalBest
        {
            get
            {
                return global;
            }
            set
            {
                global = value;
            }
        }
        public bool UseNeighborhood
        {
            get
            {
                return neighborhood;
            }
            set
            {
                neighborhood = value;
            }
        }
        public double[,] Neighborhood
        {
            get
            {
                return max_x;
            }
            set
            {
                max_x = value;
            }
        }
        public FitnessFunction FitnessFunc
        {
            set
            {
                func = value;
            }
        }
    }
}
