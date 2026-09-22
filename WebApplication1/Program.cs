using WebApplication1.Services;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IMachineService, MachineService>();//kull meta ha nsaqsi al ogett li jimxi b IMachineService, tini tip MachineService.cs

builder.Services.AddDbContext<AppDbContext>(options =>//"id db context li ha nuzaw hu l appdbcontext li bnejna"
    options.UseSqlServer(//meta qed tikkrea appdbcontext, ghamlu li juza sql server
        builder.Configuration.GetConnectionString("DefaultConnection")//liema sql server? idhol fil config u hu l connection string
    ));
//This also means that later something like MachineService can simply ask for:
//MachineService(AppDbContext context)
//and dependency injection can provide a correctly configured AppDbContext.


//wara ridna naghmlu l ewwel migration, bazikament tghid, isma, mi c# model li qed nara (appdbcontext bil machine objects), id database ghanda tidher hekk, jekk imbaghad 
//perezempju inzidu field iehor ghal machine, "Time Running", u nergu namlu migration, ha tinbidel u tghid, orrajt, issa irrid naghmel alter database u nzid
//variable al time running fid database.

//imbaghad ghamilna update-database, hawnhekk l entity framework qed taqbad mal sql u ha taghmel dak li pjanat il migration




var app = builder.Build();

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
