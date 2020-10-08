using HotelManagement.Models;
using HotelManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelManagement.Controllers
{
    public class RoomController : Controller
    {

      private  HotelDBEntities objhotelDBEntities;

        public RoomController()
        {
            objhotelDBEntities = new HotelDBEntities();
        }
        // GET: Room
        public ActionResult Index()
        {
            RoomViewModel objRoomViewModel = new RoomViewModel();
            objRoomViewModel.ListOfBookingStatus = (from obj in objhotelDBEntities.BookingStatus
                                                    select new SelectListItem()
                                                    {
                                                        Text = obj.BookingStatus,
                                                        Value = obj.BookingStatusId.ToString()

                                                    }).ToList();

            objRoomViewModel.ListOfRoomType = (from obj in objhotelDBEntities.RoomTypes
                                               select new SelectListItem()
                                               {
                                                   Text = obj.RoomTypeName,
                                                   Value = obj.RoomTypeId.ToString()

                                               }).ToList();
            return View(objRoomViewModel);
        }

        [HttpPost]
        public ActionResult Index(RoomViewModel objRoomViewModel)
        {
            //string ImageUniqueName = Guid.NewGuid().ToString();
            //string ActualImageName = ImageUniqueName + Path.GetExtension(objRoomViewModel.Image.FileName);

            //objRoomViewModel.Image.SaveAs(Server.MapPath("~/RoomImages/" + ActualImageName));

            try
            {
                Room objRoom = new Room()
                {
                    RoomNumber = objRoomViewModel.RoomNumber,
                    RoomDescription = objRoomViewModel.RoomDescription,
                    RoomPrice = objRoomViewModel.RoomPrice,
                    BookingStatusId = objRoomViewModel.BookingStatusId,
                    //RoomImage = objRoomViewModel.RoomImage,
                    //IsActive = true,
                    RoomCapacity = objRoomViewModel.RoomCapacity,
                    RoomTypeId = objRoomViewModel.RoomTypeId,
                };

                objhotelDBEntities.Rooms.Add(objRoom);
                objhotelDBEntities.SaveChanges();
            }



            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }


            return Json(new { message = "Room Succsessfully Added.", success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}