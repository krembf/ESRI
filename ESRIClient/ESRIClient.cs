using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

///The ideas for the Client: bind to a specified server (take out server configuration to external XML file)
///and upon reception of packet, attempt to deserialize and convert to an IVehicle object. Then print out.
///Queing is needed, as the received traffic could be faster than the processing (e.g. printing)

namespace ESRIClient
{
    public class ESRIClient
    {
    }
}
