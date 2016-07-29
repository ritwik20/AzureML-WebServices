using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GetWSD
{
    public class Experiment
    {
        public Experiment(string location)
        {
            Uri uri = new Uri(location);
            this.Host = uri.Host;
            var path = uri.Fragment.Split(new char[] { '/' });
            string experimentId = null;
            for (int i = 0; i < path.Length; i++)
            {
                if (string.Equals(path[i], "Experiment", StringComparison.InvariantCultureIgnoreCase))
                {
                    experimentId = path[i + 1];
                    break;
                }
            }

            if (!string.IsNullOrEmpty(experimentId))
            {
                var idParts = experimentId.Split(new char[] {'.'});
                if (idParts.Count() != 3)
                {
                    throw new ArgumentException("Bad Experiment URL - does not contain a valid experiment id.");
                }

                this.Id = experimentId;
                this.WorkspaceId = idParts[0];
            }
        }

        public async Task<string> GetWebServiceDefinition(string token)
        {
            var restURI = new Uri(string.Format("https://{0}/api/workspaces/{1}/experiments/{2}/webservicedefinition", this.Host, this.WorkspaceId, this.Id));
            var client = HttpClientFactory.Create();
            client.DefaultRequestHeaders.Add("x-ms-metaanalytics-authorizationtoken", token);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(JsonMediaTypeFormatter.DefaultMediaType.MediaType));
            var response = await client.GetAsync(restURI);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return string.Concat(@"{""Properties"":", responseContent, @"}");
            }

            return string.Format("Error creating web service definition: {0}", responseContent);
        }

        public string WorkspaceId { get; set; }

        public string Id { get; set; }

        private string Host { get; set; }

    }

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: GetWSD <experiment-url> <workspace-auth-token>");
                Environment.Exit(-1);
            }

            var experimentUrl = args[0];
            var workspaceToken = args[1];

            var experiment = new Experiment(experimentUrl);
            Console.WriteLine(experiment.GetWebServiceDefinition(workspaceToken).Result);
        }
    }
}
