using System.Collections.Generic;

namespace SimplifyHue.Shared.Model
{
    public class ImageSearchResult
    {
        public string SearchTerms { get; set; }

        public IList<ImageSearchItem> Images { get; set; }
    }
}
