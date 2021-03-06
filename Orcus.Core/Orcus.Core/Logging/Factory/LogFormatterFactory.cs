﻿using Orcus.Core.ConfigReader.Interface;
using Orcus.Core.Logging.Interface;
using System;
using System.Reflection;

namespace Orcus.Core.Logging.Factory
{
    public class LogFormatterFactory
    {
        private static ILogFormatter logFormatter;
        private static IConfigReader reader;

        public static ILogFormatter Create(IConfigReader configReader)
        {
            reader = configReader;
            string logSource = reader.ReadKey("LogFormatter");
            string[] arr = logSource.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            Assembly assembly = Assembly.Load(arr[0]);
            object obj = assembly.CreateInstance(arr[1]);
            logFormatter = (ILogFormatter)obj;
            return logFormatter;
        }
    }
}