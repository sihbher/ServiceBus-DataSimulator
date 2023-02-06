using System;
namespace ServiceBus
{
    internal class Telemetry
    {
        public int IDPlant { get; set; }
        public int IDOrder { get; set; }
        public double Amount { get; set; }
        public double Price { get; set; }
        public bool Dispatched { get; set; }
        public DateTime CreatedTimeStamp { get; set; }
        public DateTime LastUpdatedTimeStamp { get; set; }
    }
}