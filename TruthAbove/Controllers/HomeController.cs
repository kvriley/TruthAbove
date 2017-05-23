using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using TruthAbove.Models;

namespace TruthAbove.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var sightings = GetSightingsList();

            SearchOptions searchOptions = new Models.SearchOptions();

             var states = sightings
            .GroupBy(stg => stg.State)
                  .Select(g => g.First() );

            searchOptions.States = (from sts in states orderby sts.State
                                    select new SelectListItem() { Text = sts.State });

            var shapes = sightings
           .GroupBy(shp => shp.Shape)
                 .Select(g => g.First());

            searchOptions.Shapes= (from shp in shapes
                                    orderby shp.Shape
                                    select new SelectListItem() { Text = shp.Shape });

            return View(searchOptions);
        }

        [HttpGet]
        public JsonResult FilterSightings(string state, string shape)
        {
            var stgList = GetSightingsList();

            var ftg = (from stg in stgList
                       where 
                        (state == "" || stg.State.Equals(state)) && (shape=="" || stg.Shape.Equals(shape))
                        select stg);

            return Json(ftg, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetSightings()
        {
            var sightings = GetSightingsList();

            return Json(sightings, JsonRequestBehavior.AllowGet);
        }

        private List<UFOSighting> GetSightingsList()
        {
            var sightings = new List<UFOSighting>();

            var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data\\ufo_sightings.csv");
            using (TextReader reader = System.IO.File.OpenText(fileName))
            {
                var csv = new CsvReader(reader);
                sightings = csv.GetRecords<UFOSighting>().ToList();
            }

            // todo: cache this

            return sightings;
        }

        [HttpPost]
        public JsonResult SaveSighting(UFOSighting sighting)
        {
            var sightings = GetSightingsList();

            List<UFOSighting> ufoList = sightings.ToList<UFOSighting>();
            ufoList.Add(sighting);

            var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data\\ufo_sightings.csv");
            WriteCSV<UFOSighting>(ufoList, fileName);

            return Json(new { message="Success"});

        }

        public void WriteCSV<T>(IEnumerable<T> items, string path)
        {
            Type itemType = typeof(T);
            var props = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                .OrderBy(p => p.Name);

            using (var writer = new StreamWriter(path))
            {
                writer.WriteLine(string.Join(",", props.Select(p => p.Name)));

                foreach (var item in items)
                {
                    writer.WriteLine(string.Join(",", props.Select(p => p.GetValue(item, null))));
                }
            }
        }

    }
}