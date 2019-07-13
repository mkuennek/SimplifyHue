using MvvmHelpers;
using Newtonsoft.Json;
using SimplifyHue.Shared.Model;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SimplifyHue.App.Ui.Search
{
    public class SearchPageViewModel : BaseViewModel
    {
        private HttpClient HttpClient;

        public SearchPageViewModel()
        {
            HttpClient = new HttpClient();
            Images = new ObservableRangeCollection<ImageViewModel>();

            SearchCommand = new Command(async () => await PerformSearch());
        }

        private string searchTerm;
        public string SearchTerm
        {
            get => searchTerm;
            set => SetProperty(ref searchTerm, value);
        }

        private ObservableRangeCollection<ImageViewModel> images;
        public ObservableRangeCollection<ImageViewModel> Images
        {
            set => SetProperty(ref images, value);
            get => images;
        }

        private ICommand seachCommand;
        public ICommand SearchCommand
        {
            get => seachCommand;
            set => SetProperty(ref seachCommand, value);
        }

        private async Task PerformSearch()
        {
            var result = await HttpClient.GetAsync($"https://simplifyhuebackend.azurewebsites.net/api/ImageSearch?code=av6pVreNdMgnVItRmSWKoRtu6eRk/kS/4jZFFwbkSXOFv7U6OpY1Uw==&search={SearchTerm}");
            if (result.IsSuccessStatusCode)
            {
                var resultContentString = await result.Content.ReadAsStringAsync();
                var resultObject = JsonConvert.DeserializeObject<ImageSearchResult>(resultContentString);

                Images.ReplaceRange(resultObject.Images.Select(resultImage => new ImageViewModel { ImageUrl = resultImage.PreviewImageUrl }));
            }
        }
    }
}
