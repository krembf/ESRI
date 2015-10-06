using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using Vehicle;
using System.Threading;

///The ideas for the server: depending on the approach, that is, need to determine how the vehicles are updated (polling etc.), 
///need to implement the emission mechanism. It could be done, for example, with timer routine that triggers update of all vehicles
///in collection. Also, need to consider an option to abstract the transport, that is, the server shoud easily be swapped by UDP, or 
///be able to listen to both transport protocols.

namespace ESRIServer
{
    public class ESRIServer
    {
        private TcpListener tcpListener;
        private const int port = 7890;  //TBD create xml configuration file and load the transport details (optional).
        private List<IVehicle> vehicles;

        public ESRIServer()
        {
            //Start TCP Server
            tcpListener = new TcpListener(IPAddress.Any, port);

            vehicles = new List<IVehicle>();
        }

        public bool RegisterVehicle(IVehicle vehicle)
        {
            vehicles.Add(vehicle);
            return true;
        }

        public void Start()
        {
            tcpListener.Start();
        }

        private bool ServerStart()
        {
            try
            {
                Thread tcpListenerThread = new Thread(Start);
                tcpListenerThread.Start();
                return true;
            }
            catch
            {
                return false;
            }            
        }

    }
}
