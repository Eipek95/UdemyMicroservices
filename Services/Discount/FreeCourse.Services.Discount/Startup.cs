using FreeCourse.Services.Discount.Services;
using FreeCourse.Shared.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;

namespace FreeCourse.Services.Discount
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();//sharedidentityservice i�inde tan�mlanan httpcontexti kullanabilmek i�in yazd�k.ama� token �zerinden kullan�c�n�n verilerine ula�mak id gibi
            services.AddScoped<ISharedIdentityService, SharedIdentityService>();
            services.AddScoped<IDiscountService, DiscountService>();


            var requireAuthorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();//kullan�c� giri�li apiler

            //var yetki=new AuthorizationPolicyBuilder().RequireClaim("scope","discount_read").Build();//buraya yetki eklemek icin b�yle bir yol uygulayabiliriz.


            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");//sub keyini mapleme biz direk sub olarak okuyabilelim gelen id yi
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.Authority = Configuration["IdentityServerURL"];//token da��tan yer.appsetting
                options.Audience = "resource_discount";//bu isim identityserver projesi i�indeki config dosyas�nda tan�ml�
                options.RequireHttpsMetadata = false;//https olarak �al��t�rmad�g�m�z icin projeyi onu belirtik

            });

            services.AddControllers(opt =>
            {
                opt.Filters.Add(new AuthorizeFilter(requireAuthorizePolicy));//kullan�c� bekleyen filtre
                //opt.Filters.Add(new AuthorizeFilter(yetki));//yetki bekleyen filtre
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FreeCourse.Services.Discount", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FreeCourse.Services.Discount v1"));
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
