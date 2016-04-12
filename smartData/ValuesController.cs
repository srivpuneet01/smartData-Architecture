using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace smartData
{
    public class ValuesController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> GetNew()
        {
            return new string[] { "value1a", "value2a" };
        }

        // GET api/<controller>/5
        [HttpGet]
        public string GetNew(int id)
        {
            return "valuea";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}