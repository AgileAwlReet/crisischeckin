﻿using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using Services.Interfaces;

namespace Services
{
    public class ClusterCoordinatorService : IClusterCoordinatorService
    {
        readonly IDataService dataService;

        public ClusterCoordinatorService(IDataService dataService)
        {
            this.dataService = dataService;
        }

        public ClusterCoordinator AssignClusterCoordinator(int disasterId, int clusterId, int personId)
        {
            var clusterCoordinator = dataService.AddClusterCoordinator(new ClusterCoordinator
                                                                       {
                                                                           DisasterId = disasterId, 
                                                                           ClusterId = clusterId, 
                                                                           PersonId = personId,
                                                                       });
            var clusterCoordinatorLogEntry = new ClusterCoordinatorLogEntry
                                             {
                                                 Event = ClusterCoordinatorEvents.Assigned,
                                                 TimeStampUtc = DateTime.UtcNow,
                                                 ClusterId = clusterId,
                                                 ClusterName = dataService.Clusters.Single(x=>x.Id == clusterId).Name,
                                                 DisasterId = disasterId,
                                                 DisasterName = dataService.Disasters.Single(x=>x.Id == disasterId).Name,
                                                 PersonId = personId,
                                                 PersonName = dataService.Persons.Single(x=>x.Id == personId).FullName,
                                             };
            dataService.AppendClusterCoordinatorLogEntry(clusterCoordinatorLogEntry);
            return clusterCoordinator;
        }

        public void UnassignClusterCoordinator(ClusterCoordinator clusterCoordinator)
        {
            dataService.RemoveClusterCoordinator(clusterCoordinator);
            var clusterCoordinatorLogEntry = new ClusterCoordinatorLogEntry
            {
                Event = ClusterCoordinatorEvents.Unassigned,
                TimeStampUtc = DateTime.UtcNow,
                ClusterId = clusterCoordinator.ClusterId,
                ClusterName = dataService.Clusters.Single(x => x.Id == clusterCoordinator.ClusterId).Name,
                DisasterId = clusterCoordinator.DisasterId,
                DisasterName = dataService.Disasters.Single(x => x.Id == clusterCoordinator.DisasterId).Name,
                PersonId = clusterCoordinator.PersonId,
                PersonName = dataService.Persons.Single(x => x.Id == clusterCoordinator.PersonId).FullName,
            };
            dataService.AppendClusterCoordinatorLogEntry(clusterCoordinatorLogEntry);
        }

        public IEnumerable<ClusterCoordinator> GetAllCoordinators(int disasterId)
        {
            return dataService.ClusterCoordinators.Where(x => x.DisasterId == disasterId).ToList();
        }
    }
}