﻿using System.Collections;
using System.Collections.Generic;
using Models;

namespace crisicheckinweb.ViewModels
{
    public class DisasterClusterCoordinatorsViewModel
    {
        public int DisasterId { get; set; }
        public List<Cluster> AvailableClusters { get; set; }
        public List<Person> AvailablePeople { get; set; }
        public string DisasterName { get; set; }
        public List<ClusterViewModel> Clusters { get; set; }
        public int SelectedClusterId { get; set; }
        public int SelectedPersonId { get; set; }
    }

    public class ClusterViewModel
    {
        public string Name { get; set; }
        public List<ClusterCoordinatorViewModel> Coordinators { get; set; }
    }

    public class ClusterCoordinatorViewModel
    {
        public string Name { get; set; }
    }
}