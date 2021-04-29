using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
namespace InteractionFramework
{

    class Program
    {
        static void Main(string[] args)
        {

            Controller con = new Controller("A");
            
            Controller con2 = new Controller("B");
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


            tin.Start();
            while (true)
            {
                tin.Update();
            }

            Console.ReadLine();
        }
    }
}   
