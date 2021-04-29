using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractionFramework
{
    class Entity
    {

        public string ID { get; private set; }
        private List<InteractionNode> m_Nodes;

        public Entity()
        {
            ID = "unnamed";
        }
    }
}
