using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagerQAAPICORE.Identity;
using TaskManagerQAAPICORE.ServiceContracts;
using TaskManagerQAAPICORE.Services;

namespace TaskManagerQAAPICORE
{
    public class Startup
    {
    [Obsolete]
        public Startup(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            Configuration = configuration;
      var builder = new ConfigurationBuilder().SetBasePath(env.ContentRootPath).AddJsonFile("appsettings.json");
      Configuration = builder.Build();
    }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
          

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TaskManagerQAAPICORE", Version = "v1" });
            });
      //services.AddMvc();
      services.AddMvc(options => options.EnableEndpointRouting = false);
      //services.AddEntityFrameworkSqlServer();
      services.AddEntityFrameworkSqlServer().AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("TaskManagerQAAPICORE")));

      //Models And Interfaces

      services.AddTransient<IRoleStore<ApplicationRole>, ApplicationRoleStore>();
      services.AddTransient<UserManager<ApplicationUser>, ApplicationUserManager>();
      services.AddTransient<SignInManager<ApplicationUser>, ApplicationSignInManager>();
      services.AddTransient<RoleManager<ApplicationRole>, ApplicationRoleManager>();
      services.AddTransient<IUserStore<ApplicationUser>, ApplicationUserStore>();
      services.AddTransient<IUsersService, UsersService>();

      //Indendity Matters
      services.AddIdentity<ApplicationUser, ApplicationRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddUserStore<ApplicationUserStore>()
        .AddUserManager<ApplicationUserManager>()
        .AddRoleManager<ApplicationRoleManager>()
        .AddSignInManager<ApplicationSignInManager>()
        .AddRoleStore<ApplicationRoleStore>()
        .AddDefaultTokenProviders();

      services.AddScoped<ApplicationRoleStore>();
      services.AddScoped<ApplicationUserStore>();

      ////Configure JWT Authentication
      var appSettingsSection = Configuration.GetSection("AppSettings");
      services.Configure<AppSettings>(appSettingsSection);
      var appSettings = appSettingsSection.Get<AppSettings>();
      var key = System.Text.Encoding.ASCII.GetBytes(appSettings.Secret);
      services.AddAuthentication(x =>
      {
        //new anti csrf attacking
        //x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      })
      .AddCookie()
      .AddJwtBearer(x =>
      {
        x.RequireHttpsMetadata = false;
        x.TokenValidationParameters = new TokenValidationParameters()
        {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(key),
          ValidateIssuer = false,
          ValidateAudience = false
        };
      });

      services.AddAntiforgery(options =>
      {
        options.Cookie.Name = "XSRF-Cookie-TOKEN";
        options.HeaderName = "X-XSRF-TOKEN";
      });

    }











        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
      app.UseAuthentication();
      app.UseStaticFiles();
      app.UseMvc();
      if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskManagerQAAPICORE v1"));
            }


      IServiceScopeFactory serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
      using (IServiceScope scope = serviceScopeFactory.CreateScope())
      {
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        //Create Admin Role
        if (!(await roleManager.RoleExistsAsync("Admin")))
        {
          var role = new ApplicationRole();
          role.Name = "Admin";
          await roleManager.CreateAsync(role);
        }

        //Create Admin User
        if ((await userManager.FindByNameAsync("admin")) == null)
        {
          var user = new ApplicationUser();
          user.UserName = "admin";
          user.Email = "admin@gmail.com";
          var userPassword = "Admin123#";
          var chkUser = await userManager.CreateAsync(user, userPassword);
          if (chkUser.Succeeded)
          {
            await userManager.AddToRoleAsync(user, "Admin");
          }
        }
        if (!(await userManager.IsInRoleAsync(await userManager.FindByNameAsync("admin"), "Admin")))
        {
          await userManager.AddToRoleAsync(await userManager.FindByNameAsync("admin"), "Admin");
        }

        //Create Employee Role
        if (!(await roleManager.RoleExistsAsync("Employee")))
        {
          var role = new ApplicationRole();
          role.Name = "Employee";
          await roleManager.CreateAsync(role);
        }
      }
      //app.UseStaticFiles();
      //_ = app.UseMvc();
      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
