using System;
using System.Collections.Generic;
using System.Text;

namespace FullControlChanger
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                string filename = @args[0];
                FullControlChanger changer = new FullControlChanger(Environment.UserDomainName + @"\" + Environment.UserName);
                changer.Change(filename);

                foreach (string file in changer.changed)
                {
                    Console.WriteLine(file);
                }
                Console.WriteLine("({0} files changed)", changer.changed.Count);
                Console.ReadKey();
            }
        }
    }
}
