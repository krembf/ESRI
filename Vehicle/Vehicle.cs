using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Threading;
using System.Net.Sockets;

namespace Vehicle
{
    public class Attributes
    {
        public string name;
        public uint speed;
    }

    public class Geometry
    {
        public double m_x = 0.0;
        public double m_y = 0.0;
    }

    public class ESRIVehicle : IVehicle
    {
        private Timer timer;
        private TcpClient tcpClient;
        private NetworkStream tcpClientStream;
        public Attributes attributes;
        public Geometry geometry;
        private Random rnd_x = new Random();
        private Random rnd_y = new Random();

        public ESRIVehicle(string name = "Default")
        {
            attributes = new Attributes() { name = name, speed = 0 };
            geometry = new Geometry() { m_x = 0.0, m_y = 0.0 };

            tcpClient = new TcpClient();
            tcpClient.Connect("127.0.0.1", 7891);
            tcpClientStream = tcpClient.GetStream();
            timer = new Timer(UpdateVehicleStatus, null, 1000, 1000);
        }

        public string GetStatus()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(this);
        }

        public void UpdateVehicleStatus(Object state)
        {
            //Send the information to the server
            System.Console.WriteLine("Updating vehicle");

            geometry.m_x = rnd_x.Next(15000);
            geometry.m_y = rnd_y.Next(15000);

            byte[] sendBytes = Encoding.ASCII.GetBytes(GetStatus());
            tcpClientStream.Write(sendBytes, 0, sendBytes.Length);
        }

        //TBD add deserialization method for converting string to an vehicle (IVehicle) object
    }
}
