using Bible_Bot.Models;
using Bible_Bot.Models.BookResponse;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bible_Bot.Clients
{
    public class BibleClient
    {
        private HttpClient _client;

        private static string _address;
        private static string _apikey;
        public BibleClient()
        {
            _address = Constants.UrlAPI;
            _apikey = Constants.KeyAPI;

            _client = new HttpClient();
            _client.BaseAddress = new Uri(_address);
            _client.DefaultRequestHeaders.Add("api-key", _apikey);
        }
        
        //bible
        public async Task<Bibles> GetBiblesByLang(string lang)
        {
            var responce = await _client.GetAsync($"/v1/bibles?language={lang}&include-full-details=false");
            responce.EnsureSuccessStatusCode();

            var content = responce.Content.ReadAsStringAsync().Result;

            var result = JsonConvert.DeserializeObject<Bibles>(content);

            return result;
        }
        //book
        public async Task<BookPublicResponse> GetBookById(string bibleId, string bookId)
        {
            var responce = await _client.GetAsync($"v1/bibles/{bibleId}/books/{bookId}?include-chapters=true");
            responce.EnsureSuccessStatusCode();

            var content = responce.Content.ReadAsStringAsync().Result;

            var result = JsonConvert.DeserializeObject<BookPublicResponse>(content);

            return result;
        }
        public async Task<BooksPublicResponse> GetBooksByBible(string bibleId)
        {
            var responce = await _client.GetAsync($"/v1/bibles/{bibleId}/books?include-chapters=true&include-chapters-and-sections=false");
            responce.EnsureSuccessStatusCode();

            var content = responce.Content.ReadAsStringAsync().Result;

            var result = JsonConvert.DeserializeObject<BooksPublicResponse>(content);

            return result;
        }
        //chapter
        public async Task<Chapters> GetChaptersByBook(string bibleId, string chapterId)
        {
            var responce = await _client.GetAsync($"/v1/bibles/{bibleId}/chapters/{chapterId}?content-type=text&include-notes=false&include-titles=false&include-chapter-numbers=false&include-verse-numbers=false&include-verse-spans=false");
            responce.EnsureSuccessStatusCode();

            var content = responce.Content.ReadAsStringAsync().Result;

            var result = JsonConvert.DeserializeObject<Chapters>(content);

            return result;
        }
        //verse
        public async Task<Verses> GetVersesById(string bibleId, string verseId)
        {
            var responce = await _client.GetAsync($"/v1/bibles/{bibleId}/verses/{verseId}?content-type=text&include-notes=false&include-titles=false&include-chapter-numbers=false&include-verse-numbers=false&include-verse-spans=false&use-org-id=false");
            responce.EnsureSuccessStatusCode();

            var content = responce.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<Verses>(content);

            return result;
        }
        //verse query
        public async Task<VersesQuery> GetVersesByQuery(string bibleId, string queryText, int offset)
        {
            var responce = await _client.GetAsync($"/v1/bibles/{bibleId}/search?query={queryText}&offset={offset}&sort=relevance&fuzziness=AUTO");
            responce.EnsureSuccessStatusCode();

            var content = responce.Content.ReadAsStringAsync().Result;

            var result = JsonConvert.DeserializeObject<VersesQuery>(content);

            if (result.Data.Total == "0")
                return null;

            return result;
        }
    }
}
