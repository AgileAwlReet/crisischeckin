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
            if (clusterId == 0 || personId == 0)
                return null;
            var existingCoordinator = FindExistingCoordinator(disasterId, clusterId, personId);
            if (null != existingCoordinator)
                return existingCoordinator;
            var newCoordinator = AddClusterCoordinator(disasterId, clusterId, personId);
            AppendLogEntry(disasterId, clusterId, personId);
            return newCoordinator;
        }

        ClusterCoordinator FindExistingCoordinator(int disasterId, int clusterId, int personId)
        {
            var existingCoordinator = dataService.ClusterCoordinators.FirstOrDefault(
                x => x.DisasterId == disasterId
                     && x.ClusterId == clusterId
                     && x.PersonId == personId);
            return existingCoordinator;
        }

        ClusterCoordinator AddClusterCoordinator(int disasterId, int clusterId, int personId)
        {
            return dataService.AddClusterCoordinator(new ClusterCoordinator
                                                     {
                                                         DisasterId = disasterId,
                                                         ClusterId = clusterId,
                                                         PersonId = personId,
                                                     });
        }

        void AppendLogEntry(int disasterId, int clusterId, int personId)
        {
            var clusterCoordinatorLogEntry = new ClusterCoordinatorLogEntry
                                             {
                                                 Event = ClusterCoordinatorEvents.Assigned,
                                                 TimeStampUtc = DateTime.UtcNow,
                                                 ClusterId = clusterId,
                                                 ClusterName = dataService.Clusters.Single(x => x.Id == clusterId).Name,
                                                 DisasterId = disasterId,
                                                 DisasterName = dataService.Disasters.Single(x => x.Id == disasterId).Name,
                                                 PersonId = personId,
                                                 PersonName = dataService.Persons.Single(x => x.Id == personId).FullName,
                                             };
            dataService.AppendClusterCoordinatorLogEntry(clusterCoordinatorLogEntry);
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

        public ClusterCoordinator GetCoordinator(int id)
        {
            return dataService.ClusterCoordinators.SingleOrDefault(x => x.Id == id);
        }

        public IEnumerable<ClusterCoordinator> GetAllCoordinators(int disasterId)
        {
            return dataService.ClusterCoordinators.Where(x => x.DisasterId == disasterId).ToList();
        }
    }
}