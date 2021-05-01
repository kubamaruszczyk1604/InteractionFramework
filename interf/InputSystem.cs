using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractionFramework
{
    public sealed class InputSystem
    {
        public static readonly InputSystem Instance = new InputSystem();
        private List<InputNode> m_Nodes;
        static InputSystem()
        {

        }

        private InputSystem()
        {
            Console.WriteLine("Constructor InputSystem");
            m_Nodes = new List<InputNode>();
        }

        public void RegisterNode(InputNode node)
        {
            if (m_Nodes.Contains(node)) throw new Exception("Node " + node.ID + " is already registered");
            m_Nodes.Add(node);
            //node.Register();
        }



        /// <summary>
        /// Checks if the provided node implements all interfaces from the provided list
        /// </summary>
        /// <param name="node">Node to check</param>
        /// <param name="interfaces">list of interfaces</param>
        /// <returns>true: if node implements all interfaces from the list;
        /// false: if node does not inplement all interfaces from the list</returns>
        //public static bool ImplementsInterfaces(InputNode node, string [] interfaces)
        //{
        //    return node.ImplementsInterfaces(interfaces);
        //}

        //public static bool IsMathingCriteria(InputNode node, string[] implementedInterfaces, Attribute[] attributes)
        //{
        //    return node.MatchesCriteria(implementedInterfaces, attributes);
        //}

        //public static string[] FindImplementedInterfaces(InputNode node, string[] interfaces)
        //{
        //    return node.FindImplementedInterfaces(interfaces);
        //}

        /// <summary>
        /// Gets array of input nodes that implement all specified interfaces
        /// </summary>
        /// <param name="interfaces">array of interfaces in string format</param>
        /// <returns>Array of interfaces implementing specified set of interfaces</returns>
        public InputNode[] GetInputNodes(string[] interfaces)
        {
            List<InputNode> result = new List<InputNode>();
            for (int i = 0; i < m_Nodes.Count; ++i)
            {
                InputNode temp = m_Nodes[i];
                if(temp.ImplementsInterfaces(interfaces))
                {
                    result.Add(temp);
                }
            }
            return result.ToArray();
        }

        public InputNode[] FindNodesMatchingCriteria(string [] implementedInterfaces, Attribute[] attributes)
        {
            List<InputNode> nodes = new List<InputNode>();
            for(int i =0; i < m_Nodes.Count; ++i)
            {
                if(m_Nodes[i].MatchesCriteria(implementedInterfaces,attributes))
                {
                    nodes.Add(m_Nodes[i]);
                }
            }
            return nodes.ToArray();
        }

        public InputNode[] FindNodesMatchingCriteria(Type[] implementedInterfaces, Attribute[] attributes)
        {
            List<InputNode> nodes = new List<InputNode>();
            for (int i = 0; i < m_Nodes.Count; ++i)
            {
                if (m_Nodes[i].MatchesCriteria(implementedInterfaces, attributes))
                {
                    nodes.Add(m_Nodes[i]);
                }
            }
            return nodes.ToArray();
        }



    }
}
