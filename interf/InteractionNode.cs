using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractionFramework
{

    public abstract class InteractionNode
    {
       
        public string ID { get; private set; }
        private InteractionNodeState m_State;

        private Dictionary<string, Attribute> m_Attributes;
        private List<InputNode> m_InputNodes;
    

        protected InteractionNode()
        {
            m_Attributes = new Dictionary<string, Attribute>();
            m_InputNodes = new List<InputNode>();
            ID = "unnamed";
            m_State = InteractionNodeState.NoInputMatchingCriteria;
            InteractionManager.Instance.RegisterInteracionNode(this);
        }

        private bool Search()
        {
            m_InputNodes.Clear();
            bool found = FindInputNodes();
            return found;
        }

        public InteractionNodeState QueryState()
        {
            if (m_State == InteractionNodeState.InUse) return m_State;
            if(!Search())
            {
                if(m_State != InteractionNodeState.NoInputMatchingCriteria)
                {
                    m_State = InteractionNodeState.NoInputMatchingCriteria;
                    OnBecomeUnavailable(m_State);
                }

                return m_State;
            }

            for (int i = 0; i < m_InputNodes.Count; ++i)
            {
                if (m_InputNodes[i].State == InputNodeState.InUse)
                {
                    if(m_State != InteractionNodeState.UnderlyingInputInUse)
                    {
                        m_State = InteractionNodeState.UnderlyingInputInUse;
                        OnBecomeUnavailable(m_State);
                    }

                    return m_State;
                }
            }
            if(m_State != InteractionNodeState.Free)
            {
                m_State = InteractionNodeState.Free;
                OnBecomeReady();
            }

            return m_State;
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
                node.AddNewParrent(this);
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
                //if the node is in use search for a possible  better option
                if (node.IsInUse)
                {
                    for (int i = 0; i < nodes.Length; ++i)
                    {
                        if (!nodes[i].IsInUse)
                        {
                            node = nodes[i];
                            break;
                        }
                    }
                }

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
                //if the node is in use search for a possible  better option
                if(node.IsInUse)
                {
                    for(int i = 0; i < nodes.Length ;++i)
                    {
                        if(!nodes[i].IsInUse)
                        {
                            node = nodes[i];
                            break;
                        }
                    }
                }

                return true;
            }
            return false;
        }

        abstract protected bool FindInputNodes();

        virtual protected void OnBecomeReady() { }
        virtual protected void OnBecomeUnavailable(InteractionNodeState state) { }
        virtual protected void OnStart() { }
        virtual protected void OnUpdate() { }
        virtual protected void OnStop() { }
        virtual public void OnRegister() { }

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
            //TODO move responsibility of checking stuff from start and ask users to run them explicitly
            QueryState();

            if (m_State == InteractionNodeState.Free)
            {
                m_State = InteractionNodeState.InUse;
                this.OnStart();
                for (int i = 0; i < m_InputNodes.Count; ++i)
                {
                    m_InputNodes[i].Start(this);
                }

            }
            
        }

        public void Update()
        {
            if (m_State == InteractionNodeState.InUse)
            {
                for (int i = 0; i < m_InputNodes.Count; ++i)
                {
                    m_InputNodes[i].Update(this);
                }
                this.OnUpdate();
            }
            

        }

        public void Stop()
        {
            if(m_State == InteractionNodeState.InUse)
            {
                for (int i = 0; i < m_InputNodes.Count; ++i)
                {
                    m_InputNodes[i].Stop(this);
                }
                this.Stop();
            }
           
        }
    }


}
