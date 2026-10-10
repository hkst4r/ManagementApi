using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Hubs;
using WebApplication1.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSignalR();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IMachineService, MachineService>();//kull meta ha nsaqsi al ogett li jimxi b IMachineService, tini tip MachineService.cs

builder.Services.AddDbContext<AppDbContext>(options =>//"id db context li ha nuzaw hu l appdbcontext li bnejna"
    options.UseSqlServer(//meta qed tikkrea appdbcontext, ghamlu li juza sql server
        builder.Configuration.GetConnectionString("DefaultConnection")//liema sql server? idhol f appsettings u hu l connection string
    ));
//This also means that later something like MachineService can simply ask for:
//MachineService(AppDbContext context)
//and dependency injection can provide a correctly configured AppDbContext.


//wara ridna naghmlu l ewwel migration, bazikament tghid, isma, mi c# model li qed nara (appdbcontext bil machine objects), id database ghanda tidher hekk, jekk imbaghad 
//perezempju inzidu field iehor ghal machine, "Time Running", u nergu namlu migration, ha tinbidel u tghid, orrajt, issa irrid naghmel alter database u nzid
//variable al time running fid database.

//imbaghad ghamilna update-database, hawnhekk l entity framework qed taqbad mal sql u ha taghmel dak li pjanat il migration



builder.Services.AddProblemDetails();//exception handling; addproblemdetails() registers services that can generate standardized HTTP error responses.
var app = builder.Build();

app.MapHub<MachineHub>("/machineHub");//the application now has 2 endpoints, REST Controller and SignalR Hub

///machineHub is not a normal REST endpoint. You don't test it by opening that URL in Swagger.
//SignalR clients use a specific protocol to establish a connection and exchange message

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseExceptionHandler();//adds middleware to the HTTP request pipeline that can catch unhandled exceptions thrown by components later in the pipeline.

                          //exception handling: jekk ezempju s service jibat unexpected exception, l exception handling
                          //middleware jaqbad l exception bl informazzjoni li rridu, (exception jaf ikun fiha informazzjoni sensittiva, ez. database details etc)
                          //allura niddeciedu x ha nuru mil error depending if its the client or developer, client kull mghandu jkun jaf li falliet xi haga, developer
                          //irid ikun jaf x'falla u fejn biex jirranga.
app.UseStaticFiles();//llows ASP.NET Core to serve files from wwwroot so we can test signalr
app.MapControllers();

app.Run();

public partial class Program { };//extending the auto-generated Program and making it accessible to the test project.
