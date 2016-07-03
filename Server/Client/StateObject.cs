using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public class StateObject
    {
        public Socket clientSocket = null;
        public static int BufferSize = 1024;
        public byte[] buffer = new byte[BufferSize];
        public StringBuilder dataReceived = new StringBuilder();

        public bool receiving = false;
    }
}
