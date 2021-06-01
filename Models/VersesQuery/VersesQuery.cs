using System;
using System.Collections.Generic;

namespace Bible_Bot.Models
{
    public class VersesQuery
    {
        public QueryVersesData Data { get; set; }
    }

    public class QueryVersesData
    {
        public string Total { get; set; }
        public List<QueryVerseItem> Verses { get; set; }

    }
    public class QueryVerseItem
    {
        public string Id { get; set; }
        public string BookId { get; set; }
        public string BibleId { get; set; }
        public string ChapterId { get; set; }
        public string Reference { get; set; }
        public string Text { get; set; }
    }
}

/*
{
"data": {
"total": 233
"verses": [
{
"id": "1TI.1.1",
"bookId": "1TI",
"bibleId": "685d1470fe4d5c3b-01",
"chapterId": "1TI.1",
"reference": "1 Timothy 1:1",
"text": "Paul, an apostle of Jesus Christ according to the commandment of God our Savior, and the Lord Jesus Christ our hope;"
},
*/
