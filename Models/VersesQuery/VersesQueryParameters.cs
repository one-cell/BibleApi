using System;
using System.Collections.Generic;

namespace Bible_Bot.Models
{
    public class VersesQueryParameters
    {
        public string BibleId { get; set; }
        public string Query { get; set; }

        public int Offset { get; set; }

    }

}
