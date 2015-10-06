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
///TBD create xml configuration file and load the transport details (optional).

namespace ESRIServer
{
    public class ESRIServer : IMessageProcessor
    {
        private TCPListener tcpInputListener;
        private TcpListener tcpBroadcastListener;
        private int inputPort = 7891;
        private int broadcastPort = 7890;

        private List<TcpClient> broadcastClients;
        Queue<string> updates;
        System.Threading.Mutex updatesMutex = new Mutex(false, "updatesMutex");

        public ESRIServer(int _inputPort = 7891, int _broadcastPort = 7890)
        {
            inputPort = _inputPort;
            broadcastPort = _broadcastPort;

            //Initialize TCP Server
            tcpInputListener = new TCPListener(inputPort, this);
            tcpBroadcastListener = new TcpListener(IPAddress.Any, broadcastPort);

            broadcastClients = new List<TcpClient>();

            updates = new Queue<string>();
        }

        private void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
        }

        private void BroadcastServerStart()
        {
            //Start TCP server
            tcpBroadcastListener.Start();

            while (true)
            {
                TcpClient newClient = tcpBroadcastListener.AcceptTcpClient();
                broadcastClients.Add(newClient);
                System.Console.WriteLine("New broadcast client " + ((IPEndPoint)(newClient.Client.RemoteEndPoint)).ToString() + " connected");
            }
        }

        public bool Start()
        {
            try
            {
                tcpInputListener.Start();
                Thread tcpListenerBroadcastThread = new Thread(BroadcastServerStart);

                return true;
            }
            catch
            {
                return false;
            }            
        }

        public string Process(string Command)
        {
            System.Console.WriteLine("Enqueing " + Command + " update");
            //Add to queue
            updatesMutex.WaitOne();
            updates.Enqueue(Command);
            System.Console.WriteLine("Queue size " + updates.Count.ToString());
            updatesMutex.ReleaseMutex();

            return null;
        }

    }
}
