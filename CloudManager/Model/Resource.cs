using CloudManager.Common.Interfaces;
using CloudManager.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudManager.Modeld
{
    public class Resource : BaseEntity, IPath
    {
        public Infrastructure Infrastructure { get; set; }
        public List<Attribute> Attributes { get; set; }

        public string Path { get { return Infrastructure.Provider.Name + "/" + Infrastructure.Name + "/" + Name; } }

        public static string GetName()
        {
            return "Resource";
        }
    }
}
