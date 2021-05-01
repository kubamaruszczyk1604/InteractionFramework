
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace InteractionFramework
{

    class Controller : InputNode, ICommandProvider, I2DPosProvider
    {
        // We need to use unmanaged code
        #region implementation specific

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        [DllImport("user32.dll")]

        // GetCursorPos() makes everything possible
        static extern bool GetCursorPos(ref POINT lpPoint);

        POINT m_Point = default(POINT);
        POINT m_LastPoint = default(POINT);
        #endregion


        public Controller(string id) : base()
        {
            //  Console.WriteLine("Instance constructor called");
            AddAttribute(new Attribute("Name", "Mouse"));
            AddAttribute(new Attribute("Type", "2Buttons"));
            ID = id;
            //AddAttribute();
        }

        public Vector3 GetPosition()
        {
            return new Vector3(m_Point.X, m_Point.Y, 0.0f);
        }
        public OnPositionChanged onPositionChanged { get; set; }

        public OnCommand onCommand { get; set; }

        override protected void OnStart()
        {
            base.OnStart();
            Console.WriteLine("Start controllera");

        }

        override protected void OnUpdate()
        {
            base.OnUpdate();
            m_LastPoint = m_Point;

            GetCursorPos(ref m_Point);

            int dx = m_Point.X - m_LastPoint.X;
            int dy = m_Point.Y - m_LastPoint.Y;

            if (dx > 0 || dy > 0)
            {
                if (onPositionChanged != null) onPositionChanged.Invoke(new Vector3(dx, dy, 0.0f), new Vector3(m_Point.X, m_Point.Y, 0.0f));
            }
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.LeftArrow)
                {
                    CommandData cd = new CommandData();
                    cd.Command = "LeftButton " + ID;
                    if (onCommand != null) onCommand.Invoke(cd);
                }
                else if (key.Key == ConsoleKey.RightArrow)
                {
                    CommandData cd = new CommandData();
                    cd.Command = "RightButton " + ID;
                    if (onCommand != null) onCommand.Invoke(cd);
                }
            }
            // Console.WriteLine("Update controllera: X = " + m_Point.X + "  Y = " + m_Point.Y);
        }

    }


    public class TestNodeInteraction : InteractionNode
    {
        I2DPosProvider m_PosProvider = null;
        ICommandProvider m_CommandProvider = null;
        Attribute[] attribList = new Attribute[] { new Attribute("Name", "Mouse"), new Attribute("Type", "2Buttons") };

        override protected bool FindInputNodes()
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

            if (TryFindMatchingInputNode(attribList, new Type[] { Utils.GetTypeFromRef(m_PosProvider), Utils.GetTypeFromRef(m_CommandProvider) }, out InputNode node))
            {
                AddInputNode(node);
                m_PosProvider = (I2DPosProvider)node;
                m_CommandProvider = (ICommandProvider)node;


                return true;
            }
            return false;
        }

        override protected void OnBecomeReady()
        {
            m_PosProvider.onPositionChanged += OnPositionChanged;
            m_CommandProvider.onCommand += OnCommand;
        }

        override protected void OnBecomeUnavailable(InteractionNodeState state)
        {
            m_PosProvider.onPositionChanged -= OnPositionChanged;
            m_CommandProvider.onCommand -= OnCommand;
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
