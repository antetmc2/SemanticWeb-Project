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
    public class ActorController : Controller
    {
        // GET: Actor
        [HttpGet]
        public ActionResult Details(int id = 0, string lang = "en")
        {
            ActorDetailsModel model = new ActorDetailsModel();
            SparqlConnector conn = new SparqlConnector(new Uri("http://localhost:3030/ds/sparql"));
            SparqlResultSet pom; ILiteralNode node;
            List<Language> languages = new List<Language>() { new Language() { Abb = "en", LangName = "English" },
            new Language() { Abb = "ch", LangName = "Chinese" },
            new Language() { Abb = "fr", LangName = "French" },
            new Language() { Abb = "de", LangName = "German" },
            new Language() { Abb = "hu", LangName = "Hungarian" },
            new Language() { Abb = "it", LangName = "Italian" },
            new Language() { Abb = "ja", LangName = "Japanese" }, 
            new Language() { Abb = "ko", LangName = "Korean" },
            new Language() { Abb = "pt", LangName = "Portugese" },
            new Language() { Abb = "ru", LangName = "Russian" },
            new Language() { Abb = "es", LangName = "Spanish" },
            new Language() { Abb = "tr", LangName = "Turkish" },};
            var q = (from l in languages
                     select l).ToList();
            model.Language = new SelectList(q, "Abb", "LangName");

            string qry = String.Format(@"PREFIX movie: <http://data.linkedmdb.org/resource/movie/>
PREFIX actor: <http://data.linkedmdb.org/resource/actor/>
PREFIX owl: <http://www.w3.org/2002/07/owl#>
PREFIX dbo: <http://dbpedia.org/ontology/>
PREFIX dbp: <http://dbpedia.org/property/>
PREFIX dbr: <http://dbpedia.org/resource/>
PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#>
PREFIX foaf: <http://xmlns.com/foaf/0.1/>
PREFIX xsd: <http://www.w3.org/2001/XMLSchema#>
SELECT ?name ?descr ?birthDate ?birthPlace ?bio ?mov ?thumb
WHERE {{
?act a movie:actor .
?act movie:actor_name ?name .
?act movie:actor_actorid {0} .
?act movie:performance ?mp .
?mp  movie:performance_film ?mov .
OPTIONAL {{ ?act owl:sameAs ?url .
FILTER regex(STR(?url), ""dbpedia"") .
SERVICE <http://dbpedia.org/sparql>
{{
?url a dbo:Agent.
OPTIONAL {{ ?url dbp:dateOfBirth ?birthDate }}
OPTIONAL {{ ?url dbp:birthPlace ?birthPlace }}
OPTIONAL {{ ?url dbp:shortDescription ?descr }}
OPTIONAL {{ ?url dbo:abstract ?bio }}
OPTIONAL {{ ?url dbo:thumbnail ?thumb }}
FILTER(lang(?bio) = ""{1}"")
}} }}
OPTIONAL 
{{ 
SERVICE <http://dbpedia.org/sparql>
{{
?url a dbo:Agent.
?url dbp:name ?linkName.
OPTIONAL
{{ ?url dbp:dateOfBirth ?birthDate }}
OPTIONAL {{ ?url dbp:birthPlace ?birthPlace }}
OPTIONAL {{ ?url dbp:shortDescription ?descr }}
OPTIONAL {{ ?url dbo:abstract ?bio }}
OPTIONAL {{ ?url dbo:thumbnail ?thumb }}
FILTER(STR(?linkName) = ?name)
FILTER(lang(?bio) = ""{1}"")
}} }} }}", id, lang);
            try
            {
                model.Movies = new List<string>();
                var results = conn.Query(qry);
                pom = (SparqlResultSet)results;

                foreach (SparqlResult sr in pom)
                {
                    try
                    {
                        node = (ILiteralNode)sr.Value("name");
                        model.ActorName = node.Value;
                    }
                    catch { }

                    try
                    {
                        node = (ILiteralNode)sr.Value("birthDate");
                        model.BirthDate = node.Value;
                    }
                    catch { }

                    try
                    {
                        node = (ILiteralNode)sr.Value("deathDate");
                        model.DeathDate = node.Value;
                    }
                    catch { }

                    try
                    {
                        node = (ILiteralNode)sr.Value("birthPlace");
                        model.BirthPlace = node.Value;
                    }
                    catch { }

                    try
                    {
                        node = (ILiteralNode)sr.Value("descr");
                        model.ShortDescription = node.Value;
                    }
                    catch { }

                    try
                    {
                        node = (ILiteralNode)sr.Value("bio");
                        model.Biography = node.Value;
                    }
                    catch { }

                    try
                    {
                        UriNode Unode = (UriNode)sr.Value("thumb");
                        model.Pic = Unode.Uri.AbsoluteUri;
                    }
                    catch { }

                    try
                    {
                        node = (ILiteralNode)sr.Value("mov");
                        if (!model.Movies.Contains(node.Value)) model.Movies.Add(node.Value);
                    }
                    catch { }

                }
            }
            catch
            {
                qry = String.Format(@"PREFIX movie: <http://data.linkedmdb.org/resource/movie/>
PREFIX actor: <http://data.linkedmdb.org/resource/actor/>
PREFIX owl: <http://www.w3.org/2002/07/owl#>
PREFIX dbo: <http://dbpedia.org/ontology/>
PREFIX dbp: <http://dbpedia.org/property/>
PREFIX dbr: <http://dbpedia.org/resource/>
PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#>
PREFIX foaf: <http://xmlns.com/foaf/0.1/>
PREFIX xsd: <http://www.w3.org/2001/XMLSchema#>
SELECT ?name ?mov
WHERE {{
?act movie:actor_name ?name .
?act movie:actor_actorid {0} .
?act movie:performance ?mp .
?mp  movie:performance_film ?mov
}}", id);

                model.Movies = new List<string>();
                var results = conn.Query(qry);
                pom = (SparqlResultSet)results;

                foreach (SparqlResult sr in pom)
                {
                    try
                    {
                        node = (ILiteralNode)sr.Value("name");
                        model.ActorName = node.Value;
                    }
                    catch { }

                    try
                    {
                        node = (ILiteralNode)sr.Value("mov");
                        if (!model.Movies.Contains(node.Value)) model.Movies.Add(node.Value);
                    }
                    catch { }
                }
            }

            model.ActorId = id;


            return View(model);
        }

        [HttpPost, ActionName("Details")]
        public ActionResult ADetails(int id, string lang)
        {
            return RedirectToAction("Details", new { id = id, lang = lang });
        }
    }
}