using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq;
using System.Collections;

namespace BOTAMVC3
{
    public static class ConvertBOTA
    {
        public static EntitySet<T> ToEntitySet<T>(this IEnumerable<T> source) where T : class
        {
            var es = new EntitySet<T>();
            es.AddRange(source);
            return es;
        }

        // note that the template is not used, and we never need to pass one in...
        public static IEnumerable<T> Cast<T>(this IEnumerable source, Func<T> template)
        {
            return Enumerable.Cast<T>(source);
        }

    }
}
