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
using LumenWorks.Framework.IO.Csv;


namespace ConsoleApplication1

    /* This example:
     * creates an embedded server instance and makes it available to other clients
     * defines, deploys, binds, and runs a simple source, query, and sink
     * waits for the user to stop the server
     */
{
    public static class Program
    {
        //static IQStreamable<TwitterData> GetTollReadings(this Application app)
        //{

            //return app.DefineEnumerable(() =>
                // CSV LIBRARY
                //new[] {
                //    PointEvent<TwitterData>.CreateInsert(DateTime.Today, new TwitterData()
                //)}).
                //ToPointStreamable(e => e, AdvanceTimeSettings.IncreasingStartTime);
        //}
        static void Main(string[] args)
        {
            // Create an embedded StreamInsight server
            using (var server = Server.Create("pauline"))
            {
                using (CsvReader csv =
                    new CsvReader(new StreamReader("Resources\\top.csv"), false, '\t', new Char(), new Char(), new Char(), new ValueTrimmingOptions(), null))
                {
                    // Create a StreamInsight App
                    var app = server.CreateApplication("name");
                    // Language identifier
                    var factory = new RankedLanguageIdentifierFactory();
                    var identifier = factory.Load("Resources\\Core14.profile.xml");

                    // Tweet list
                    var data = new List<TwitterData>();

                    // Get the whole CSV 
                    string[] headers = { "TWEET_ID", "TWEET_CREATIONDATE", "TWEET_CONTENT", "TWEET_RETWEETED", "TWEET_RETWEETCOUNT", "TWEET_COORDS", "TWEET_SOURCE", "TWEET_URLS", "MEDIA_URLS", "TWEET_PLACE", "TWEET_REPLYTOSTATUS", "TWEET_REPLYTOUSERID", "TWEET_REPLYTOUSERNAME", "USER_ID", "USER_NAME", "USER_SCREENNAME", "USER_CREATIONDATE", "USER_LANGUAGE", "USER_STATUSESCOUNT", "USER_FOLLOWERSCOUNT", "USER_LOCATION", "USER_DESCRIPTION", "USER_FRIENDSCOUNT", "USER_TIMEZONE", "USER_LISTEDCOUNT", "USER_UTCOFFSET" };
                    while (csv.ReadNextRecord())
                    {
                        var tweet = new TwitterData(csv[0], csv[1], csv[2], csv[3], csv[4], csv[5], csv[6], csv[7], csv[8], csv[9], csv[10], csv[11], csv[12], csv[13], csv[14], csv[15], csv[16], csv[17], csv[18], csv[19], csv[20], csv[21], csv[22], csv[23], csv[24], csv[25]);
                        // Only use english tweets
                        if (identifier.Identify(tweet.TWEET_CONTENT).FirstOrDefault().Item1.Iso639_3 != "eng")
                            continue;

                        data.Add(tweet);
                    }

                    // Create CEP stream from tweet list
                    var stream = app.DefineEnumerable(() => data).ToPointStreamable(t => PointEvent.CreateInsert(
                        DateTime.ParseExact(t.TWEET_CREATIONDATE, "ddd MMM d HH:mm:ss 'GMT' yyyy", System.Globalization.CultureInfo.InvariantCulture), t), AdvanceTimeSettings.IncreasingStartTime);
                    // Split tweets into terms
                    var terms = stream.Scan(() => new TwitterTextToTermsUDO());

                    // Create and attach the console observer
                    var consoleObserver = app.DefineObserver(() => Observer.Create<PointEvent<TwitterDataTerm>>(ConsoleWritePoint));
                    var binding = terms.Bind(consoleObserver);

                    // Run everything, stop programming upon enter
                    using (binding.Run("Goooooo"))
                    {
                        Console.ReadLine();
                    }
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
    }

}