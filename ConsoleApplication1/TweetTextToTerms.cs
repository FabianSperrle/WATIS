using Microsoft.ComplexEventProcessing;
using Microsoft.ComplexEventProcessing.Linq;
using Microsoft.ComplexEventProcessing.Extensibility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class TweetTextToTerms : CepPointStreamOperator<TwitterData, TwitterDataTerm>
    {

        public override IEnumerable<TwitterDataTerm> ProcessEvent(PointEvent<TwitterData> inputEvent)
        {
            string[] seperators = { " ", ",", ".", "!", "?", ";" };
            string[] terms = inputEvent.Payload.TWEET_CONTENT.Split(seperators, StringSplitOptions.RemoveEmptyEntries);
            return terms.Select(e => new TwitterDataTerm(e, inputEvent.Payload));
        }

        public override bool IsEmpty
        {
            get { throw new NotImplementedException(); }
        }
    }
}