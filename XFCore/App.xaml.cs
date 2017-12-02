using System;
using System.Collections.Generic;
using Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace XFCore
{
    public partial class App : Application
    {
        public App(IRepository<WordDB> wordsRepository)
        {
            InitializeComponent();
            MainPage = new RootTabbedPage(wordsRepository);
        }
    }
}
