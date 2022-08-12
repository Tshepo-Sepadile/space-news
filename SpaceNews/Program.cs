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
            var groupedAricles = article.GroupedArticles(articles);
            article.DisplayArticles(groupedAricles);
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
            try
            {
                var articles = new List<Article>();
                string url = "https://api.spaceflightnewsapi.net/v3/articles?_limit=100";
                _client = new HttpClient();
                _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("json/application"));
                var json = _client.GetStringAsync(url).Result;

                articles = JsonConvert.DeserializeObject<List<Article>>(json);

                return articles;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public List<Article> GroupedArticles(List<Article> articles)
        {
            try
            {
                var sites = new List<string>();
                var groupedArticles = new List<Article>();

                foreach (var article in articles)
                {
                    if (!sites.Contains(article.NewsSite))
                    {
                        article.NumberOfArticles++;
                        groupedArticles.Add(article);
                        sites.Add(article.NewsSite);
                    }
                    else
                    {
                        var exisingSite = groupedArticles.Where(ac => ac.NewsSite == article.NewsSite).FirstOrDefault();
                        exisingSite.NumberOfArticles++;
                    }
                }

                return groupedArticles;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public void DisplayArticles(List<Article> articles)
        {
            try
            {
                foreach (var article in articles)
                    Console.WriteLine($"{article.PublishedAt.ToString("MMMM")} {article.PublishedAt.Year} {article.NewsSite} { article.NumberOfArticles}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }


}
