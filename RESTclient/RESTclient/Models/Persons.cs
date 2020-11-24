using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTclient.Models
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

        public override string ToString()
        {
            return this.name + " " + this.surname ;
        }
    }
}
