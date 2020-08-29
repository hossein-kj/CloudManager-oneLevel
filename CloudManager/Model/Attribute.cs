using CloudManager.Common.Interfaces;
using CloudManager.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudManager.Modeld
{
    public class Attribute: BaseEntity, IPath
    {
        public string Value { get; set; }
        public Resource Resource { get; set; }

        public string Path { get { return Resource.Infrastructure.Provider.Name 
                    + "/" + Resource.Infrastructure.Name + "/" + Resource.Infrastructure.Name + "_" + Resource.Name+".json"; } }

        public static string GetName()
        {
            return "Attribute";
        }
    }
}
