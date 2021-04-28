using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractionFramework
{

    public class InteractionNode
    {
        public bool Available { get; protected set; }

        private Dictionary<string, Attribute> m_Attributes;
        private List<InputNode> m_InputNodes;
    

        protected InteractionNode()
        {
            m_Attributes = new Dictionary<string, Attribute>();
            m_InputNodes = new List<InputNode>();
        }

        protected bool AddInputNode(InputNode node)
        {
            if(node.IsInUse)
            {
                throw new Exception("Node already in use");
            }

            if (m_InputNodes.Contains(node))
            {
                return false;
            }
            else
            {
                node.OnEngaged();
                m_InputNodes.Add(node);

            }
            return true;
        }

        protected bool TryFindMatchingNode<T>(Attribute[] attribs, T interfaceRef, out InputNode node)
        {
            node = null;
            InputNode [] nodes = InputSystem.Instance.FindNodesMatchingCriteria(new Type[] { Utils.GetTypeFromRef(interfaceRef) }, attribs);
            if(nodes.Length > 0)
            {
                node = nodes[0];
                  
                return true;
            }
            return false;
        }

        protected bool TryFindMatchingNode(Attribute[] attribs, Type[] interfaces, out InputNode node)
        {
            node = null;
            InputNode[] nodes = InputSystem.Instance.FindNodesMatchingCriteria(interfaces, attribs);
            if (nodes.Length > 0)
            {
                node = nodes[0];

                return true;
            }
            return false;
        }

        public string[] GetInterfaceList()
        {
            List<string> interfaces = new List<string>();
            Type t = this.GetType();
            foreach (Type tinterface in t.GetInterfaces())
            {
                interfaces.Add(tinterface.ToString());
            }
            return interfaces.ToArray();
        }

        public Type[] GetInterfaceListAsTypes()
        {
            List<Type> interfaces = new List<Type>();
            Type t = this.GetType();
            foreach (Type tinterface in t.GetInterfaces())
            {
                interfaces.Add(tinterface);
            }
            return interfaces.ToArray();
        }

        public virtual void OnStart()
        {
            for(int i = 0; i < m_InputNodes.Count; ++i)
            {
                m_InputNodes[i].OnStart();
            }
        }
        public virtual void OnUpdate()
        {
            for (int i = 0; i < m_InputNodes.Count; ++i)
            {
                m_InputNodes[i].Update();
            }
        }
        public virtual void OnStop()
        {
            for (int i = 0; i < m_InputNodes.Count; ++i)
            {
                m_InputNodes[i].OnStop();
            }
        }
    }


}
