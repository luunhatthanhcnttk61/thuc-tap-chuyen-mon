using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aspnet_mvc_real_estate.Models
{
    public class EmailModels
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public EmailModels()
        {

        }
        public EmailModels (string To, string Subject, string Body)
        {
            this.To = To;
            this.Subject = Subject;
            this.Body = Body;
        }
    }
}