using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using UnilifeClassesRoomsDiplomServerDLL;

namespace Host
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(UnilifeClassesRoomsDiplomServerDLL.UnilifeClassesRoomsDiplomServerDDL)))
            {
                host.Open();
                Console.WriteLine("Хост стартовал!");
                Console.ReadLine();
            }

        }
    }
}
