using Antlr.Runtime.Misc;
using QuanLyKhachSan.Daos;
using QuanLyKhachSan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Xml.Linq;
using static System.Collections.Specialized.BitVector32;

namespace QuanLyKhachSan.Controllers.Admin
{
    public class EmptyRoomController : Controller
    {
        // GET: EmptyRoom
        RoomDao roomDao = new RoomDao();
        TypeDao typeDao = new TypeDao();
        BookingDao bookingDao = new BookingDao();
        public ActionResult Index(string msg)
        {
            List<Room> listroom= TempData["ListRoom"] as List<Room>;
            ViewBag.Msg = msg;
            ViewBag.List = listroom ?? roomDao.GetRooms();
            ViewBag.listType = typeDao.GetTypes();
            return View();
        }
        
        [HttpPost]
        public ActionResult Check(Booking booking)
        {
            string action = "EmptyRoom";
            List<Booking> checkExist = bookingDao.getAll();
            List<Room> listroom= new List<Room>() ;
            DateTime dateCheckout = DateTime.Parse(booking.checkOutDate);
            DateTime dateCheckin = DateTime.Parse(booking.checkInDate);
            TimeSpan time = dateCheckout - dateCheckin;
            int numberBooking = time.Days;
            if (numberBooking <= 0)
            {
                return RedirectToAction(action, new { mess = "Error" });
            }
            //123456789
            //123456789
            else
            {
                foreach (Booking checkbooking in checkExist)
                {
                    

                    if ((dateCheckin <= DateTime.Parse(checkbooking.checkOutDate) && dateCheckin >= DateTime.Parse(checkbooking.checkInDate)) || (dateCheckout <= DateTime.Parse(checkbooking.checkOutDate) && dateCheckout >= DateTime.Parse(checkbooking.checkInDate)) || (dateCheckin <= DateTime.Parse(checkbooking.checkInDate) && dateCheckout >= DateTime.Parse(checkbooking.checkOutDate)))
                    {
                        int idroom = checkbooking.idRoom;
                        Room room = roomDao.GetDetail(idroom);
                        bool roomExists = false;
                        foreach (Room checkroom in listroom)
                        {
                            if (checkroom.idRoom == room.idRoom)
                            {
                                roomExists = true;
                                break;
                            }
                        }
                        if (!roomExists)
                        {
                            listroom.Add(room);
                        }
                    }
                    
                }
                List<Room> listr = roomDao.GetRooms();
                List<Room> listroomtrong = new List<Room>();

                foreach (Room room in listr)
                {
                    bool roomExists = false;
                    foreach (Room checkroom in listroom)
                    {
                        if (checkroom.idRoom == room.idRoom)
                        {
                            roomExists = true;
                            break;
                        }
                    }
                    if (!roomExists)
                    {
                        listroomtrong.Add(room);
                    }
                }
                TempData["ListRoom"] = listroomtrong;
                return RedirectToAction("Index", new { msg = "Success", listroom = listroomtrong});
            }
           
        }

        

    }
}