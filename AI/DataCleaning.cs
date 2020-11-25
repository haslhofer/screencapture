using System.Collections.Generic;
using System.IO;
using System.Text;

namespace screencapture
{
    public class NameClean
    {
        private static HashSet<string> firstNames = null;

        public static bool IsHighConfidenceName(string name)
        {
            var subs = name.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
            if (subs.Length > 1 && IsCommonFirstName(subs)) return true;
            return false;

        }

        private static bool ContainsNumber(string s)
        {
            foreach (char aChar in s)
            {
                if (char.IsNumber(aChar)) return true;
                
            }
            return false;
        }

        private static bool IsCommonFirstName(string[] nameParts)
        {
            if (firstNames == null)
            {
                firstNames = new HashSet<string>();
                StreamReader r = new StreamReader(@"C:\Users\gerhas\Documents\GitHub\screencapture\data\firstnames.txt");
                while (!r.EndOfStream)
                {

                    string aName = r.ReadLine().ToLower();
                    if (!firstNames.Contains(aName)) firstNames.Add(aName);
                }
                r.Close();
            }
            
            foreach (var avar1 in nameParts)
            {
                if (ContainsNumber(avar1)) return false;
            }

            foreach (var avar in nameParts)
            {
                if (ContainsNumber(avar)) return false;
                if (firstNames.Contains(avar.ToLower()))
                {
                    return true;
                }
            }
            return false;

        }
    }
}

