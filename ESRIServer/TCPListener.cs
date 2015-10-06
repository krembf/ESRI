using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace ESRIServer
{
    class TCPListener
    {
        private TcpListener tcpListener;
        private Thread ListenerThread;
        private int PortNumber = 0;
        private IMessageProcessor MessageProcessor;

        public TCPListener(int _portNumber, IMessageProcessor _messageProcessor)
        {
            PortNumber = _portNumber;
            MessageProcessor = _messageProcessor;

            tcpListener = new TcpListener(IPAddress.Any, PortNumber);
        }

        public bool Start()
        {
            try
            {
                tcpListener.Start();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Exception: " + ex.Message);

                return false;
            }

            try
            {
                ListenerThread = new Thread(ListenerWorker);
                ListenerThread.Start();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Exception: " + ex.Message);

                return false;
            }

            return true;
        }

        // Private Methods

        internal bool Send(TcpClient client, string response)
        {
            bool result = false;
            NetworkStream stream = client.GetStream();

            //If there is nothing to send back, return immediately
            if (null == response || "" == response) return true;

            int xmlLength = response.Length;
            byte[] retBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(response);

            try
            {
                stream.Write(retBytes, 0, retBytes.Length);

                result = true;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Exception: " + ex.Message);

                result = false;
            }

            return result;
        }

        internal string Receive(TcpClient client)
        {
            byte[] received = new byte[8192];
            int receivedBytes = 0;
            string response = null;


            NetworkStream stream = client.GetStream();

            receivedBytes = stream.Read(received, 0, received.Length);
            System.Console.WriteLine("Received " + receivedBytes + " bytes");

            // convert received bytes to string
            response = Encoding.UTF8.GetString(received, 0, receivedBytes);
            response.Trim();

            return response;
        }

        class MessageProcessingParameters
        {
            public TcpClient m_client;
        }

        internal void ProcessClient(Object param)
        {
            string message = null;
            string response;

            while (true)
            {
                message = Receive(((MessageProcessingParameters)param).m_client);

                response = MessageProcessor.Process(message);

                bool ret = Send(((MessageProcessingParameters)param).m_client, response);
            }            
        }

        internal void ListenerWorker()
        {
            while (true)
            {
                try
                {
                    TcpClient newClient = tcpListener.AcceptTcpClient();
                    System.Console.WriteLine("New input client " + ((IPEndPoint)(newClient.Client.RemoteEndPoint)).ToString() + " connected");

                    Thread MessageThread = new Thread(ProcessClient);
                    MessageThread.Start(new MessageProcessingParameters
                    {
                        m_client = newClient,
                    });
                }
                catch (Exception ex)
                {
                    //this will catch socket exception, if there is any socket error, stop this thread 
                    System.Console.WriteLine("Exception: " + ex.Message);
                    tcpListener.Server.Close();
                    return;
                }
            }

        }

    }
}
