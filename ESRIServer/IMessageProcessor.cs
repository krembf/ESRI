using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESRIServer
{
    public interface IMessageProcessor
    {
        string Process(string Command);
    }
}
