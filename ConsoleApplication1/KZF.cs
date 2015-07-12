using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    static class KZF
    {
        /**
         * Runs the KZ algorithm.
         * @param x list with values
         * @param size size of windows
         * @param iterations number of iterations
         * @return list with smoothed values
         */
        public double[] kz(double[] x, int size, int iterations)
        {
            int m = size + 1;
            int p = (m - 1) / 2;
            double[] tmp = x;
            double[] ans = null;
            for (int k = 0; k < iterations; k++)
            {
                ans = new double[tmp.Length];
                for (int i = 0; i < x.Length; i++)
                {
                    ans[i] = this.movingAverageKZ(tmp, i, p);
                }
                tmp = ans;
            }
            return ans;
        }

        /**
         * Moving average method for KZ analysis.
         * @param x vector with values.
         * @param col column
         * @param w width
         * @return double value
         */
        private double movingAverageKZ(double[] x, int col, int w)
        {
            double s = 0.0;
            int z = 0;
            int startcol = col - w > 0 ? col - w : 0;
            int endcol = col + w < x.Length ? col + w + 1 : x.Length;
            for (int i = startcol; i < endcol; i++)
            {
                z++;
                s += x[i];
            }
            if (z == 0)
            {
                return Double.NaN;
            }
            return s / z;
        }

        /**
         * Runs the KZA algorithm.
         * @param x list with values
         * @param y list with values from KZ
         * @param window number of windows
         * @param iterations number of iterations
         * @return list with smoothed values
         */
        public double[] kza(double[] x, double[] y, int window,
               int iterations)
        {
            //      return this.kza(x, y, window, iterations, Math.round(0.05 * window), 1.0e-5, false);
            return this.kza(x, y, window, iterations, 1.0, 1.0e-5, false);
        }

        /**
         * Runs the KZA algorithm.
         * @param x list with values
         * @param y list with values from KZ
         * @param window number of windows
         * @param iterations number of iterations
         * @param minSize minimum size of window q
         * @param tolerance the smallest value to accept as nonzero
         * @param imputeTails {@code true} to impute tails, {@code false} otherwise
         * @return list with smoothed values
         */
        public double[] kza(double[] x, double[] y, int window,
               int iterations, double minSize, double tolerance,
               bool imputeTails)
        {
            int q = window;
            int minWindowLength = (int)minSize;
            int n = y.Length;

            double[] d = new double[n];
            double[] dprime = new double[n];
            double[] tmp = x;
            this.differenced(y, d, dprime, q);

            double m = this.maximum(d);

            long qh;
            long qt;
            double[] ans = null;
            for (int i = 0; i < iterations; i++)
            {
                ans = new double[n];
                for (int t = 0; t < n; t++)
                {
                    if (Math.Abs(dprime[t]) < tolerance)
                    {
                        qh = (int)Math.Floor(q * this.adaptive(d[t], m));
                        qt = (int)Math.Floor(q * this.adaptive(d[t], m));
                    }
                    else if (dprime[t] < 0)
                    {
                        qh = q;
                        qt = (int)Math.Floor(q * this.adaptive(d[t], m));
                    }
                    else
                    {
                        qh = (int)Math.Floor(q * this.adaptive(d[t], m));
                        qt = q;
                    }
                    qt = qt < minWindowLength ? minWindowLength : qt;
                    qh = qh < minWindowLength ? minWindowLength : qh;
                    qh = qh > n - t - 1 ? n - t - 1 : qh;
                    qt = qt > t ? t : qt;
                    ans[t] = this.movingAverageKZA(tmp, (int)(t - qt), (int)(t + qh + 1));
                }
                tmp = ans;
            }
            if (!imputeTails)
            {
                Fill(ans, 0, q, Double.NaN);
                Fill(ans, ans.Length - q - 1, ans.Length, Double.NaN);
            }
            return ans;
        }

        public static void Fill(this Array x, int start, int end, object y)
        {
            for (int i = start; i < end; i++)
            {
                x.SetValue(y, i);
            }
        }

        /**
         * Moving average method for KZA analysis.
         * @param x list of values
         * @param a a position
         * @param b b position
         * @return double value
         */
        private double movingAverageKZA(double[] x, int a, int b)
        {
            double s = 0.0;
            int z = 0;
            for (int i = a; i < b; i++)
            {
                z++;
                s += x[i];
            }
            if (z == 0)
            {
                return Double.NaN;
            }
            return s / z;
        }

        /**
         * Adaptive curvature function.
         * @param d double
         * @param m double
         * @return double result
         */
        private double adaptive(double d, double m)
        {
            return 1.0 - d / m;
        }

        /**
         * Calculates maximum of list.
         * @param x list with values
         * @return double maximum value
         */
        private double maximum(double[] x)
        {
            double max = Double.MinValue;
            foreach (double element in x)
            {
                if (element > max)
                {
                    max = element;
                }
            }
            return max;
        }

        /**
         * Calculates the differences between lists.
         * @param y list with values
         * @param d list with values
         * @param dprime list with values
         * @param q Length value
         */
        private void differenced(double[] y, double[] d, double[] dprime,
               int q)
        {
            int n = y.Length;
            for (int i = 0; i < q; i++)
            {
                d[i] = Math.Abs(y[i + q] - y[0]);
            }
            for (int i = q; i < n - q; i++)
            {
                d[i] = Math.Abs(y[i + q] - y[i - q]);
            }
            for (int i = n - q; i < n; i++)
            {
                d[i] = Math.Abs(y[n - 1] - y[i - q]);
            }
            for (int i = 0; i < n - 1; i++)
            {
                dprime[i] = d[i + 1] - d[i];
            }
            dprime[n - 1] = dprime[n - 2];
        }
    }
}