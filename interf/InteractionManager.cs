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
        private List<InteractionManager> m_UsedNodes;
        static InteractionManager()
        {

        }

        private InteractionManager()
        {
            m_AvailableNodes = new List<InteractionNode>();
            m_UsedNodes = new List<InteractionManager>();
        }


    }
}
