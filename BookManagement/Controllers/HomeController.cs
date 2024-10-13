using Microsoft.AspNetCore.Mvc;
using BookManagement.Models;
using System.Diagnostics;

namespace BookManagement.Controllers
{
    public class HomeController : Controller
    {
        const string SessionUserName = "_username";
        const string SessionUserId = "_id";
        const string SessionUserRole = "_role";
        private readonly IUserRepository _userRepository;
        private readonly IBookRepository _bookRepository;

        public HomeController(IUserRepository userRepository, IBookRepository bookRepository)
        {
            _userRepository = userRepository;
            _bookRepository = bookRepository;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString(SessionUserName) != null)
            {
                IEnumerable<BookModel> books = _bookRepository.GetAll();
                ViewBag.username = HttpContext.Session.GetString(SessionUserName);
                ViewData["role"] = HttpContext.Session.GetString(SessionUserRole);
                return View(books);
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
                    HttpContext.Session.SetString(SessionUserRole, user.Role.ToString());
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

        [HttpGet]
        public IActionResult Details(int id)
        {
            BookModel book = _bookRepository.GetById(id);
            return View(book);
        }

        [HttpGet]
        public IActionResult AdminIndex()
        {
            if (HttpContext.Session.GetString(SessionUserRole) == "Admin")
            {
                IEnumerable<UserModel> users = _userRepository.GetAll();
                return View(users);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DemoteToReader(int id)
        {
            UserModel user = _userRepository.GetById(id);
            if (user == null || user.Role != Roles.Author)
            {
                return RedirectToAction("AdminIndex");
            }
            user.Role = Roles.Reader;
            _userRepository.Update(user);
            return RedirectToAction("AdminIndex");
        }

        [HttpPost]
        public IActionResult PromoteToAuthor(int id)
        {
            UserModel user = _userRepository.GetById(id);
            if (user == null || user.Role != Roles.Reader)
            {
                return RedirectToAction("AdminIndex");
            }
            user.Role = Roles.Author;
            _userRepository.Update(user);
            return RedirectToAction("AdminIndex");
        }
    }
}
