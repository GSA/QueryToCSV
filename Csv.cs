using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueryToCSV
{
    class Csv
    {
        private const string QUOTE = "\"";
        private const string ESCAPED_QUOTE = "\"\"";
        //private const char[] CHARACTERS_THAT_MUST_BE_QUOTED;

        public static string Escape(string s)
        {
            char[] CHARACTERS_THAT_MUST_BE_QUOTED = { ',', '"' };

            if (s.Contains(QUOTE))
                s = s.Replace(QUOTE, ESCAPED_QUOTE);

            if (s.IndexOfAny(CHARACTERS_THAT_MUST_BE_QUOTED) > - 1)
                s = QUOTE + s + QUOTE;

            return s;
        }
    }
}
