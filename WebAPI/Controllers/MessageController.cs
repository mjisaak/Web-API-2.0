using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebAPI.Models;
using WebAPI.Repositories;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Message resource controller
    /// </summary>
    [RoutePrefix("api/default")]
    public class MessageController : ApiController
    {
        private readonly IMessageRepository mMessageRepository;

        public MessageController(IMessageRepository aMessageRepository)
        {
            mMessageRepository = aMessageRepository;
        }
        
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
        [Route("{id:guid}", Name="GetMessage")]
        [HttpGet]
        public async Task<IHttpActionResult> GetMessageAsync(Guid id)
        {
            try
            {
                return await Task.FromResult(Ok(mMessageRepository.GetMessageById(id)));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Gets all messages.
        /// </summary>
        /// <returns>A dictionary of id, messages.</returns>
        /// <response code="200">OK</response>
        /// <response code="500">Internal Server Error</response>
        [ResponseType(typeof(IDictionary<Guid, string>))]
        [Route("", Name = "GetMessages")]
        [HttpGet]
        public async Task<IHttpActionResult> GetMessages()
        {
            return Ok(mMessageRepository.GetAll());
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
        [Route("{id:guid}/{message:string}", Name = "AddMessage")]
        [HttpPost]
        public async Task<IHttpActionResult> AddMessageAsync(Guid id, string message)
        {
            if (Guid.Empty.Equals(id))
            {
                return BadRequest("Invalid id.");
            }

            try
            {
                mMessageRepository.InsertMesage(id, message);
                return CreatedAtRoute("GetMessage", new { id = id }, message);
            }
            catch (Exception)
            {
                return Conflict();
            }
        }

        /// <summary>
        /// Adds a message using the specified MessageModel and returns the new resource location.
        /// </summary>
        /// <returns>The route of the added message.</returns>
        /// <response code="201">Created</response>
        /// <response code="400">Bad Request</response>
        /// <response code="409">Conflict</response>
        /// <response code="500">Internal Server Error</response>
        [ResponseType(typeof(string))]
        [Route("", Name = "AddMessageByModel")]
        [HttpPost]
        public async Task<IHttpActionResult> AddMessageAsync([FromBody]MessageModel messageModel)
        {
            try
            {
                var id = messageModel.Id.Equals(Guid.Empty) ? Guid.NewGuid() : messageModel.Id;
                mMessageRepository.InsertMesage(id, messageModel.Message);
                return CreatedAtRoute("GetMessage", new { id = id }, messageModel);
            }
            catch (Exception)
            {
                return Conflict();
            }
        }

        /// <summary>
        /// Deletes the message with the correlated id.
        /// </summary>
        /// <param name="id">The id of the message to delete.</param>
        /// <response code="204">No Content</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [Route("{id:guid}")]
        [HttpDelete]
        public IHttpActionResult DeleteMessage(Guid id)
        {
            if (!mMessageRepository.DeleteMessage(id))
            {
                return NotFound();
            }
            
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
