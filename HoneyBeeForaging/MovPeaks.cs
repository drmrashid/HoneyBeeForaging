using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace HoneyBeeForaging
{
    delegate double BasisFunctionDelegate(double[] b);
    delegate double PeakFunctionDelegate(double[] b, int peakNumber);

    class MovPeaks : FitnessFunction
    {
        //private int functionEvaluations;
        //private int dimensions;
        //private double[,] searchSpace;
        //private double ngh;

        private Random r;
        private int changeFrequency = 5000;
        private double vLength = 1.0;
        private double heightSeverity = 7.0;
        private double widthSeverity = 0.01;
        private double lambda = 0.0;
        private int numberOfPeaks = 10;
        private bool useBasisFunction = false;
        private bool calculateAverageError = true;
        private bool calculateOfflinePerformance = true;
        private bool calculateRightPeak = false;
        private double minHeight = 30.0;
        private double maxHeight = 70.0;
        private double standardHeight = 0.0;
        
        private double minWidth = 1.0;
        private double maxWidth = 12.0;
        private double standardWidth = 0.0;

        private bool recentChange = true; /* indicates that a change has just ocurred */
        private int currentPeak;      /* peak on which the current best individual is located */
        private int minimumPeak;       /* number of highest peak */
        private double currentMinimum; /* fitness value of currently best individual */
        private double offlinePerformance = 0.0;
        private double offlineError = 0.0;
        private double avgError = 0;    /* average error so far */
        private double currentError = 0;/* error of the currently best individual */
        private double globalMin;     /* absolute maximum in the fitness landscape */
        //private int evals = 0;         /* number of evaluations so far */
        //private double[][] peak;         /* data structure to store peak data */
        //private Peak[] peaks;
        private double[] shift;
        private double[] coordinates;
        private int[] coveredPeaks;    /* which peaks are covered by the population ? */

        //private int[] peakFitnessEvaluations;
        //private double[] peakError;

        private double[][] prevMovement;/* to store every peak's previous movement */

        private bool backup = false;
        private double x2;

        private BasisFunctionDelegate BasisFunction;
        private PeakFunctionDelegate PeakFunction;

        public MovPeaks(int d, double successThld, int noOfPeaks )
            : base(d, successThld)
        {
            numberOfPeaks = noOfPeaks;
            int i;
            //double dummy;

            r = new Random();

            //dimensions = d;
            //successThreshold = successThld;
            //functionEvaluations = 0;
            //searchSpace = new double[dimensions, 2];
            for (i = 0; i < dimensions; i++)
            {
                searchSpace[i, 0] = 0;
                searchSpace[i, 1] = 100;
            }
            ngh = 2;


            PeakFunction = PeakFunction1;
            peakError = new double[numberOfPeaks];
            peakFitnessEvaluations = new int[numberOfPeaks];


            shift = new double[dimensions]; // (double*)calloc(geno_size, sizeof(double));
            coordinates = new double[dimensions]; // (double*)calloc(geno_size, sizeof(double));
            coveredPeaks = new int[numberOfPeaks]; // (int*)calloc(number_of_peaks, sizeof(int));
            //peak = new double[numberOfPeaks][]; //(double**)calloc(number_of_peaks, sizeof(double*));
            peaks = new Peak[numberOfPeaks];
            prevMovement = new double[numberOfPeaks][]; // (double**)calloc(number_of_peaks, sizeof(double*));
            for (i = 0; i < numberOfPeaks; i++)
            {
                //peak[i] = new double[dimensions + 2]; //(double*)calloc(geno_size + 2, sizeof(double));
                peaks[i] = new Peak(dimensions);
                prevMovement[i] = new double[dimensions]; //(double*)calloc(geno_size, sizeof(double));
            }
            InitializeRandomPeaks();

        }

        private void InitializeRandomPeaks()
        {
            int i, j;
            for (i = 0; i < numberOfPeaks; i++)
                for (j = 0; j < dimensions; j++)
                {
                    //peak[i][j] = 100.0 * r.NextDouble(); // movrand();
                    peaks[i].Position[j] = 100.0 * r.NextDouble(); // movrand();
                    prevMovement[i][j] = r.NextDouble() - 0.5;
                }
            if (standardHeight <= 0.0)
                for (i = 0; i < numberOfPeaks; i++)
                    //peak[i][dimensions + 1] = (maxHeight - minHeight) * r.NextDouble() + minHeight;
                    peaks[i].Height = (maxHeight - minHeight) * r.NextDouble() + minHeight;

            else
                for (i = 0; i < numberOfPeaks; i++)
                    //peak[i][dimensions + 1] = standardHeight;
                    peaks[i].Height = standardHeight;
            if (standardWidth <= 0.0)
                for (i = 0; i < numberOfPeaks; i++)
                    //peak[i][dimensions] = (maxWidth - minWidth) * r.NextDouble() + minWidth;
                    peaks[i].Width = (maxWidth - minWidth) * r.NextDouble() + minWidth;
            else
                for (i = 0; i < numberOfPeaks; i++)
                    //peak[i][dimensions] = standardWidth;
                    peaks[i].Width = standardWidth;
            //return j;
            if (calculateAverageError)
            {
                globalMin = Double.MaxValue;// -100000.0;
                for (i = 0; i < numberOfPeaks; i++)
                {
                    //for (j = 0; j < dimensions; j++)
                    //coordinates[j] = peak[i][j];
                    //coordinates[j] = peaks[i].Position[j];
                    peaks[i].Fitness = EvaluateDummy(peaks[i].Position);
                    if (peaks[i].Fitness < globalMin)
                        globalMin = peaks[i].Fitness;
                }
            }
        }
        public void SavePeaks(string fileName)
        {
            StreamWriter sw = new StreamWriter(fileName);
            for (int i = 0; i < numberOfPeaks; i++)
                sw.WriteLine(peaks[i].SaveToString());
            sw.Close();
        }
        public void InitializeSavedPeaks(string fileName)
        {
            StreamReader sr = new StreamReader(fileName);
            List<string> lines = new List<string>();
            string line;
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                lines.Add(line);
            }
            sr.Close();
            peaks = new Peak[numberOfPeaks];
            for (int i = 0; i < numberOfPeaks; i++)
            {
                peaks[i] = new Peak(0);
                peaks[i].LoadFromString(lines[i]);
            }
            
            //int i, j;
            //for (i = 0; i < numberOfPeaks; i++)
            //    for (j = 0; j < dimensions; j++)
            //    {
            //        //peak[i][j] = 100.0 * r.NextDouble(); // movrand();
            //        peaks[i].Position[j] = 100.0 * r.NextDouble(); // movrand();
            //        prevMovement[i][j] = r.NextDouble() - 0.5;
            //    }
            //if (standardHeight <= 0.0)
            //    for (i = 0; i < numberOfPeaks; i++)
            //        //peak[i][dimensions + 1] = (maxHeight - minHeight) * r.NextDouble() + minHeight;
            //        peaks[i].Height = (maxHeight - minHeight) * r.NextDouble() + minHeight;

            //else
            //    for (i = 0; i < numberOfPeaks; i++)
            //        //peak[i][dimensions + 1] = standardHeight;
            //        peaks[i].Height = standardHeight;
            //if (standardWidth <= 0.0)
            //    for (i = 0; i < numberOfPeaks; i++)
            //        //peak[i][dimensions] = (maxWidth - minWidth) * r.NextDouble() + minWidth;
            //        peaks[i].Width = (maxWidth - minWidth) * r.NextDouble() + minWidth;
            //else
            //    for (i = 0; i < numberOfPeaks; i++)
            //        //peak[i][dimensions] = standardWidth;
            //        peaks[i].Width = standardWidth;
            //return j;
            if (calculateAverageError)
            {
                globalMin = Double.MaxValue;// -100000.0;
                for (int i = 0; i < numberOfPeaks; i++)
                {
                    //for (j = 0; j < dimensions; j++)
                    //coordinates[j] = peak[i][j];
                    //coordinates[j] = peaks[i].Position[j];
                    peaks[i].Fitness = EvaluateDummy(peaks[i].Position);
                    if (peaks[i].Fitness < globalMin)
                        globalMin = peaks[i].Fitness;
                }
            }
        }
        void CurrentPeakCalc(double[] b)
        {
            int i;
            double minimum = Double.MaxValue/*-100000.0*/, dummy;

            currentPeak = 0;
            minimum = PeakFunction(b, 0);
            for (i = 1; i < numberOfPeaks; i++)
            {
                dummy = PeakFunction(b, i);
                if (dummy < minimum)
                {
                    minimum = dummy;
                    currentPeak = i;
                }
            }
        }
        public override double Evaluate(Bee b)
        {
            return Evaluate(b.Position);
        }
        public override double Evaluate(double[] b)
        {
            int i;
            double minimum = Double.MaxValue/*-100000.0*/, dummy;

            if ((changeFrequency > 0) && (functionEvaluations % changeFrequency == 0))
                ChangePeaks();

            for (i = 0; i < numberOfPeaks; i++)
            {
                dummy = PeakFunction(b, i);
                if (dummy < minimum)
                    minimum = dummy;
            }

            if (useBasisFunction)
            {

                dummy = BasisFunction(b);
                /* If value of basis function is higher return it */
                if (minimum > dummy)
                    minimum = dummy;
            }
            if (calculateAverageError)
            {
                avgError += minimum - globalMin;
            }
            if (calculateOfflinePerformance)
            {
                if (recentChange || (minimum < currentMinimum))
                {
                    currentError = minimum - globalMin;
                    if (calculateRightPeak)
                        CurrentPeakCalc(b);
                    currentMinimum = minimum;
                    recentChange = false;
                }
                offlinePerformance += currentMinimum;
                offlineError += currentError;
            }
            functionEvaluations++;     /* increase the number of evaluations by one */
            return (minimum);
        }
        private double EvaluateDummy(double[] b)
        {
            int i;
            double minimum = Double.MaxValue/*-100000.0*/, dummy;

            for (i = 0; i < numberOfPeaks; i++)
            {
                dummy = PeakFunction(b, i);
                if (dummy < minimum)
                    minimum = dummy;
            }

            if (useBasisFunction)
            {

                dummy = BasisFunction(b);
                /* If value of basis function is higher return it */
                if (minimum > dummy)
                    minimum = dummy;
            }
            return (minimum);
        }
        private double MovNRand()
        {
            double x1, w;

            if (backup)
            {
                backup = false;
                return x2;
            }
            else
            {
                do
                {
                    x1 = 2.0 * r.NextDouble() - 1.0;
                    x2 = 2.0 * r.NextDouble() - 1.0;
                    w = x1 * x1 + x2 * x2;
                } while (w >= 1.0);
                w = Math.Sqrt((-2.0 * Math.Log(w)) / w);
                x2 = w * x2;
                backup = true;
                return (x1 * w);
            }
        }
        public void ChangePeaks()
        {
            //Console.Write("+");
            int i, j;
            double sum, sum2, offset/*, dummy*/;

            for (i = 0; i < numberOfPeaks; i++)
            {
                /* shift peak locations */
                sum = 0.0;
                for (j = 0; j < dimensions; j++)
                {
                    shift[j] = r.NextDouble() - 0.5;
                    sum += shift[j] * shift[j];
                }
                if (sum > 0.0)
                    sum = vLength / Math.Sqrt(sum);
                else                           /* only in case of rounding errors */
                    sum = 0.0;
                sum2 = 0.0;
                for (j = 0; j < dimensions; j++)
                {
                    shift[j] = sum * (1.0 - lambda) * shift[j] + lambda * prevMovement[i][j];
                    sum2 += shift[j] * shift[j];
                }
                if (sum2 > 0.0)
                    sum2 = vLength / Math.Sqrt(sum2);
                else                           /* only in case of rounding errors */
                    sum2 = 0.0;
                for (j = 0; j < dimensions; j++)
                {
                    shift[j] *= sum2;
                    prevMovement[i][j] = shift[j];
                    if ((/*peak[i][j]*/peaks[i].Position[j] + prevMovement[i][j]) < searchSpace[j,0] /*mincoordinate*/)
                    {
                        peaks[i].Position[j]/*peak[i][j]*/ = 2.0 * searchSpace[j,0]/*mincoordinate*/ - peaks[i].Position[j]/*peak[i][j]*/ - prevMovement[i][j];
                        prevMovement[i][j] *= -1.0;
                    }
                    else if ((peaks[i].Position[j]/*(peak[i][j]*/ + prevMovement[i][j]) > searchSpace[j,1]/*maxcoordinate*/)
                    {
                        peaks[i].Position[j]/*peak[i][j]*/ = 2.0 * searchSpace[j, 1]/*maxcoordinate*/ - peaks[i].Position[j]/*peak[i][j]*/ - prevMovement[i][j];
                        prevMovement[i][j] *= -1.0;
                    }
                    else
                        peaks[i].Position[j]/*peak[i][j]*/ += prevMovement[i][j];
                }
                /* change peak width */
                j = dimensions;
                offset = MovNRand() * widthSeverity;
                if ((peaks[i].Width/*peak[i][j]*/ + offset) < minWidth)
                    peaks[i].Width/*peak[i][j]*/ = 2.0 * minWidth - peaks[i].Width/*peak[i][j]*/ - offset;
                else if ((peaks[i].Width/*peak[i][j]*/ + offset) > maxWidth)
                    peaks[i].Width/*peak[i][j]*/ = 2.0 * maxWidth - peaks[i].Width/*peak[i][j]*/ - offset;
                else
                    peaks[i].Width/*peak[i][j]*/ += offset;
                /* change peak height */
                j++;
                offset = heightSeverity * MovNRand();
                if ((peaks[i].Height/*peak[i][j]*/ + offset) < minHeight)
                    peaks[i].Height/*peak[i][j]*/ = 2.0 * minHeight - peaks[i].Height/*peak[i][j]*/ - offset;
                else if ((peaks[i].Height/*peak[i][j]*/ + offset) > maxHeight)
                    peaks[i].Height/*peak[i][j]*/ = 2.0 * maxHeight - peaks[i].Height/*peak[i][j]*/ - offset;
                else
                    peaks[i].Height/*peak[i][j]*/ += offset;
            }
            if (calculateAverageError)
            {
                globalMin = Double.MaxValue/*-100000.0*/;
                for (i = 0; i < numberOfPeaks; i++)
                {
                    //for (j = 0; j < dimensions; j++)
                        //coordinates[j] = peak[i][j];
                    //    coordinates[j] = peaks[i].Position[j];
                    peaks[i].Fitness = EvaluateDummy(peaks[i].Position);
                    if (peaks[i].Fitness < globalMin)
                    {
                        globalMin = peaks[i].Fitness;
                        minimumPeak = i;
                    }
                }
            }
            recentChange = true;
        }
        /* Basis Functions */

        /* This gives a constant value back to the eval-function that chooses the max of them */
        double ConstantBasisFunc(double[] b)
        {
            return 0.0;
        }

        double FivePeakBasisFunc(double[] b)
        {
            int i, j;
            double maximum = -100000.0, dummy;
            double[,] basis_peak = 
            {
                {8.0,  64.0,  67.0,  55.0,   4.0, 0.1, 50.0},
                {50.0,  13.0,  76.0,  15.0,   7.0, 0.1, 50.0},
                {9.0,  19.0,  27.0,  67.0,  24.0, 0.1, 50.0},
                {66.0,  87.0,  65.0,  19.0,  43.0, 0.1, 50.0},
                {76.0,  32.0,  43.0,  54.0,  65.0, 0.1, 50.0},
            };
            for (i = 0; i < 5; i++)
            {
                dummy = (b[0] - basis_peak[i,0]) * (b[0] - basis_peak[i,0]);
                for (j = 1; j < dimensions; j++)
                    dummy += (b[j] - basis_peak[i,j]) * (b[j] - basis_peak[i,j]);
                dummy = basis_peak[i,dimensions + 1] - (basis_peak[i,dimensions] * dummy);
                if (dummy > maximum)
                    maximum = dummy;
            }
            return maximum;
        }



        /* Peak Functions */

        /* sharp peaks */
        double PeakFunction1(double[] b, int peak_number)
        {
            int j;
            double dummy;

            dummy = (b[0] - peaks[peak_number].Position[0]) * (b[0] - peaks[peak_number].Position[0]);
            for (j = 1; j < dimensions; j++)
                dummy += (b[j] - peaks[peak_number].Position[j]) * (b[j] - peaks[peak_number].Position[j]);
            return maxHeight - peaks[peak_number].Height / (1 + (peaks[peak_number].Width) * dummy);
        }

        double PeakFunctionCone(double[] gen, int peak_number)
        {
            int j;
            double dummy;

            dummy = (gen[0] - peaks[peak_number].Position[0]) * (gen[0] - peaks[peak_number].Position[0]);
            for (j = 1; j < dimensions; j++)
                dummy += (gen[j] - peaks[peak_number].Position[j]) * (gen[j] - peaks[peak_number].Position[j]);
            return peaks[peak_number].Height - (peaks[peak_number].Width * Math.Sqrt(dummy));
        }

        double PeakFunctionHilly(double[] gen, int peak_number)
        {
            int j;
            double dummy;

            dummy = (gen[0] - peaks[peak_number].Position[0]) * (gen[0] - peaks[peak_number].Position[0]);
            for (j = 1; j < dimensions; j++)
                dummy += (gen[j] - peaks[peak_number].Position[j]) * (gen[j] - peaks[peak_number].Position[j]);
            return peaks[peak_number].Height - (peaks[peak_number].Width * dummy) - 0.01 * Math.Sin(20.0 * dummy);
        }

        double PeakFunctionTwin(double[] gen, int peak_number) /* two twin peaks moving together */
        {
            int j;
            double maximum = -100000.0, dummy;
            double[] twin_peak = /* difference to first peak */
            {
                1.0,  1.0,  1.0,  1.0,   1.0, 0.0, 0.0,
            };

            dummy = Math.Pow(gen[0] - peaks[peak_number].Position[0], 2);
            for (j = 1; j < dimensions; j++)
                dummy += Math.Pow(gen[j] - peaks[peak_number].Position[j], 2);
            dummy = peaks[peak_number].Height - (peaks[peak_number].Width * dummy);
            maximum = dummy;
            dummy = Math.Pow(gen[j] - (peaks[peak_number].Position[0] + twin_peak[0]), 2);
            for (j = 1; j < dimensions; j++)
                dummy += Math.Pow(gen[j] - (peaks[peak_number].Position[j] + twin_peak[0]), 2);
            dummy = peaks[peak_number].Height + twin_peak[dimensions + 1] - ((peaks[peak_number].Width + twin_peak[dimensions]) * dummy);
            if (dummy > maximum)
                maximum = dummy;

            return maximum;
        }


        public double GetAvgError() /* returns the average error of all evaluation calls so far */
        {
            return (avgError / (double)functionEvaluations);
        }

        public double GetCurrentError() /* returns the error of the best individual evaluated since last change */
        /* To use this function, calculate_average_error and calculate_offline_performance must be set */
        {
            return currentError;
        }

        public double GetOfflinePerformance() /* returns offline performance */
        {
            return (offlinePerformance / (double)functionEvaluations);
        }

        public double GetOfflineError() /* returns offline error */
        {
            return (offlineError / (double)functionEvaluations);
        }


        int GetNumberOfEvals() /* returns the number of evaluations so far */
        {
            return functionEvaluations;
        }

        int GetRightPeak()  /* returns 1 if current best individual is on highest peak, 0 otherwise */
        {
            if (currentPeak == minimumPeak)
                return 1;
            else
                return 0;
        }
        //public double[,] SearchSpace
        //{
        //    get
        //    {
        //        return searchSpace;
        //    }
        //    set
        //    {
        //        searchSpace = value;
        //    }
        //}
        //public int FunctionEvalutions
        //{
        //    get
        //    {
        //        return functionEvaluations;
        //    }
        //}
        //public int Dimension
        //{
        //    get
        //    {
        //        return dimensions;
        //    }
        //}
        //public double[,] SearchSpace
        //{
        //    get
        //    {
        //        return searchSpace;
        //    }
        //    set
        //    {
        //        searchSpace = value;
        //    }
        //}
        //public double Neighborhood
        //{
        //    get
        //    {
        //        return ngh;
        //    }
        //}
        //public int GlobalFitnessEvaluations
        //{
        //    get
        //    {
        //        return peakFitnessEvaluations[0];
        //    }
        //}
        //public double GlobalError
        //{
        //    get
        //    {
        //        return peakError[0];
        //    }
        //}
        //public int[] LocalFitnessEvaluations
        //{
        //    get
        //    {
        //        return peakFitnessEvaluations;
        //    }
        //}
        //public double LocalError
        //{
        //    get
        //    {
        //        double temp = 0;
        //        for (int i = 0; i < peakError.Length; i++)
        //        {
        //            temp += peakError[i];
        //        }
        //        temp = temp / peakError.Length;
        //        return temp;
        //    }
        //}
        public int NumberOfPeaks
        {
            get
            {
                return numberOfPeaks;
            }
            set 
            {
                numberOfPeaks = value;
            }
        }
        public int ChangeFrequency
        {
            get
            {
                return changeFrequency;
            }
            set
            {
                changeFrequency = value;
            }
        }
    }
}
