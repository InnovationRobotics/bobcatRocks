
namespace Assets.MonogoScripts
{
    public class WayPoint
    {
        public string Id { get; set; } 
        public string Position { get; set; }
        public int VehicleSpeed { get; set; }

        public WayPoint(string id, string position,int vehicleSpeed)
        {
            Id = id;
            Position = position;
            VehicleSpeed = vehicleSpeed;
        }

        public WayPoint()
        {
            
        }
    }
}
