using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractionFramework
{
    class InteractionManager
    {

        public static readonly InteractionManager Instance = new InteractionManager();
        private List<InteractionNode> m_AvailableNodes;

        static InteractionManager()
        {

        }

        private InteractionManager()
        {
            m_AvailableNodes = new List<InteractionNode>();
        }

        public void RegisterInteracionNode(InteractionNode node)
        {
            if (m_AvailableNodes.Contains(node)) throw new Exception("Interaction node: " + node.ID + " already registered with Interaction Manager!");
            m_AvailableNodes.Add(node);
            node.OnRegister();
        }


        public void Engage(InteractionNode node, Entity parent)
        {
            //TODO: engage node
        }



    }
}
