using System;
using System.Collections.Generic;
using System.Text;

namespace XFCore
{
    public static class Extensions
    {
        public static string ExtractGrammarKeys(this string str)
        {
            var Definition = str;
            if (string.IsNullOrWhiteSpace(Definition))
                return string.Empty;

            List<string> stuff = new List<string>();

            List<string> keys = new List<string> {
                "noun",
                "verb",
                "adjective",
            };
            var fixedTxt = Definition.ToLower();

            keys.ForEach(o => { if (fixedTxt.Contains(o)) stuff.Add(o); });

            if (stuff.Count == 0)
                return string.Empty;

            StringBuilder builder = new StringBuilder();
            var len = Math.Min(stuff.Count - 1, 2);
            for (int i = 0; i <= len; i++)
            {
                builder.Append(stuff[i]);

                if (i != len)
                    builder.Append($" \u2022 ");

            }

            return builder.ToString();
        }
    }
}
