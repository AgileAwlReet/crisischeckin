﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using crisicheckinweb.ViewModels;
using Models;
using Services.Interfaces;
using WebMatrix.WebData;

namespace crisicheckinweb.Controllers
{
    public class HomeController : BaseController
    {

        private readonly IDisaster _disasterSvc;
        private readonly IVolunteer _volunteerSvc;

        public HomeController(IDisaster disasterSvc, IVolunteer volunteerSvc)
        {
            _disasterSvc = disasterSvc;
            _volunteerSvc = volunteerSvc;
        }

        // GET: /Home/
        public ActionResult Index()
        {
            IQueryable<Commitment> comms = _volunteerSvc.RetrieveCommitments(new Person() { Id = WebSecurity.CurrentUserId }, true); 

            var model = new VolunteerViewModel { Disasters = _disasterSvc.GetActiveList(),
                MyCommitments = comms };
            return View(model);
        }

        [HttpPost]
        public RedirectResult Assign(VolunteerViewModel model)
        {

            _disasterSvc.AssignToVolunteer(new Disaster { Id = model.SelectedDisaster },
                new Person { Id = WebSecurity.CurrentUserId }, model.SelectedStartDate, model.SelectedEndDate);

            return Redirect("/Home");
        }

    }
}
