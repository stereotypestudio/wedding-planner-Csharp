using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LoginReg.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LoginReg.Controllers
{
    public class HomeController : Controller
    {

        private MainContext _context;

        public HomeController(MainContext context)
        {
        _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("createuser")]
        public IActionResult Register(TestUser validUser){

            if(ModelState.IsValid)
            { 
                List<User> getUsers = _context.user.Where(u => u.Email == validUser.Email).ToList();
                if (getUsers.Count != 0) {
                    ViewBag.Message = "Email already exists!";
                    return View("Index");
                }
                else{
                    PasswordHasher<TestUser> Hasher = new PasswordHasher<TestUser>();
                    validUser.Password = Hasher.HashPassword(validUser, validUser.Password);

                    User newUser = new User();
                    newUser.Firstname = validUser.Firstname;
                    newUser.Lastname = validUser.Lastname;
                    newUser.Email = validUser.Email;
                    newUser.Password = validUser.Password;

                    _context.user.Add(newUser);
                    _context.SaveChanges();
                    ViewBag.Message = "Successfully registered! Please log in.";
                    return View("Index");
                }
            } else {
                return View("Index");
            }   
        }
        [HttpPost("loginuser")]
          public IActionResult LoginMethod(TestUser loginUser)
        {
            // Attempt to retrieve a user from your database based on the Email submitted
            // var user = userFactory.GetUserByEmail(Email);
            List<User> getUsers = _context.user.Where(u => u.Email == loginUser.Email).ToList();
            User user = getUsers[0];
            if(user != null && loginUser.Password != null)
            {
                var Hasher = new PasswordHasher<User>();
                // Pass the user object, the hashed password, and the PasswordToCheck
                if(0 != Hasher.VerifyHashedPassword(user, user.Password, loginUser.Password))
                {
                    HttpContext.Session.SetInt32("UserId", user.UserId);
                   return RedirectToAction("Homepage"); //Handle success
                }
            }
            //Handle failure
            return View();
        }
        [HttpGet]
        [Route("homepage")]
        public IActionResult Homepage(){
            int? IntVariable = HttpContext.Session.GetInt32("UserId");
            if (IntVariable != 0){
                ViewBag.UserId = (int)IntVariable;
                List<Wedding> AllWeddings = _context.wedding.Include(w => w.RsvpList).ThenInclude(u => u.User).ToList();
                return View(AllWeddings);
            } else {
                ViewBag.Message = "Please login";
                return View("Index");
            }
        }

        [HttpGet]
        [Route("newwedding")]
        public IActionResult NewWedding(){
            int? IntVariable = HttpContext.Session.GetInt32("UserId");
            if (IntVariable != 0){
                return View("NewWedding");
            } else {
                ViewBag.Message = "Please login";
                return View("Index");
            }
        }

        [HttpGet]
        [Route("addrsvp/{weddingid}")]
        public IActionResult AddRsvp(int weddingid){
            int? IntVariable = HttpContext.Session.GetInt32("UserId");
            if (IntVariable != 0){
                Rsvp NewRsvp = new Rsvp();
                NewRsvp.UserId = (int)IntVariable;
                NewRsvp.WeddingId = weddingid;
                _context.rsvp.Add(NewRsvp);
                _context.SaveChanges();
                return RedirectToAction("Homepage");
            } else {
                ViewBag.Message = "Please login";
                return View("Index");
            }
        }
        [HttpGet]
        [Route("removersvp/{weddingid}")]
        public IActionResult RemoveRsvp(int weddingid){
            int? IntVariable = HttpContext.Session.GetInt32("UserId");
            if (IntVariable != 0){
                Rsvp CurrRsvp = _context.rsvp.Where(r => r.UserId == IntVariable).Where(r => r.WeddingId == weddingid).SingleOrDefault();
                
                _context.rsvp.Remove(CurrRsvp);
                _context.SaveChanges();
                return RedirectToAction("Homepage");
            } else {
                ViewBag.Message = "Please login";
                return View("Index");
            }
        }

        [HttpPost("createwedding")]
        public IActionResult CreateWedding(Wedding newWed) {
            int? IntVariable = HttpContext.Session.GetInt32("UserId");
            if (IntVariable != 0) {
                newWed.CreatorId = (int)IntVariable;
                _context.wedding.Add(newWed);
                _context.SaveChanges();
                return RedirectToAction("Homepage");
            } else {
                ViewBag.Message = "Please login";
                return View("Index");
            }     
        }

        [HttpGet]
        [Route("wedding/{weddingid}")]
        public IActionResult ViewWedding(int weddingid){
            Wedding CurrWedding = _context.wedding.Include(w => w.RsvpList).ThenInclude(u => u.User).SingleOrDefault(w => w.WeddingId == weddingid);

            return View("Wedding", CurrWedding);
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout(){
            HttpContext.Session.SetInt32("UserId", 0);
            ViewBag.Message = "Thanks! Come back soon!";
            return View("Index");
        }


    }
}
