﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IDisaster
    {
        bool AssignToVolunteer(int disasterId, string volunteerEmail, DateTime startDate, DateTime endDate);

    }
}
