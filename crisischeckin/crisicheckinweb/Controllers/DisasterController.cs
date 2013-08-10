﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using Services.Interfaces;

namespace crisicheckinweb.Controllers
{
    public class DisasterController : Controller
    {
        private readonly IDisaster _disasterSvc;
        public DisasterController(IDisaster disasterSvc)
        {
            _disasterSvc = disasterSvc;
        }

        //
        // GET: /Disaster/
        public ActionResult List()
        {
            var viewData = new List<Disaster>();

            for (int intI = 0; intI < 5; intI++)
            {
                var disaster = new Disaster {Id = intI, IsActive = true, Name = "Disaster Name " + intI.ToString()};
                viewData.Add(disaster);
            }

            // TODO: Pull actual List of all disasters here
            return View(viewData);
        }
        
        [HttpGet]
        public ActionResult Edit(string id)
        {
            bool validId = false;
            int disasterId = 0;

            Disaster viewData;

            validId = int.TryParse(id, out disasterId);

            if (validId && disasterId != -1)
            {
                // TODO: Pull actual disaster by ID
                viewData = new Disaster{
                        Id = disasterId,
                        Name = "Disaster Name " + disasterId.ToString(),
                        IsActive = true
                    };
            }
            else
            {
                // Adding new Disaster here
                viewData = new Disaster();
                viewData.IsActive = true;
            }
            
            return View(viewData);
        }

        [HttpPost]
        public RedirectResult Edit(Disaster disaster)
        {
            // TODO: Update the disaster data by ID
            if (disaster.Id == -1)
            {

            }
            else
            {
                
            }

            return Redirect("/Disaster/List");
        }

        #region api methods
        public JsonResult GetActiveDisasters()
        {
            return Json(_disasterSvc.GetActiveList(), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
