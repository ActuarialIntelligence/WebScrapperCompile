﻿using RandomNameGeneratorLibrary;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace WebScraper.Logic
{
    public static class HTML
    {
        public static string GetHTMLFromUrl(string urlAddress)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string data="";
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }

                 data = readStream.ReadToEnd();

                response.Close();
                readStream.Close(); 
            }
            return data;
        }

        public static bool CheckFor(string wordInHTML, string urlAddress)
        {
            var html = GetHTMLFromUrl(urlAddress);
            return html.Contains(wordInHTML) == true ? true : false;
        }
        /// <summary>
        /// For each word not in the dictionary, check to see if the word is preceeded by CEO etc...
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static IList<string> ReturnWordsThatAreNotInEnglishDictionary(string html)
        {
            var list = new List<string>();
            var oDict = new NetSpell.SpellChecker.Dictionary.WordDictionary
            {
                DictionaryFile = "en-US.dic"
            };
            oDict.Initialize();
            var words = html.Split(' ');
            foreach (var wordToCheck in words)
            {
                NetSpell.SpellChecker.Spelling oSpell = new NetSpell.SpellChecker.Spelling
                {
                    Dictionary = oDict
                };
                if (!oSpell.TestWord(wordToCheck) && wordToCheck.Length >= 5)
                {
                    list.Add(wordToCheck);
                }
            }
            return list;
        }
        /// <summary>
        /// visit media URL and verify against inner text.
        /// </summary>
        /// <param name="html"></param>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public static IList<string> GetAllInnerTextByNode(string html, string nodeName)
        {
            var innerTextList = new List<string>();
            var doc = new XmlDocument();
            doc.Load(html);
            foreach (XmlNode td in doc.DocumentElement.SelectNodes(nodeName))
            {
                string text = td.InnerText;
                innerTextList.Add(text);
            }
            return innerTextList;
        }
        /// <summary>
        /// Randomly generate name and get a list of google search results for name+linkedin
        /// then iterate through all of those values and retrieve data.
        /// </summary>
        /// <returns></returns>
        public static IList<string> GenerateRandomName()
        {
            var personGenerator = new PersonNameGenerator();
            var name = personGenerator.GenerateRandomFirstAndLastName();
            var nameSurname = name.Split(' ');
            return nameSurname;
        }

        public static string FormGoogleSearchUrl(IList<string> keyWords)
        {
            //https://www.google.co.za/search?q=James+Snow+LinkedIN&safe=active
            var baseUrl = "https://www.google.co.za/search?q=";
            var q = "";
            var count = keyWords.Count;
            var cnt = 0;
            foreach (var keyword in keyWords)
            {
                q += cnt == count - 1 ? keyword : keyword + "+";
            }
            return baseUrl + q + "&safe=active";
        }

        public static IList<string> GetGoogleResultUrlsContainingKeyword(string url, string keyword)
        {
            var urlList = new List<string>();
            var html = GetHTMLFromUrl(url);
            var split = html.Split('"');
            foreach(var str in split)
            {
                if(str.Contains("https://") && str.Substring(0,5) =="https" && str.Contains(keyword))
                {
                    urlList.Add(str);
                }
            }
            return urlList;
        }
    }
}
