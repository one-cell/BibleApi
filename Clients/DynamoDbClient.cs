using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Bible_Bot.Extensions;
using Bible_Bot.Models;
using Bible_Bot.Models.BookResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bible_Bot.Clients
{
    public class DynamoDbClient : IDynamoDbClient
    {
        public string _tableVerses;
        public string _tableUsers;
        public string _tableBibles;
        public string _tableBooks;
        public string _tableChapters;
        public string _tableBooksChapters;
        private readonly IAmazonDynamoDB _dynamoDB;

        public DynamoDbClient(IAmazonDynamoDB dynamoDB)
        {
            _dynamoDB = dynamoDB;
            _tableUsers = Constants.UserTable;
            _tableVerses = Constants.VerseTable;
            _tableBibles = Constants.BiblesTable;
            _tableBooks = Constants.BooksTable;
            _tableChapters = Constants.ChaptersTable;
            _tableBooksChapters = Constants.BooksChaptersTable;
        }

        ///////////////////////////////////\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        public async Task<List<BibleResponse>> GetBiblesByLangDb(string lang)
        {
            var result = new List<BibleResponse>();

            var request = new ScanRequest
            {
                TableName = _tableBibles
            };

            var response = await _dynamoDB.ScanAsync(request);

            foreach (Dictionary<string, AttributeValue> item in response.Items)
            {
                BibleResponse bbl = item.ToClass<BibleResponse>();

                if (bbl.Language == lang)
                    result.Add(bbl);
            }

            return result;
        }
        
        public async Task<bool> AddBiblesToDb(List<BibleResponse> biblesLang)
        {
            bool success = true;

            for (int i = 0; i < biblesLang.Count; i++)
            {
                var request = new PutItemRequest
                {
                    TableName = _tableBibles,
                    Item = new Dictionary<string, AttributeValue>
                    {
                        { "BibleId", new AttributeValue { S = biblesLang[i].BibleId } },
                        { "Name", new AttributeValue { S = biblesLang[i].Name } },
                        { "NameLocal", new AttributeValue { S = biblesLang[i].NameLocal } },
                        { "Abbreviation", new AttributeValue { S = biblesLang[i].Abbreviation } },
                        { "AbbreviationLocal", new AttributeValue { S = biblesLang[i].AbbreviationLocal } },
                        { "Language", new AttributeValue { S = biblesLang[i].Language } }
                    }
                };
                try
                {
                    var response = await _dynamoDB.PutItemAsync(request);
                    //return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
                }
                catch (Exception e)
                {
                    //Console.WriteLine("You've goddamn wrong \n" + e);

                    success = false;
                }
            }

            return success;
        }

        ///////////////////////////////////\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        public async Task<VersesDbRepository> GetVersesByIdDb(string bibleId, string verseId)
        {
            var item = new GetItemRequest
            {
                TableName = _tableVerses,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "Verse", new AttributeValue{S = bibleId + verseId} }
                }
            };

            var response = await _dynamoDB.GetItemAsync(item);

            if (response.Item == null || !response.IsItemSet)
                return null;

            var result = response.Item.ToClass<VersesDbRepository>();

            return result; 
        }

        public async Task<bool> AddVerseToDb(VersesDbRepository data)
        {
            var request = new PutItemRequest
            {
                TableName = _tableVerses,
                Item = new Dictionary<string, AttributeValue>
                {
                    { "Verse", new AttributeValue { S = data.BibleId + data.VerseId } },
                    { "VerseId", new AttributeValue { S = data.VerseId } },
                    { "BibleId", new AttributeValue { S = data.BibleId } },
                    { "BookId", new AttributeValue { S = data.BookId } },
                    { "ChapterId", new AttributeValue { S = data.ChapterId } },
                    { "Reference", new AttributeValue { S = data.Reference } },
                    { "Content", new AttributeValue { S = data.Content } }
                }
            };
            try
            {
                var response = await _dynamoDB.PutItemAsync(request);

                return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch(Exception e)
            {
                Console.WriteLine("You've goddamn wrong \n" + e);
                return false;
            }
        }

        ///////////////////////////////////\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        public async Task<UserDbRepository> GetUserByIdDb(string userId)
        {
            var item = new GetItemRequest
            {
                TableName = _tableUsers,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "UserId", new AttributeValue{S = userId} }
                }
            };

            var response = await _dynamoDB.GetItemAsync(item);

            if (response.Item == null || !response.IsItemSet)
                return null;

            var result = response.Item.ToClass<UserDbRepository>();

            return result;
        }
 
        public async Task<bool> UpdateUserInDb(UserDbRepository userObj)
        {
            var request = new UpdateItemRequest
            {
                TableName = _tableUsers,
                Key = new Dictionary<string, AttributeValue>()
                {
                    { "UserId", new AttributeValue { S = userObj.UserId } }
                },
                ExpressionAttributeNames = new Dictionary<string, string>()
                {
                    {"#BI", "BibleId"}
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
                {
                    {":newBbl",new AttributeValue {S = userObj.BibleId}}
                },

                UpdateExpression = "SET #BI = :newBbl"
            };

            try
            {
                var response = await _dynamoDB.UpdateItemAsync(request);

                return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                Console.WriteLine("You've goddamn wrong \n" + e);
                return false;
            }
        }

        ///////////////////////////////////\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        public async Task<BookDb> GetBookDb(string book)
        {
            var item = new GetItemRequest
            {
                TableName = _tableBooksChapters,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "Book", new AttributeValue{S = book} }
                }
            };

            var response = await _dynamoDB.GetItemAsync(item);

            if (response.Item == null || !response.IsItemSet)
                return null;

            var result = response.Item.ToClass<BookDb>();

            return result;
        }
        public async Task<bool> AddBookByToDb(BookDb bookBible)
        {
            var request = new PutItemRequest
            {
                TableName = _tableBooksChapters,
                Item = new Dictionary<string, AttributeValue>
                {
                    { "Book", new AttributeValue { S = bookBible.Book } },
                    { "BibleId", new AttributeValue { S = bookBible.BibleId } },
                    { "BookId", new AttributeValue { S = bookBible.BookId } },
                    { "Abbreviation", new AttributeValue { S = bookBible.Abbreviation } },
                    { "Name", new AttributeValue { S = bookBible.Name } },
                    { "NameLong", new AttributeValue { S = bookBible.NameLong } },
                    { "Chapters", new AttributeValue { SS = new List<string>(bookBible.Chapters) } }
                }
            };
            try
            {
                var response = await _dynamoDB.PutItemAsync(request);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<BookDbNoChapters>> GetBooksByBibleDb(string bibleId)
        {
            var result = new List<BookDbNoChapters>();

            var request = new ScanRequest
            {
                TableName = _tableBooks
            };

            var response = await _dynamoDB.ScanAsync(request);
            if (response.Items == null || response.Items.Count == 0)
                return null;

            foreach (Dictionary<string, AttributeValue> item in response.Items)
            {
                BookDbNoChapters bk = item.ToClass<BookDbNoChapters>();

                if (bk.BibleId == bibleId)
                    result.Add(bk);
            }

            return result;
        }

        public async Task<bool> AddBooksByBibleToDb(List<BookDbNoChapters> bookBible)
        {
            bool success = true;

            for (int i = 0; i < bookBible.Count; i++)
            {
                var request = new PutItemRequest
                {
                    TableName = _tableBooks,
                    Item = new Dictionary<string, AttributeValue>
                    {
                        { "Book", new AttributeValue { S = bookBible[i].Book } },
                        { "BibleId", new AttributeValue { S = bookBible[i].BibleId } },
                        { "BookId", new AttributeValue { S = bookBible[i].BookId } },
                        { "Abbreviation", new AttributeValue { S = bookBible[i].Abbreviation } },
                        { "Name", new AttributeValue { S = bookBible[i].Name } },
                        { "NameLong", new AttributeValue { S = bookBible[i].NameLong } }
                    }
                };
                try
                {
                    var response = await _dynamoDB.PutItemAsync(request);
                    //return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
                }
                catch (Exception e)
                {
                    //Console.WriteLine("You've goddamn wrong \n" + e);
                    success = false;
                }
            }

            return success;
        }

        ///////////////////////////////////\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        public async Task<ChapterResponse> GetChapterByIdDb(string bibleId, string chapterId)
        {
            var item = new GetItemRequest
            {
                TableName = _tableChapters,
                Key = new Dictionary<string, AttributeValue>
                {
                    { "Chapter", new AttributeValue{S = bibleId + chapterId} }
                }
            };

            var response = await _dynamoDB.GetItemAsync(item);

            if (response.Item == null || !response.IsItemSet)
                return null;

            var result = response.Item.ToClass<ChapterResponse>();

            return result;
        }

        public async Task<bool> AddChapterByBibleToDb(ChapterResponse chapterBible)
        { 
            var request = new PutItemRequest
            {
                TableName = _tableChapters,
                Item = new Dictionary<string, AttributeValue>
                {
                    { "Chapter", new AttributeValue { S = chapterBible.BibleId + chapterBible.ChapterId } },
                    { "BibleId", new AttributeValue { S = chapterBible.BibleId } },
                    { "BookId", new AttributeValue { S = chapterBible.BookId } },
                    { "ChapterId", new AttributeValue { S = chapterBible.ChapterId } },
                    { "Number", new AttributeValue { S = chapterBible.Number } },
                    { "VerseCount", new AttributeValue { N = Convert.ToString(chapterBible.VerseCount) } },
                    { "Reference", new AttributeValue { S = chapterBible.Reference } },
                    { "Content", new AttributeValue { S = chapterBible.Content } }
                }
            };
            try
            {
                var response = await _dynamoDB.PutItemAsync(request);

                return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                Console.WriteLine("You've goddamn wrong \n" + e);
                return false;
            }
        }

        ///////////////////////////////////\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\


    }
}

/*
 public async Task<bool> AddUserToDb(UserDbRepository usersBbl)
 {
     var request = new PutItemRequest
     {
         TableName = _tableUsers,
         Item = new Dictionary<string, AttributeValue>
         {
             { "BibleId", new AttributeValue { S = usersBbl.BibleId } },
             { "UserId", new AttributeValue { S = usersBbl.UserId } }
         }
     };
     try
     {
         var response = await _dynamoDB.PutItemAsync(request);

         return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
     }
     catch (Exception e)
     {
         Console.WriteLine("You've goddamn wrong \n" + e);
         return false;
     }
 }
 */