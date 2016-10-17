using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orcus.Log
{
    public class TestConfigReader : IConfigReader
    {
        private string _LogSource;
        private string _LogFormatter;
        public TestConfigReader(string logSource, string logFormatter)
        {
            this._LogSource = logSource;
            this._LogFormatter = logFormatter;
        }

        public string ReadKey(string name)
        {
            if (string.Equals(name, "LogSource"))
            {
                return _LogSource;
            }
            if (string.Equals(name, "LogFormatter"))
            {
                return _LogFormatter;
            }

            return string.Empty;
        }
    }
}
