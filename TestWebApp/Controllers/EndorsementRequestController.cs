using Frost.Models;
using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Frost.Controllers
{
    public class EndorsementRequestController : Controller
    {       
        // GET: Request
        public ActionResult Index()
        {           
            EndorsementRequest req = new EndorsementRequest();
            return View(req);
        }

        // POST: Request/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EndorsementRequest request)
        {
            if (!ModelState.IsValid)
                return View("Index", request);

            request.Id = Guid.NewGuid();
            request.Status = EndorsementRequestStatus.New;

            try
            {
                EndorsementRequestCollection.Current.Add(request);
            }
            catch
            {
                ModelState.TryAddModelError("", "Error saving file");
                return View("Index", request);
            }

            return View("CreateSuccess");
        }

        // GET: Request/Delete/5
        public ActionResult Delete(int id)        {
            return View();
        }
    }
}
