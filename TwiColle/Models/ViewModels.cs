using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwiColle.Models
{
    public class InputData
    {
        public string Address { get; set; }
        public string Source { get; set; }
        public string Date_8601 { get; set; }
        public string Artist { get; set; }
        public string Tweet { get; set; }
        public List<string> Tag { get; set; }

        /// <summary>
        /// 解析Address並分離出Artist與Tweet
        /// </summary>
        public void Analyze()
        {
            string[] key = { "https://twitter.com/", "/status/" };
            string[] str = Address.Split(key, StringSplitOptions.RemoveEmptyEntries);
            Artist = str[0];
            Tweet = str[1];
        }
    }
    public class PhotoData
    {
        public int Id { get; set; }
        public string Artist { get; set; }
        public string Source { get; set; }
        public string Tweet { get; set; }
        public List<string> Tag { get; set; }
    }
    public class PhotoTag
    {
        public int PhotoId { get; set; }
        public string TagName { get; set; }
    }
}