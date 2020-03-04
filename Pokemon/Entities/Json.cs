using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon.Entities
{
    class Json
    {
        public static string GetPath(string fileName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"C:\FitBank\Pokemon\")
                .Append(fileName);

            return sb.ToString();
        }

    }
}
