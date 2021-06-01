using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bible_Bot.Models.BookResponse
{
    public class BookDb
    {
        public string Book { get; set; }
        public string BookId { get; set; }
        public string BibleId { get; set; }
        public string Abbreviation { get; set; }
        public string Name { get; set; }
        public string NameLong { get; set; }
        public List<string> Chapters { get; set; }
    }
    public class BookPublicResponse
    {
        public BookPublicChaptersInclude Data { get; set; }
    }
    public class BooksPublicResponse
    {
        public List<BookPublic> Data { get; set; } 
    }
    public class BookPublicChaptersInclude
    {
        public string Id { get; set; }
        public string BibleId { get; set; }
        public string Abbreviation { get; set; }
        public string Name { get; set; }
        public string NameLong { get; set; }
        public List<ChapterBook> Chapters { get; set; }
    }
    public class ChapterBook
    {
        public string Id { get; set; }
    }
    public class BookPublic
    {
        public string Id { get; set; }
        public string BibleId { get; set; }
        public string Abbreviation { get; set; }
        public string Name { get; set; }
        public string NameLong { get; set; }
    }
    public class BookDbNoChapters
    {
        public string Book { get; set; }
        public string BookId { get; set; }
        public string BibleId { get; set; }
        public string Abbreviation { get; set; }
        public string Name { get; set; }
        public string NameLong { get; set; }
    }
}
