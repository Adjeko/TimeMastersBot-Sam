using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TimeMasters.WebCore.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TimeMasters.WebCore.Controllers
{
    [Route("api/[controller]")]
    public class LogController : Controller
    {
        public ILogMessageRepository LogMessages { get; set; }

        public LogController(ILogMessageRepository logMessages)
        {
            LogMessages = logMessages;
        }

        public IEnumerable<LogMessage> GetAll()
        {
            return LogMessages.GetAll();
        }

        [HttpGet("{id}", Name = "GetLogMessage")]
        public IActionResult GetByID(string id)
        {
            var log = LogMessages.Find(id);
            if(log == null)
            {
                return NotFound();
            }
            return new ObjectResult(log);
        }

        [HttpPost]
        public IActionResult Create([FromBody] LogMessage logMessage)
        {
            if(logMessage == null)
            {
                return BadRequest();
            }
            LogMessages.Add(logMessage);
            return CreatedAtRoute("GetLogMessage", new { id = logMessage.Key}, logMessage);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody] LogMessage logMessage)
        {
            if(logMessage == null || logMessage.Key != id)
            {
                return BadRequest();
            }

            var log = LogMessages.Find(id);
            if(log == null)
            {
                return NotFound();
            }

            LogMessages.Update(logMessage);
            return new NoContentResult();
        }
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            LogMessages.Remove(id);
        }
    }
}
