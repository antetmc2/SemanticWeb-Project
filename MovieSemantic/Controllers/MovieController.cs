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
    public class MovieController : Controller
    {
        // GET: Movie
        public ActionResult Details(int id = 0)
        {
            MovieDetailsModel model = new MovieDetailsModel();
            SparqlConnector conn = new SparqlConnector(new Uri("http://localhost:3030/ds/sparql"));
            IGraph pom;

            string qry = String.Format(@"PREFIX movie: <http://data.linkedmdb.org/resource/movie/>
PREFIX dc: <http://purl.org/dc/terms/>
PREFIX owl: <http://www.w3.org/2002/07/owl#>
PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX foaf: <http://xmlns.com/foaf/0.1/>
DESCRIBE ?film ?dir ?wr ?act ?cou
WHERE {{
?film a movie:film .
OPTIONAL {{ ?film movie:director ?dir .
?dir movie:director_name ?director }} .
OPTIONAL {{ ?film movie:writer ?wr .
?wr movie:writer_name ?writer }} .
OPTIONAL {{ ?film movie:actor ?act .
?act movie:actor_name ?actor }} .
OPTIONAL {{ ?film  movie:country ?cou .
?cou  movie:country_name ?countryName }} .
?film movie:filmid {0}
}}", id);
            var upit = conn.Query(qry);

            pom = (Graph)upit;
            TripleStore store = new TripleStore();
            store.Add(pom);

            string subjekt = ""; string predikat = "";
            List<string> directors = new List<string>();
            List<string> actors = new List<string>();
            List<string> writers = new List<string>();
            string owl = "";
            foreach (var triple in store.Triples)
            {
                subjekt = triple.Subject.ToString();
                predikat = triple.Predicate.ToString();
                var objekt = triple.Object;

                if (objekt.NodeType.Equals(NodeType.Literal))
                {
                    LiteralNode ln = (LiteralNode)objekt;
                    string val = ln.Value;

                    // Glumci, Direktori, Scenaristi, Država
                    if (predikat.Contains("name"))
                    {
                        if (subjekt.Contains("actor")) actors.Add(val);
                        else if (subjekt.Contains("director")) directors.Add(val);
                        else if (subjekt.Contains("writer")) writers.Add(val);
                        else if (subjekt.Contains("country")) model.MovieCountry = val;
                    }

                    // Naslov, Datum izlaska
                    else if (subjekt.Contains("film"))
                    {
                        if (predikat.Contains("#label")) model.MovieTitle = val;
                        else if (predikat.Contains("date")) model.MovieDate = val;
                    }

                }

                // owl:sameAs
                if (predikat.Contains("sameAs"))
                {
                    UriNode un = (UriNode)objekt;
                    string val = un.ToString();
                    if (val.Contains("dbpedia")) owl = val;
                }
            }
            model.Actors = actors;
            model.Directors = directors;
            model.Writers = writers;

            return View(model);
        }
    }
}