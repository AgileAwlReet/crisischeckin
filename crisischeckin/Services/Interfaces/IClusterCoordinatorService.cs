﻿using System.Collections.Generic;
using Models;

namespace Services.Interfaces
{
    public interface IClusterCoordinatorService
    {
        IEnumerable<ClusterCoordinator> GetAllCoordinators(int disasterId);
        ClusterCoordinator AssignClusterCoordinator(int disasterId, int clusterId, int personId);
        void UnassignClusterCoordinator(ClusterCoordinator clusterCoordinator);
        ClusterCoordinator GetCoordinator(int id);
        IEnumerable<ClusterCoordinator> GetAllCoordinatorsForDisplay(int disasterId, out IList<Person> allPersonsForDisplay);
        ClusterCoordinator GetCoordinatorFullyLoaded(int id);
        ClusterCoordinator GetCoordinatorForUnassign(int id);
    }
}