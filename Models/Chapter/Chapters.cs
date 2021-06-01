using System;
using System.Collections.Generic;

namespace Bible_Bot.Models
{
    public class Chapters
    {
        public Chapter Data { get; set; }

    }
    public class Chapter
    {
        public string Id { get; set; }
        public string BibleId { get; set; }
        public string BookId { get; set; }
        public string Number { get; set; }
        public string Reference { get; set; }
        public int VerseCount { get; set; }
        public string Content { get; set; }

    }
    /*
    "data": {
    "id": "ROM.1",
    "bibleId": "685d1470fe4d5c3b-01",
    "number": "1",
    "bookId": "ROM",
    "reference": "Romans 1",
    "copyright": "public domain",
    "verseCount": 32,
     */
}

