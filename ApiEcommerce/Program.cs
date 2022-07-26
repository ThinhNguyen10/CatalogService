using ApiEcommerce.DbCommerce;
using ApiEcommerce.Middleware;
using ApiEcommerce.Repository;
using ApiEcommerce.Repository.Contract;
using ApiEcommerce.Service;
using ApiEcommerce.Service.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Add automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Add repository layer
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

//Inject Service Layer
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IUriService>(o =>
{
    var accessor = o.GetRequiredService<IHttpContextAccessor>();
    var request = accessor.HttpContext.Request;
    var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
    return new UriService(uri);
});


// add controller and dbcontext

builder.Services.AddControllers();
builder.Services.AddDbContext<ECOMMERCEContext>(options => options.UseSqlServer(@"Data Source=DESKTOP-D33VGG8\SQLEXPRESS;Initial Catalog=ECOMMERCE;Integrated Security=True"));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseAuthenticationMiddleware();
app.Use(async (context,next) =>
{
    using (var client = new HttpClient())
    {

        var isContainToken = context.Request.Headers.TryGetValue("Authorization", out var value);
        if (isContainToken)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                    value.ToString().Split(' ')[1]);

            string url = "https://localhost:7091/Token";
            var result = await client.PostAsync(url, new StringContent("", Encoding.UTF8, "application/json"));
            var content = await result.Content.ReadAsStringAsync();

            JObject json = JObject.Parse(content);

            if (json.Value<string>("success") != "True")
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsJsonAsync(new { message = json.Value<string>("success") });
            }
            else
            {
                await next();
            }
        }
        else
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(new { message = "Header request not include Token" });
        }
    }
});




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
