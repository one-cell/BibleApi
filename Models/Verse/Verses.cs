using System;
using System.Collections.Generic;

namespace Bible_Bot.Models
{
    public class Verses
    {
        public Verse Data { get; set; }

    }
    public class Verse
    {
        public string Id { get; set; }
        public string BookId { get; set; }
        public string ChapterId { get; set; }

        public string BibleId { get; set; }
        public string Reference { get; set; }
        public string Content { get; set; }
    }
    /*
    "data": {
    "id": "ROM.1.1",
    "bookId": "ROM",
    "chapterId": "ROM.1",
    "bibleId": "685d1470fe4d5c3b-01",
    "reference": "Romans 1:1",
    "content": "     [1] Paul, a servant of Jesus Christ, called to be an apostle, separated unto the gospel of God, \n",
    "verseCount": 1,
    "copyright": "public domain",
    "next": {
      "id": "ROM.1.2",
      "number": "2"
    },
    "previous": {
      "id": "ROM.intro.0",
      "number": "0"
    }
  }
    /*
{
"data": {
 "reference": "Ѕџ÷№÷® 2:23"
 "content": [
   {
     "items": [
       {
         "text": " ≤ сказаҐ чалавек: У¬ось, на гэты раз, гэта костка з костак ма≥х ≥ цела з цела майго. яна будзе называцца жанчынаю, бо Ґз€та€ з мужаФ."
       }
     ]
   }
 ]
 "next": {
   "id": "GEN.2.24"
 },
 "previous": {
   "id": "GEN.2.22"
 }
}
}
  */
}
