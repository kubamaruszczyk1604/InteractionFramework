using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractionFramework
{
   public class Attribute
    {
        public string Name { get; private set; }
        public string Value { get; private set; }
        public object OptionalData { get; private set; }

        public Attribute(string name, string value)
        {
            Name = name;
            Value = value;
            OptionalData = null;
        }

        public Attribute(string name, string value, object optionalData)
        {
            Name = name;
            Value = value;
            OptionalData = optionalData;
        }
    }
}
