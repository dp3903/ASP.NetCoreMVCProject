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

        public IActionResult Index(string? searchstr)
        {
            if (HttpContext.Session.GetString(SessionUserName) != null)
            {
                IEnumerable<BookModel> books = _bookRepository.GetAll();

                // Check if a search string is provided
                if (!string.IsNullOrEmpty(searchstr))
                {
                    // Filter books where the Title or Author contains the search string (case-insensitive)
                    books = books.Where(b =>
                        b.Title.Contains(searchstr, StringComparison.OrdinalIgnoreCase) ||
                        b.Author.Contains(searchstr, StringComparison.OrdinalIgnoreCase));
                }

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
			if (id <= 0)
			{
				return NotFound();
			}
			BookModel book = _bookRepository.GetById(id);
			if (book == null)
			{
				return NotFound();
			}
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

		[HttpGet]
		public IActionResult CreateBook()
		{
			string role = HttpContext.Session.GetString(SessionUserRole);
			if (role == "Admin" || role == "Author")
			{
				return View();
			}
			// Redirect to Index if the user is a Reader or not logged in
			return RedirectToAction("Index");
		}

		[HttpPost]
		public IActionResult CreateBook(BookModel book)
		{
			string role = HttpContext.Session.GetString(SessionUserRole);
			if (role == "Admin" || role == "Author")
			{
				book = _bookRepository.Add(book);

				// Check if the book's ID is correctly generated
				if (book.Id > 0)
				{
					return RedirectToAction("Details", new { id = book.Id });
				}

				ViewBag.ErrorMessage = "Error adding book. Please try again.";
				return View(book);
			}

			return RedirectToAction("Index");
		}

		[HttpGet]
		public IActionResult EditBook(int id)
		{
			string role = HttpContext.Session.GetString(SessionUserRole);
			if (role == "Admin" || role == "Author")
			{
				BookModel book = _bookRepository.GetById(id);
				if (book == null)
				{
					return NotFound();
				}
				return View(book);
			}
			return RedirectToAction("Index");
		}

		[HttpPost]
		public IActionResult EditBook(BookModel book)
		{
			string role = HttpContext.Session.GetString(SessionUserRole);
			if (role == "Admin" || role == "Author")
			{
				_bookRepository.Update(book);
				return RedirectToAction("Details", new { id = book.Id });
			}
			return RedirectToAction("Index");
		}

		[HttpPost]
		public IActionResult DeleteBook(int id)
		{
			string role = HttpContext.Session.GetString(SessionUserRole);
			if (role == "Admin" || role == "Author")
			{
				BookModel book = _bookRepository.Delete(id);
				if (book == null)
				{
					return NotFound();
				}
				return RedirectToAction("Index");
			}
			return RedirectToAction("Index");
		}

        //User Profile Functionality

        [HttpGet]
        public IActionResult UserProfile()
        {
            int? userId = HttpContext.Session.GetInt32(SessionUserId);
            if (userId.HasValue)
            {
                UserModel user = _userRepository.GetById(userId.Value);
                if (user != null)
                {
                    return View(user);  // Ensure you have a UserProfile view.
                }
            }
            return RedirectToAction("SignIn");
        }

        [HttpGet]
        public IActionResult EditProfile()
        {
            int? userId = HttpContext.Session.GetInt32(SessionUserId);
            if (userId.HasValue)
            {
                UserModel user = _userRepository.GetById(userId.Value);
                if (user != null)
                {
                    return View(user);
                }
            }
            return RedirectToAction("SignIn");
        }

        [HttpPost]
        public IActionResult EditProfile(UserModel model)
        {
            if (ModelState.IsValid)
            {
                UserModel currentUser = _userRepository.GetById(model.Id); // Fetch current user by ID

                if (currentUser != null)
                {
                    // Update password only if the user enters a new one, otherwise keep the old one
                    if (!string.IsNullOrEmpty(model.UserName) && !string.IsNullOrEmpty(model.Password))
                    {
                        currentUser.UserName = model.UserName;
                        currentUser.Password = model.Password; // Update password
                    }

                    _userRepository.Update(currentUser);    // Update user in the database
                    return RedirectToAction("UserProfile"); // Redirect to profile page after editing
                }
            }

            // Return the view with the model if validation fails or user not found
            return View(model);
        }

        [HttpPost]
        public IActionResult DeleteProfile()
        {
            int? userId = HttpContext.Session.GetInt32(SessionUserId);
            if (userId.HasValue)
            {
                _userRepository.Delete(userId.Value);
                HttpContext.Session.Clear();
                return RedirectToAction("SignIn");
            }
            return RedirectToAction("Index");
        }
    }
}
