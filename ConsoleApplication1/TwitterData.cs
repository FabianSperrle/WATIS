using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class TwitterData
    {

        public TwitterData() { }

        public TwitterData(String TWEET_ID, String TWEET_CREATIONDATE, String TWEET_CONTENT, String TWEET_RETWEETED, String TWEET_RETWEETCOUNT, String TWEET_COORDS, String TWEET_SOURCE, String TWEET_URLS, String MEDIA_URLS, String TWEET_PLACE, String TWEET_REPLYTOSTATUS, String TWEET_REPLYTOUSERID, String TWEET_REPLYTOUSERNAME, String USER_ID, String USER_NAME, String USER_SCREENNAME, String USER_CREATIONDATE, String USER_LANGUAGE, String USER_STATUSESCOUNT, String USER_FOLLOWERSCOUNT, String USER_LOCATION, String USER_DESCRIPTION, String USER_FRIENDSCOUNT, String USER_TIMEZONE, String USER_LISTEDCOUNT, String USER_UTCOFFSET)
        {
            this.TWEET_ID = TWEET_ID;
            this.TWEET_CREATIONDATE = TWEET_CREATIONDATE;
            this.TWEET_CONTENT = TWEET_CONTENT;
            this.TWEET_RETWEETED = TWEET_RETWEETED;
            this.TWEET_RETWEETCOUNT = TWEET_RETWEETCOUNT;
            this.TWEET_COORDS = TWEET_COORDS;
            this.TWEET_SOURCE = TWEET_SOURCE;
            this.TWEET_URLS = TWEET_URLS;
            this.MEDIA_URLS = MEDIA_URLS;
            this.TWEET_PLACE = TWEET_PLACE;
            this.TWEET_REPLYTOSTATUS = TWEET_REPLYTOSTATUS;
            this.TWEET_REPLYTOUSERID = TWEET_REPLYTOUSERID;
            this.TWEET_REPLYTOUSERNAME = TWEET_REPLYTOUSERNAME;
            this.USER_ID = USER_ID;
            this.USER_NAME = USER_NAME;
            this.USER_SCREENNAME = USER_SCREENNAME;
            this.USER_CREATIONDATE = USER_CREATIONDATE;
            this.USER_LANGUAGE = USER_LANGUAGE;
            this.USER_STATUSESCOUNT = USER_STATUSESCOUNT;
            this.USER_FOLLOWERSCOUNT = USER_FOLLOWERSCOUNT;
            this.USER_LOCATION = USER_LOCATION;
            this.USER_DESCRIPTION = USER_DESCRIPTION;
            this.USER_FRIENDSCOUNT = USER_FRIENDSCOUNT;
            this.USER_TIMEZONE = USER_TIMEZONE;
            this.USER_LISTEDCOUNT = USER_LISTEDCOUNT;
            this.USER_UTCOFFSET = USER_UTCOFFSET;
        }

        public string TWEET_ID { get; set; }
        public string TWEET_CREATIONDATE { get; set; }
        public string TWEET_CONTENT { get; set; }
        public string TWEET_RETWEETED { get; set; }
        public string TWEET_RETWEETCOUNT { get; set; }
        public string TWEET_COORDS { get; set; }
        public string TWEET_SOURCE { get; set; }
        public string TWEET_URLS { get; set; }
        public string MEDIA_URLS { get; set; }
        public string TWEET_PLACE { get; set; }
        public string TWEET_REPLYTOSTATUS { get; set; }
        public string TWEET_REPLYTOUSERID { get; set; }
        public string TWEET_REPLYTOUSERNAME { get; set; }
        public string USER_ID { get; set; }
        public string USER_NAME { get; set; }
        public string USER_SCREENNAME { get; set; }
        public string USER_CREATIONDATE { get; set; }
        public string USER_LANGUAGE { get; set; }
        public string USER_STATUSESCOUNT { get; set; }
        public string USER_FOLLOWERSCOUNT { get; set; }
        public string USER_LOCATION { get; set; }
        public string USER_DESCRIPTION { get; set; }
        public string USER_FRIENDSCOUNT { get; set; }
        public string USER_TIMEZONE { get; set; }
        public string USER_LISTEDCOUNT { get; set; }
        public string USER_UTCOFFSET { get; set; }

        public override string ToString()
        {
            return this.TWEET_CONTENT;
        }
    }
}
