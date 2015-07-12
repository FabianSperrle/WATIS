using Microsoft.ComplexEventProcessing.Extensibility;
using Microsoft.ComplexEventProcessing.Linq;
using Microsoft.ComplexEventProcessing.Adapters;
using Microsoft.ComplexEventProcessing.Integration;
using Microsoft.ComplexEventProcessing.ManagementService;
using Microsoft.ComplexEventProcessing.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApplication1
{
    public class Watis : CepTimeSensitiveAggregate<TwitterDataTerm, string>
    {
        public override string GenerateOutput(IEnumerable<Microsoft.ComplexEventProcessing.IntervalEvent<TwitterDataTerm>> eventData, WindowDescriptor windowDescriptor)
        {
            var tweets = (from t in eventData
                          select t.Payload.TwitterData.TWEET_CONTENT).Distinct();
            var distinctTerms = String.Join(" ", eventData.Select(e => e.Payload.Term).Distinct());

            
            return distinctTerms;

            /*
            var g = eventData.GroupBy(e => e.Payload.Term);
            foreach (var group in g)
            {
                var documents = tweets.ToArray();
                double[][] inputs = TFIDF.Transform(documents);

                return TFIDF.Normalize(inputs).ToString();
            }*/
        }
    }

    public static class MyUDAExtensionMethods
    {
        [CepUserDefinedAggregate(typeof(Watis))]
        public static string WTS<TwitterDataTerm>(this CepWindow<TwitterDataTerm> window)
        {
            throw CepUtility.DoNotCall();
        }
    }
}