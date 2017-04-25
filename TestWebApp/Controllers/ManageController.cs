using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

using Frost.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Frost.Controllers
{
    public class ManageController : Controller
    {
        public ActionResult Index()
        {
            List<EndorsementRequest> requests = new List<EndorsementRequest>(EndorsementRequestCollection.Current.Where(r => r.Status == EndorsementRequestStatus.New));
            return View(requests);
        }

        // GET: Manage/Details/5
        public ActionResult Details(Guid id)
        {
            return View(EndorsementRequestCollection.Current.FirstOrDefault(r => r.Id == id));
        }

        // GET: Manage/Details/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approve(Guid id)
        {
            var request = EndorsementRequestCollection.Current.FirstOrDefault(r => r.Id == id);

            if (request == null)
                throw new ArgumentException();

            request.Status = EndorsementRequestStatus.Approved;
            EndorsementRequestCollection.Current.Write();

            var encoder = new PoetEncoder("CN=tempuri.org", StoreLocation.LocalMachine, StoreName.My);
            var jwt = encoder.Sign(request);

            var handler = new JwtSecurityTokenHandler();
            var jwtString = handler.WriteToken(jwt);
            return File(Encoding.ASCII.GetBytes(jwtString), "application/octet-stream", $"{id}.jot");
        }

        // GET: Manage/Create
        public ActionResult Create()
        {
            return View();
        }

        // GET: Manage/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }
        // GET: Manage/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

    }
}