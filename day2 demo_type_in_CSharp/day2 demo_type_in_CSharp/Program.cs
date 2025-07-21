using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day2_demo_type_in_CSharp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int a = 10;
            int b = a;
            b = 20;
            Console.WriteLine($"value type a={a} ,b={b}");

            Person p1 = new Person();
            p1.name = "raj";
            Person p2 = new Person();
            p2 = p1;
            p2.name = "khush";
            Console.WriteLine(p1.name);


         }
    }

    public class Person
    {
        public string name;
    }
}
