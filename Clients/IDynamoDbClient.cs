using Bible_Bot.Models;
using Bible_Bot.Models.BookResponse;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bible_Bot.Clients
{
    public interface IDynamoDbClient
    {
        public Task<VersesDbRepository> GetVersesByIdDb(string bibleId, string verseId);
        public Task<bool> AddVerseToDb(VersesDbRepository verse);

        public Task<UserDbRepository> GetUserByIdDb(string userId);
        public Task<bool> UpdateUserInDb(UserDbRepository usersBbl);

        public Task<List<BibleResponse>> GetBiblesByLangDb(string lang);
        public Task<bool> AddBiblesToDb(List<BibleResponse> biblesLang);

        public Task<List<BookDbNoChapters>> GetBooksByBibleDb(string bibleId);
        public Task<bool> AddBooksByBibleToDb(List<BookDbNoChapters> booksBible);

        public Task<ChapterResponse> GetChapterByIdDb(string bibleId, string chapterId);
        public Task<bool> AddChapterByBibleToDb(ChapterResponse chapterBible);

        public Task<BookDb> GetBookDb(string book);
        public Task<bool> AddBookByToDb(BookDb booksBible);
    }
}
//public Task<bool> AddUserToDb(UserDbRepository usersInfo);