using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractionFramework
{

    public class InteractionNode
    {
        public bool Available { get; private set; }
        public string ID { get; protected set; }

        private Dictionary<string, Attribute> m_Attributes;
        private List<InputNode> m_InputNodes;

        private bool m_AvailabilitySet;
    

        protected InteractionNode()
        {
            m_AvailabilitySet = false;
            m_Attributes = new Dictionary<string, Attribute>();
            m_InputNodes = new List<InputNode>();
            ID = "unnamed";
            InteractionManager.Instance.RegisterInteracionNode(this);
        }

        protected void SetAvailable(bool available)
        {
            Available = available;
            m_AvailabilitySet = true;
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
                node.AssignTo(this);
                m_InputNodes.Add(node);

            }
            return true;
        }

        protected bool TryFindMatchingInputNode<T>(Attribute[] attribs, T interfaceRef, out InputNode node)
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

        protected bool TryFindMatchingInputNode(Attribute[] attribs, Type[] interfaces, out InputNode node)
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

        virtual public void OnRegister() { }

        protected virtual void OnStart()
        {

        }
        protected virtual void OnUpdate()
        {

        }
        protected virtual void OnStop()
        {

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

        public void Register()
        {
            this.OnRegister();
        }

        public void Start()
        {
            if (!m_AvailabilitySet) throw new Exception("Interaction Node: " + this.ID + " - Attempting to start an interaction with unspecified availability!");
            if (!Available) throw new Exception("Interaction Node: " + this.ID + " - Attempting to start unavailable node!");
            for (int i = 0; i < m_InputNodes.Count; ++i)
            {
               // m_InputNodes[i].Engage(this);
                m_InputNodes[i].Start();
            }
            this.OnStart();
        }

        public void Update()
        {
            if (!m_AvailabilitySet || !Available) return;
            for (int i = 0; i < m_InputNodes.Count; ++i)
            {
                m_InputNodes[i].Update();
            }
            this.OnUpdate();

        }

        public void Stop()
        {
            if (!m_AvailabilitySet || !Available) return;
            for (int i = 0; i < m_InputNodes.Count; ++i)
            {
                m_InputNodes[i].Stop();
            }
            this.Stop();
        }
    }


}
