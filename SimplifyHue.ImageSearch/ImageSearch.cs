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
            log.LogInformation("C# HTTP trigger function processed a request.");

            string searchTerm = req.Query["search"];
            if (string.IsNullOrEmpty(searchTerm))
            {
                new BadRequestObjectResult("Please pass a search on the query string");
            }

            string apiKey = "0925e5e2fa954342a834345604c5e855";
            var client = new ImageSearchClient(new ApiKeyServiceClientCredentials(apiKey));

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
