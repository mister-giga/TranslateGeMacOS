using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Abstractions;
using Xamarin.Forms;

namespace XFCore
{
    public class LocalWordWrapper : WordDB
    {
        Action<int> _removeAction;
        public LocalWordWrapper(WordDB word, Action<int> removeAction)
        {
            this.Definition = word.Definition;
            this.Id = word.Id;
            this.OnlineId = word.OnlineId;
            this.Word = word.Word;
            _removeAction = removeAction;
        }

        public ICommand RemoveCommand => new Command<int>(_removeAction);

        public string Grammars => Definition.ExtractGrammarKeys();

    }
}
