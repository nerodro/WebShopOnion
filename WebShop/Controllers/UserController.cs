using DomainLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer;
using ServiceLayer.Property.Category;
using ServiceLayer.Property.UserProfileService;
using ServiceLayer.Property.UserService;
using System.Data;
using TestOnion.Models;
using WebShop.Models;

namespace WebShop.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly IUserProfileService userProfileService;
        private readonly ICategoryService categoryService;
        private ApplicationContext context;

        public UserController(IUserService userService, IUserProfileService userProfileService, ICategoryService category)
        {
            this.userService = userService;
            this.userProfileService = userProfileService;
            this.categoryService = category;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Moder,User")]
        public async Task<IActionResult> Index()
        {
            List<UserViewModel> model = new List<UserViewModel>();
            if (userProfileService != null)
            {
                userService.GetUsers().ToList().ForEach(u =>
                {
                    UserProfile userProfile = userProfileService.GetUserProfile(u.Id);
                    UserViewModel user = new UserViewModel
                    {
                        Id = u.Id,
                        Name = $"{userProfile.FirstName} {userProfile.LastName}",
                        Email = u.Email,
                        Address = userProfile.Address
                    };
                    model.Add(user);
                });
            }
            List<CategoryViewModel> categories = new List<CategoryViewModel>();
            if (categoryService != null)
            {
                categoryService.GetAll().ToList().ForEach(f =>
                {
                    Category category = categoryService.Get(f.Id);
                    CategoryViewModel viewModel = new CategoryViewModel
                    {
                        Id = f.Id,
                        CategoryName = f.CategoryName
                    };
                    categories.Add(viewModel);
                });
            }
            //ViewData["categories"] = categories;
            MultiViewModel multi = new MultiViewModel();
            multi.UserViewModels = model;
            multi.CategoryViewModels = categories;
            //var tuple = new Tuple<List<UserViewModel>, List<CategoryViewModel>>((List<UserViewModel>)userService.GetUsers(), (List<CategoryViewModel>)categoryService.GetAll());
            return View(multi);
        }

        [HttpGet]
        public ActionResult AddUser()
        {
            UserViewModel model = new UserViewModel();

            return View("AddUser", model);
        }

        [HttpPost]
        public ActionResult AddUser(UserViewModel model)
        {
            User userEntity = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                Password = model.Password,
                AddedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                RoleId = model.RoleId,
                //IPAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                UserProfile = new UserProfile
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address = model.Address,
                    AddedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    //RoleId = model.RoleId,
                    //IPAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString()
                }
            };
            userService.CreateUser(userEntity);
            if (userEntity.Id > 0)
            {
                return RedirectToAction("index");
            }
            return View(model);
        }

        public ActionResult EditUser(int? id)
        {
            UserViewModel model = new UserViewModel();
            if (id.HasValue && id != 0)
            {
                User userEntity = userService.GetUser(id.Value);
                UserProfile userProfileEntity = userProfileService.GetUserProfile(id.Value);
                model.FirstName = userProfileEntity.FirstName;
                model.LastName = userProfileEntity.LastName;
                model.Address = userProfileEntity.Address;
                model.Email = userEntity.Email;
            }
            return View("EditUser", model);
        }

        [HttpPost]
        public ActionResult EditUser(UserViewModel model)
        {
            User userEntity = userService.GetUser(model.Id);
            userEntity.Email = model.Email;
            userEntity.ModifiedDate = DateTime.UtcNow;
           // userEntity.IPAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            UserProfile userProfileEntity = userProfileService.GetUserProfile(model.Id);
            userProfileEntity.FirstName = model.FirstName;
            userProfileEntity.LastName = model.LastName;
            userProfileEntity.Address = model.Address;
            userProfileEntity.ModifiedDate = DateTime.UtcNow;
            //userProfileEntity.IPAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            userEntity.UserProfile = userProfileEntity;
            userService.UpdateUser(userEntity);
            if (userEntity.Id > 0)
            {
                return RedirectToAction("index");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult DeleteUser(int id)
        {
            UserProfile userProfile = userProfileService.GetUserProfile(id);
            string name = $"{userProfile.FirstName} {userProfile.LastName}";
            return View("DeleteUser", name);
        }

        [HttpPost]
        public ActionResult DeleteUser(long id)
        {
            userService.DeleteUser(id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult UserProfile(int? id)
        {
            UserViewModel model = new UserViewModel();
            if (id.HasValue && id != 0)
            {
                User userEntity = userService.GetUser(id.Value);
                UserProfile userProfileEntity = userProfileService.GetUserProfile(id.Value);
                model.FirstName = userProfileEntity.FirstName;
                model.LastName = userProfileEntity.LastName;
                model.Address = userProfileEntity.Address;
                model.Email = userEntity.Email;
            }
            return View("UserProfile", model);
        }
        [HttpPost]
        public ActionResult UserProfile(UserViewModel model)
        {
            User userEntity = userService.GetUser(model.Id);
            userEntity.Email = model.Email;
            userEntity.ModifiedDate = DateTime.UtcNow;
            // userEntity.IPAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            UserProfile userProfileEntity = userProfileService.GetUserProfile(model.Id);
            userProfileEntity.FirstName = model.FirstName;
            userProfileEntity.LastName = model.LastName;
            userProfileEntity.Address = model.Address;
            userProfileEntity.ModifiedDate = DateTime.UtcNow;
            //userProfileEntity.IPAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            userEntity.UserProfile = userProfileEntity;
            return View(model);
        }

    }
}
