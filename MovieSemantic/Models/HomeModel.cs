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
    public enum Criteria
    {
        Title = 0,
        Actor = 1
    }

    public class MovieSearchFormat
    {
        public int MovieId { get; set; }
        public string MovieName { get; set; }
    }

    public class ActorSearchFormat
    {
        public int ActorId { get; set; }
        public string ActorName { get; set; }
    }

    public class SearchModel
    {
        public string Term { get; set; }
        public Criteria Criteria { get; set; }
        public int BrojTrojki { get; set; }
        public int BrojGlumaca { get; set; }
        public int BrojDirektora { get; set; }
        public int BrojScenarista { get; set; }

    }

    public class SearchResultsModel
    {
        public string Term { get; set; }
        public Criteria Criteria { get; set; }
        public List<MovieSearchFormat> MoviesList { get; set; }
        public List<ActorSearchFormat> ActorsList { get; set; }

    }
}