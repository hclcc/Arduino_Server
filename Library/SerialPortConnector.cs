using System.IO.Ports;

namespace Arduino.Library
{
    public class SerialPortConnector: ISerialPortConnector
    {
        private readonly int _baudRate = 9600;
        private readonly string _portName = "COM3";

        public void Send(string command)
        {
            using (var serialPort = new SerialPort(_portName, _baudRate))
            {
                serialPort.Open();
                serialPort.Write(command);
            }
        }
    }
}
