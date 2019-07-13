using MvvmHelpers;

namespace SimplifyHue.App.Ui.Search
{
    public class ImageViewModel : BaseViewModel
    {
        private string imageUrl;
        public string ImageUrl
        {
            set => SetProperty(ref imageUrl, value);
            get => imageUrl;
        }
    }
}
