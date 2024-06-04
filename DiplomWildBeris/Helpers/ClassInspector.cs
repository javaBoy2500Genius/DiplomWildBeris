using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomWildBeris.Helpers
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Collections.Generic;

    public static class ClassInspector
    {
        public static List<string> GetClassNamesInNamespace(string @namespace)
        {
            var classNames = new List<string>();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();

                var filteredTypes = types.Where(t => t.IsClass && t.Namespace == @namespace);

                foreach (var type in filteredTypes)
                {
                    classNames.Add(type.Name);
                }
            }

            return classNames;
        }



    }

}
