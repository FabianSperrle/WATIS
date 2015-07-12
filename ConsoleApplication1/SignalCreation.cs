using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class SignalCreation
    {
        /** Interval. */
        private IntervalCreation intervals;
        /** WindowSize. */
        private int windowSize;
        /** IterationCount. */
        private int iterationCount;
        /** KolmogovorZurbenkoFilter. */
        private KZF kzfilter;

        /**
         * Default constructor.
         * @param intervals intervals
         * @param windowSize size
         * @param iterationCount iterationCount
         */
        public SignalCreation(IntervalCreation intervals, int windowSize, int iterationCount)
        {
            this.intervals = intervals;
            this.windowSize = windowSize;
            this.iterationCount = iterationCount;
            this.kzfilter = new KZF();
        }

        /**
         * Computes signal for terms.
         * @param term term
         * @return signal of term
         */
        public int[] computeSignal(String term)
        {
            double[] termC = this.intervals.getTermCountInIntervals(term);
            double[] tupleC = this.intervals.getTupleCountInIntervals();
            double[] inputData = new double[termC.Length];

            for (int i = 0; i < termC.Length; i++)
            {
                double tuplecc = tupleC[i] + 1.0;
                double termcc = termC[i] + 1.0;
                double value = Math.Log(tuplecc) - Math.Log(tuplecc / termcc);
                inputData[i] = value > 0.0 ? value : 1.0;
            }

            double[] kzResults = this.kzfilter.kz(inputData, this.windowSize, this.iterationCount);
            double[] kzaResults = this.kzfilter.kza(inputData, kzResults, this.windowSize, this.iterationCount);

            long[] times = new long[inputData.Length];
            for (int i = 0; i < times.Length; i++)
            {
                times[i] = (long)i;
            }

            //Create signal series
            SignalSeries signalSeries = new SignalSeries(kzaResults, times);
            //Apply continuous wavelet transformation
            WavCwt wavCwt = new WavCwt(signalSeries);
            //Apply tree map
            WavCwtTree tree = new WavCwtTree(wavCwt);
            if (tree.getBranches() != null)
            {
                //Find peaks of tree
                WavCwtTreePeaks wavCwtTreePeaks = new WavCwtTreePeaks(tree);

                //         if (term.equals("healthy")) {
                //            for (double d : inputData) {
                //               System.out.println(String.valueOf(d));
                //            }
                //            System.exit(0);
                //         }
                return wavCwtTreePeaks.getIEndTimes();
            }
            else
            {
                return new int[] { };
            }
        }
    }
}


