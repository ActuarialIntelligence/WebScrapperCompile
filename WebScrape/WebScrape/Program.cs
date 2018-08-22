using System.Collections.Generic;
using WebScraper.Logic;

namespace WebScrape
{
    class Program
    {
        static void Main(string[] args)
        {
            var nameAndSurname = HTML.GenerateRandomName();
            var keywordsList = new List<string>();
            foreach (var val in nameAndSurname)
            {
                keywordsList.Add(val);
            }
            keywordsList.Add("CEO");
            keywordsList.Add("LinkedIn");
            var url = HTML.FormGoogleSearchUrl(keywordsList);
            var results = HTML.GetGoogleResultUrlsContainingKeyword(url,"Linked");
        }
    }
}
