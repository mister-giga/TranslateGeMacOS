using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XFCore
{
    public partial class WordsPage : ContentPage
    {
        public WordsPage(ContentView innerView)
        {
            InitializeComponent();

            if (innerView is OnlineWordsView online)
                online.ShowPopup = showPopup;
            if (innerView is LocalWordsView local)
                local.ShowPopup = showPopup;

            rootGrid.Children.Remove(popupRootView);
            rootGrid.Children.Add(innerView);
            rootGrid.Children.Add(popupRootView);

            popupBackView.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command((o) => hidePopup()) });
            popupRootView.IsVisible = false;
        }


        uint _popupShowDuration = 222;
        private async void showPopup()
        {
            popupRootView.IsVisible = true;
            await popupRootView.FadeTo(1, _popupShowDuration, Easing.SinIn);
        }
        private async void hidePopup()
        {
            await popupRootView.FadeTo(0, _popupShowDuration, Easing.SinOut);
            popupRootView.IsVisible = false;
        }

    }
}
