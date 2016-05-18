using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Data;

namespace MovieSemantic.Models
{
    public class MovieDetailsModel
    {
        public string MovieTitle { get; set; }
        public string MovieDate { get; set; }
        public string MovieCountry { get; set; }
        public List<string> Directors { get; set; }
        public List<string> Writers { get; set; }
        public List<string> Actors { get; set; }

    }
}