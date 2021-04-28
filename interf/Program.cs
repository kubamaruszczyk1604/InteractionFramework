using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
namespace InteractionFramework
{



    class Controller: InputNode, ICommandProvider, I2DPosProvider
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


        public Controller():base()
        {
          //  Console.WriteLine("Instance constructor called");
            AddAttribute(new Attribute("Name", "Mouse"));
            AddAttribute(new Attribute("Type", "2Buttons"));
            //AddAttribute();
        }

        public Vector3 GetPosition()
        {
            return new Vector3(m_Point.X, m_Point.Y, 0.0f);
        }
        public OnPositionChanged onPositionChanged { get; set; }

        public OnCommand onCommand { get; set; }

        override public void OnStart()
        {
            base.OnStart();
            Console.WriteLine("Start controllera");
        }

        public override void Update()
        {
            base.Update();
            m_LastPoint = m_Point;
           
            GetCursorPos(ref m_Point);

            int dx = m_Point.X - m_LastPoint.X;
            int dy = m_Point.Y - m_LastPoint.Y;

            if(dx > 0 || dy > 0)
            {
                if (onPositionChanged != null) onPositionChanged.Invoke(new Vector3(dx, dy, 0.0f), new Vector3(m_Point.X,m_Point.Y,0.0f));
            }
            if(Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                if(key.Key == ConsoleKey.LeftArrow)
                {
                    CommandData cd = new CommandData();
                    cd.Command = "LeftButton";
                    if (onCommand != null) onCommand.Invoke(cd);
                }
                else if(key.Key == ConsoleKey.RightArrow)
                {
                    CommandData cd = new CommandData();
                    cd.Command = "RightButton";
                    if (onCommand != null) onCommand.Invoke(cd);
                }
            }
            // Console.WriteLine("Update controllera: X = " + m_Point.X + "  Y = " + m_Point.Y);
        }

    }

    class Program
    {



        static void Main(string[] args)
        {

            Controller con = new Controller();
            //Type[] inter = con.GetInterfaceListAsTypes();
            //List<Type> tst = new List<Type>();
            //tst.AddRange(inter);
            //tst.Remove(inter[1 ]);

            //foreach(Type s in inter)
            //{
            //    Console.WriteLine(s.Name);
            //}

            // InputSystem refer = InputSystem.Instance;

            // Console.WriteLine(InputSystem.ImplementsInterfaces(con, tst.ToArray()));
           // Attribute[] attr = new Attribute[] { new Attribute("a", "2"), new Attribute("b", "3") };
            // bool val =  InputSystem.IsMathingCriteria(con, tst.ToArray(), attr);
            // Console.WriteLine("Matches criteria: " + val.ToString());

            //InputNode[] matches = InputSystem.Instance.FindNodesMatchingCriteria(tst.ToArray(), attr);
            //Console.WriteLine(matches.Length);

            TestNodeInteraction tin = new TestNodeInteraction();
            tin.FindNodes();

            tin.OnStart();
            while(true)
            {
                tin.OnUpdate();
            }

            Console.ReadLine();
        }
    }
}
