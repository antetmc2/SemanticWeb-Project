using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MovieSemantic.Models
{
    public class Language
    {
        public string Abb { get; set; }
        public string LangName { get; set; }
    }

    public class ActorDetailsModel
    {
        public int ActorId { get; set; }
        public string ActorName { get; set; }
        public string BirthDate { get; set; }
        public string DeathDate { get; set; }
        public string BirthPlace { get; set; }
        public string Biography { get; set; }
        public string ShortDescription { get; set; }
        public string Pic { get; set; }
        public SelectList Language { get; set; }
        public List<string> Movies { get; set; }
    }
}