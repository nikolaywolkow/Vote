using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace Vote
{
    public class Loading : ContentPage
    {
        public Loading()
        {
            Image logo = new Image()
            {
                Source = ImageSource.FromResource("Vote.vote.png")
                ,HeightRequest = 90

            };
            StackLayout page = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.Center
                ,VerticalOptions = LayoutOptions.Center
            };

            Label copiring = new Label()
            {
                TextColor = Color.Black
                ,FontSize = 17
                ,Text = "\n Copyright (c) 2018 AlvaStudio. All rights reserved. \n"
                ,VerticalOptions = LayoutOptions.End
            };


            page.Children.Add(logo);
            page.Children.Add(copiring);
            Content = page;
            DisplayTime();

        }
        bool alive = true;
        private bool OnTimerTick()
        {
            
            return alive;
        }
        private async void DisplayTime()
        {
            while (alive)
            {
                await Task.Delay(6000);
                await Navigation.PopModalAsync();
                break;
            }
        }
        private void TimerButton_Clicked(object sender, EventArgs e)
        {
            if (alive == true)
            {
                alive = false;
            }
            else
            {
                alive = true;
                Device.StartTimer(TimeSpan.FromSeconds(3), OnTimerTick);
            }
        }
        public async void ToMainPage(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}