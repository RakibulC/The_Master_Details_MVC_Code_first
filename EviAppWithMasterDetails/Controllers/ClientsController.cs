using EviAppWithMasterDetails.Models;
using EviAppWithMasterDetails.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EviAppWithMasterDetails.Controllers
{
    public class ClientsController : Controller
    {
        private TravelDbContext db = new TravelDbContext();
        // GET: Clients
        public ActionResult Index()
        {
            var clients = db.Clients.Include(c => c.BookingEntries.Select(b => b.Spot)).OrderByDescending(x => x.ClientId).ToList();
            return View(clients);
        }

        public ActionResult AddNewSpot(int? id)
        {
            ViewBag.spots = new SelectList(db.Spots.ToList(), "SpotId", "SpotName", (id != null) ? id.ToString() : "");
            return PartialView("_addNewSpot");
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(ClientVM clientVM, int[] spotId)
        {
            if(ModelState.IsValid)
            {
                Client client = new Client()
                {
                    ClientName=clientVM.ClientName,
                    BirthDate=clientVM.BirthDate,
                    Age=clientVM.Age,
                    MaritalStatus=clientVM.MaritalStatus
                };

                HttpPostedFileBase file = clientVM.PictureFile;
                if(file!= null)
                {
                    string filePath = Path.Combine("/Images/", Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));
                    file.SaveAs(Server.MapPath(filePath));
                    client.Picture = filePath;
                }

                //Save all spot from SpotId

                foreach (var item in spotId)
                {
                    BookingEntry bookingEntry = new BookingEntry()
                    {
                        Client=client,
                        ClientId=client.ClientId,
                        SpotId=item
                    };
                    db.BookingEntries.Add(bookingEntry);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        public ActionResult Edit(int? id)
        {
            Client client = db.Clients.First(x => x.ClientId == id);
            var clientSpots = db.BookingEntries.Where(x => x.ClientId == id).ToList();
            ClientVM clientVM = new ClientVM()
            {
                ClientId= client.ClientId,
                ClientName=client.ClientName,
                Age=client.Age,
                BirthDate=client.BirthDate,
                Picture=client.Picture,
                MaritalStatus=client.MaritalStatus
            };
            if (clientSpots.Count() > 0)
            {
                foreach (var item in clientSpots)
                {
                    clientVM.SpotList.Add(item.SpotId);
                }
            }
            return View(clientVM);
        }
        [HttpPost]
        public ActionResult Edit(ClientVM clientVM, int[] spotId)
        {
            if (ModelState.IsValid)
            {
                Client client = new Client()
                {
                    ClientId=clientVM.ClientId,
                    ClientName = clientVM.ClientName,
                    BirthDate = clientVM.BirthDate,
                    Age = clientVM.Age,
                    MaritalStatus = clientVM.MaritalStatus
                };

                HttpPostedFileBase file = clientVM.PictureFile;
                if (file != null)
                {
                    string filePath = Path.Combine("/Images/", Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));
                    file.SaveAs(Server.MapPath(filePath));
                    client.Picture = filePath;
                }
                else
                {
                    client.Picture = "";//Home work...
                }

                var existsSpotEntry = db.BookingEntries.Where(x => x.ClientId == client.ClientId).ToList();

                //For delete spot
                foreach (var bookingEntry in existsSpotEntry)
                {
                    db.BookingEntries.Remove(bookingEntry);
                }

                //Add Spot
                foreach (var item in spotId)
                {
                    BookingEntry bookingEntry = new BookingEntry()
                    {
                        ClientId = client.ClientId,
                        SpotId = item
                    };
                    db.BookingEntries.Add(bookingEntry);
                }
                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

    }
}