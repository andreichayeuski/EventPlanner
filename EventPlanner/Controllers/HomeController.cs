using EventPlanner.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EventPlanner.Controllers
{
    public class HomeController : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();
        public ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? this.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Index()
        {
            this.ViewBag.Admin = this.User.IsInRole("Администратор мероприятия");
            this.ViewBag.IsAuthenticated = this.User.Identity.IsAuthenticated;
            this.ViewBag.UserId = this.User.Identity.GetUserId();
            this.ViewBag.Events = this.db.Events;
            List<EventUserRelationship> EventUser = new List<EventUserRelationship>();
            EventUser.AddRange(this.db.EventUserRelationship);
            this.ViewBag.EventUser = EventUser;
            List<Field> fields = new List<Field>();
            fields.AddRange(this.db.Fields);
            this.ViewBag.Fields = fields;  
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Web-приложение Event planning для получения работы в компании Центр банковских технологий.";

            return View();
        }

        [HttpPost]
        public ActionResult SignProjectAsync(string id)
        {
            try
            {
                Event current_event = this.db.Events.Find(Convert.ToInt32(Request.Params[0]));
                var Sign = new EventUserRelationship { EventId = current_event.Id, UserId = this.User.Identity.GetUserId() };
                if (this.db.EventUserRelationship.Any(e => e.EventId == Sign.EventId && e.UserId == Sign.UserId))
                {
                    this.db.EventUserRelationship.Remove(this.db.EventUserRelationship.Find(Sign.EventId, Sign.UserId));
                    if (current_event.SignedUsersCount > 0)
                    {
                        this.db.Events.Find(current_event.Id).SignedUsersCount -= 1;
                        this.db.Events.Find(current_event.Id).EventUsersSigned.Remove(Sign);
                        this.db.SaveChanges();
                    }
                }
                else
                {
                    this.db.EventUserRelationship.Add(Sign);
                    if (!this.db.Events.Any(p => p.EventUsersSigned.Any(f => f.EventId == current_event.Id)))
                    {
                        if (current_event.MaxUsers != current_event.SignedUsersCount)
                        {
                            this.db.Events.Find(current_event.Id).SignedUsersCount += 1;
                            this.db.Events.Find(current_event.Id).EventUsersSigned.Add(Sign);
                            this.db.SaveChanges();
                        }
                    }
                }
                this.ViewBag.Fields = this.db.Fields;
                this.ViewBag.IsAuthenticated = this.User.Identity.IsAuthenticated;
                this.ViewBag.UserId = this.User.Identity.GetUserId();
                List<EventUserRelationship> EventUser = new List<EventUserRelationship>();
                EventUser.AddRange(this.db.EventUserRelationship);
                this.ViewBag.EventUser = EventUser;
                this.ViewBag.Fields = this.db.Fields;

                return PartialView("EventPartialView", current_event);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return PartialView();
            }
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Мои контакты";

            return View();
        }
    }
}