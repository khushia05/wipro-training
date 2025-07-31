var builder = WebApplication.CreateBuilder(args);
//Adding MVC
builder.Services.AddControllersWithViews();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");// Minimal API endpoint
//Ex1: Returning a list ( hard coded)
//here is am testing end points 
app.MapGet("/Mobilephones", () => new List<string> { "Samsung", "Oneplus", "Apple", "Xiaomi" });
//Ex2: Dyanmic List(Query parameters)
app.MapGet("/RepeatNames", (string name, int count) => Enumerable.Repeat(name, count).ToList());
//Adding MVC Route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
//Admin/Login
//Guest/Welcome

app.Run();

//Step 1: Run this template as it is
//Step 2: Need to add MVC(HTML Page)
// wE WILL BE ADDING MVC
// Create a controller 
//Creating a View 
// Running your App 