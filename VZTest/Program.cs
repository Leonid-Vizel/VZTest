using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VZTest.Data;
using VZTest.Data.IRepository;
using VZTest.Data.Repository;
using VZTest.Instruments;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddMvcOptions(options =>
{
    options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(_ => "Неверное значение");
    options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(_ => "Неверное значение");
    options.ModelBindingMessageProvider.SetValueIsInvalidAccessor(_ => "Неверное значение");
});
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

IServiceScope scope = app.Services.CreateScope();
TestTimerChecker checker = new TestTimerChecker(scope.ServiceProvider.GetService<IUnitOfWork>());

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
