using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ServeLog.Models;
using ServeLog.Bo;
using System.Net;
using ServeLog.Model;
using Microsoft.AspNetCore.Http;
using System.Net.Mime;

namespace ServeLog.Controllers
{
    /// <summary>
    /// The Logger Controller
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class LogController :  ControllerBase
    {
        private readonly ILogBo bo;

        public LogController(ILogBo bo)
        {
            this.bo = bo;
        }
        
        /// <summary>
        /// Simple method to test if the log is alive
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAlive()
        {
            return Ok("Logger is alive 1");
        }

        /// <summary>
        /// Enter the info in the log.
        /// if error return a http 500 internal server error 
        /// and a HttpError based in the exception information
        /// </summary>
        /// <param name="request">
        /// The information to be logged
        /// </param>
        /// <returns>null is ok</returns>
        /// <exception cref="HttpError">If error return a httpError Class</exception>
        [HttpPost("LogEntryAsync")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ErrorManager), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ErrorManager), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> PostLogEntryAsync([FromBody] LogRequest request)
        {
                await bo.PostLogEntryAsync(request);
                return Ok();
        }
    }
}
