﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TimeMastersClassLibrary.Calendar.Google;
using TimeMastersClassLibrary.Logging;

namespace TimeMasters.Bot.Controllers
{

    public class GoogleRegistration
    {
        public string State { get; set; }
        public string Code { get; set; }
    }

    public class RegisterController : ApiController
    {

        //GET: api/Register
        
        public string Get([FromUri] GoogleRegistration reg)
        {
            //Logger.GetInstance().Info<RegisterController>($"{reg.Code}  ||||   {reg.State}");
            
            GoogleTokkenHandler.UserCodeDictionary.Add(reg.State, reg.Code + "#");
            LoggerFactory.GetFileLogger().Info<RegisterController>($"State: {reg.State} Code:{reg.Code}");

            return $"VIELES NETTES DANKESCHÖN \n Request Content: {Request.Content.ToString()} \n Request {Request.ToString()}";
        }
    }
}
