using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using unirest_net.http;
using Xamarin.Forms;

namespace MovieApp2
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public IList<Top250DataDetail> Movies { get; private set; }
        public MainPage()
        {
            InitializeComponent();

            getDataFromServer();
            
            BindingContext = this;

            MovieXamlList.RefreshCommand = new Command(() => {
                getDataFromServer();
                MovieXamlList.IsRefreshing = false;
            });
        }

        private void getDataFromServer()
        {
            Movies = new List<Top250DataDetail>();
            HttpResponse<Top250Data> request = Unirest.get("https://imdb-api.com/en/API/Top250Movies/k_kosg24pB")
            .header("X-RapidAPI-Key", "k_kosg24pB")
            .asJson<Top250Data>();
            for (int i = 0; i < 250; i++)
            {
                Top250DataDetail movie = request.Body.Items[i];
                movie.IMDbRating = getRating(movie);
                Movies.Add(movie);

            }
            if (request.Body.ErrorMessage != null && request.Body.ErrorMessage != "" )
            {
                DisplayAlert("Alert", request.Body.ErrorMessage, "OK");
            }
        }

        private string getRating(Top250DataDetail content)
        {
            int emoji = 0x1F3AC;
            Console.WriteLine(content.IMDbRating);
            var sb = new StringBuilder(Convert.ToInt32(Double.Parse(content.IMDbRating)));
            for (int i = 0; i < Convert.ToInt32(Double.Parse(content.IMDbRating)); i++)
            {
                sb.Append(Char.ConvertFromUtf32(emoji) + " ");
            }
            sb.Append(" (" + content.IMDbRating + ")");
            return sb.ToString(); ;
        }

        private void List_view_item_tapped(object sender, ItemTappedEventArgs e)
        {
            var content = e.Item as Top250DataDetail;
            Navigation.PushAsync(new DetailView(content));
        }

    }
    public class Top250Data
    {
        public List<Top250DataDetail> Items { get; set; }

        public string ErrorMessage { get; set; }
    }

    public class Top250DataDetail
    {
        public string Id { get; set; }
        public string Rank { get; set; }
        public string Title { set; get; }
        public string FullTitle { set; get; }
        public string Year { set; get; }
        public string Image { get; set; }
        public string Crew { get; set; }
        public string IMDbRating { get; set; }
        public string IMDbRatingCount { get; set; }
    }

}
