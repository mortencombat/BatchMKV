using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace MKVTools
{
    public static class Common
    {
        public static string SanitizeFilename(string Filename)
        {
            return (string.IsNullOrWhiteSpace(Filename)
                ? null
                : Regex.Replace(Filename.Trim(), "[\\/:*?\"<>|]", ""));
        }

    }
}
