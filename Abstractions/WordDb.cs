using System;
namespace Abstractions
{
    public class WordDB
    {
        public int Id { get; set; }
        public int OnlineId { get; set; }
        public string Word { get; set; }
        public string Definition { get; set; }
    }
}
