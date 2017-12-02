using System;
using Xamarin.Forms;

namespace XFCore
{
    public class RootTabbedPage : TabbedPage
    {
        WordsPage _onlinePage;
        WordsPage _offlinePage;
        WordsViewModel _wordsViewModel;
        public RootTabbedPage(Abstractions.IRepository<Abstractions.WordDB> wordsRepo)
        {
            _wordsViewModel = new WordsViewModel(wordsRepo, DisplayAlert);

            _onlinePage = new WordsPage(new OnlineWordsView()) { Title = "ლექსიკონი", BindingContext = _wordsViewModel };
            _offlinePage = new WordsPage(new LocalWordsView()) { Title = "შენახული", BindingContext = _wordsViewModel };

            Children.Add(_onlinePage);
            Children.Add(_offlinePage);
        }
    }
}
