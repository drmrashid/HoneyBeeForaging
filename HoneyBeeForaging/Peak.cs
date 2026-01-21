using System;
using System.Collections.Generic;
using System.Text;

namespace HoneyBeeForaging
{
    class Peak
    {
        private double f;
        private double[] x;
        private double h;
        private double w;
        private int d;
        public Peak(int dimensions)
        {
            d = dimensions;
            x = new double[d];
        }
        public double GetDistance(double[] x2)
        {
            double distance = 0;
            for (int i = 0; i < d; i++)
                distance += (x[i] - x2[i]) * (x[i] - x2[i]);
            distance = Math.Sqrt(distance);
            return distance;
        }
        public string SaveToString()
        {
            string str;
            str = f + ",";
            for (int i = 0; i < d; i++)
                str += x[i] + ",";
            str += w + "," + h;
            return str;
        }
        public void LoadFromString(string line)
        {
            string[] str = line.Split(',');
            f = Double.Parse(str[0]);
            d = str.Length - 3;
            x = new double[d];
            for (int i = 0; i < d; i++)
                x[i] = Double.Parse(str[i + 1]);
            w = Double.Parse(str[d + 1]);
            h = Double.Parse(str[d + 2]);
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

        public double Height
        {
            get
            {
                return h;
            }
            set
            {
                h = value;
            }
        }

        public double Width
        {
            get
            {
                return w;
            }
            set
            {
                w = value;
            }
        }
    }
}
