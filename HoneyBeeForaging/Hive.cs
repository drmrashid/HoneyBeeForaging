using System;
using System.Collections.Generic;
using System.Text;

namespace HoneyBeeForaging
{
    class Hive
    {
        private double[,] searchSpace;
        private int d;
        private int n;
        private int e;
        private int nep;
        private double ngh;
        private int maxPSOIterations;
        private int maxHBOIterations;
        private int foragedThreshold;
        private double errorThreshold;
        private FitnessFunction func;
        private Random r;
        private Swarm[] swarms;
        private List<Bee> eliteBeesList;
        private List<Swarm> eliteSwarmsList;
        private List<Bee> bestBeesList;
        private List<Bee> foragedRegionsList;

        public Hive(FitnessFunction function, int n, int e, int nep, int maxPSOIter, int maxHBOIter, int foragedThld)
        {
            func = function;
            searchSpace = func.SearchSpace;///////
            this.d = func.Dimension;
            this.n = n;
            this.e = e;
            this.nep = nep;
            this.ngh = func.Neighborhood;
            maxPSOIterations = maxPSOIter;
            maxHBOIterations = maxHBOIter;
            foragedThreshold = foragedThld;
            errorThreshold = func.SuccessThreshold;// errorThld;
            r = new Random();
            //Console.WriteLine(r.NextDouble());
            //swarms = new Swarm[e + n - (e * nep)];
            swarms = new Swarm[n];
            foragedRegionsList = new List<Bee>();
            eliteBeesList = new List<Bee>();
            bestBeesList = new List<Bee>();
            eliteSwarmsList = new List<Swarm>();
        }

        public void Run()
        {
            //double prevFitness = Double.MaxValue;
            //double bestFitness = Double.MaxValue;
//            int temp = 0;
            Bee bestBee = new Bee(func,r);
            bestBee.BestFitness = Double.MaxValue;
            //Random r = new Random();
            //FitnessFunction func = new FitnessFunction(FitnessFunction.FunctionType.Foxholes2D);
            int eliteFound = e;

            Bee[] bees = new Bee[n];
            for (int i = 0; i < bees.Length; i++)
            {
                bees[i] = new Bee(func, r);
                bees[i].Initialize(searchSpace, searchSpace);
            }
            
            Array.Sort(bees);

            //Swarm[] swarms = new Swarm[e + n - (e * nep)];
            for (int i = 0; i < e; i++)
                //SetupEliteSwarm(bees, i);
                swarms[i] = SetupEliteSwarm(bees[i]);
            //SetupSwarms(bees);
            //while (f.FunctionEvalutions < 4000000) ;
            for (int iter = 0; iter < maxHBOIterations; iter++)
            {
                //if (func is MovPeaks)
                //{
                //    if ((iter > 0) && (iter % ((MovPeaks)func).ChangeFrequency == 0))
                //        ((MovPeaks)func).ChangePeaks();
                //}
//                Swarm[] swarms = new Swarm[e + 1];

                Bee[] newBees = new Bee[nep - 1];
    
                for (int i = 0; i < eliteFound; i++)
                {
                    //eliteBeesList.Add(new Bee(bees[i]));
                    //AddEliteBee(bees[i]);
                    if (swarms[i].UseGlobalBest == false)
                    {
                        double[,] max_x = new double[d, 2];

                        for (int j = 0; j < d; j++)
                        {
                            max_x[j, 0] = swarms[i].BestBee.BestPosition[j] - ngh;
                            max_x[j, 1] = swarms[i].BestBee.BestPosition[j] + ngh;
                        }
                        for (int j = 0; j < nep - 1; j++)
                        {
                            newBees[j] = new Bee(func, r);
                            newBees[j].Initialize(max_x, max_x);
                        }
                        swarms[i].Neighborhood = max_x;
                        swarms[i].UseGlobalBest = true;
                        swarms[i].AddBees(newBees);
                    }
                    swarms[i].Fly();
                    AddEliteSwarm(swarms[i]);

                    //Predicate<Bee>
                    //eliteBeesList.Find();
                    //eliteBees = new Bee[nep];
                    //eliteBees[0] = bees[i];
                    //eliteBees[0].ResetBest();
                    //eliteBees[0].InitializeVelocity(ngh);
                    //for (int j = 0; j < d; j++)
                    //{
                    //    max_x[j, 0] = eliteBees[0].Position[j] - ngh;
                    //    max_x[j, 1] = eliteBees[0].Position[j] + ngh;
                    //}
                    //for (int j = 1; j < nep; j++)
                    //{
                    //    eliteBees[j] = bees[e + (nep - 1) * i + j - 1];
                    //    eliteBees[j].Initialize(max_x, max_x);
                    //}
                    //swarms[i] = new Swarm(func, eliteBees, Swarm.TerminationCriteria.Iterations, true, false);
                    //swarms[i].MaxIterations = maxPSOIterations;
                    //swarms[i].K = 0.729844;
                    //swarms[i].C1 = 2.05;
                    //swarms[i].C2 = 2.05;
                    //swarms[i].Neighborhood = max_x;
                }
                for (int i = eliteFound; i < eliteFound + (n - (eliteFound * nep)); i++)
                {
                    swarms[i] = SetupNonEliteSwarm();
                    //SetupNonEliteSwarm(i);
                    swarms[i].Fly();
                }
                //Console.WriteLine(func.GetOfflineError());
                //for (int i = 0; i < maxPSOIterations; i++)
                //{
                //    for (int j = 0; j < eliteFound + (n - (eliteFound * nep)); j++)
                //    {
                //        swarms[j].Step();
                //    }
                //    Console.WriteLine(func.GetOfflineError());
                //}
                //for(int i = 0; i < eliteFound; i++)
                //    AddEliteSwarm(swarms[i]);

                //eliteBees = new Bee[n - (eliteFound/*e*/ * nep)];
                //for (int i = (eliteFound/*e*/ * nep); i < n; i++)
                //{
                //    eliteBees[i - (eliteFound/*e*/ * nep)] = bees[i];
                //    eliteBees[i - (eliteFound/*e*/ * nep)].Initialize(searchSpace, searchSpace);
                //}
                //swarms[eliteFound] = new Swarm(func, eliteBees, Swarm.TerminationCriteria.Iterations, false, false);
                //swarms[eliteFound].MaxIterations = maxPSOIterations;
                //swarms[eliteFound].K = 0.729844;
                //swarms[eliteFound].C1 = 2.05;
                ////swarms[e].C2 = 2.05;
                //swarms[eliteFound].Neighborhood = searchSpace;

                //for (int i = 0; i < maxPSOIterations; i++)
                //{
                //    for (int j = 0; j < swarms.Length; j++)
                //        swarms[j].Step();
                //}

                //for (int i = 0; i < maxPSOIterations; i++)
                //{
                //for (int j = 0; j < swarms.Length; j++)
                //    swarms[j].Fly();
                //}

                //Bee[] selectedBees = new Bee[n - (e * nep) + e];
                //for (int i = 0; i < eliteFound/*e*/; i++)
                //    selectedBees[i] = swarms[i].BestBee;
                //for (int i = eliteFound/*e*/; i < selectedBees.Length; i++)
                //    selectedBees[i] = eliteBees[i - eliteFound/*e*/];
                //Array.Sort(selectedBees);

                //List<Bee> tempEliteBeeList = new List<Bee>(eliteBeesList);
                List<Swarm> tempEliteSwarmsList = new List<Swarm>(eliteSwarmsList);
                //List<Swarm> tempEliteSwarmsList = new List<Swarm>();
                //for (int i = 0; i < swarms.Length; i++)
                for (int i = 0; i < eliteFound + (n - (eliteFound * nep)); i++)
                {
                    tempEliteSwarmsList.Add(new Swarm(swarms[i]));
                }
                tempEliteSwarmsList.Sort();
                //swarms = new Swarm[e + n - (e * nep)];
                swarms = new Swarm[n];
                eliteFound = 0;
                for (int i = 0; i < tempEliteSwarmsList.Count; i++)
                {
//                    bees[Array.IndexOf(bees, selectedBees[i])] = bees[i];
//                    bees[i] = selectedBees[i];
//                    if (foragedRegionsList.Count < func.NumberOfPeaks)
                    {
                        if (!IsForaged(tempEliteSwarmsList[i].BestBee) && !IsSelected(tempEliteSwarmsList[i].BestBee))
                        {
                            //bees[eliteFound] = new Bee(tempEliteBeeList[i]);
                            swarms[eliteFound] = new Swarm(tempEliteSwarmsList[i]);
                            //bees[eliteFound].ResetBest();
                            eliteFound = eliteFound + 1;
                        }
                    }
/*                    else
                    {
                        //bees[eliteFound] = new Bee(tempEliteBeeList[i]);
                        swarms[eliteFound] = new Swarm(tempEliteSwarmsList[i]);
                        //bees[eliteFound].ResetBest();
                        eliteFound = eliteFound + 1;
                    }*/
                    if (eliteFound == e)
                        break;
                }
                //if (eliteFound < e)
                //    Console.Write("busted");
                //if (swarms[0].BestBee.BestFitness > prevFitness)
                //    Console.WriteLine("{0} {1}", swarms[0].BestBee.BestFitness, prevFitness);
                //else
                //    Console.WriteLine("Good");
                if (eliteFound > 0)
                {
                    //prevFitness = swarms[0].BestBee.BestFitness;
                    if (swarms[0].BestBee.BestFitness < bestBee.BestFitness)
                    {
                        //bestFitness = swarms[0].BestBee.BestFitness;
                        bestBee = new Bee(swarms[0].BestBee);
                    }
                    //AddBestBee(swarms[0].BestBee);
                    CheckForagedRegions();
                }
                CheckChange();
                //Console.WriteLine(func.FunctionEvalutions - temp);
                //temp = func.FunctionEvalutions;
            }
            if (foragedRegionsList.Count < 1)
                foragedRegionsList.Add(bestBee);
            foragedRegionsList.Sort();
            func.CalculateStatistics(foragedRegionsList);
            
            //Console.Write("{0:e16}",foragedRegionsList[0].BestFitness*100);
            //for(int i = 1; i < foragedRegionsList.Count; i++)
            //    Console.Write("\t{0:e16}",foragedRegionsList[i].BestFitness*100);
            //Console.WriteLine();
        }
        //2.5799275570298694
        //4.8840652580911517
        //0.0073960403341150061,3.140,4.438
        private void CheckChange()
        {
            if (foragedRegionsList.Count > 0 && foragedRegionsList[0].BestFitness != func.Evaluate(foragedRegionsList[0]))
            {
                //Console.WriteLine(foragedRegionsList.Count);
                eliteSwarmsList = new List<Swarm>();
                for (int i = 0; i < foragedRegionsList.Count; i++)
                {
                    foragedRegionsList[i].ResetBest();
                    foragedRegionsList[i].ReCalculate();
                    eliteSwarmsList.Add(SetupEliteSwarm(foragedRegionsList[i]));
                }
                foragedRegionsList = new List<Bee>();
                for (int i = 0; i < swarms.Length; i++)
                    if (swarms[i] != null)
                        swarms[i].ReCalculate();
            }
/*            for (int i = 0; i < foragedRegionsList.Count; i++)
            { 
                if(foragedRegionsList[i].
            }*/
        }
        private void CheckForagedRegions()
        {
            for (int i = 0; i < swarms.Length; i++)
                //if ((swarms[i] != null) && 
                //    ((swarms[i].BestBee.NoChangeCount > foragedThreshold) || 
                //    (func.CheckErrorThreshold(swarms[i].BestBee,errorThreshold))))
                if ((swarms[i] != null) &&
                    (swarms[i].BestBee.NoChangeCount > foragedThreshold))
                    foragedRegionsList.Add(new Bee(swarms[i].BestBee));
//                    return true;
//            return false;
        }
        private bool IsSelected(Bee b)
        {
            for (int i = 0; i < swarms.Length; i++)
                if ((swarms[i] != null) && (swarms[i].BestBee.BestInNeighborhood(ngh, b)))
                    return true;
            return false;
        }

        private void SetupSwarms(Bee[] bees)
        {

            for (int i = 0; i < swarms.Length; i++)
            {
                if (i < e)
                {
                    SetupEliteSwarm(bees, i);
                }
                else
                {
                    SetupNonEliteSwarm(i);
                }
            }
        }

        private void SetupEliteSwarm(Bee[] bees, int i)
        {
            double[,] max_x = new double[d, 2];

            Bee[] newBees = new Bee[nep];
            newBees[0] = bees[i];
            newBees[0].ResetBest();
            newBees[0].InitializeVelocity(ngh);
            for (int j = 0; j < d; j++)
            {
                max_x[j, 0] = newBees[0].Position[j] - ngh;
                max_x[j, 1] = newBees[0].Position[j] + ngh;
            }
            for (int j = 1; j < nep; j++)
            {
                newBees[j] = new Bee(func, r);
                newBees[j].Initialize(max_x, max_x);
            }
            swarms[i] = new Swarm(func, newBees, Swarm.TerminationCriteria.Iterations, true, false);
            swarms[i].MaxIterations = maxPSOIterations;
            swarms[i].K = 0.729844;
            swarms[i].C1 = 2.05;
            swarms[i].C2 = 2.05;
            swarms[i].Neighborhood = max_x;
        }
        private Swarm SetupEliteSwarm(Bee b)
        {
            double[,] max_x = new double[d, 2];

            Bee[] newBees = new Bee[nep];
            newBees[0] = new Bee(b);
            newBees[0].ResetBest();
            newBees[0].InitializeVelocity(ngh);
            for (int j = 0; j < d; j++)
            {
                max_x[j, 0] = newBees[0].Position[j] - ngh;
                max_x[j, 1] = newBees[0].Position[j] + ngh;
            }
            for (int j = 1; j < nep; j++)
            {
                newBees[j] = new Bee(func, r);
                newBees[j].Initialize(max_x, max_x);
            }
            Swarm s = new Swarm(func, newBees, Swarm.TerminationCriteria.Iterations, true, false);
            s.MaxIterations = maxPSOIterations;
            s.K = 0.729844;
            s.C1 = 2.05;
            s.C2 = 2.05;
            s.Neighborhood = max_x;
            return s;
        }
        private Swarm SetupNonEliteSwarm()
        {
            Bee[] newBee = new Bee[1];
            newBee[0] = new Bee(func, r);
            newBee[0].Initialize(searchSpace, searchSpace);
            Swarm s = new Swarm(func, newBee, Swarm.TerminationCriteria.Iterations, false, false);
            s.MaxIterations = maxPSOIterations;
            s.K = 0.729844;
            s.C1 = 2.05;
            s.C2 = 2.05;
            s.Neighborhood = searchSpace;
            return s;
        }

        private void SetupNonEliteSwarm(int i)
        {
            Bee[] newBee = new Bee[1];
            newBee[0] = new Bee(func, r);
            newBee[0].Initialize(searchSpace, searchSpace);
            swarms[i] = new Swarm(func, newBee, Swarm.TerminationCriteria.Iterations, false, false);
            swarms[i].MaxIterations = maxPSOIterations;
            swarms[i].K = 0.729844;
            swarms[i].C1 = 2.05;
            swarms[i].C2 = 2.05;
            swarms[i].Neighborhood = searchSpace;
        }
        bool IsForaged(Bee b)
        {
            for (int i = 0; i < foragedRegionsList.Count; i++)
                if (foragedRegionsList[i].BestInNeighborhood(ngh, b))
                    return true;
            return false;
        }
        void AddEliteSwarm(Swarm s)
        {
            for (int i = 0; i < eliteSwarmsList.Count; i++)
                if (eliteSwarmsList[i].BestBee.BestInNeighborhood(ngh, s.BestBee))
                    return;
            eliteSwarmsList.Add(new Swarm(s));
        }
        void AddEliteBee(Bee b)
        {
            for (int i = 0; i < eliteBeesList.Count; i++)
                if (eliteBeesList[i].BestInNeighborhood(ngh, b))
                    return;
            eliteBeesList.Add(new Bee(b));
        }
        void AddBestBee(Bee b)
        {
            for(int i = 0; i < bestBeesList.Count; i++)
                if (bestBeesList[i].BestInNeighborhood(ngh, b))
                {
                    if (bestBeesList[i].EliteCount <= foragedThreshold)
                    {
                        if (bestBeesList[i].BestFitness > b.BestFitness)
                        {
                            bestBeesList[i].Copy(b);
                        }
                        else
                        {
                            bestBeesList[i].EliteCount = bestBeesList[i].EliteCount + 1;
                            if (bestBeesList[i].EliteCount > foragedThreshold)
                            {
                                foragedRegionsList.Add(bestBeesList[i]);
                            }
                        }
                    }
                    return;
                }
            bestBeesList.Add(new Bee(b));
        }
    }
}
