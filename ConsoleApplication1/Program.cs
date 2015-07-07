using System;
using System.ServiceModel;
using Microsoft.ComplexEventProcessing;
using Microsoft.ComplexEventProcessing.Linq;
using Microsoft.ComplexEventProcessing.ManagementService;
using System.Reactive;
using System.Reactive.Linq;
using Microsoft.ComplexEventProcessing.Extensibility;
using System.IO;

using System.Collections.Generic;
using System.Globalization;
using LumenWorks.Framework.IO.Csv;


namespace StreamInsight21_example_Server

    /* This example:
     * creates an embedded server instance and makes it available to other clients
     * defines, deploys, binds, and runs a simple source, query, and sink
     * waits for the user to stop the server
     */
{
    class Program
    {

        static IQStreamable<TwitterData> GetTollReadings(this Application app)
        {

            return app.DefineEnumerable(() =>
                // CSV LIBRARY
                new[] {
                    PointEvent<TwitterData>.CreateInsert(DateTime.Today, new TwitterData()
                )}).
                ToPointStreamable(e => e, AdvanceTimeSettings.IncreasingStartTime);
        }
        static void Main(string[] args)
        {
            // Create an embedded StreamInsight server
            using (var server = Server.Create("pauline"))
            {
                // Create a local end point for the server embedded in this program
                var host = new ServiceHost(server.CreateManagementService());
                host.AddServiceEndpoint(typeof(IManagementService), new WSHttpBinding(SecurityMode.Message), "http://localhost:8000/MyStreamInsightServer/Default/");
                host.Open();
                using (CsvReader csv =
                    new CsvReader(new StreamReader(@"C:\Users\leo\documents\visual studio 2013\Projects\ConsoleApplication1\ConsoleApplication1\Resources\2015_02_01_06.csv"), true))
                    {
                        int fieldCount = csv.FieldCount;

                        string[] headers = {"TWEET_ID", "TWEET_CREATIONDATE", "TWEET_CONTENT", "TWEET_RETWEETED", "TWEET_RETWEETCOUNT", "TWEET_COORDS", "TWEET_SOURCE", "TWEET_URLS", "MEDIA_URLS", "TWEET_PLACE", "TWEET_REPLYTOSTATUS", "TWEET_REPLYTOUSERID", "TWEET_REPLYTOUSERNAME", "USER_ID", "USER_NAME", "USER_SCREENNAME", "USER_CREATIONDATE", "USER_LANGUAGE", "USER_STATUSESCOUNT", "USER_FOLLOWERSCOUNT", "USER_LOCATION", "USER_DESCRIPTION", "USER_FRIENDSCOUNT", "USER_TIMEZONE", "USER_LISTEDCOUNT", "USER_UTCOFFSET"};
                        var twitterData = new List<PointEvent<TwitterData>>();
                        while (csv.ReadNextRecord())
                        {
                            var time = DateTime.ParseExact(csv[1], "ddd MMM dd HH:mm:ss zzz yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            twitterData.Add(PointEvent<TwitterData>.CreateInsert(time.ToUniversalTime(), new TwitterData(csv[0], csv[1], csv[2], csv[3], csv[4], csv[5], csv[6], csv[7], csv[8], csv[9], csv[10], csv[11], csv[12], csv[13], csv[14], csv[15], csv[16], csv[17], csv[18], csv[19], csv[20], csv[21], csv[22], csv[23], csv[24], csv[25])));
                        }
                    }

                host.Close();
            }
        }

        void ReadCsv()
{
    // open the file "data.csv" which is a CSV file with headers
    
}
    }

}