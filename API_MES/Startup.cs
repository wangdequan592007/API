using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using API_MES.Data;
using API_MES.Helper;
using API_MES.Mappers;
using API_MES.Servise;
using AutoMapper;
using EF_ORACLE;
using EF_SQL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace API_MES
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            //Configuration = configuration.ReplacePlaceholders();
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public readonly string anyAllowSpecificOrigins = "any";
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            NLogHelp.WriteInfo("开始启动api服务");
            //services.AddControllers(configure: setup =>
            //{
            //    setup.ReturnHttpNotAcceptable = true;//请求类型和服务器请求不一致返回406
            //    //格式化器集合  --添加xml old funtion
            //    //setup.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
            //    //t调整默认返回格式，把xml加入到0位置 old funtion
            //    //setup.OutputFormatters.Insert(0, new XmlDataContractSerializerOutputFormatter());
            //}).AddXmlDataContractSerializerFormatters();//AddXmlDataContractSerializerFormatters添加xml
            //扫描寻找AddAutoMapper配置文件
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<ISqlRepository, SqlRepository>();
            services.AddScoped<IMesRepository, MesRepository>();
            services.AddScoped<IRepairRepository, RepairRepository>();
            services.AddScoped<OtherInterface, OtherRepository>();
            ////var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            ////logger.LogInformation("开始运行！");
            //services.AddControllers();    
            ////注册Oracle数据库连接
            var UrlOracle = Configuration.GetConnectionString("OracleConnection");
            services.AddDbContext<MESContext>(option => option.UseOracle(UrlOracle, b => b.UseOracleSQLCompatibility("11")));
            var T100Oracle = Configuration.GetConnectionString("OracleDbaT100");
            services.AddDbContext<T100Context>(option => option.UseOracle(T100Oracle, b => b.UseOracleSQLCompatibility("12")));
            OracleHelper.OracleConnection = UrlOracle;
            ////注册Sqlit数据库连接
            //services.AddDbContext<RoutineDbContext>(option =>
            //{
            //    option.UseSqlite("Data Source=routine.db");
            //});
            //using (var dbcontext = new MESContext())
            //{
            //    var objectContext = ((IObjectContextAdapter)dbcontext).ObjectContext;
            //    var mappingCollection = (StorageMappingItemCollection)objectContext.MetadataWorkspace.GetItemCollection(DataSpace.CSSpace);
            //    mappingCollection.GenerateViews(new List<EdmSchemaError>());
            //}
            //注册SqlServer数据库连接  
            var DemoDbInitial1 = Configuration["DbConn:SqlConn"];
            services.AddDbContext<SqlDbContext>(option => option.UseSqlServer(Configuration["DbConn:SqlConn"]));
            services.AddDbContext<EF6sqlserver23>(option => option.UseSqlServer(Configuration["DbConn:SqlConn23"]));
            //注入Options  
            //OptionsConfigure(services);
            //var identityServerOptions = new IdentityServerOptions();
            //Configuration.Bind("IdentityServerOptions", identityServerOptions);
            //services.AddAuthentication(identityServerOptions.IdentityScheme)
            //    .AddIdentityServerAuthentication(options =>
            //    {
            //        options.RequireHttpsMetadata = false; //是否启用https
            //        options.Authority = $"http://{identityServerOptions.ServerIP}:{identityServerOptions.ServerPort}";//配置授权认证的地址
            //        options.ApiName = identityServerOptions.ResourceName; //资源名称，跟认证服务中注册的资源列表名称中的apiResource一致
            //    }
            //    ); 
            // services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //认证方式一
            #region 认证方式一 详细
            services.AddJwtConfiguration(Configuration);
            #endregion 
            #region Swagger
            // Add MvcFramework
            services.AddControllers(setp => {
                setp.Filters.Add<ActionAttribute>();
            }
                )
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                   
                });
            // Add api version
            // https://www.hanselman.com/blog/ASPNETCoreRESTfulWebAPIVersioningMadeEasy.aspx
            //注册swagger服务,定义1个或者多个swagger文档
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = ApiVersion.Default;
                options.ReportApiVersions = true;
            });
            // swagger
            // https://stackoverflow.com/questions/58197244/swaggerui-with-netcore-3-0-bearer-token-authorization
            services.AddSwaggerGen(option =>
            { //设置swagger文档相关信息
                option.SwaggerDoc("sparktodo", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "SparkTodo API",
                    Description = "API for SparkTodo",
                    Contact = new OpenApiContact() { Name = "wangdequan", Email = "wangdequan@chino-e.com" }
                });

                option.SwaggerDoc("v1", new OpenApiInfo { Version = "v1", Title = "MES" });
                option.SwaggerDoc("v2", new OpenApiInfo { Version = "v2", Title = "Picture" });
                option.SwaggerDoc("v3", new OpenApiInfo { Version = "v3", Title = "SQL" });
                option.SwaggerDoc("v4", new OpenApiInfo { Version = "v4", Title = "Repair" });
                option.DocInclusionPredicate((docName, apiDesc) =>
                {
                    var versions = apiDesc.CustomAttributes()
                        .OfType<ApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions);

                    return versions.Any(v => $"v{v}" == docName);
                });

                option.OperationFilter<RemoveVersionParameterOperationFilter>();
                option.DocumentFilter<SetVersionInPathDocumentFilter>();

                // include document file
                //获取xml注释文件的目录   // 启用xml注释
               // option.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{typeof(Startup).Assembly.GetName().Name}.xml"), true);
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "Please enter into field the word 'Bearer' followed by a space and the JWT value",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference()
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    }, Array.Empty<string>() }
                });
            });
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("any", builder =>
            //    {
            //        //builder.WithOrigins("http://a.example.com", "http://c.example.com") 
            //        builder.AllowAnyOrigin()
            //        .AllowAnyMethod()
            //        .AllowAnyHeader()
            //        .AllowCredentials();
            //    });
            //});
            //services.AddControllers(option => {
            //    //设置异常过滤器
            //    option.Filters.Add(new MyExceptionFilter());
            //});
            #endregion
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowAll",
            //    builder =>
            //    {
            //        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            //    });
            //});

            //services.Configure(options =>
            //{
            //    options.Filters.Add(new CorsAuthorizationFilterFactory("AllowAll"));
            //});
            //配置跨域处理
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("any", builder =>
            //    {
            //        builder.AllowAnyOrigin() //允许任何来源的主机访问
            //        .AllowAnyMethod()
            //        .AllowAnyHeader()
            //        .AllowCredentials();//指定处理cookie
            //    });
            //});
            //配置跨域处理   可以多配置Policy，应对不同的访问域
            //跨域
            //services.AddCors(options =>
            //{
            //    options.AddDefaultPolicy(builder =>
            //    {
            //        builder
            //        //允许任何来源的主机访问
            //        //TODO: 新的 CORS 中间件已经阻止允许任意 Origin，即设置 AllowAnyOrigin 也不会生效
            //        //AllowAnyOrigin()
            //        //设置允许访问的域
            //        //TODO: 目前.NET Core 3.1 有 bug, 暂时通过 SetIsOriginAllowed 解决
            //        //.WithOrigins(Configuration["CorsConfig:Origin"])
            //        .SetIsOriginAllowed(t => true)
            //        .AllowAnyMethod()
            //        .AllowAnyHeader()
            //        .AllowCredentials();
            //    });
            //});
            //配置跨域处理cors
            //services.AddCors(options =>
            //{
            //    options.AddPolicy(anyAllowSpecificOrigins, corsbuilder =>
            //    {
            //        var corsPath = Configuration.GetSection("CorsPaths").GetChildren().Select(p => p.Value).ToArray();
            //        corsbuilder.WithOrigins(corsPath)
            //        .AllowAnyMethod()
            //        .AllowAnyHeader()
            //        .AllowCredentials();//指定处理cookie
            //    });
            //});
            //services.AddCors(option => option.AddPolicy("any", _build => _build.AllowAnyOrigin().AllowAnyMethod()));


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ////不能删除 只能查询  如何使IIS支持Put、delete请求 ①删除IIS安装的WebDav模块，修改你项目的web.config ，在<system.webServer>标签内加上以下代码
            ////https://blog.csdn.net/lynehylo/article/details/80623190?utm_medium=distribute.pc_relevant_t0.none-task-blog-BlogCommendFromMachineLearnPai2-1.nonecase&depth_1-utm_source=distribute.pc_relevant_t0.none-task-blog-BlogCommendFromMachineLearnPai2-1.nonecase
            #region Swagger
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultOutboundAlgorithmMap.Clear();
            app.UseStaticFiles();

            //Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            //Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint("/swagger/v1/swagger.json", "MES");
                option.SwaggerEndpoint("/swagger/v2/swagger.json", "Picture");
                option.SwaggerEndpoint("/swagger/v3/swagger.json", "SQL");
                option.SwaggerEndpoint("/swagger/v4/swagger.json", "Repair");
                option.RoutePrefix = string.Empty;
                option.DocumentTitle = "SparkTodo API";
                //option.InjectOnCompleteJavaScript("/Scripts/swagger.js");
            });
            #endregion
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseHttpsRedirection().UseCors(builder =>
            builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            //app.UseCors("AllowAll");
            app.UseCors("any");
            //app.UseMvc();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
            //// 跳转https
            //app.UseHttpsRedirection();
            //// 使用静态文件
            //app.UseStaticFiles();
            //// 使用cookie
            //app.UseCookiePolicy();
            //// 返回错误码
            //app.UseStatusCodePages();//把错误码返回前台，比如是404

            //app.UseMvc();

        }
        /// <summary> 
        /// </summary>
        /// <param name="services">服务容器</param>
        //private void OptionsConfigure(IServiceCollection services)
        //{
        //    //MongodbHost信息
        //    services.Configure<MongodbHostOptions>(Configuration.GetSection("MongodbHost"));
        //    //图片选项
        //    services.Configure<PictureOptions>(Configuration.GetSection("PictureOptions"));

        //}
    }
}
