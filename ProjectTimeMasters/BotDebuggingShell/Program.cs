﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeMastersClassLibrary.Logging;

namespace BotDebuggingShell
{
    class Program
    {
        static void Main(string[] args)
        {
            LoggerFactory.GetFileLogger().Error<Program>("Das ist ein Test Log");
        }
    }
}