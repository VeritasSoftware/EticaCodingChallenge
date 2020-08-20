using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;

namespace Etica.Repository.Entitites
{
    public enum RateType
    {
        Flat,
        Charge
    }

    public class RateEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual RateType Type { get; set; }
        public DateTime StartMin { get; set; }
        public DateTime StartMax { get; set; }
        public DateTime EndMin { get; set; }
        public DateTime EndMax { get; set; }
        public decimal Price { get; set; }
    }

    public enum RateCharge
    {
        One = 1,
        Two = 2,
        Three = 3,
        Daily = 4
    }
}
