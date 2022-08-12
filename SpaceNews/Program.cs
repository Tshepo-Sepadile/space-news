using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SpaceNews
{
    class Program
    {
        

        static void Main(string[] args)
        {
            Article article = new Article();
            var articles = article.GetArticles();
            article.DisplayArticles(articles);
            Console.ReadLine();
        }

        
    }

    public class Article
    {
        HttpClient _client;

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("newsSite")]
        public string NewsSite { get; set; }

        [JsonProperty("publishedAt")]
        public DateTime PublishedAt { get; set; }

        public int NumberOfArticles { get; set; }

        public List<Article> GetArticles()
        {
            var articles = new List<Article>();

            try
            {
                string url = "https://api.spaceflightnewsapi.net/v3/articles?_limit=100";
                _client = new HttpClient();
                _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("json/application"));
                var json = _client.GetStringAsync(url).Result;

                articles = JsonConvert.DeserializeObject<List<Article>>(json);
            }
            catch (Exception ex)
            {
                throw;
            }

            return articles;

        }

        public void DisplayArticles(List<Article> articles)
        {
            int count = 0;

            var articleCount = new List<Article>();

            foreach(var article in articles)
            {
                if (!articleCount.Contains(article))
                {
                    article.NumberOfArticles = 1;
                    articleCount.Add(article);
                }
                else
                {

                    articleCount.Remove(article);
                    article.NumberOfArticles++;
                    articleCount.Add(article);
                     
                }
            }

            foreach(var article in articleCount)
            {
                Console.WriteLine($"{article.PublishedAt.ToString("MMMM")} {article.PublishedAt.Year} {article.NewsSite} { article.NumberOfArticles}");
            }
        }
    }

    
}
