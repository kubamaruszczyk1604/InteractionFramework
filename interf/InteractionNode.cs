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

    public class TestNodeInteraction: InteractionNode
    {
        I2DPosProvider m_PosProvider = null;
        ICommandProvider m_CommandProvider = null;
        Attribute[] attribList = new Attribute[] { new Attribute("Name", "Mouse"), new Attribute("Type", "2Buttons") };

        public void FindNodes()
        {

            //if(TryFindMatchingNode(attribList, m_DirectionProvider, out InputNode node))
            //{
            //    m_DirectionProvider = (IDirectionProvider)node;
            //    m_DirectionProvider.onDirectionChanged += OnDirectionChanged;
            //}
            //else
            //{
            //    Available = false;
            //}

            if(TryFindMatchingNode(attribList, new Type[] { Utils.GetTypeFromRef(m_PosProvider), Utils.GetTypeFromRef(m_CommandProvider) }, out InputNode node))
            {
                AddInputNode(node);
                m_PosProvider = (I2DPosProvider)node;
                m_CommandProvider = (ICommandProvider)node;

                m_PosProvider.onPositionChanged += OnPositionChanged;
                m_CommandProvider.onCommand += OnCommand;
            }

        }

        void OnPositionChanged(Vector3 delta, Vector3 posNow)
        {
            Console.WriteLine("pos changed: x = " + posNow.X + "  y = " + posNow.Y);
        }

        void OnCommand(CommandData command)
        {
            Console.WriteLine("Command received = " + command.Command);
        }

    }
}
