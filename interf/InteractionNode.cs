using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractionFramework
{

    public abstract class InteractionNode
    {
       
        public bool InputSatisfied { get; private set; }
        public string ID { get; private set; }
        public InteractionNodeState State { get; private set; }

        private Dictionary<string, Attribute> m_Attributes;
        private List<InputNode> m_InputNodes;
    

        protected InteractionNode()
        {
            m_Attributes = new Dictionary<string, Attribute>();
            m_InputNodes = new List<InputNode>();
            ID = "unnamed";
           
            InteractionManager.Instance.RegisterInteracionNode(this);
        }

        public bool Search()
        {
            m_InputNodes.Clear();
            bool found = DoSearch();
            if(found)
            {
                if(!InputSatisfied) OnSearchSucess();
                InputSatisfied = true;
                return true;
            }
            else
            {
                if (InputSatisfied) OnSearchFailed();
                InputSatisfied = false;
                return false;
            }
        }

        InteractionNodeState CheckBusy()
        {
            if (State == InteractionNodeState.InUse) return State;
            

            for (int i = 0; i < m_InputNodes.Count; ++i)
            {
                if (m_InputNodes[i].State == InputNodeState.InUse)
                {
                    State = InteractionNodeState.UnderlyingInputInUse; 
                    return InteractionNodeState.UnderlyingInputInUse;
                }
            }
            State = InteractionNodeState.Free;
            return InteractionNodeState.Free;
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

        abstract protected bool DoSearch();

        virtual protected void OnSearchSucess() { }
        virtual protected void OnSearchFailed() { }
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

            if (CheckBusy() != InteractionNodeState.Free) throw new Exception("Node already in use!");

            InputSatisfied = this.DoSearch();
            if (InputSatisfied) this.OnSearchSucess();
            else this.OnSearchFailed();


           

            if (InputSatisfied)
            {
                State = InteractionNodeState.InUse;
                this.OnStart();
                for (int i = 0; i < m_InputNodes.Count; ++i)
                {
                    m_InputNodes[i].Start(this);
                }

            }
            
        }

        public void Update()
        {
            if (InputSatisfied)
            {
                for (int i = 0; i < m_InputNodes.Count; ++i)
                {
                    m_InputNodes[i].Update(this);
                }
            }
            this.OnUpdate();

        }

        public void Stop()
        {
            if(InputSatisfied)
            {
                for (int i = 0; i < m_InputNodes.Count; ++i)
                {
                    m_InputNodes[i].Stop(this);
                }

            }
            this.Stop();
        }
    }


}
