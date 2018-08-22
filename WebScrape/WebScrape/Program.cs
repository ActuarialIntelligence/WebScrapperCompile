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
            var linkedInSearchResults = GetUrlList(url, "linkedin.com/in/");
            var finalResultSet = new List<string>();
            foreach (var link in linkedInSearchResults)
            {
                if(link.Substring(0,5)=="https")
                {
                    finalResultSet.Add(link);
                }
            }
            var rawHTML = new List<string>();
            foreach (var link in finalResultSet)
            {
                rawHTML.Add(HTML.GetHTMLFromUrl(link));
            }

        }

        private static IList<string> GetUrlList(string url, string keyword)
        {
            var urlsList = new List<string>();
            if (url != "")
            {
                var results = HTML.GetGoogleResultUrlsContainingKeyword(url, keyword);
                
                foreach (var res in results)
                {
                    var removed = res.Replace("/url?q=", "");
                    removed = removed.Replace("&amp", "");
                    urlsList.Add(removed);
                }
                return urlsList;
            }
            else
            {
                return null;
            }
        }
    }
}
