﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Services.Interfaces
{
    public interface IVolunteer
    {
        Person Register(string firstName, string lastName, string email, string phoneNumber);


    }
}
