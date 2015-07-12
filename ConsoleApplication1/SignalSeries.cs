using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace ConsoleApplication1
{
    class SignalSeries
    {

        /** Signal. */
        private double[] signal;
        /** The difference between positions[] members (they are uniformely sampled, so it is one number). */
        private long deltaT;
        /** A row of 1,2,3...n numbers. Used as times in future calculations. */
        private long[] positions;

        /**
         * Constructor.
         * @param data Double object
         * @param timeIntervals Long object
         */
        public SignalSeries(double[] data, long[] timeIntervals)
        {
            this.makeSignal(data, timeIntervals);
        }

        /**
         * Creates signal.
         * @param data Double object
         * @param timeIntervals Long object
         */
        private void makeSignal(double[] data, long[] timeIntervals)
        {
            if (data == null)
            {
                throw new Exception("Passed empty data");
            }
            long[] timeIntervalsTemp;
            if (timeIntervals != null && (timeIntervals.Length == data.Length))
            {
                timeIntervalsTemp = timeIntervals;
            }
            else
            {
                timeIntervalsTemp = this.createUniformelySampledTimePositions(data.Length);
            }

            List<long> timesL = new List<long>();
            List<Double> dataL = new List<Double>();
            Double[] dataObjects = data.ToArray();
            for (int i = 0; i < dataObjects.Length; i++)
            {
                if (!dataObjects[i].Equals(Double.NaN))
                {
                    long initialTime = timeIntervalsTemp[i];
                    timesL.Add(initialTime);
                    dataL.Add(data[i]);
                }
            }

            //this.signal = ArrayUtils.toPrimitive(dataL.ToArray(new Double[timesL.size()]));
            this.signal = dataL.ToArray<double>();
            //Remove NaNs.
            if (this.signal.Length > 1)
            {
                this.positions = this.createUniformelySampledTimePositions(this.signal.Length);
                this.deltaT = this.positions[1] - this.positions[0];
            }
        }

        /**
         * Creates an array of numbers where the difference
         * between any two members is the same number ( "1" ).
         * @param Length Length of an array needed
         * @return an array of uniformely sampled numbers
         */
        private long[] createUniformelySampledTimePositions(int Length)
        {
            long[] uniformelySampledTimes = new long[Length];
            for (int i = 0; i < Length; i++)
            {
                uniformelySampledTimes[i] = i + 1;
            }
            return uniformelySampledTimes;
        }

        /**
         * Returns the Length of signal.
         * @return Length int
         */
        public int Length()
        {
            return this.signal.Length;
        }

        /**
         * Returns time.
         * @return time long array
         */
        public long[] getTime()
        {
            return this.positions;
        }

        /**
         * Returns signal.
         * @return signal double array
         */
        public double[] getSignal()
        {
            return this.signal;
        }

        /**
         * Transform normal numbers forming a signal into Complex numbers also known as 'i',
         * consisting of Real and Imaginary parts.
         * @return an array of complex numbers
         */
        public double[] getComplexSignal()
        {
            return this.getComplexSignal(this.getSignal());
        }

        /**
         * Returns complex signal.
         * @param signal signal double array
         * @return signal double array
         */
        private double[] getComplexSignal(double[] signal)
        {
            double[] complexSignal = new double[signal.Length * 2];
            for (int i = 0; i < complexSignal.Length; i++)
            {
                if (i % 2 == 0)
                {
                    complexSignal[i] = signal[i / 2];
                }
            }
            return complexSignal;
        }
    }
}
