using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepeatingWords
{
   public class ErrorHandler
    {
        public static void getError(Exception er, string ClassAndMethod)
        {
            Debug.WriteLine("\n\n--------------------------------------------------------------------------------------" +
                "\n--------------CUSTOM ERROR: "+ClassAndMethod+"---------\n" + er.Message+"\n---------STACKTRACE----------\n"+er.StackTrace);
            
        }
    }
}
