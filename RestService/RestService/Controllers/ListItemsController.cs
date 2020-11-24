using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RestService.Models;

namespace RestService.Controllers
{
    public class ListItemsController : ApiController
    {
        private static List<Persons> lstPersons { get; set; } = new List<Persons>();
        // GET api/<controller>
        public IEnumerable<Persons> Get()
        {
            return lstPersons;
        }

        // GET api/<controller>/5
        public HttpResponseMessage Get(int id)
        {
            var item = lstPersons.FirstOrDefault(x => x.id == id);
            if (item != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, item);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        // POST api/<controller>
        public HttpResponseMessage Post([FromBody] Persons model)
        {
            if (string.IsNullOrEmpty(model?.name) || string.IsNullOrEmpty(model?.surname))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            var maxId = 0;

            if (lstPersons.Count > 0)
            {
                maxId = lstPersons.Max(x => x.id);
            }
            model.id = maxId + 1;
            lstPersons.Add(model);
            return Request.CreateResponse(HttpStatusCode.Created, model);
        }

        // PUT api/<controller>/5
        public HttpResponseMessage Put(int id, [FromBody] Persons model)
        {
            if (string.IsNullOrEmpty(model?.name) || string.IsNullOrEmpty(model?.surname))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var item = lstPersons.FirstOrDefault(x => x.id == id);

            if (item != null)
            {
                // Update *all* of the item's properties
                item.name = model.name;
                item.surname = model.surname;
                item.city = model.city;
                item.year = model.year;

                item.pet.animal = model.pet.animal;
                item.pet.name = model.pet.name;
                item.pet.year = model.pet.year;

                item.car.theCarBrand = model.car.theCarBrand;
                item.car.color = model.car.color;
                item.car.year = model.car.year;
                item.car.mileage = model.car.mileage;

                return Request.CreateResponse(HttpStatusCode.OK, item);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        // DELETE api/<controller>/5
        public HttpResponseMessage Delete(int id)
        {
            var item = lstPersons.FirstOrDefault(x => x.id == id);
            if (item != null)
            {
                lstPersons.Remove(item);
                return Request.CreateResponse(HttpStatusCode.OK, item);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }
    }
}