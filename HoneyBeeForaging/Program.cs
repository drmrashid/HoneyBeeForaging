using System;
using System.Collections.Generic;
using System.Text;

namespace HoneyBeeForaging
{
    class Program
    {
        static void Main(string[] args)
        {
            //Random r = new Random();
            //FitnessFunction f = new FitnessFunction(FitnessFunction.FunctionType.Sphere);

            //Bee[] b = new Bee[10];
            //double[,] max_x = {
            //                {-5,5},
            //                {-5,5}
            //                };
            
            //for (int i = 0; i < b.Length; i++)
            //{
            //    b[i] = new Bee(2, f, r);
            //    b[i].Initialize(max_x, max_x); 
            //}
            //Swarm s = new Swarm(f, b,Swarm.TerminationCriteria.Iterations,true,true);
            //s.K = 0.729844;
            //s.C1 = 2.05;
            //s.C2 = 2.05;
            ////s.MaxEvaluations = 40000;
            //s.MaxIterations = 200;
            //s.Neighborhood = max_x;
            //s.Fly();
            //System.Console.Write(s.BestBee.BestFitness);
//            FitnessFunction f = new FitnessFunction(FitnessFunction.FunctionType.Ratrigin, 2);
//            //Random r = new Random();
//            //Bee b = new Bee(f, r);
//            double[] result = new double[3];
//            double temp;
//            result[0] = Double.MaxValue;
//            for(double i = -32.01; i < -31.09; i+=.00000000000001)
//            for (double j = -32.01; j < -31.09; j += .00000000000001)
//            {
//                //b.Position = new double[] { i, j };
//                //temp = f.Evaluate(b);
//                temp = f.Rastrigin(new double[] { i, j });
//                if (temp < result[0])
//                {
//                    result[0] = temp;
//                    result[1] = i;
//                    result[2] = j;
//                }
////                Console.WriteLine("[{0:F3},{1:F3}]{2}", b.Position[0], b.Position[1], f.Evaluate(b));
//            }
//        Console.WriteLine("{0:E16}[{1:E16,{2:E16}]", result[0], result[1], result[2]);
            //double[,] max_x = {
            //                {-65.535,65.535},
            //                {-65.535,65.535}
            //                };
            //double[,] max_x = {
            //                {-5.12,5.12},
            //                {-5.12,5.12}
            //                };
            //for (int i = 0; i < 200; i++)
            //{
            //FitnessFunction f = new FitnessFunction(FitnessFunction.FunctionType.Griewank, 2);
            //Hive h = new Hive(f, 20, 3, 5, 15, 500, 30, 0.0001);
            //h.Run();
            //}

            /***************************************************************************************
            // Calculating graph between swarm size and fitness evaluations
            double[] avg = new double[101];
            for (int i = 2; i <= 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    FitnessFunction f = new FitnessFunction(FitnessFunction.FunctionType.Sphere, 2);
                    Random r = new Random();
                    Bee[] bees = new Bee[i];
                    for (int k = 0; k < i; k++)
                    {
                        bees[k] = new Bee(f, r);
                        bees[k].Initialize(f.SearchSpace, f.SearchSpace);
                    }
                    Swarm s = new Swarm(f, bees, Swarm.TerminationCriteria.Error, true, false);
                    s.MaxError = 0.01;
                    s.K = 0.729844;
                    s.C1 = 2.05;
                    s.C2 = 2.05;
                    s.Neighborhood = f.SearchSpace;
                    s.Fly();
                    avg[i] += f.FunctionEvalutions;
                }
                avg[i] /= 100;
                Console.WriteLine("{0}\t{1}", i, avg[i]);

            }
            ***************************************************************************************/

            /***************************************************************************************
            // Multimodal problem run
            foreach (FitnessFunction.FunctionType fType in Enum.GetValues(typeof(FitnessFunction.FunctionType)))
            {*/
            //for (int j = 5; j <= 200; j+=5)
            //{
            //    Statistics stats = new Statistics(FitnessFunction.FunctionType.F5);
            //    for (int i = 0; i < 50; i++)
            //    {
            //        FitnessFunction f = new F5(2, 0.0001);
            //        Hive h = new Hive(f, 15, 4, 3, j, 2000/j, 40);
            //        h.Run();
            //        stats.AddStatistic(f, false);
            //    }
            //    Console.Write("{0}\t{1}\t",j,2000/j);
            //    stats.WriteBrief();
            //}
            //Console.ReadKey(true);

            //for (int j = 0; j < 100; j++)
            //{
            //    Statistics stats = new Statistics(FitnessFunction.FunctionType.F5);
            //    for (int i = 0; i < 50; i++)
            //    {
            //        FitnessFunction f = new F5(2, 0.0001);
            //        Hive h = new Hive(f, 20, 5, 3, 40, 50, 40);
            //        h.Run();
            //        stats.AddStatistic(f, false);
            //    }
            //    stats.WriteBrief();
            //}
            //Console.ReadKey(true);

            //for (int j = 2; j <= 10; j++)
            //{
            //    Statistics stats = new Statistics(FitnessFunction.FunctionType.Ratrigin);
            //    for (int i = 0; i < 50; i++)
            //    {
            //        FitnessFunction f = new Rastrigin(j, 0.0001);
            //        //Hive h;
            //        //switch (j)
            //        //{
            //        //    case 2: h = new Hive(f, 36, 9, 3, 40, 50, 40);
            //        //        h.Run();
            //        //        break;
            //        //    case 3: h = new Hive(f, 108, 27, 3, 40, 50, 40);
            //        //        h.Run();
            //        //        break;
            //        //    case 4: h = new Hive(f, 324, 81, 3, 40, 50, 40);
            //        //        h.Run();
            //        //        break;
            //        //    case 5: h = new Hive(f, 972, 243, 3, 40, 50, 40);
            //        //        h.Run();
            //        //        break;
            //        //    case 6: h = new Hive(f, 972, 243, 3, 40, 50, 40);
            //        //        h.Run();
            //        //        break;
            //        //    case 7: h = new Hive(f, 972, 243, 3, 40, 50, 40);
            //        //        h.Run();
            //        //        break;
            //        //    case 8: h = new Hive(f, 972, 243, 3, 40, 50, 40);
            //        //        h.Run();
            //        //        break;
            //        //    case 9: h = new Hive(f, 972, 243, 3, 40, 50, 40);
            //        //        h.Run();
            //        //        break;
            //        //    case 10: h = new Hive(f, 972, 243, 3, 40, 50, 40);
            //        //        h.Run();
            //        //        break;
            //        //}
            //        Hive h = new Hive(f, 30, 8, 3, 40, 50, 40);
            //        h.Run();
            //        stats.AddStatistic(f, false);
            //    }
            //    stats.WriteBrief();
            //}
            //Console.ReadKey(true);

            /*            }
            ***************************************************************************************/

            /***************************************************************************************
            // Simple PSO
            foreach (FitnessFunction.FunctionType fType in Enum.GetValues(typeof(FitnessFunction.FunctionType)))
            {
                Statistics stats = new Statistics(fType, 0.0001);
                for (int i = 0; i < 100; i++)
                {
                    FitnessFunction f = new FitnessFunction(fType, 2);
                    Random r = new Random();
                    Bee[] bees = new Bee[10];
                    for (int k = 0; k < 10; k++)
                    {
                        bees[k] = new Bee(f, r);
                        bees[k].Initialize(f.SearchSpace, f.SearchSpace);
                    }
                    Swarm s = new Swarm(f, bees, Swarm.TerminationCriteria.Error, true, false);
                    s.MaxError = 0.0001;
                    s.MaxIterations = 4500;
                    s.K = 0.729844;
                    s.C1 = 2.05;
                    s.C2 = 2.05;
                    s.Neighborhood = f.SearchSpace;
                    s.Fly();
                    List<Bee> temp = new List<Bee>();
                    temp.Add(s.BestBee);
                    f.CalculateStatistics(temp);

                    //Hive h = new Hive(f, 10, 2, 3, 15, 300, 30);
                    //h.Run();
                    stats.AddStatistic(f, true);
                }
                stats.Write();
            }
            ***************************************************************************************/
 // int i;
 // double dummy;
 // /* just an arbitrary individual to test functionality */
 // double[] genotype = { 51.0, 30.0 };
 //   //  { 5.0, 10.0, 12.0, 15.5, 2.3};
 // /*int n;
 // for (n = 1; n < argc; n ++)
 //   handle(argv[n]);*/
 // /* initialize peaks at beginning */
 // MovPeaks m = new MovPeaks(2);
 // double[,] max_x = {
 //                 {0,100},
 //                 {0,100}
 //                 };
 // m.SearchSpace = max_x;
  

 // /* evaluation of an individual */
 // dummy = m.Evaluate(genotype);
 // Console.WriteLine(dummy);

 // /* change the peaks */
 // for (i=0; i<50; i++)
 //   m.ChangePeaks();
  
 // dummy = m.Evaluate(genotype);
 //Console.WriteLine(dummy);
            //for(int s = 1; s <= 10; s++)
            //    for(int e = 2; e <= 10; e++)
            //        for (int r = 0; r <= 10; r++)
            //        {
            //            MovPeaks f = new MovPeaks(2, 0.001);
            //            f.NumberOfPeaks = 5;
            //            f.ChangeFrequency = 0;
            //            f.InitializeSavedPeaks("peaks.txt");
            //            //f.GenerateMatlabFile(1, "movpeaks.txt");
            //            //Rastrigin f = new Rastrigin(2, 0.00001);
            //            Hive h = new Hive(f, s * e + r, s, e, 15, 100, 30, 0.001);
            //            h.Run();
            //            Console.WriteLine("{0}\t{1}\t{2}\t{3:F2}\t{4}\t{5}",s,e,r,100*((double)f.PeaksFound/f.TotalPeaks),f.FunctionEvalutions, f.LocalError);
            //        }


            //FitnessFunction f = new FitnessFunction(2);
            //f.NumberOfPeaks = 5;
            //Hive h = new Hive(f, 20, 5, 3, 15, 500, 15, 0.0001);
            //h.Run();

            for (int i = 1; i < 41; i++)
            {
                double[] offlineError = new double[10];
                double meanOfflineError = 0;
                for (int j = 0; j < 10; j++)
                {
                    //FitnessFunction f = new FitnessFunction(2);
                    //f.NumberOfPeaks = 10;
                    MovPeaks f = new MovPeaks(2, 0.0001, 5);
                    //f.Neighborhood = i * 0.05;
                    //f.ChangeFrequency = 1;
                    //f.NumberOfPeaks = 10;
                    //f.InitializeSavedPeaks("peaks.txt");
                    //f.InitializeR

                    //string fileName = i + "-" + j + ".txt";
                    //f.SavePeaks(fileName);
                    //f.SaveToFile("test.xml");
                    Hive h = new Hive(f, 60, 15, 3, i*5, 1000, 40);
                    h.Run();
                    offlineError[j] = f.GetOfflineError();
                    meanOfflineError += offlineError[j];
                    //                    Console.WriteLine("{0}", f.GetOfflineError());
                    //Console.WriteLine("{0}", f.GetOfflinePerformance());
                    Console.Write(".");
                }
                meanOfflineError /= 10;
                double stdDevOfflineError = 0;
                for (int j = 0; j < 10; j++)
                    stdDevOfflineError += Math.Abs(meanOfflineError - offlineError[j]);
                stdDevOfflineError /= 10;
                Console.WriteLine();
                Console.WriteLine("{0}\t\t{1:F6}\t\t{2:F6}", i, meanOfflineError, stdDevOfflineError);
          }
          Console.ReadKey();

            //FitnessFunction f = new FitnessFunction(2);
            //f.NumberOfPeaks = 5;
            //MovPeaks f = new MovPeaks(2);
            //f.NumberOfPeaks = 5;
            //Hive h = new Hive(f, 20, 5, 3, 15, 7500, 15, 0.0001);
            //h.Run();

        }
    }
}
