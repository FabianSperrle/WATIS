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
               


                host.Close();
            }
        }

        void ReadCsv()
{
    // open the file "data.csv" which is a CSV file with headers
    using (CsvReader csv =
           new CsvReader(new StreamReader("data.csv"), true))
    {
        int fieldCount = csv.FieldCount;

        string[] headers = csv.GetFieldHeaders();
        while (csv.ReadNextRecord())
        {
            for (int i = 0; i < fieldCount; i++)
                Console.Write(string.Format("{0} = {1};",
                              headers[i], csv[i]));
            Console.WriteLine();
        }
    }
}
    }

}