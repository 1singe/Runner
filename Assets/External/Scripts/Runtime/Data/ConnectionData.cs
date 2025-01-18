using System;

namespace PGG
{
    [Serializable]
    public struct Connection
    {
        public ConnectionPort In;
        public ConnectionPort Out;

        public Connection(ConnectionPort input, ConnectionPort output)
        {
            In = input;
            Out = output;
        }

        public Connection(string inputPortId, int inputPortIndex, string outputPortId, int outputPortIndex)
        {
            In = new ConnectionPort(inputPortId, inputPortIndex);
            Out = new ConnectionPort(outputPortId, outputPortIndex);
        }
    }

    [Serializable]
    public struct ConnectionPort
    {
        public string NodeId;
        public int PortIndex;

        public ConnectionPort(string id, int portIndex)
        {
            NodeId = id;
            PortIndex = portIndex;
        }
    }
}