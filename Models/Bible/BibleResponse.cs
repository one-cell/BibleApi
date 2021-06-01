using System;
using System.Collections.Generic;

namespace Bible_Bot.Models
{
    public class BibleResponse
    {
        public string BibleId { get; set; }
        public string Name { get; set; }
        public string NameLocal { get; set; }
        public string Abbreviation { get; set; }
        public string AbbreviationLocal { get; set; }
        public string Language { get; set; }
    }
}
