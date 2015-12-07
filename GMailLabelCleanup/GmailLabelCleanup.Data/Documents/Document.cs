using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MongoDB.Bson;
using System.Configuration;

namespace GMailLabelCleanup.Data.Documents
{
    public abstract class Document
    {

        public ObjectId Id { get; set; }        

        public string UserId { get; set; }
        public DateTime DateCreatedUtc { get; set; }
        public string EnvironmentMode { get; set; }

        protected Document()
        {
            this.EnvironmentMode = ConfigurationManager.AppSettings["EnvironmentMode"];
        }

    }
}