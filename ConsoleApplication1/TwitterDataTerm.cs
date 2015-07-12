using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class TwitterDataTerm
    {
        public string Term { get; set; }
        public TwitterData TwitterData { get; set; }

        public TwitterDataTerm() { }

        public TwitterDataTerm(string Term, TwitterData TwitterData)
        {
            this.Term = Term;
            this.TwitterData = TwitterData;
        }

        public override string ToString()
        {
            return String.Format("{0}: {1}", Term, TwitterData.TWEET_CONTENT.Substring(0, Math.Min(TwitterData.TWEET_CONTENT.Length, 20)));
        }
    }
}
