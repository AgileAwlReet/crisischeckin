﻿using System;
using System.Collections.Generic;
using Models;

namespace Services.Interfaces
{
    public interface IAdmin
    {
        IEnumerable<Person> GetVolunteers(Disaster disaster, bool checkedInOnly = false);
        IEnumerable<Person> GetVolunteersForDate(Disaster disaster, DateTime date, bool clusterCoordinatorsOnly, bool checkedInOnly = false);
        IEnumerable<Person> GetVolunteersForDate(int disasterId, DateTime date, bool clusterCoordinatorsOnly, bool checkedInOnly = false);
        IEnumerable<Person> GetVolunteersForDisaster(int disasterId, DateTime? commitmentDate, bool checkedInOnly = false);
    }
}
