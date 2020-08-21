using System;

namespace Etica.Repository.Entitites
{
    public enum RateType
    {
        Flat,
        Hourly
    }

    public enum RateDay
    {
        Weekday,
        Weekend
    }

    public abstract class BaseRateEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual RateType Type { get; set; }
        public double Price { get; set; }
    }

    public class RateEntity : BaseRateEntity
    {        
        public RateDay RateDay { get; set; }
        public DateTime EntryMin { get; set; }
        public DateTime EntryMax { get; set; }
        public DateTime ExitMin { get; set; }
        public DateTime ExitMax { get; set; }        
    }

    public class HourlyRateEntity : BaseRateEntity
    {
        public int DurationMin { get; set; }
        public int DurationMax { get; set; }
        public bool IsHourly { get; set; }
        public bool IsDaily { get; set; }
    }
}
