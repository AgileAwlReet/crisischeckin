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

            var people = GetPeople(disaster.Id);

            if (people == null)
                throw new NullReferenceException(string.Format("Attempt to get volunteers for disaster ID {0} returned null.", disaster.Id));
            return people.ToList();
        }

        public IEnumerable<Person> GetVolunteersForDate(Disaster disaster, DateTime date)
        {
            if (disaster == null)
                throw new ArgumentNullException("disaster", "disaster cannot be null");

            return GetVolunteersForDate(disaster.Id, date);
        }

        public IEnumerable<Person> GetVolunteersForDate(int disasterId, DateTime date)
        {
            if (disasterId <= 0)
                throw new ArgumentException("disasterId is invalid.", "disasterId");

            var people = GetVolunteersForDateQueryable(disasterId, date);

            if (people == null)
                throw new NullReferenceException(string.Format("Attempt to get volunteers for disaster ID {0} returned null.", disasterId));
            return people.ToList();
        }

        private IQueryable<Person> GetVolunteersForDateQueryable(int disasterId, DateTime date)
        {
            if (disasterId <= 0)
                throw new ArgumentException("disasterId must be greater than zero", "disasterId");

            // TODO: Use GetPeople() IQueryable here and apply the date filter
            // Probably need to add navigation property to Commitment for Person in order to do it properly
            var people = from p in dataService.Persons
                         join c in dataService.Commitments on p.Id equals c.PersonId
                         where c.DisasterId == disasterId
                         where date >= c.StartDate && date <= c.EndDate
                         select p;
            //people.Include(x => x.Cluster);

            return people.Distinct();
        }

        public IEnumerable<Person> GetVolunteersForDisaster(int disasterId, DateTime? commitmentDate)
        {
            IEnumerable<Person> people;
            if (commitmentDate.HasValue)
            {
                people = GetVolunteersForDate(disasterId, commitmentDate.Value);
            }
            else
            {
                people = GetPeople(disasterId);
            }

            if (people == null)
                throw new NullReferenceException(string.Format("Attempt to get volunteers for disaster ID {0} returned null.", disasterId));
            return people.ToList();
        }

        private IQueryable<Person> GetPeople(int disasterId)
        {
            if (disasterId <= 0)
                throw new ArgumentException("disasterId is invalid.", "disasterId");

            var people = from p in dataService.Persons
                         join c in dataService.Commitments on p.Id equals c.PersonId
                         where c.DisasterId == disasterId
                         select p;

            return people.Distinct();
        }
    }
}
