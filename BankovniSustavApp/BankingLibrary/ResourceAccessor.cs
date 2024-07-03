using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingLibrary
{
    public class ResourceAccessor
    {
        public static string GetStringResource(string key)
        {
            return LibraryResources.ResourceManager.GetString(key);
        }
    }
}
