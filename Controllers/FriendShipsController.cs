using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UsersManager.Models;

namespace UsersManager.Controllers
{
    public class FriendShipsController : Controller
    {
        public UsersDBEntities DB = new UsersDBEntities();

        // GET: FriendShips
        public ActionResult Index()
        {
            return View(DB.FriendShips);
        }

        public bool IsFriendShipStatusUpToDate()
        {
            return ((string)Session["FriendShipStatusSerialNumber"] == (string)HttpRuntime.Cache["FriendShipStatusSerialNumber"]);
        }
        public void RenewFriendShipStatusSerialNumber()
        {
            HttpRuntime.Cache["FriendShipStatusSerialNumber"] = Guid.NewGuid().ToString();
        }

        public string GetFriendShipStatusSerialNumber()
        {
            if (HttpRuntime.Cache["FriendShipStatusSerialNumber"] == null)
            {
                RenewFriendShipStatusSerialNumber();
            }
            return (string)HttpRuntime.Cache["FriendShipStatusSerialNumber"];
        }

        public void SetLocalFriendShipStatusSerialNumber()
        {
            Session["FriendShipStatusSerialNumber"] = GetFriendShipStatusSerialNumber();
        }


        public PartialViewResult GetFriendShipsStatus(bool forceRefresh=false)
        {
            if (forceRefresh || !IsFriendShipStatusUpToDate())
            {
                SetLocalFriendShipStatusSerialNumber();
                return PartialView(UsersDBDAL.FriendShipsStatus(DB, OnlineUsers.CurrentUserId));
            }
            return null;
        }
        public ActionResult SendFriendShipRequest(int id)
        {
            UsersDBDAL.Add_FiendShipRequest(DB, OnlineUsers.CurrentUserId, id);
            RenewFriendShipStatusSerialNumber();
            return View();
        }
        public ActionResult RemoveFriendShipRequest(int id)
        {
            UsersDBDAL.Remove_FiendShipRequest(DB, OnlineUsers.CurrentUserId, id);
            RenewFriendShipStatusSerialNumber();
            return View();
        }
        public ActionResult AcceptFriendShipRequest(int id)
        {
            UsersDBDAL.Accept_FriendShip(DB, OnlineUsers.CurrentUserId, id);
            RenewFriendShipStatusSerialNumber();
            return View();
        }
        public ActionResult DeclineFriendShipRequest(int id)
        {
            UsersDBDAL.Decline_FriendShip(DB, OnlineUsers.CurrentUserId, id);
            RenewFriendShipStatusSerialNumber();
            return View();
        }
    }
}