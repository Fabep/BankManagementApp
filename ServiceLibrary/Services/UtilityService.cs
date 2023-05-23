using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary.Services
{
    public class UtilityService : IUtilityService
    {
        private TextInfo TextInfo => CultureInfo.CurrentCulture.TextInfo;
        public string ToTitleCase(string text)
        {
            return TextInfo.ToTitleCase(text);
        }
    }
}
