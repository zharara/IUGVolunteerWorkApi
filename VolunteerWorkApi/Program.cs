global using VolunteerWorkApi.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using VolunteerWorkApi.Constants;
using VolunteerWorkApi.Data;
using VolunteerWorkApi.Helpers.ErrorHandling;
using VolunteerWorkApi.Services.Announcements;
using VolunteerWorkApi.Services.Categories;
using VolunteerWorkApi.Services.Conversations;
using VolunteerWorkApi.Services.FCMNotifications;
using VolunteerWorkApi.Services.Interests;
using VolunteerWorkApi.Services.Jwts;
using VolunteerWorkApi.Services.ManagementEmployees;
using VolunteerWorkApi.Services.Messages;
using VolunteerWorkApi.Services.Notifications;
using VolunteerWorkApi.Services.Organizations;
using VolunteerWorkApi.Services.SavedFiles;
using VolunteerWorkApi.Services.Skills;
using VolunteerWorkApi.Services.StaticFiles;
using VolunteerWorkApi.Services.StudentApplications;
using VolunteerWorkApi.Services.Students;
using VolunteerWorkApi.Services.TempFiles;
using VolunteerWorkApi.Services.Users;
using VolunteerWorkApi.Services.VolunteerOpportunities;
using VolunteerWorkApi.Services.VolunteerProgramActivities;
using VolunteerWorkApi.Services.VolunteerProgramGalleryPhotos;
using VolunteerWorkApi.Services.VolunteerPrograms;
using VolunteerWorkApi.Services.VolunteerProgramTasks;
using VolunteerWorkApi.Services.VolunteerProgramWorkDays;
using VolunteerWorkApi.Services.VolunteerStudentActivityAttendances;
using VolunteerWorkApi.Services.VolunteerStudents;
using VolunteerWorkApi.Services.VolunteerStudentTaskAccomplishes;
using VolunteerWorkApi.Services.VolunteerStudentWorkAttendances;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
    {
        //options.Filters.Add<HttpResponseExceptionFilter>();
    })
    .AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration?.GetSection("Jwt:Key").Value!)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen(option =>
{

    option.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "VolunteerWorkAPI",
            Version = "v1"
        });

    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Please enter a valid token",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });

    option.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme,
                    }
                },
                Array.Empty<string>()
            }
        });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(
   builder.Configuration.GetConnectionString("DefaultConnectionString")));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services
    .AddIdentityCore<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.SignIn.RequireConfirmedEmail = false;
        options.User.RequireUniqueEmail = false;
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
    }).AddRoles<IdentityRole<long>>()
    .AddDefaultTokenProviders()
    .AddErrorDescriber<ArabicIdentityErrorDescriber>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddTransient<ApiErrorHandlingMiddleware>();

// Adding services
builder.Services.AddScoped<IAnnouncementService, AnnouncementService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IConversationService, ConversationService>();
builder.Services.AddScoped<IFCMNotificationsService, FCMNotificationsService>();
builder.Services.AddScoped<IInterestService, InterestService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IManagementEmployeeService, ManagementEmployeeService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IOrganizationService, OrganizationService>();
builder.Services.AddScoped<ISavedFileService, SavedFileService>();
builder.Services.AddScoped<ISkillService, SkillService>();
builder.Services.AddScoped<IStaticFilesService, StaticFilesService>();
builder.Services.AddScoped<IStudentApplicationService, StudentApplicationService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ITempFileService, TempFileService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IVolunteerOpportunityService, VolunteerOpportunityService>();
builder.Services.AddScoped<IVolunteerProgramActivityService, VolunteerProgramActivityService>();
builder.Services.AddScoped<IVolunteerProgramGalleryPhotoService, VolunteerProgramGalleryPhotoService>();
builder.Services.AddScoped<IVolunteerProgramService, VolunteerProgramService>();
builder.Services.AddScoped<IVolunteerProgramTaskService, VolunteerProgramTaskService>();
builder.Services.AddScoped<IVolunteerProgramWorkDayService, VolunteerProgramWorkDayService>();
builder.Services.AddScoped<IVolunteerStudentActivityAttendanceService, VolunteerStudentActivityAttendanceService>();
builder.Services.AddScoped<IVolunteerStudentService, VolunteerStudentService>();
builder.Services.AddScoped<IVolunteerStudentTaskAccomplishService, VolunteerStudentTaskAccomplishService>();
builder.Services.AddScoped<IVolunteerStudentWorkAttendanceService, VolunteerStudentWorkAttendanceService>();

builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
});

var app = builder.Build();

app.UseMiddleware<ApiErrorHandlingMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var usersService = scope.ServiceProvider
        .GetRequiredService<IUsersService>();

    await usersService.SeedDefaultUser();
}

//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI(options => options.EnablePersistAuthorization());
//}

app.UseHttpsRedirection();

app.UseAuthorization();

var usersFilesFolder = Path.Combine(
    Directory.GetCurrentDirectory(), FoldersNames.UsersFiles);

var savedFilesPath = Path.Combine(
    usersFilesFolder, FoldersNames.SavedFiles);

if (!Directory.Exists(savedFilesPath))
    Directory.CreateDirectory(savedFilesPath);

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(savedFilesPath),
    RequestPath = new PathString("/files"),

    OnPrepareResponse = ctx =>
    {
        if (!(ctx.Context.User.Identity?.IsAuthenticated ?? false))
        {
            ctx.Context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            ctx.Context.Response.ContentLength = 0;
            ctx.Context.Response.Body = Stream.Null;
        }
    }
});

app.MapControllers();

app.Run();