using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.CognitiveServices.Search.ImageSearch;
using System.Linq;
using SimplifyHue.Shared.Model;
using System.Net.Mime;
using System.Net;
using System.Web.Http;

namespace SimplifyHue.Backend
{
    public static class ImageSearch
    {
        [FunctionName("ImageSearch")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log,
            ExecutionContext context)
        {
            // check request
            log.LogInformation("Starting image search function");

            string searchTerm = req.Query["search"];
            if (string.IsNullOrEmpty(searchTerm))
            {
                return new BadRequestObjectResult("Please pass a search on the query string");
            }

            log.LogInformation($"Search term is: {searchTerm}");

            // get cognitive services api key from config and create image search client
            var config = new ConfigurationBuilder()
             .SetBasePath(context.FunctionAppDirectory)
             .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
             .AddEnvironmentVariables()
             .Build();
            string apiKey = config["CognitiveServicesKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                return new InternalServerErrorResult();
            }
            var client = new ImageSearchClient(new ApiKeyServiceClientCredentials(apiKey));

            // perform image search and return result
            var searchResult = await client.Images.SearchAsync(searchTerm);
            var images = searchResult.Value.Select(searchItem => new ImageSearchItem { FullImageUrl = searchItem.ContentUrl, PreviewImageUrl = searchItem.ThumbnailUrl }).ToList();
            return new ContentResult()
            {
                Content = JsonConvert.SerializeObject(new ImageSearchResult { Images = images, SearchTerms = searchTerm }),
                ContentType = "application/json",
                StatusCode = (int)HttpStatusCode.OK
            };
        }
    }
}
