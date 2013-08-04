using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayCalc
{
    public class LocalizedStrings
    {
        public LocalizedStrings()
        {}

        private static PayCalc.Resources.AppResources stringsResource = new PayCalc.Resources.AppResources();

        public PayCalc.Resources.AppResources StringsResource { get { return stringsResource; } }
    }

    
}
