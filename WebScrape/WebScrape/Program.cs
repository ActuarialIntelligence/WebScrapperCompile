using System.Collections.Generic;
using WebScraper.Logic;

namespace WebScrape
{
    class Program
    {
        static void Main(string[] args)
        {
            var namesTest = new List<string>() { "Sathyandranath Iyer"};
            var nameAndSurname = namesTest; //HTML.GenerateRandomName();
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
            var metaDataResults = HTML.GetAllInnerTextByNode(url, "span");
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
