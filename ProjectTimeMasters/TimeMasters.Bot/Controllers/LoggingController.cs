using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;
using TimeMastersClassLibrary.Logging;

namespace TimeMasters.Bot.Controllers
{
    public class LoggingController : ApiController
    {

        //GET: api/Logging
        public List<string> Get()
        {
            List<string> result = new List<string>();
            try
            {
                
                LoggerFactory.GetFileLogger(HostingEnvironment.MapPath("~/TestLogFile.txt")).Error<LoggingController>("Bot Test Log");

                result = LoggerFactory.GetFileLogger("").GetAllLines().ToList<string>();

            }
            catch (Exception ex)
            {
                result.Add("Exception| " + ex.Message);
            }
            return result;
        }
    }
}
