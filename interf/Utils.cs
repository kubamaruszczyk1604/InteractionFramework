using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractionFramework
{
   public class Utils
   {
        private static Type GetTypeFromReference<T>(out T destination)
        {
            //Console.WriteLine("typeof(T)=" + typeof(T).Name);
            destination = default(T);
            return typeof(T);
        }

        public static Type GetTypeFromRef<T>(T findType)
        {
           return GetTypeFromReference(out findType);
        }


    }
}
