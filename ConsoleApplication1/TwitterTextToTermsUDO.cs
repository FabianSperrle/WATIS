using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ComplexEventProcessing;
using Microsoft.ComplexEventProcessing.Extensibility;
using Microsoft.ComplexEventProcessing.Linq;

namespace ConsoleApplication1
{
    class TwitterTextToTermsUDO : CepPointStreamOperator<TwitterData, TwitterDataTerm>
    {
        public override bool IsEmpty
        {
            get { return false; }
        }
        public override IEnumerable<TwitterDataTerm> ProcessEvent(PointEvent<TwitterData> e)
        {
            TwitterData tweet = e.Payload;
            var output = new List<TwitterDataTerm>();
            string[] seperators = {",", ".", ";", "!", "?", " ", ":"};
            string[] terms = tweet.TWEET_CONTENT.Split(seperators, StringSplitOptions.RemoveEmptyEntries);

            foreach (string Term in terms)
            {
                output.Add(new TwitterDataTerm(Term, tweet));
            }

            return output;
        }
    }
}
