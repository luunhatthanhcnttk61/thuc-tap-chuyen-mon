using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aspnet_mvc_real_estate.Models
{
    public class NewsViewModels
    {
        public news news { get; set; }
        public news_comment getCommentModel { get; set; }
        public List<news_comment> news_comment { get; set; }
        public NewsViewModels (news news, List<news_comment> news_Comments)
        {
            this.news = news;
            this.news_comment = news_Comments;
        }
    }
    public class NewsComment
    {
        public string username { get; set; }
        public string email { get; set; }
        public string message { get; set; }
    }
}