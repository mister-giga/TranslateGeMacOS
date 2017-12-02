using System;
using System.Windows.Input;
using Abstractions;
using Xamarin.Forms;

namespace XFCore
{
    public class OnlineWordWrapper : WordDB
    {

        Action<WordDB> _addAction;
        public OnlineWordWrapper(WordDB word, Action<WordDB> addAction)
        {
            this.Definition = word.Definition;
            this.Id = word.Id;
            this.OnlineId = word.OnlineId;
            this.Word = word.Word;
            _addAction = addAction;
        }

        public bool IsSavedLocally { get; set; }

        public ICommand SaveCommand => new Command<WordDB>(_addAction);

        public string Grammars => Definition.ExtractGrammarKeys();
    }
}
