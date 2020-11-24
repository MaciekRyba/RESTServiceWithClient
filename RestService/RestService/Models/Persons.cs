using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestService.Models
{
    public class Persons
    {
        public int id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string city { get; set; }
        public int year { get; set; }
        public Pets pet { get; set; }
        public Cars car { get; set; }


    }
}