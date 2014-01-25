﻿using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Interfaces;

namespace Services
{
    public class DataService : IDataService
    {
        // This class does not dispose of the context,
        // because the Ninject librar takes care of that for us.

        private readonly CrisisCheckin context;

        public DataService(CrisisCheckin ctx)
        {
            context = ctx;
        }

        public IQueryable<Cluster> Clusters
        { get { return context.Clusters; } }

        public IQueryable<Commitment> Commitments
        { get { return context.Commitments; } }

        public IQueryable<Disaster> Disasters
        { get { return context.Disasters; } }

        public IQueryable<Person> Persons
        { get { return context.Persons; } }

        public IQueryable<User> Users
        { get { return context.Users; } }


        public Person AddPerson(Person newPerson)
        {
            Person result = context.Persons.Add(newPerson);
            context.SaveChanges();
            return result;
        }

        public Person UpdatePerson(Person updatedPerson)
        {
            Person result = context.Persons.FirstOrDefault(a => a.Id == updatedPerson.Id);

            result.FirstName = updatedPerson.FirstName;
            result.LastName = updatedPerson.LastName;
            result.Email = updatedPerson.Email;
            result.PhoneNumber = updatedPerson.PhoneNumber;

            context.SaveChanges();

            return result;
        }

        public void AddCommitment(Commitment newCommitment)
        {
            context.Commitments.Add(newCommitment);
            context.SaveChanges();
        }

       public void AddDisaster(Disaster newDisaster)
       {
           context.Disasters.Add(newDisaster);
           context.SaveChanges();
       }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
    }
}
