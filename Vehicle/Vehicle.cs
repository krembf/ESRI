using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

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
        public Attributes attributes;
        public Geometry geometry;

        public ESRIVehicle()
        {
            attributes = new Attributes() { name = "Default", speed = 0 };
            geometry = new Geometry() { m_x = 0.0, m_y = 0.0 };
        }

        public string GetStatus()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(this);
        }

        //TBD add deserialization method for converting string to an vehicle (IVehicle) object
    }
}
