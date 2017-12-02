using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Abstractions;
using Xamarin.Forms;

namespace XFCore
{
    public class WordsViewModel : INotifyPropertyChanged
    {
        private readonly IRepository<WordDB> _wordsRepository;
        private readonly Func<string, string, string, Task> _displayAlert;

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }


        private string _wordToAdd;
        public string WordToAdd
        {
            get => _wordToAdd;
            set => SetProperty(ref _wordToAdd, value, AddNewWordCommand.ChangeCanExecute);
        }

        private string _wordDescriptionToAdd;
        public string WordDescriotionToAdd
        {
            get => _wordDescriptionToAdd;
            set => SetProperty(ref _wordDescriptionToAdd, value, AddNewWordCommand.ChangeCanExecute);
        }

        private string _localSearchText = string.Empty;
        public string LocalSearchText
        {
            get => _localSearchText;
            set => SetProperty(ref _localSearchText, value, () => OnPropertyChanged(nameof(LocalWords)));
        }

        private string _onlineSearchText = string.Empty;
        public string OnlineSearchText
        {
            get => _onlineSearchText;
            set => SetProperty(ref _onlineSearchText, value, () => startDownloadingNewWordsByKey(value));
        }

        public IEnumerable<LocalWordWrapper> LocalWords => _wordsRepository.ContaintsInWord(LocalSearchText).Select(o => new LocalWordWrapper(o, (id) => { _wordsRepository.RemoveItem(id); OnPropertyChanged(nameof(LocalWords)); })).OrderByDescending(o => o.Id);

        private IEnumerable<OnlineWordWrapper> _onlineWords;
        public IEnumerable<OnlineWordWrapper> OnlineWords
        {
            get
            {
                if (_onlineWords == null)
                    return null;

                var converted = _onlineWords.ToList();
                var matchingIds = _wordsRepository.GetMatchingIds(converted.Select(o => o.OnlineId).ToList());


                for (int i = 0; i < converted.Count; i++)
                {
                    var item = converted[i];
                    bool isStored = matchingIds.Contains(item.Id);

                    item.IsSavedLocally = !isStored;
                }

                return converted;
            }
        }

        async void startDownloadingNewWordsByKey(string key)
        {

            IEnumerable<WordDB> onlineWords = null;
            try
            {
                IsLoading = true;
                onlineWords = await OnlineDictionaryClient.Instance.GetWordsAsync(OnlineSearchText);
                IsLoading = false;
            }
            catch (NoInternetException)
            {
                IsLoading = false;
                await _displayAlert("ყურადღება", "არ არის კავშირი ინტერნეტთან", "დახურვა");
            }
            catch
            {
                IsLoading = false;
                return;
            }
            if (onlineWords == null || onlineWords.Count() == 0)
                return;


            IEnumerable<OnlineWordWrapper> wrappedList = onlineWords.Select(o => new OnlineWordWrapper(o, save)
            {
            });

            _onlineWords = wrappedList;
            OnPropertyChanged(nameof(OnlineWords));

            void save(WordDB word)
            {
                _wordsRepository.AddItem(new WordDB
                {
                    OnlineId = word.OnlineId,
                    Word = word.Word,
                    Definition = word.Definition
                });
                displayWordAddSuccess(word.Word);
                OnPropertyChanged(nameof(LocalWords));
            }
        }

        public Command AddNewWordCommand { get; }

        public WordsViewModel(IRepository<WordDB> wordsRepository, Func<string, string, string, Task> displayAlert)
        {
            _wordsRepository = wordsRepository;
            _displayAlert = displayAlert;
            AddNewWordCommand = new Command(addNewWordLocally, canAddnewWordLocally);
        }

        void addNewWordLocally()
        {
            var wasAdded = _wordsRepository.AddItem(new WordDB()
            {
                Word = WordToAdd,
                Definition = WordDescriotionToAdd,
            });

            displayWordAddSuccess(WordToAdd);
            WordToAdd = string.Empty;
            WordDescriotionToAdd = string.Empty;
            OnPropertyChanged(nameof(LocalWords));

        }
        bool canAddnewWordLocally()
        {
            return !(string.IsNullOrWhiteSpace(WordToAdd) || string.IsNullOrWhiteSpace(WordDescriotionToAdd));
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual bool SetProperty<T>(ref T storage, T value, Action action = null, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value)) return false;

            storage = value;

            action?.Invoke();

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

            return true;
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        async void displayWordAddSuccess(string word)
        {
            await _displayAlert("ყურადღება", $"სიტყვა: {word} დაემატა ლოკალურ ლექსიკონში", "დახურვა");
        }
        async void displayWordAddFail(string word)
        {
            await _displayAlert("ყურადღება", $"სიტყვა: {word} ვერ დაემატა ლოკალურ ლექსიკონში", "დახურვა");
        }
    }
}
