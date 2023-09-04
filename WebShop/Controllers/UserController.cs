using DomainLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer;
using ServiceLayer.Property.UserProfileService;
using ServiceLayer.Property.UserService;
using System.Data;
using TestOnion.Models;

namespace WebShop.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly IUserProfileService userProfileService;
        private ApplicationContext context;

        public UserController(IUserService userService, IUserProfileService userProfileService)
        {
            this.userService = userService;
            this.userProfileService = userProfileService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Moder,User")]
        public IActionResult Index()
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

            return View(model);
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
