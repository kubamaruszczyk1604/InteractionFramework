using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractionFramework
{
    
    public abstract class InputNode
    {
        public string ID { get; protected set; }
        public bool IsInUse { get { return State == InputNodeState.InUse; } }
        public InputNodeState State { get; private set; }
        public List<InteractionNode> Parents { get; private set; }


        private InteractionNode p_CurrentUser;
        private Dictionary<string, Attribute> m_Attributes;
        protected InputNode()
        {
           // Console.WriteLine("Abstract constructor called");
            m_Attributes = new Dictionary<string, Attribute>();
            InputSystem.Instance.RegisterNode(this);
            ID = "unnamed";
            Parents = new List<InteractionNode>();
            State = InputNodeState.Available;
            p_CurrentUser = null;
        }


        public void Register()
        {
            this.OnRegister();
        }

        public void AddNewParrent(InteractionNode parent)
        {
            if(Parents.Contains(parent))
            {
                throw new Exception("Cannod Assign to the same parent twice, ID: " + this.ID );
            }
            if(State == InputNodeState.Unavailable)
            {
                throw new Exception("Attempt to engage unavailable node:, ID " + this.ID);
            }

            Parents.Add(parent);
            this.OnNewParentAdded(parent);
        }

        public void Start(InteractionNode caller)
        {
            if(State == InputNodeState.InUse)
            {
                throw new Exception("Node already engaged");
            }
            p_CurrentUser = caller;
            State = InputNodeState.InUse;
            this.OnStart();
        }

        public void Update(InteractionNode caller)
        {
            if (State != InputNodeState.InUse) return;
            if (p_CurrentUser != caller) return;
            this.OnUpdate();
        }

        public void Stop(InteractionNode caller)
        {
            if (State != InputNodeState.InUse) return;
            if (p_CurrentUser != caller) return;
            State = InputNodeState.Available;
            this.OnStop();
        }



        public string[] GetInterfaceList()
        {
            List<string> interfaces = new List<string>();
            Type t = this.GetType();
            foreach (Type tinterface in t.GetInterfaces())
            {
                interfaces.Add(tinterface.Name);
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

        public bool HasAttribute(string name)
        {
            return m_Attributes.TryGetValue(name, out _);
        }

        public bool HasAttribute(string name, string value)
        {
            if (m_Attributes.TryGetValue(name, out Attribute attrib))
            {
                return (attrib.Value.Trim() == value.Trim());
            }
            else
            {
                return false;
            }
        }

        public bool HasAttributes(Attribute [] attributes)
        {
            for(int i = 0; i < attributes.Length; ++i)
            {
                if (!HasAttribute(attributes[i].Name, attributes[i].Value)) return false;
            }
            return true;
        }

        public string GetAttributeValue(string name)
        {
            if(m_Attributes.TryGetValue(name, out Attribute val))
            {
                return val.Value;
            }
            else
            {
                return "";
            }
        }


        public Attribute GetAttribute(string name)
        {
            if (m_Attributes.TryGetValue(name, out Attribute val))
            {
                return val;
            }
            else
            {
                return null;
            }
        }

        public bool TryGetAttribute(string name, out Attribute attrib)
        {
            if (m_Attributes.TryGetValue(name, out attrib))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected void AddAttribute(Attribute attrib)
        {
            m_Attributes.Add(attrib.Name, attrib);
        }

        protected void AddAttribute(string name, string value)
        {
            m_Attributes.Add(name, new Attribute(name, value));
        }

        protected void AddAttribute(string name, string value, object optionalData)
        {
            m_Attributes.Add(name, new Attribute(name, value, optionalData));
        }

        protected void RemoveAttribute(string name)
        {
            m_Attributes.Remove(name);
        }

        /// <summary>
        /// Checks if the node implements all interfaces from the provided list
        /// </summary>
        /// <param name="interfaces">list of interfaces</param>
        /// <returns>true: if node implements all interfaces from the list;
        /// false: if node does not inplement all interfaces from the list</returns>
        public bool ImplementsInterfaces(string [] interfaces)
        {
            string[] nodeInterfaces =this.GetInterfaceList();
            //check if interfaces is a subset of nodeInterfaces
            return interfaces.Intersect(nodeInterfaces).Count() == interfaces.Length;
        }

        public bool ImplementsInterfaces(Type[] interfaces)
        {
            Type[] nodeInterfaces = this.GetInterfaceListAsTypes();
            int intersectionCount = interfaces.Intersect(nodeInterfaces).Count();
            return intersectionCount == interfaces.Length;
        }

        /// <summary>
        /// Finds which interfaces from the provided list are implemented by this node. 
        /// Set intersection.
        /// </summary>
        /// <param name="interfaces">list of interfaces to check</param>
        /// <returns>list of nodes implemented by this node</returns>
        public string[] FindImplementedInterfaces(string[] interfaces)
        {
            return interfaces.Intersect(this.GetInterfaceList()).ToArray();
        }

        public Type[] FindImplementedInterfaces(Type[] interfaces)
        {
            return interfaces.Intersect(this.GetInterfaceListAsTypes()).ToArray();
        }

        public bool MatchesCriteria(string[] implementedInterfaces, Attribute[] attributes)
        {
            if (!ImplementsInterfaces(implementedInterfaces)) return false;
            if (!HasAttributes(attributes)) return false;
            return true;
        }

        public bool MatchesCriteria(Type[] implementedInterfaces, Attribute[] attributes)
        {
            if (!ImplementsInterfaces(implementedInterfaces)) return false;
            if (!HasAttributes(attributes)) return false;
            return true;
        }




        /// <summary>
        /// Called when the node is registered with input system
        /// </summary>
        virtual protected void OnRegister() { }
        /// <summary>
        /// Called when the node is added to an interaction node
        /// </summary>
        virtual protected void OnNewParentAdded(InteractionNode parent)
        {
            
        }
        /// <summary>
        /// Called on owner interaction node start
        /// </summary>
        virtual protected void OnStart() { }
        /// <summary>
        /// Called every frame
        /// </summary>
        virtual protected void OnUpdate() { }
        /// <summary>
        /// Called when owner interacction node shuts down
        /// </summary>
        virtual protected void OnStop() { }
        /// <summary>
        /// Called when this node is released to the pool of available nodes by it's current owner(interaction node)
        /// </summary>
        virtual protected void OnReleased()
        {

        }

    }
}
