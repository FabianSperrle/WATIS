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
    }
}
