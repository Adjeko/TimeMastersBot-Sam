﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeMasters.PortableClassLibrary.DirectLine;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {

            BotConnector connector = new BotConnector();
            connector.httpTest();

        }
    }
}
