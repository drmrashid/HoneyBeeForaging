using System;
using System.Collections.Generic;
using System.Text;

namespace HoneyBeeForaging
{
    class Statistics
    {
        private FitnessFunction.FunctionType functionType;
        private double successThreshold;
        private int statsCount;
        
        private List<int> globalFitnessEvaluations;
        private List<double> globalError;
//        private List<int> globalPeaksFound;
//        private int globalPeaks;
        private int globalSuccess;
        private int globalSuccessTrunc;
        
        private List<double> localError;
        private List<int> peaksFound;
        private int totalPeaksFound;
        private int totalPeaksFoundTrunc;
        private int allPeaksSuccess;
        private int allPeaksSuccessTrunc;
        private int totalPeaks;
        private int localSuccess;
        private int localSuccessTrunc;

        private List<int> firstPeakFitnessEvaluations;
        //private List<int> ninthPeakFitnessEvaluations;
        //private List<int> twentyfifthPeakFitnessEvaluations;
        private List<int> lastPeakFitnessEvaluations;
                
        private double meanGlobalFitnessEvaluations;
        private double stdDevGlobalFitnessEvaluations;
        private double meanGlobalError;
        private double stdDevGlobalError;
        private double meanLocalError;
        private double stdDevLocalError;
        private double meanFirstPeakFitnessEvaluations;
        private double stdDevFirstPeakFitnessEvaluations;
        //private double meanNinthPeakFitnessEvaluations;
        //private double meanTwentyfifthPeakFitnessEvaluations;
        private double meanLastPeakFitnessEvaluations;
        private double stdDevLastPeakFitnessEvaluations;

        private double meanGlobalFitnessEvaluationsTrunc;
        private double stdDevGlobalFitnessEvaluationsTrunc;
        private double meanGlobalErrorTrunc;
        private double stdDevGlobalErrorTrunc;
        private double meanLocalErrorTrunc;
        private double stdDevLocalErrorTrunc;
        private double meanFirstPeakFitnessEvaluationsTrunc;
        private double stdDevFirstPeakFitnessEvaluationsTrunc;
        //private double meanNinthPeakFitnessEvaluations;
        //private double meanTwentyfifthPeakFitnessEvaluations;
        private double meanLastPeakFitnessEvaluationsTrunc;
        private double stdDevLastPeakFitnessEvaluationsTrunc;
        
        public Statistics(FitnessFunction.FunctionType funcType)
        {
            functionType = funcType;
            //successThreshold = successThld;
            globalSuccess = 0;
            localSuccess = 0;
            statsCount = 0;

            globalFitnessEvaluations = new List<int>();
            globalError = new List<double>();
            localError = new List<double>();
            firstPeakFitnessEvaluations = new List<int>();
            //ninthPeakFitnessEvaluations = new List<int>();
            //twentyfifthPeakFitnessEvaluations = new List<int>();
            lastPeakFitnessEvaluations = new List<int>();
            peaksFound = new List<int>();

            meanGlobalFitnessEvaluations = 0;
            meanGlobalError = 0;
            stdDevGlobalError = 0;
            meanLocalError = 0;
            stdDevLocalError = 0;
            meanFirstPeakFitnessEvaluations = 0;
            //meanNinthPeakFitnessEvaluations = 0;
            //meanTwentyfifthPeakFitnessEvaluations = 0;
            meanLastPeakFitnessEvaluations = 0;
        }
        public void AddStatistic(FitnessFunction f, bool verbose)
        {
            statsCount++;
            successThreshold = f.SuccessThreshold;
            
            globalFitnessEvaluations.Add(f.GlobalFitnessEvaluations);
            if (verbose)
                Console.Write("{0}\t", f.GlobalFitnessEvaluations);
            
            globalError.Add(f.GlobalError);
            if (verbose)
                Console.Write("{0:E2}\t", f.GlobalError);
            
            if (f.GlobalError < successThreshold)
                globalSuccess++;
            
            localError.Add(f.LocalError);
            if (verbose)
                Console.Write("{0:E2}\t", f.LocalError);
            
            if (f.LocalError < successThreshold)
                localSuccess++;
            int[] temp = f.LocalFitnessEvaluations;
            Array.Sort(temp);

            firstPeakFitnessEvaluations.Add(temp[0]);
            if (verbose)
                Console.Write("{0}\t", firstPeakFitnessEvaluations[firstPeakFitnessEvaluations.Count - 1]);
            /*
            if (temp.Length > 7)
                ninthPeakFitnessEvaluations.Add(temp[8]);
            else
                ninthPeakFitnessEvaluations.Add(0);
            if (verbose)
                Console.Write("{0}\t", ninthPeakFitnessEvaluations[ninthPeakFitnessEvaluations.Count - 1]);
            
            if (temp.Length > 24)
                twentyfifthPeakFitnessEvaluations.Add(temp[24]);
            else
                twentyfifthPeakFitnessEvaluations.Add(0);
            if (verbose)
                Console.Write("{0}\t", twentyfifthPeakFitnessEvaluations[twentyfifthPeakFitnessEvaluations.Count - 1]);
            */
            lastPeakFitnessEvaluations.Add(temp[temp.Length - 1]);
            if (verbose)
                Console.Write("{0}\t", lastPeakFitnessEvaluations[lastPeakFitnessEvaluations.Count - 1]);

            peaksFound.Add(f.PeaksFound);
            if (verbose)
                Console.Write("{0}\t", peaksFound[peaksFound.Count - 1]);

            if (f.PeaksFound == f.TotalPeaks)
                allPeaksSuccess++;

            totalPeaks = f.TotalPeaks;

            if (verbose)
                Console.WriteLine();
        }
        public void CalculateMean()
        {
            globalError.Sort();
            globalFitnessEvaluations.Sort();
            firstPeakFitnessEvaluations.Sort();
            lastPeakFitnessEvaluations.Sort();
            for (int i = 0; i < statsCount; i++)
            {
                meanGlobalError += globalError[i];
                meanGlobalFitnessEvaluations += globalFitnessEvaluations[i];
                meanLocalError += localError[i];
                meanFirstPeakFitnessEvaluations += firstPeakFitnessEvaluations[i];
                //meanNinthPeakFitnessEvaluations += ninthPeakFitnessEvaluations[i];
                //meanTwentyfifthPeakFitnessEvaluations += twentyfifthPeakFitnessEvaluations[i];
                meanLastPeakFitnessEvaluations += lastPeakFitnessEvaluations[i];
                totalPeaksFound += peaksFound[i];
                if (i >= statsCount * 0.1 && i < statsCount - statsCount * 0.1)
                {
                    meanGlobalErrorTrunc += globalError[i];
                    meanGlobalFitnessEvaluationsTrunc += globalFitnessEvaluations[i];
                    meanLocalErrorTrunc += localError[i];
                    meanFirstPeakFitnessEvaluationsTrunc += firstPeakFitnessEvaluations[i];
                    meanLastPeakFitnessEvaluationsTrunc += lastPeakFitnessEvaluations[i];
                    totalPeaksFoundTrunc += peaksFound[i];
                    if (globalError[i] < successThreshold)
                        globalSuccessTrunc++;
                    if (localError[i] < successThreshold)
                        localSuccessTrunc++;
                    if (peaksFound[i] == totalPeaks)
                        allPeaksSuccessTrunc++;
                }
            }
            meanGlobalError /= statsCount;
            meanGlobalFitnessEvaluations /= statsCount;
            meanLocalError /= statsCount;
            meanFirstPeakFitnessEvaluations /= statsCount;
            //meanNinthPeakFitnessEvaluations /= statsCount;
            //meanTwentyfifthPeakFitnessEvaluations /= statsCount;
            meanLastPeakFitnessEvaluations /= statsCount;

            meanGlobalErrorTrunc /= statsCount - statsCount * 0.2;
            meanGlobalFitnessEvaluationsTrunc /= statsCount - statsCount * 0.2;
            meanLocalErrorTrunc/= statsCount - statsCount * 0.2;
            meanFirstPeakFitnessEvaluationsTrunc /= statsCount - statsCount * 0.2;
            meanLastPeakFitnessEvaluationsTrunc /= statsCount - statsCount * 0.2;
            for (int i = 0; i < statsCount; i++)
            {
                stdDevGlobalError += Math.Abs(globalError[i] - meanGlobalError);
                stdDevLocalError += Math.Abs(localError[i] - meanLocalError);
                stdDevGlobalFitnessEvaluations += Math.Abs(globalFitnessEvaluations[i] - meanGlobalFitnessEvaluations);
                stdDevFirstPeakFitnessEvaluations += Math.Abs(firstPeakFitnessEvaluations[i] - meanFirstPeakFitnessEvaluations);
                stdDevLastPeakFitnessEvaluations += Math.Abs(lastPeakFitnessEvaluations[i] - meanLastPeakFitnessEvaluations);
                if (i >= statsCount * 0.1 && i < statsCount - statsCount * 0.1)
                {
                    stdDevGlobalErrorTrunc += Math.Abs(globalError[i] - meanGlobalErrorTrunc);
                    stdDevLocalErrorTrunc += Math.Abs(localError[i] - meanLocalErrorTrunc);
                    stdDevGlobalFitnessEvaluationsTrunc += Math.Abs(globalFitnessEvaluations[i] - meanGlobalFitnessEvaluationsTrunc);
                    stdDevFirstPeakFitnessEvaluationsTrunc += Math.Abs(firstPeakFitnessEvaluations[i] - meanFirstPeakFitnessEvaluationsTrunc);
                    stdDevLastPeakFitnessEvaluationsTrunc += Math.Abs(lastPeakFitnessEvaluations[i] - meanLastPeakFitnessEvaluationsTrunc);
                }
            }
            stdDevGlobalError /= statsCount;
            stdDevLocalError /= statsCount;
            stdDevGlobalFitnessEvaluations /= statsCount;
            stdDevGlobalFitnessEvaluationsTrunc /= statsCount - statsCount * 0.2;
            stdDevGlobalErrorTrunc /= statsCount - statsCount * 0.2;
            stdDevLocalErrorTrunc /= statsCount - statsCount * 0.2;
            stdDevFirstPeakFitnessEvaluations /= statsCount;
            stdDevLastPeakFitnessEvaluations /= statsCount;
            stdDevFirstPeakFitnessEvaluationsTrunc /= statsCount - statsCount * 0.2;
            stdDevLastPeakFitnessEvaluationsTrunc /= statsCount - statsCount * 0.2;

        }
        public void Write()
        {
            CalculateMean();
            Console.WriteLine("=============================================================");
            Console.WriteLine("Function                             :\t {0}", functionType.ToString());
            Console.WriteLine("Mean Fitness Evaluations [Global]    :\t {0:F2}", meanGlobalFitnessEvaluations);
            Console.WriteLine("Mean Error [Global]                  :\t {0:E2}", meanGlobalError);
            Console.WriteLine("Std. Dev. Error [Global]             :\t {0:E2}", stdDevGlobalError);
            Console.WriteLine("Sucess [Peaks Found]                 :\t {0:F2}%", 100 * (double)totalPeaksFound / (totalPeaks * statsCount));
            Console.WriteLine("Sucess [All Peaks Found]             :\t {0:F2}%", 100 * (double)allPeaksSuccess / statsCount);
            Console.WriteLine("Sucess [Global]                      :\t {0:F2}%", 100 * (double)globalSuccess / statsCount);
            Console.WriteLine("Mean Error [Local]                   :\t {0:E2}", meanLocalError);
            Console.WriteLine("Std. Dev. Error [Local]              :\t {0:E2}", stdDevLocalError);
            Console.WriteLine("Success [Local]                      :\t {0:F2}%", 100 * (double)localSuccess / statsCount);
            Console.WriteLine("Mean Fitness Evaluations [1st Peak]  :\t {0:F2}", meanFirstPeakFitnessEvaluations);
            //Console.WriteLine("Mean Fitness Evaluations [9th Peak]  : {0:F2}", meanNinthPeakFitnessEvaluations);
            //Console.WriteLine("Mean Fitness Evaluations [25th Peak] : {0:F2}", meanTwentyfifthPeakFitnessEvaluations);
            Console.WriteLine("Mean Fitness Evaluations [Last Peak] :\t {0:F2}", meanLastPeakFitnessEvaluations);
            Console.WriteLine("Truncated Mean");
            Console.WriteLine("Mean Fitness Evaluations [Global]    :\t {0:F2}", meanGlobalFitnessEvaluationsTrunc);
            Console.WriteLine("Mean Error [Global]                  :\t {0:E2}", meanGlobalErrorTrunc);
            Console.WriteLine("Std. Dev. Error [Global]             :\t {0:E2}", stdDevGlobalErrorTrunc);
            Console.WriteLine("Sucess [Peaks Found]                 :\t {0:F2}%", 100 * (double)totalPeaksFoundTrunc / (totalPeaks * (statsCount * 0.8)));
            Console.WriteLine("Sucess [All Peaks Found]             :\t {0:F2}%", 100 * (double)allPeaksSuccessTrunc / (statsCount * 0.8));
            Console.WriteLine("Sucess [Global]                      :\t {0:F2}%", 100 * (double)globalSuccessTrunc / (statsCount * 0.8));
            Console.WriteLine("Mean Error [Local]                   :\t {0:E2}", meanLocalErrorTrunc);
            Console.WriteLine("Std. Dev. Error [Local]              :\t {0:E2}", stdDevLocalErrorTrunc);
            Console.WriteLine("Success [Local]                      :\t {0:F2}%", 100 * (double)localSuccessTrunc / (statsCount * 0.8));
            Console.WriteLine("Mean Fitness Evaluations [1st Peak]  :\t {0:F2}", meanFirstPeakFitnessEvaluationsTrunc);
            //Console.WriteLine("Mean Fitness Evaluations [9th Peak]  : {0:F2}", meanNinthPeakFitnessEvaluations);
            //Console.WriteLine("Mean Fitness Evaluations [25th Peak] : {0:F2}", meanTwentyfifthPeakFitnessEvaluations);
            Console.WriteLine("Mean Fitness Evaluations [Last Peak] :\t {0:F2}", meanLastPeakFitnessEvaluationsTrunc);
        }
        public void WriteBrief()
        {
            CalculateMean();
            Console.Write("{0:E2}\t", meanGlobalError);
            Console.Write("{0:E2}\t", stdDevGlobalError);
            Console.Write("{0:F2}%\t", 100 * (double)globalSuccess / statsCount);
            Console.Write("{0:F2}\t", meanGlobalFitnessEvaluations);
            Console.Write("{0:F2}\t", stdDevGlobalFitnessEvaluations);
            Console.Write("{0:E2}\t", meanLocalError);
            Console.Write("{0:E2}\t", stdDevLocalError);
            Console.Write("{0:F2}%\t", 100 * (double)localSuccess / statsCount);
            Console.Write("{0:F2}\t", meanLastPeakFitnessEvaluations);
            Console.WriteLine("{0:F2}", stdDevLastPeakFitnessEvaluations);
        }
    }
}
