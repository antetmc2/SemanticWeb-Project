using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VDS.RDF.Parsing;
using VDS.RDF.Query;
using VDS.RDF.Storage;
using MovieSemantic.Models;
using VDS.RDF;

namespace MovieSemantic.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            SearchModel model = new SearchModel();
            SparqlResultSet pom;

            SparqlConnector conn = new SparqlConnector(new Uri("http://localhost:3030/ds/sparql"));

            string qry = @"PREFIX movie: <http://data.linkedmdb.org/resource/movie/>
SELECT (COUNT(*) AS ?ukupnoFilmova)
WHERE {
?movie a movie:film
}";

            var broj = conn.Query(qry);

            pom = (SparqlResultSet)broj;
            foreach(SparqlResult sr in pom)
            {
                ILiteralNode trojke = (ILiteralNode)sr.Value("ukupnoFilmova");
                var tr = Convert.ToInt32(trojke.Value);
                model.BrojTrojki = tr;
            }

            qry = @"PREFIX movie: <http://data.linkedmdb.org/resource/movie/>
SELECT (COUNT(*) AS ?ukupnoGlumaca)
WHERE {
?movie a movie:actor
}";

            broj = conn.Query(qry);

            pom = (SparqlResultSet)broj;
            foreach (SparqlResult sr in pom)
            {
                ILiteralNode trojke = (ILiteralNode)sr.Value("ukupnoGlumaca");
                var tr = Convert.ToInt32(trojke.Value);
                model.BrojGlumaca = tr;
            }

            qry = @"PREFIX movie: <http://data.linkedmdb.org/resource/movie/>
SELECT (COUNT(*) AS ?ukupnoDirektora)
WHERE {
?movie a movie:director
}";

            broj = conn.Query(qry);

            pom = (SparqlResultSet)broj;
            foreach (SparqlResult sr in pom)
            {
                ILiteralNode trojke = (ILiteralNode)sr.Value("ukupnoDirektora");
                var tr = Convert.ToInt32(trojke.Value);
                model.BrojDirektora = tr;
            }

            qry = @"PREFIX movie: <http://data.linkedmdb.org/resource/movie/>
SELECT (COUNT(*) AS ?ukupnoScenarista)
WHERE {
?movie a movie:writer
}";

            broj = conn.Query(qry);

            pom = (SparqlResultSet)broj;
            foreach (SparqlResult sr in pom)
            {
                ILiteralNode trojke = (ILiteralNode)sr.Value("ukupnoScenarista");
                var tr = Convert.ToInt32(trojke.Value);
                model.BrojScenarista = tr;
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(SearchModel model)
        {
            return RedirectToAction("Search", new { niz = model.Term, crit = model.Criteria });
        }

        public ActionResult Search(string niz, Criteria crit)
        {
            SearchResultsModel model = new SearchResultsModel();
            model.Term = niz;
            model.Criteria = crit;
            SparqlResultSet pom;
            List<MovieSearchFormat> moviesPom = new List<MovieSearchFormat>();
            List<ActorSearchFormat> actorsPom = new List<ActorSearchFormat>();
            ILiteralNode nodeT; ILiteralNode nodeI;
            MovieSearchFormat mov = new MovieSearchFormat();

            SparqlConnector conn = new SparqlConnector(new Uri("http://localhost:3030/ds/sparql"));

            if(crit == Criteria.Title)
            {
                string qry = String.Format(@"PREFIX movie: <http://data.linkedmdb.org/resource/movie/>
PREFIX dc: <http://purl.org/dc/terms/>
SELECT ?id ?title
WHERE {{
?film dc:title ?title .
?film movie:filmid ?id.
FILTER regex(lcase(str(?title)), ""{0}"")
}}
ORDER BY ASC(?title)", niz.ToLower());

                var upit = conn.Query(qry);

                pom = (SparqlResultSet)upit;

                foreach (SparqlResult sr in pom)
                {
                    nodeT = (ILiteralNode)sr.Value("title");
                    nodeI = (ILiteralNode)sr.Value("id");
                    moviesPom.Add(new MovieSearchFormat() { MovieId = Convert.ToInt32(nodeI.Value), MovieName = nodeT.Value });
                }

                model.MoviesList = moviesPom;
            }

            else if (crit == Criteria.Actor)
            {
                string qry = String.Format(@"PREFIX movie: <http://data.linkedmdb.org/resource/movie/>
PREFIX actor: <http://data.linkedmdb.org/resource/actor/>
PREFIX dc: <http://purl.org/dc/terms/>
SELECT ?id ?name
WHERE {{
?act movie:actor_name ?name .
?act movie:actor_actorid ?id
FILTER regex(lcase(str(?name)), ""{0}"")
}}
ORDER BY ASC(?name)", niz.ToLower());

                var upit = conn.Query(qry);

                pom = (SparqlResultSet)upit;

                foreach (SparqlResult sr in pom)
                {
                    nodeT = (ILiteralNode)sr.Value("name");
                    nodeI = (ILiteralNode)sr.Value("id");
                    actorsPom.Add(new ActorSearchFormat() { ActorId = Convert.ToInt32(nodeI.Value), ActorName = nodeT.Value });
                }

                model.ActorsList = actorsPom;
            }



            return View(model);
        }

    }
}