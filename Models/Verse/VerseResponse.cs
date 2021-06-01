using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bible_Bot.Models
{
    public class VerseResponse
    {
        public string Id { get; set; }
        public string BookId { get; set; }
        public string ChapterId { get; set; }

        public string BibleId { get; set; }
        public string Reference { get; set; }
        public string Content { get; set; }
    }
}
