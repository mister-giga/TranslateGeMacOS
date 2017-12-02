using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abstractions;
using Plugin.Connectivity;

namespace XFCore
{
    public class OnlineDictionaryClient
    {
        private static OnlineDictionaryClient _instance;
        public static OnlineDictionaryClient Instance => _instance ?? (_instance = new OnlineDictionaryClient());
        private OnlineDictionaryClient()
        {
            _client = new System.Net.Http.HttpClient();
        }
        System.Net.Http.HttpClient _client;

        public async Task<IEnumerable<WordDB>> GetWordsAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return null;
            _client.CancelPendingRequests();

            if (CrossConnectivity.Current.IsConnected == false)
                throw new NoInternetException();

            var json = await _client.GetStringAsync($"http://translate.ge/api/{key}");
            var jObj = Newtonsoft.Json.JsonConvert.DeserializeObject<RootObject>(json);

            return jObj.rows.Select(o => new WordDB
            {
                OnlineId = o.value.wordID,
                Word = o.value.Word,
                Definition = o.value.Text
            });
        }


        public class Value
        {
            public string _id { get; set; }
            public string _rev { get; set; }
            public int wordID { get; set; }
            public string Word { get; set; }
            public string Text { get; set; }
            public int DictType { get; set; }
            public string DictName { get; set; }
            public int soundcode { get; set; }
        }

        public class Row
        {
            public string id { get; set; }
            public string key { get; set; }
            public Value value { get; set; }
        }

        public class RootObject
        {
            public int total_rows { get; set; }
            public int offset { get; set; }
            public List<Row> rows { get; set; }
        }
    }
    public class NoInternetException : Exception { }
}
