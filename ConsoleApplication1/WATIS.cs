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
            return "watis";

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
        public static string WATIS<TwitterDataTerm>(this CepWindow<TwitterDataTerm> window)
        {
            throw CepUtility.DoNotCall();
        }
    }
}