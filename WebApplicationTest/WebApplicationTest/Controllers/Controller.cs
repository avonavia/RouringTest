using ExampleService.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ExampleService.Controllers
{
    public class Controller : ControllerBase
    {
        List<ColoredAlienCat> catList = new List<ColoredAlienCat>();

        [HttpPost]
        public string NoCats()
        {
            string sadMessage = "No cats here(";
            return sadMessage;
        }

        [HttpGet, Route("cats")]
        public IEnumerable<ColoredAlienCat> FormCatList()
        {
            MakeCats();
            return catList;
        }

        [HttpGet, Route("cats/{id}")]
        public IEnumerable<ColoredAlienCat> GetCatByID(int id)
        {
            MakeCats();
            List<ColoredAlienCat> list = new List<ColoredAlienCat>();
            foreach (var item in catList)
            {
                if (item.Id == id)
                {
                    list.Add(item);
                }
            }
            return list;
        }

        public void MakeCats()
        {
            for (var i = 0; i < 10; i++)
            {
                ColoredAlienCat cat = new ColoredAlienCat(i + 1);
                catList.Add(cat);
            }
        }
    }
}