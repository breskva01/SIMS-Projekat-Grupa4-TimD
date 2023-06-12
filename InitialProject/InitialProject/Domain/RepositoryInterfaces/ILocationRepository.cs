﻿using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Domain.RepositoryInterfaces
{
    public interface ILocationRepository : IRepository<Location>
    {
        public Location GetById(int id);
        public Location GetByCityAndCountry(string city, string country);
    }
}
