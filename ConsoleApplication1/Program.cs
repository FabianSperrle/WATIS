using System;
using System.ServiceModel;
using Microsoft.ComplexEventProcessing;
using Microsoft.ComplexEventProcessing.Linq;
using Microsoft.ComplexEventProcessing.ManagementService;
using System.Reactive;
using System.Reactive.Linq;
using System.Linq;
using Microsoft.ComplexEventProcessing.Extensibility;
using System.IO;
using IvanAkcheurov.NTextCat.Lib;
using IvanAkcheurov.Commons;
using IvanAkcheurov.NClassify;
using System.Collections.Generic;
using System.Globalization;
using StreamInsight.Samples.Adapters.AsyncCsv;


namespace ConsoleApplication1
{
    public static class Program
    {
        private static string[] headers = { "TWEET_ID", "TWEET_CREATIONDATE", "TWEET_CONTENT", "TWEET_RETWEETED", "TWEET_RETWEETCOUNT", "TWEET_COORDS", "TWEET_SOURCE", "TWEET_URLS", "MEDIA_URLS", "TWEET_PLACE", "TWEET_REPLYTOSTATUS", "TWEET_REPLYTOUSERID", "TWEET_REPLYTOUSERNAME", "USER_ID", "USER_NAME", "USER_SCREENNAME", "USER_CREATIONDATE", "USER_LANGUAGE", "USER_STATUSESCOUNT", "USER_FOLLOWERSCOUNT", "USER_LOCATION", "USER_DESCRIPTION", "USER_FRIENDSCOUNT", "USER_TIMEZONE", "USER_LISTEDCOUNT", "USER_UTCOFFSET" };
        static HashSet<string> stopwords;
        static void Main(string[] args)
        {
            // Create an embedded StreamInsight server
            using (var server = Server.Create("pauline"))
            {
                // Create a StreamInsight App
                var app = server.CreateApplication("name");

                var conf = new CsvInputConfig()
                {
                    CultureName = System.Globalization.CultureInfo.InvariantCulture.Name,
                    InputFileName = "Resources\\day1.csv",
                    Delimiter = new char[] { '\t' },
                    NonPayloadFieldCount = 0,
                    Fields = headers.ToList(),
                    CtiFrequency = 0,
                    BufferSize = 1000000,
                    StartTimePos = 2
                };

                //define iqstreamable
                var a = app.DefineStreamable<TwitterData>(typeof(CsvInputFactory), conf, EventShape.Point, AdvanceTimeSettings.IncreasingStartTime);

                //filter english
                var filteredCepStream = a.Scan(() => new FilterEnglish());
                // Split tweets into terms
                var terms = filteredCepStream.Scan(() => new TwitterTextToTermsUDO());

                // Get all the stopwords
                 var stopwordsfromfile = File.ReadAllLines("Resources\\stopwords.txt");
                 stopwords = new HashSet<string>(stopwordsfromfile);
                // Filter all terms based on stopwords
                var noStopWords = from t in terms
                                  where !Program.isStopword(t.Term)
                                  select t;

                var watis = from t in noStopWords.TumblingWindow(TimeSpan.FromSeconds(60))
                          select t.WATIS();

                // Create and attach the console observer
                var consoleObserver = app.DefineObserver(() => Observer.Create<PointEvent<string>>(ConsoleWritePoint));
                var binding = watis.Bind(consoleObserver);

                // Run everything, stop programming upon enter
                using (binding.Run("Goooooo"))
                {
                    Console.ReadLine();
                }
            }
        }


        static void ConsoleWritePoint<TPayload>(PointEvent<TPayload> e)
        {
            if (e.EventKind == EventKind.Insert)
                Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "INSERT <{0}> {1}", e.StartTime.DateTime, e.Payload.ToString()));
            else
                Console.WriteLine(string.Format(CultureInfo.InvariantCulture, "CTI    <{0}>", e.StartTime.DateTime));
        }

        public static bool isStopword(String Term)
        {
            return stopwords.Contains(Term);
        }
    }

}