using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace WebScraper.Logic
{
    public class HTML
    {
        public string GetHTMLFromUrl(string urlAddress)
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

        public bool CheckFor(string wordInHTML, string urlAddress)
        {
            var html = GetHTMLFromUrl(urlAddress);
            return html.Contains(wordInHTML) == true ? true : false;
        }
        /// <summary>
        /// For each word not in the dictionary, check to see if the word is preceeded by CEO etc...
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public IList<string> ReturnWordsThatAreNotInEnglishDictionary(string html)
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
    }
}
