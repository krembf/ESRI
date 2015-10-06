using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vehicle;

namespace ESRIServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ESRIServer server = new ESRIServer();
            server.Start();

            List<IVehicle> vehicles = new List<IVehicle>();

            for (int i = 0; i < 5; i++)
            {
                vehicles.Add(new ESRIVehicle("Vehicle" + i.ToString()));
            }

            Console.ReadKey();
        }
    }
}
