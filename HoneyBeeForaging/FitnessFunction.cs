using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace HoneyBeeForaging
{
    abstract class FitnessFunction
    {
        public enum FunctionType
        {
            Sphere = 0,
            Foxholes2D = 1,
            Ratrigin = 2,
            Ackley = 3,
            Griewank = 4,
            MovPeaks = 5,
            F1 = 6,
            F2 = 7,
            F3 = 8,
            F4 = 9,
            F5 = 10
        };

        //private FunctionType functionIndex;
        protected int functionEvaluations;
        protected int dimensions;
        protected double[,] searchSpace;
        protected double ngh;

        private double successThreshold;

        //private double globalFitnessEvaluations;
        //private double globalError;
        //private bool globalSuccess;

        //private double[] localFitnessEvaluations;
        //private double[] localError;
        //private bool localSuccess;

        protected int[] peakFitnessEvaluations;
        protected double[] peakError;
        private int peaksFound;
        //private bool success;

        protected Peak[] peaks;


        //public FitnessFunction(int d)
        //{
        //    //functionIndex = f;
        //    dimensions = d;
        //    //successThreshold = successThld;
        //    functionEvaluations = 0;
        //    searchSpace = new double[dimensions, 2];
        //    for (int i = 0; i < dimensions; i++)
        //    {
        //        searchSpace[i, 0] = 0;
        //        searchSpace[i, 1] = 100;
        //    }
        //    ngh = 1;
        //}
        public FitnessFunction(int d, double successThld)
        {
            //functionIndex = f;
            dimensions = d;
            successThreshold = successThld;
            functionEvaluations = 0;
            searchSpace = new double[dimensions, 2];
            //switch (functionIndex)
            //{
            //    case FunctionType.Sphere:
            //        break;
            //    case FunctionType.Foxholes2D:
            //        break;
            //    case FunctionType.Ratrigin:
            //        break;
            //    case FunctionType.Ackley:
            //        break;
            //    case FunctionType.Griewank:
            //        break;
            //}
            //peakError = new double[peaks.GetUpperBound(0) + 1];
            //peakFitnessEvaluations = new int[peaks.GetUpperBound(0) + 1];
        }
        public abstract double Evaluate(Bee b);
        public abstract double Evaluate(double[] b);
        //{
        //    double f = 0;
        //    functionEvaluations++;
        //    switch (functionIndex)
        //    {
        //        case FunctionType.Sphere: 
        //            f = Sphere(b.Position);
        //            break;
        //        case FunctionType.Foxholes2D:
        //            f = Foxholes2D(b.Position);
        //            break;
        //        case FunctionType.Ratrigin:
        //            f = Rastrigin(b.Position);
        //            break;
        //        case FunctionType.Ackley:
        //            f = Ackley(b.Position);
        //            break;
        //        case FunctionType.Griewank:
        //            f = Griewank(b.Position);
        //            break;
        //    }
        //    return f;
        //}
        protected void InitializePeaks(double[,] p)
        {
            peaks = new Peak[p.GetUpperBound(0) + 1];
            for (int i = 0; i <= p.GetUpperBound(0); i++)
            {
                peaks[i] = new Peak(dimensions);
                peaks[i].Fitness = p[i, 0];
                for (int j = 0; j < dimensions; j++)
                    peaks[i].Position[j] = p[i, j + 1];
                if (p.GetUpperBound(1) > dimensions)
                {
                    peaks[i].Width = p[i, dimensions + 1];
                    peaks[i].Height = p[i, dimensions + 2];
                }
            }
        }
 
        public bool CheckErrorThreshold(Bee b, double threshold)
        {
            double distance = Double.MaxValue;
            int closestPeak = 0;
            for (int i = 0; i <= peaks.Length; i++)
            {
                double temp = peaks[i].GetDistance(b.Position);
                //for (int k = 0; k < dimensions; k++)
                //    temp += (peaks[i, k + 1] - b.Position[k]) * (peaks[i, k + 1] - b.Position[k]);
                //temp = Math.Sqrt(temp);
                if (temp < distance)
                {
                    distance = temp;
                    closestPeak = i;
                }
            }
            if (Math.Abs(peaks[closestPeak].Fitness - b.BestFitness) < threshold)
                return true;
            else
                return false;

        }
        //public void CalculateStatistics(List<Bee> peaksFound)
        //{
        //    List<Bee> tempPeaks = new List<Bee>(peaksFound);
        //    Bee tempPeak = tempPeaks[0];
        //    double distance = Double.MaxValue;
        //    for (int i = 0; i <= peaks.Length; i++)
        //    {
        //        if (tempPeaks.Count > 0)
        //        {
        //            //tempPeak = tempPeaks[0];
        //            //peakError[i] = Math.Abs(peaks[i,0] - tempPeak.BestFitness);
        //            distance = Double.MaxValue;
        //            for (int j = 0; j < tempPeaks.Count; j++)
        //            {
        //                double temp = peaks[i].GetDistance(tempPeaks[j].Position);
        //                //for (int k = 0; k < dimensions; k++)
        //                //    temp += (peaks[i, k + 1] - tempPeaks[j].Position[k]) * (peaks[i, k + 1] - tempPeaks[j].Position[k]);
        //                //temp = Math.Sqrt(temp);
        //                if (temp < distance)
        //                {
        //                    distance = temp;
        //                    tempPeak = tempPeaks[j];
        //                }
        //                //if (Math.Abs(peaks[i, 0] - tempPeaks[j].BestFitness) < peakError[i])
        //                //{
        //                //    tempPeak = tempPeaks[j];
        //                //}
        //            }
        //            peakError[i] = Math.Abs(peaks[i].Fitness - tempPeak.BestFitness);
        //            peakFitnessEvaluations[i] = tempPeak.FitnessEvaluationsBest;
        //            tempPeaks.Remove(tempPeak);
        //        }
        //        else
        //        {
        //            peakError[i] = Math.Abs(peaks[i].Fitness - tempPeak.BestFitness);
        //            peakFitnessEvaluations[i] = tempPeak.FitnessEvaluationsBest;
        //        }
        //    }
        //}
        public void CalculateStatistics(List<Bee> peaksFoundList)
        {
            peaksFound = 0;
            List<Bee> tempPeaks = new List<Bee>(peaksFoundList);
            Bee tempPeak = tempPeaks[0];
            double distance = Double.MaxValue;
            for (int i = 0; i <= peaks.GetUpperBound(0); i++)
            {
                if (tempPeaks.Count > 0)
                {
                    //tempPeak = tempPeaks[0];
                    //peakError[i] = Math.Abs(peaks[i,0] - tempPeak.BestFitness);
                    distance = Double.MaxValue;
                    for (int j = 0; j < tempPeaks.Count; j++)
                    {
                        double temp = peaks[i].GetDistance(tempPeaks[j].Position);
                        //for (int k = 0; k < dimensions; k++)
                        //    temp += (peaks[i, k + 1] - tempPeaks[j].Position[k]) * (peaks[i, k + 1] - tempPeaks[j].Position[k]);
                        //temp = Math.Sqrt(temp);
                        if (temp < distance)
                        {
                            distance = temp;
                            tempPeak = tempPeaks[j];
                        }
                        //if (Math.Abs(peaks[i, 0] - tempPeaks[j].BestFitness) < peakError[i])
                        //{
                        //    tempPeak = tempPeaks[j];
                        //}
                    }
//                    peakError[i] = Math.Abs(peaks[i].Fitness - tempPeak.BestFitness);
//                    peakFitnessEvaluations[i] = tempPeak.FitnessEvaluationsBest;
                    //tempPeaks.Remove(tempPeak);
                }
//                else
//                {
                    peakError[i] = Math.Abs(peaks[i].Fitness - tempPeak.BestFitness);
                    peakFitnessEvaluations[i] = tempPeak.FitnessEvaluationsBest;
                    if (peakError[i] < successThreshold)
                        peaksFound++;
                    //Console.Write("[({0}", peaks[i].Fitness);
                    //for (int j = 0; j < peaks[i].Position.Length; j++)
                    //    Console.Write(", {0}", peaks[i].Position[j]);
                    //Console.Write(")({0}", tempPeak.BestFitness);
                    //for (int j = 0; j < tempPeak.BestPosition.Length; j++)
                    //    Console.Write(", {0}", tempPeak.BestPosition[j]);
                    //Console.Write(")]\t");
                    //Console.Write("{0}\t", peaks[i].Fitness);
                    //for (int j = 0; j < peaks[i].Position.Length; j++)
                    //    Console.Write("{0}\t", peaks[i].Position[j]);
                    //Console.Write("{0}\t", tempPeak.BestFitness);
                    //for (int j = 0; j < tempPeak.BestPosition.Length; j++)
                    //    Console.Write("{0}\t", tempPeak.BestPosition[j]);
                    //                }
            }
        }
        public void GenerateMatlabFile(double step, string fileName)
        {
            StreamWriter xFile = new StreamWriter("x-" + fileName);
            StreamWriter yFile = new StreamWriter("y-" + fileName);
            StreamWriter zFile = new StreamWriter("z-" + fileName);
            double[] b = new double[2];
            double x, y, z;
            for (x = searchSpace[0, 0]; x < searchSpace[0, 1]; x += step)
            {
                for (y = searchSpace[1, 0]; y < searchSpace[1, 1] - step; y += step)
                {
                    b[0] = x;
                    b[1] = y;
                    xFile.Write(x);
                    yFile.Write(y);
                    z = Evaluate(b);
                    zFile.Write(z);
                    xFile.Write("\t");
                    yFile.Write("\t");
                    zFile.Write("\t");
                }
                b[0] = x;
                b[1] = y;
                xFile.Write(x);
                yFile.Write(y);
                z = Evaluate(b); 
                zFile.Write(z);
                xFile.WriteLine();
                yFile.WriteLine();
                zFile.WriteLine();
            }
            xFile.Close();
            yFile.Close();
            zFile.Close();
                
        }

        public int GlobalFitnessEvaluations
        {
            get
            {
                return peakFitnessEvaluations[0];
            }
        }
        public double GlobalError
        {
            get
            {
                return peakError[0];
            }
        }
        public int[] LocalFitnessEvaluations
        {
            get
            {
                return peakFitnessEvaluations;
            }
        }
        public double LocalError
        {
            get
            {
                double temp = 0;
                for(int i = 0; i < peakError.Length; i++)
                {
                    temp += peakError[i];
                }
                temp = temp / peakError.Length;
                return temp;
            }
        }
        public int FunctionEvalutions
        {
            get
            {
                return functionEvaluations;
            }
        }
        public int Dimension
        {
            get
            {
                return dimensions;
            }
        }
        public double[,] SearchSpace
        {
            get
            {
                return searchSpace;
            }
            set
            {
                searchSpace = value;
            }
        }
        public double Neighborhood
        {
            get
            {
                return ngh;
            }
            set
            {
                ngh = value;
            }
        }
        public int PeaksFound
        {
            get
            {
                return peaksFound;
            }
        }
        public int TotalPeaks
        {
            get
            {
                return peaks.GetUpperBound(0) + 1;
            }
        }
        public double SuccessThreshold
        {
            get
            {
                return successThreshold;
            }
        }

    }
}
