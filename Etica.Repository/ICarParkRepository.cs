﻿using Etica.Repository.Entitites;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Etica.Repository
{
    public interface ICarParkRepository
    {
        Task<RateEntity> GetApplicableRate(DateTime start, DateTime end);
    }
}