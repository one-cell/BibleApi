using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Bible_Bot.Clients;
using Bible_Bot.Extensions;
using Bible_Bot.Models;
using Bible_Bot.Models.BookResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bible_Bot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BibleDataController : ControllerBase
    {
        private readonly ILogger<BibleDataController> _logger;
        private readonly Clients.BibleClient _bibleClient;
        private readonly IDynamoDbClient _dynamoDbClient;
        public BibleDataController(ILogger<BibleDataController> logger, Clients.BibleClient bibleClient, IDynamoDbClient dynamoDbClient)
        {
            _logger = logger;
            _bibleClient = bibleClient;
            _dynamoDbClient = dynamoDbClient;
        }

        [HttpGet("getBiblesPublic")]
        public async Task<Bibles> GetBibleByLang([FromQuery] BibleParameters parameters)
        {
            var bibles = await _bibleClient.GetBiblesByLang(parameters.Language);

            return bibles;
        }

        [HttpGet("getChapterPublic")]
        public async Task<ChapterResponse> GetChaptersByBook([FromQuery] ChapterParameters parameters)
        {
            var chapter = await _bibleClient.GetChaptersByBook(parameters.BibleId, parameters.ChapterId);

            ChapterResponse result = new ChapterResponse
            {
                Chapter = chapter.Data.BibleId + chapter.Data.Id,
                BibleId = chapter.Data.BibleId,
                ChapterId = chapter.Data.Id,
                Content = chapter.Data.Content,
                BookId = chapter.Data.BookId,
                Number = chapter.Data.Number,
                Reference = chapter.Data.Reference,
                VerseCount = chapter.Data.VerseCount
            };

            return result;
        }

        [HttpGet("getVersePublic")]
        public async Task<VerseResponse> GetVersesById([FromQuery] VerseParameters parameters)
        {
            var verse = await _bibleClient.GetVersesById(parameters.BibleId, parameters.VerseId);

            var result = new VerseResponse
            {
                Id = verse.Data.Id,
                BibleId = verse.Data.BibleId,
                BookId = verse.Data.BookId,
                ChapterId = verse.Data.ChapterId,
                Reference = verse.Data.Reference,
                Content = verse.Data.Content
            };

            return result;
        }

        [HttpGet("getVersesQuery")]
        public async Task<VersesQuery> GetVersesByQuery([FromQuery] VersesQueryParameters parameters)
        {
            var verse = await _bibleClient.GetVersesByQuery(parameters.BibleId, parameters.Query, parameters.Offset);



            return verse;
        }

        ///////////////////////////////////\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        
        [HttpGet("getVerseDb")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetVersesByIdFromDB([FromQuery] VerseParameters parameters)
        {
            var result = await _dynamoDbClient.GetVersesByIdDb(parameters.BibleId, parameters.VerseId);

            if (result == null)
                return NotFound("no");
            else
            {
                var responce = new VerseResponse
                {
                    Id = result.VerseId,
                    BibleId = result.BibleId,
                    BookId = result.BookId,
                    ChapterId = result.ChapterId,
                    Reference = result.Reference,
                    Content = result.Content
                };

                return Ok(responce);
            }
        }

        [HttpPost("addVerseDb")]
        public async Task<IActionResult> AddVerseToDb([FromBody] VerseResponse verse)
        {
            var data = new VersesDbRepository
            {
                VerseId = verse.Id,
                BibleId = verse.BibleId,
                BookId = verse.BookId,
                ChapterId = verse.ChapterId,
                Reference = verse.Reference,
                Content = verse.Content
            };

            var result = await _dynamoDbClient.AddVerseToDb(data);

            if (result == false)
                return BadRequest("Can't insert the value to database.");

            return Ok("The value has been successfully added to database.");
        }

        ///////////////////////////////////\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        [HttpGet("getUsersBibleDb")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUsersByIdFromDB([FromQuery] string userId)
        {
            var result = await _dynamoDbClient.GetUserByIdDb(userId);

            if (result == null)
                return NotFound("no");
            else
                return Ok(result);
        }

        [HttpPost("addUsersBibleDb")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUserInDb([FromBody] UserDbRepository data)
        {
            var result = await _dynamoDbClient.UpdateUserInDb(data);

            if (result == false)
                return null;

            return Ok("The value has been successfully added to database.");
        }

        ///////////////////////////////////\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        [HttpPost("addBiblesDb")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddBiblesToDb([FromBody] Bibles biblesLang)
        {
            List<BibleResponse> data = new List<BibleResponse>();

            for (int i = 0; i < biblesLang.Data.Count; i++)
            {
                BibleResponse tempBbl = new BibleResponse
                {
                    BibleId = biblesLang.Data[i].Id,
                    Name = biblesLang.Data[i].Name,
                    NameLocal = biblesLang.Data[i].NameLocal,
                    Abbreviation = biblesLang.Data[i].Abbreviation,
                    AbbreviationLocal = biblesLang.Data[i].AbbreviationLocal,
                    Language = biblesLang.Data[i].Language.Id
                };

                data.Add(tempBbl);
            }  

            var result = await _dynamoDbClient.AddBiblesToDb(data);

            if (result == false)
                return BadRequest("Can't insert the value to database.");

            return Ok("The value has been successfully added to database.");
        }

        [HttpGet("getBiblesDb")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBiblesByLangDb(string lang)
        {
            var result = await _dynamoDbClient.GetBiblesByLangDb(lang);

            try
            {
                if (result.Count > 0)
                    return Ok(result);
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }

        ///////////////////////////////////\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        [HttpGet("getChapterDb")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetChapterByIdDb([FromQuery] string bibleId, string chapterId)
        {
            var result = await _dynamoDbClient.GetChapterByIdDb(bibleId, chapterId);

            if (result == null)
                return NotFound("no");
            else
            {
                return Ok(result);
            }
        }

        [HttpPost("addChapterDb")]
        public async Task<IActionResult> AddChapterByBibleToDb([FromBody] ChapterResponse chapt)
        {
            var result = await _dynamoDbClient.AddChapterByBibleToDb(chapt);

            if (result == false)
                return BadRequest("Can't insert the value to database.");

            return Ok("The value has been successfully added to database.");
        }
        
        [HttpGet("getSingleBookPublic")]
        public async Task<BookDb> GetBookById([FromQuery] string bibleId, string bookId)
        {
            var books = await _bibleClient.GetBookById(bibleId, bookId);

            List<string> chapts = new List<string>();
            for (int i = 0; i < books.Data.Chapters.Count; i++)
            {
                if (!Regex.IsMatch(books.Data.Chapters[i].Id, @"(\w*)intro"))
                {
                    chapts.Add(books.Data.Chapters[i].Id);
                }
            }

            BookDb book = new BookDb
            {
                Book = books.Data.BibleId + books.Data.Id,
                BookId = books.Data.Id,
                BibleId = books.Data.BibleId,
                Abbreviation = books.Data.Abbreviation,
                Name = books.Data.Name,
                NameLong = books.Data.NameLong,
                Chapters = new List<string>(chapts)
            };

            return book;
        }
        [HttpGet("getSingleBookDb")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBookDB([FromQuery] string book)
        {
            var result = await _dynamoDbClient.GetBookDb(book);

            if (result == null)
                return NotFound("no");
            else
                return Ok(result);
        }
        [HttpPost("addSingleBookToDb")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddBookByToDb([FromBody] BookDb books)
        {
            var result = await _dynamoDbClient.AddBookByToDb(books);

            if (result == false)
                return BadRequest("Can't insert the value to database.");

            return Ok("The value has been successfully added to database.");
        }

        [HttpGet("getMultipleBooksPublic")]
        public async Task<List<BookDbNoChapters>> GetBooksByBible([FromQuery] string bibleId)
        {
            var books = await _bibleClient.GetBooksByBible(bibleId);
            
            List<BookDbNoChapters> arr = new List<BookDbNoChapters>();

            for (int i = 0; i < books.Data.Count; i++)
            {

                BookDbNoChapters book = new BookDbNoChapters
                {
                    Book = books.Data[i].BibleId + books.Data[i].Id,
                    BookId = books.Data[i].Id,
                    BibleId = books.Data[i].BibleId,
                    Abbreviation = books.Data[i].Abbreviation,
                    Name = books.Data[i].Name,
                    NameLong = books.Data[i].NameLong,

                };
                arr.Add(book);
            }
//
           return arr;
    }
        [HttpGet("getMultipleBooksDb")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBooksByBibleDb(string bibleId)
        {
            var result = await _dynamoDbClient.GetBooksByBibleDb(bibleId);

            if (result.Count == 0)
                return NotFound();

            return Ok(result);
        }

        [HttpPost("addMultipleBooksDb")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddBooksByBibleToDb([FromBody] List<BookDbNoChapters> books)
        {
            var result = await _dynamoDbClient.AddBooksByBibleToDb(books);

            if (result == false)
                return BadRequest("Can't insert the value to database.");

            return Ok("The value has been successfully added to database.");
        }
 
    }
}