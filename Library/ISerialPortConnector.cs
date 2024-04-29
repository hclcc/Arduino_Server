using System.IO.Ports;

    namespace Arduino.Library
{
    public interface ISerialPortConnector
    {
        public void Send(string command);
    }
}
