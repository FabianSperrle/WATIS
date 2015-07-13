using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ComplexEventProcessing;
using Microsoft.ComplexEventProcessing.Extensibility;
using Microsoft.ComplexEventProcessing.Linq;
using IvanAkcheurov.NTextCat.Lib;
using IvanAkcheurov.Commons;
using IvanAkcheurov.NClassify;

namespace ConsoleApplication1
{
    
    [Serializable]
    class FilterEnglish : CepPointStreamOperator<TwitterData, TwitterData>
    {
        //private RankedLanguageIdentifierFactory factory = new RankedLanguageIdentifierFactory();

        public override bool IsEmpty
        {
            get { return false; }
        }
        public override IEnumerable<TwitterData> ProcessEvent(PointEvent<TwitterData> e)
        {
            
            //var identifier = factory.Load("Resources\\Core14.profile.xml");
            //if (identifier.Identify(e.Payload.TWEET_CONTENT).FirstOrDefault().Item1.Iso639_3 == "eng"){
            if(e.Payload.USER_LANGUAGE.Equals("en")){
                var returnvalue = new List<TwitterData>();
                returnvalue.Add(e.Payload);
                return returnvalue;
            }
            return new List<TwitterData>();
        }
    }
}
