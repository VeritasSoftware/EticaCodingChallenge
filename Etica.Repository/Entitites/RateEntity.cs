using System;

namespace Etica.Repository.Entitites
{
    public enum RateType
    {
        Flat
    }

    public enum RateDay
    {
        Weekday,
        Weekend
    }

    public class RateEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual RateType Type { get; set; }
        public RateDay RateDay { get; set; }
        public DateTime StartMin { get; set; }
        public DateTime StartMax { get; set; }
        public DateTime EndMin { get; set; }
        public DateTime EndMax { get; set; }
        public decimal Price { get; set; }
    }
}
