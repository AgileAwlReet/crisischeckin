﻿using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using Services.Interfaces;

namespace Services
{
    public class AdminService : IAdmin
    {
        private readonly IDataService dataService;

        public AdminService(IDataService service)
        {
            if (service == null)
                throw new ArgumentNullException("service", "Service Interface must not be null");
            this.dataService = service;
        }

        public IEnumerable<Person> GetVolunteers(Disaster disaster)
        {
            if (disaster == null)
                throw new ArgumentNullException("disaster", "disaster cannot be null");
            var storedDisaster = dataService.Disasters.SingleOrDefault(d => d.Id == disaster.Id);
            if (storedDisaster == null)
                throw new ArgumentException("Disaster was not found", "disaster");
            IQueryable<Person> people = GetPeople(disaster);
            return people;
        }

        public IEnumerable<Person> GetVolunteersForDate(Disaster disaster, DateTime date)
        {
            return GetVolunteersForDate(disaster.Id, date);
        }

        public IReadOnlyCollection<Person> GetVolunteersForDate(int disasterId, DateTime date)
        {
            if (0 == disasterId)
                throw new ArgumentException("disasterId must be greater than zero", "disasterId");

            return dataService.Commitments.Where(
                   x => x.DisasterId.Equals(disasterId) &&
                   x.StartDate <= date &&
                   x.EndDate >= date).Select(x => x.Person).Distinct().ToList();
        }

        private IQueryable<Person> GetPeople(Disaster disaster)
        {
            var commitments = from c in dataService.Commitments
                              where c.DisasterId == disaster.Id
                              select c;
            var people = from c in commitments
                         join p in dataService.Persons on c.PersonId equals p.Id
                         select p;
            return people;
        }
    }
}
