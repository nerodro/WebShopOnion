using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer;
using RepositoryLayer.Infrascructure.Cart;
using RepositoryLayer.Infrascructure.Company;
using RepositoryLayer.Infrascructure.Login;
using RepositoryLayer.Infrascructure.Products;
using RepositoryLayer.Infrascructure.Registration;
using RepositoryLayer.Infrascructure.User;
using ServiceLayer.Property.CartService;
using ServiceLayer.Property.CompanyService;
using ServiceLayer.Property.LoginService;
using ServiceLayer.Property.ProductServce;
using ServiceLayer.Property.RegisterService;
using ServiceLayer.Property.UserProfileService;
using ServiceLayer.Property.UserService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

string connection = builder.Configuration.GetConnectionString("DefaultConnection");

// добавляем контекст ApplicationContext в качестве сервиса в приложение
builder.Services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(connection));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
              .AddCookie(options =>
              {
                  options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                  options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
              });

builder.Services.AddScoped(typeof(IUserLogic<>), typeof(UserLogic<>));
builder.Services.AddScoped(typeof(IRegistration<>), typeof(RegistrationLogic<>));
builder.Services.AddScoped(typeof(ILogin<>), typeof(LoginLogic<>));
builder.Services.AddScoped(typeof(ICompany<>), typeof(CompanyLogic<>));
builder.Services.AddScoped(typeof(IProducts<>), typeof(ProductLogic<>));
builder.Services.AddScoped(typeof(ICart<>), typeof(CartLogic<>));
builder.Services.AddTransient<IUserProfileService, UserProfileService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ILoginService, LoginService>();
builder.Services.AddTransient<IRegistrationService, RegistrationService>();
builder.Services.AddTransient<ICompanyService, CompanyService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<ICartService, CartService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
//app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
