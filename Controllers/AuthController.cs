using Login_RegisterFormSession.DAL;
using Login_RegisterFormSession.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Login_RegisterFormSession.Controllers
{
    public class AuthController : Controller
    {
        private UserDAL _dal;
        public AuthController()
        {
            _dal = new UserDAL();
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
        [HttpPost]

        public IActionResult Register(User user)
        {
            if (user.email == null || user.password == null || user.username == null)
            {
                return View();
            }

            bool result = _dal.CreateUser(user);

            if (!result)
            {
                return View();
            }
            else
            {
                TempData["UserCreated"] = "User successfully created";
                return RedirectToAction("Login");
            }

        }

        [HttpGet]
        public IActionResult Login()
        {

            if (HttpContext.Session.GetString("UserSession") != null)
            {
                return RedirectToAction("Index", "Home");
            }


            return View();
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            if (user.email == null || user.password == null)
            {
                return View();
            }

            var userFromDB = _dal.GetUserByEmail(user.email);

            if (userFromDB != null && userFromDB.id > 0 && userFromDB.password == user.password)
            {
                UserSession userSession = new UserSession() { id = userFromDB.id, email = userFromDB.email };
                var userSessionJson = JsonConvert.SerializeObject(userSession);

                HttpContext.Session.SetString("UserSession", userSessionJson);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["UserNotFound"] = "Invalid Credentials";
                return View();
            }
        }
        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                HttpContext.Session.Remove("UserSession");
                return RedirectToAction("Login");
            }
            return View();
        }
    }
}
