using Arduino.Library;
using Arduino.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Arduino.Services
{

    public class ArduinoService:IArduinoService
    {
        private ISerialPortConnector _serialPortConnector;
        private ILogActionsMethods _logActionsMethods;
        public ArduinoService(ISerialPortConnector serialPortConnector, ILogActionsMethods logActionsMethods)
        {
            _serialPortConnector = serialPortConnector;
            _logActionsMethods = logActionsMethods;
        }
        public string Send(Ardcommand ardcommand)
        {
            Serilog.Log.Information($"Started--{ardcommand.command + ardcommand.vlnumber}");
            try
            {
                //_serialPortConnector.Send(ardcommand.command + ardcommand.vlnumber);
                _logActionsMethods.CreateLogAction(new Models.LogAction() { Action = "Send", Command = ardcommand.command, ValveNr = ardcommand.vlnumber.ToString(), DateExec = DateTime.Now, Result = true });

                Serilog.Log.Information($"Send:{ardcommand.command}-{ardcommand.vlnumber}:{ardcommand.seconds.ToString()} - Success");
                return "Ok";
            }
            catch (Exception ex)
            {
                _logActionsMethods.CreateLogAction(new Models.LogAction() { Action = "Send", Command = ardcommand.command, ValveNr = ardcommand.vlnumber.ToString(), DateExec = DateTime.Now, Result = false, Info = ex.Message });

                Serilog.Log.Error($"{ardcommand.command + ardcommand.vlnumber} - Fail");
                return $"{ardcommand.command + ardcommand.vlnumber} - Fail";
            }
        }

        public string OnThenOff(Ardcommand ardcommand)
        {
                Serilog.Log.Information($"Started--{ardcommand.command + ardcommand.vlnumber}");
                try
                {
                    //_serialPortConnector.Send(ardcommand.command + ardcommand.vlnumber);
                    _logActionsMethods.CreateLogAction(new Models.LogAction() { Action = "Send", Command = ardcommand.command, ValveNr = ardcommand.vlnumber.ToString(), DateExec = DateTime.Now, Result = true });

                    Serilog.Log.Information($"OnThenOff:{ardcommand.command}-{ardcommand.vlnumber}:{ardcommand.seconds.ToString()} - Success");

                    StartClock(ardcommand);
                    return "Ok";
                }
                catch (Exception ex)
                {
                    _logActionsMethods.CreateLogAction(new Models.LogAction() { Action = "Send", Command = ardcommand.command, ValveNr = ardcommand.vlnumber.ToString(), DateExec = DateTime.Now, Result = false, Info = ex.Message });

                    Serilog.Log.Error($"{ardcommand.command + ardcommand.vlnumber} - Fail");
                    return $"{ardcommand.command + ardcommand.vlnumber} - Fail";
                }
        }
        public CancellationTokenSource _cancelToken;
        public Timer _waterTimer;
        public Task _waterTask;
        public void StartClock(Ardcommand ardcommand)
        {
            _waterTimer = new Timer(TimerElapsedHandler, ardcommand, ardcommand.seconds * 1000, 0);
        }

        public void TimerElapsedHandler(object state)
        {
            Ardcommand ardcommand= (Ardcommand)state;
            ardcommand.command = "0";
            _cancelToken = new CancellationTokenSource(TimeSpan.FromMilliseconds(ardcommand.seconds * 1000));
            Send(ardcommand);
        }
 
    }
}
