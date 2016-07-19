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
    /// <summary>
    /// Message resource controller
    /// </summary>
    [RoutePrefix("api/default")]
    public class MessageController : ApiController
    {
        private static readonly IDictionary<Guid, string> mMessages = new ConcurrentDictionary<Guid, string>();

        /// <summary>
        /// Gets the message with the specified id.
        /// </summary>
        /// <param name="id">The id of the message to get.</param>
        /// <returns>The message related to the specified id.</returns>
        /// <remarks>Currently, only one message can be retrieved at once.</remarks>
        /// <response code="200">OK</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
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

        /// <summary>
        /// Gets all messages.
        /// </summary>
        /// <returns>A dictionary of id, messages.</returns>
        /// <response code="200">OK</response>
        /// <response code="500">Internal Server Error</response>
        [ResponseType(typeof(IDictionary<Guid, string>))]
        [Route("", Name = "GetMessages")]
        public async Task<IHttpActionResult> GetMessages()
        {
            return Ok(mMessages);
        }
        
        /// <summary>
        /// Adds a message and returns the new resource location.
        /// </summary>
        /// <param name="id">The id of the message to add.</param>
        /// <param name="message">The message.</param>
        /// <returns>The route of the added message.</returns>
        /// <response code="201">Created</response>
        /// <response code="400">Bad Request</response>
        /// <response code="409">Conflict</response>
        /// <response code="500">Internal Server Error</response>
        [ResponseType(typeof(string))]
        [Route("{id:guid}/{message}", Name = "AddMessage")]
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

        /// <summary>
        /// Deletes the message with the correlated id.
        /// </summary>
        /// <param name="id">The id of the message to delete.</param>
        /// <response code="204">No Content</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [Route("{id}")]
        public IHttpActionResult DeleteMessage(Guid id)
        {
            if (!mMessages.Remove(id))
            {
                return NotFound();
            }
            
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
