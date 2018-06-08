using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using EventPlanner.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace EventPlanner.Controllers
{
    public class EventController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private RoleManager<IdentityRole> _roleManager;
        public ApplicationDbContext db = new ApplicationDbContext();
        public ApplicationUser CurrentUser { get; set; }

        public EventController()
        {
        }

        public EventController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public RoleManager<IdentityRole> RoleManager
        {
            get
            {
                return _roleManager ?? new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            }
            private set
            {
                _roleManager = value;
            }
        }

        [Authorize(Roles = "Администратор мероприятия")]
        // GET: Event
        [HttpGet]
        public ActionResult Index()
        {
            List<EventUserRelationship> EventUser = new List<EventUserRelationship>();
            EventUser.AddRange(this.db.EventUserRelationship);
            this.ViewBag.EventUser = EventUser;
            List<Field> fields = new List<Field>();
            fields.AddRange(this.db.Fields);
            this.ViewBag.Fields = fields;
            List<Event> events = new List<Event>();
            ApplicationUser cur_user = this.db.Users.Find(this.User.Identity.GetUserId());
            foreach (Event e in this.db.Events)
            {
                if (cur_user.CreatedEvents.Contains(e))
                {
                    events.Add(e);
                }
            }
            this.ViewBag.Admin = this.User.IsInRole("Администратор мероприятия");
            this.ViewBag.IsAuthenticated = false;
            this.ViewBag.UserId = this.User.Identity.GetUserId();
            this.ViewBag.Events = events;
            return View();
        }
        
        [HttpGet]
        // GET: Event/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        [HttpGet]
        // GET: Event/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Event/Create
        [HttpPost]
        [Authorize(Roles = "Администратор мероприятия")]
        public ActionResult Create(Event model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Event res_event = new Event { Title = model.Title, MaxUsers = model.MaxUsers, SignedUsersCount = model.SignedUsersCount, Fields = model.Fields };
                    List<Field> fields = model.Fields.ToList();
                    foreach (Field item in fields)
                    {
                       
                        item.Event = res_event;
                        res_event.Fields.Add(item);
                    }

                    //res_event.UserId = this.User.Identity.GetUserId();
                    this.db.Events.Add(res_event);
                    ApplicationUser cur_user = this.db.Users.Find(this.User.Identity.GetUserId());
                    cur_user.CreatedEvents.Add(res_event);
                    this.db.SaveChanges();
                }

                // Появление этого сообщения означает наличие ошибки; повторное отображение формы
                //return View(model);
                return RedirectToAction("IndexAdmin");
            }
            catch (Exception ex)
            {
                ViewBag.Title = ex.Message;
                return RedirectToAction("IndexAdmin");
            }
        }
        [HttpGet]
        // GET: Event/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Event/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Event/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Event/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
