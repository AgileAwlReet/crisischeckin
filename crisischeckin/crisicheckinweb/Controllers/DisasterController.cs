﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using Services.Interfaces;
using Services.Exceptions;

namespace crisicheckinweb.Controllers
{
    [Authorize(Roles = Common.Constants.RoleAdmin)]
    public class DisasterController : BaseController
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
            var viewData = _disasterSvc.GetList();

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
                viewData = _disasterSvc.Get(disasterId);
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
        public ActionResult Edit(Disaster disaster)
        {
            if (ModelState.IsValid && !String.IsNullOrWhiteSpace(disaster.Name))
            {
                if (disaster.Id == -1)
                {
                    try
                    {
                        _disasterSvc.Create(disaster);
                    }
                    catch (DisasterAlreadyExistsException)
                    {
                        ModelState.AddModelError("Name", "A Disaster already exists with that Name!");
                        return View("Edit", disaster);
                    }   
                }
                else
                {
                    _disasterSvc.Update(disaster.Id, disaster.Name, disaster.IsActive);
                }

                return Redirect("/Disaster/List");
            }


            ModelState.AddModelError("Name", "Disaster Name is required!");
            return View(disaster);
        }


        #region api methods
        public JsonResult GetActiveDisasters()
        {
            return Json(_disasterSvc.GetActiveList(), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
