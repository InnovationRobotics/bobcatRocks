using System;

namespace Assets.MonogoScripts
{
    public class ReceivedCommand
    {
        public string CommandName { get; set; } //TODO Change to Enum
        public DateTime Time { get; set; }
        public int VehicleSpeed { get; set; }
        public string VehiclePosition { get; set; } // TODO Change to Vector

        public string VehicleOrientation { get; set; } // TODO Change to Vector

        public ReceivedCommand(string commandName , DateTime time,int vehicleSpeed,string vehiclePosition,string vehicleOrientation )
        {
            CommandName = commandName;
            Time = time;
            VehicleSpeed = vehicleSpeed;
            VehiclePosition = vehiclePosition;
            VehicleOrientation = vehicleOrientation;
        }

        public ReceivedCommand()
        {
            
        }
    }
}
