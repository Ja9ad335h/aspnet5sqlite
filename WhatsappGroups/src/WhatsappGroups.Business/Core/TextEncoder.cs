using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappGroups.Business.Core
{
    public class TextEncoder
    {
        public static string ToBase64Url(byte[] ary)
        {
            char[] padding = { '=' };
            return System.Convert.ToBase64String(ary)
            .TrimEnd(padding).Replace('+', '-').Replace('/', '_');
        }

        public static string FromBase64Url(string str)
        {
            string returnValue = str.Replace('_', '/').Replace('-', '+');
            switch (str.Length % 4)
            {
                case 2: returnValue += "=="; break;
                case 3: returnValue += "="; break;
            }
            return Encoding.ASCII.GetString(Convert.FromBase64String(returnValue));
        }
    }
}
