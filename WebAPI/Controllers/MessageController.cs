using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace WebAPI.Controllers
{
    [RoutePrefix("api/default")]
    public class MessageController : ApiController
    {
        private static readonly IDictionary<Guid, string> mMessages = new ConcurrentDictionary<Guid, string>();

        [ResponseType(typeof(string))]
        [Route("{id}", Name="GetMessage")]
        public async Task<IHttpActionResult> GetMessageAsync(Guid id)
        {
            string retrievedMessage;
            
            if (!mMessages.TryGetValue(id, out retrievedMessage))
            {
                return NotFound();
            }

            return Ok(retrievedMessage);
        }

        [ResponseType(typeof(IDictionary<Guid, string>))]
        [Route("", Name = "GetMessages")]
        public async Task<IHttpActionResult> GetMessages()
        {
            return Ok(mMessages);
        }


        [ResponseType(typeof(string))]
        [Route("{id}/{message}", Name = "AddMessage")]
        [HttpPost]
        public async Task<IHttpActionResult> AddMessageAsync(Guid id, string message)
        {
            if (Guid.Empty.Equals(id))
            {
                return BadRequest("Invalid id.");
            }

            if (mMessages.ContainsKey(id))
            {
                return Conflict();
            }
            
            mMessages.Add(id, message);

            return CreatedAtRoute("GetMessage", new {id = id}, message);
        }

        [Route("{id}")]
        public IHttpActionResult DeleteCustomer(Guid id)
        {
            if (!mMessages.Remove(id))
            {
                return NotFound();
            }
            
            return StatusCode(HttpStatusCode.NoContent);
        }


    }
}
