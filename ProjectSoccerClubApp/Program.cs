using ProjectSoccerClubApp.Database_Helper;
using ProjectSoccerClubApp.Repositories;
using ProjectSoccerClubApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DatabaseContext>();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

// Add DI
builder.Services.AddScoped<IMatchService, MatchServiceImp>();
builder.Services.AddScoped<ITeamServices, TeamServicesImp>();
builder.Services.AddScoped<IAuthenService, AuthenServiceImp>();
builder.Services.AddScoped<IPlayerServices, PlayerServicesImp>();
builder.Services.AddScoped<ICreateAcountService, CreateAccountServiceImp>();
builder.Services.AddScoped<ICategoryService, CategoryServiceImp>();
builder.Services.AddScoped<IProductService, ProductServiceImp>();
builder.Services.AddScoped<IOrderService, OrderServiceImp>();
builder.Services.AddScoped<ICompetitionService, CompetitionServiceImp>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Frontend/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Frontend}/{action=Index}/{id?}");

app.Run();
