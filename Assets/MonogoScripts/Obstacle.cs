namespace Assets.MonogoScripts
{
    public class Obstacle
    {
        public string Name { get; set; } 
        public string Position { get; set; }
        public int MinRangeFromVehicle { get; set; }//?

        public int VehicleSpeed { get; set; }

        public Obstacle(string obstacleName ,string obstaclePosition,int minRangeFromVehicle, int vehicleSpeed)
        {
            Name = obstacleName;
            Position = obstaclePosition;
            MinRangeFromVehicle = minRangeFromVehicle;
            VehicleSpeed = vehicleSpeed;
        }

        public Obstacle()
        {
            
        }
    }
}
