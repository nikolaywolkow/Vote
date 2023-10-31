using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Vote
{
    public class InformationCandidate : ContentPage
    {
        public InformationCandidate(сandidate сandidate)
        {

            Label secondname = new Label
            {
                Text = сandidate.secondname,
                FontSize = 28,
                TextColor = Color.Black,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start
            };
            Label firstname = new Label
            {
                Text = сandidate.firstname + " " + сandidate.thirdname,
                FontSize = 22,
                TextColor = Color.Black,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start
            };
            Image face = new Image()
            {
                Source = сandidate.img_url + сandidate.image
            };
            Label party = new Label()
            {
                Text = "Партийность " + сandidate.party,
                FontSize = 15,
                TextColor = Color.Black,
                Margin = 12
            };
            Label description = new Label()
            {
                Text = сandidate.description,
                TextColor = Color.Black,
                FontSize = 15,
                Margin = 12
            };
            Button ToMainPageButton = new Button
            {
                Text = "Назад",
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start
            };
            ScrollView scroll = new ScrollView()
            {
                Content = description
            };
            
            StackLayout page = new StackLayout();
            page.Children.Add(ToMainPageButton);
            page.Children.Add(secondname);
            page.Children.Add(firstname);
            page.Children.Add(face);
            page.Children.Add(party);
            page.Children.Add(scroll);
            ToMainPageButton.Clicked += ToMainPage;
            Content = page;

        }
        private async void ToMainPage(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}