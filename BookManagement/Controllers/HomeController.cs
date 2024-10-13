using Microsoft.AspNetCore.Mvc;
using BookManagement.Models;
using System.Diagnostics;

namespace BookManagement.Controllers
{
    public class HomeController : Controller
    {
        const string SessionUserName = "_username";
        const string SessionUserId = "_id";
        private readonly IUserRepository _userRepository;

        public HomeController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString(SessionUserName) != null)
            {
                ViewBag.username = HttpContext.Session.GetString(SessionUserName);
                return View();
            }
            return RedirectToAction("SignIn");
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignIn(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserModel user = _userRepository.GetByUserNameAndPassword(model.UserName, model.Password);
                if(user != null)
                {
                    HttpContext.Session.SetString(SessionUserName, user.UserName);
                    HttpContext.Session.SetInt32(SessionUserId, user.Id);
                    return RedirectToAction("Index");
                }
                ViewBag.errormsg = "Invalid user information. no such user exists.";
                return View(model);
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(SignUpUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserModel user = _userRepository.GetByUserNameAndPassword(model.UserName, model.Password);
                if (user == null)
                {
                    user = new UserModel();
                    user.UserName = model.UserName;
                    user.Password = model.Password;
                    user.Role = Roles.Reader;
                    user = _userRepository.Add(user);
                    HttpContext.Session.SetString(SessionUserName, user.UserName);
                    HttpContext.Session.SetInt32(SessionUserId, user.Id);
                    return RedirectToAction("Index");
                }
                ViewBag.errormsg = "User already exists. try changing username.";
                return View(model);
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult SignOut()
        {
            HttpContext.Session.Remove(SessionUserName);
            HttpContext.Session.Remove(SessionUserId);

            return RedirectToAction("Index");
        }
    }
}
