using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using unirest_net.http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MovieApp2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailView : ContentPage, INotifyPropertyChanged
    {
        public DetailView(Top250DataDetail content)
        {
            InitializeComponent();
            lblTitle.Text = content.Title;
            lblCrew.Text = content.Crew;
            lblYear.Text = content.Year;
            image.Source = content.Image;
            lblShortDescr.Text = getWikipediaDataPlotShort(content.Id);
            lblRating.Text = content.IMDbRating;
        }
    
        private string getWikipediaDataPlotShort(string id)
        {
            WikipediaData WikiData;
            HttpResponse<WikipediaData> request = Unirest.get("https://imdb-api.com/en/API/Wikipedia/k_kosg24pB/"+id)
            .header("X-RapidAPI-Key", "k_kosg24pB")
            .asJson<WikipediaData>();
            WikiData = request.Body;
            if (request.Body.ErrorMessage != null && request.Body.ErrorMessage != "")
            {
                DisplayAlert("Alert", request.Body.ErrorMessage, "OK");
            }
            return WikiData.PlotShort.PlainText;
        }

    }

    public class WikipediaData
    {
        public string IMDbId { get; set; }
        public string Title { get; set; }
        public string FullTitle { get; set; }
        public string Type { get; set; }
        public string Year { get; set; }

        public string Language { get; set; }
        public string TitleInLanguage { get; set; }
        public string Url { get; set; }

        public WikipediaDataPlot PlotShort { get; set; }
        public WikipediaDataPlot PlotFull { get; set; }

        public string ErrorMessage { get; set; }
    }

    public class WikipediaDataPlot
    {
        public string PlainText { get; set; }
        public string Html { get; set; }
    }
}