using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace XFCore
{
    public partial class LocalWordsView : ContentView
    {
        public Action ShowPopup { get; set; }
        public LocalWordsView()
        {
            InitializeComponent();
        }

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            ShowPopup?.Invoke();
        }
    }
}
