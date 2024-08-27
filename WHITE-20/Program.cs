using Microsoft.AspNetCore.Authentication.Cookies;
using WHITE_20.Components;
using WHITE_20.PLC;
using WHITE_20.MongoDB;
using WHITE_20.Repository;

var builder = WebApplication.CreateBuilder(args).Inject();

builder.Services.AddAntDesign();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// 注入身份验证配置
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
// 身份验证服务
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(o =>
{
    o.Events.OnRedirectToLogin = context =>
    {
        return Task.CompletedTask;
    };
});
// 授权服务
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("admin", policy => policy.RequireRole("admin"));
    options.AddPolicy("superadmin", policy => policy.RequireRole("superadmin"));
});
// 注入控制器
builder.Services.AddControllers().AddInject();
// 注入bootstrap组件
builder.Services.AddBootstrapBlazor();

// 注册 MongoDB 连接
//var mongoSettings = builder.Configuration.GetSection("MongoDatabase").Get<Dictionary<string, string>>();
//builder.Services.AddSingleton(new MongoDBConnection(mongoSettings["ConnectionString"], mongoSettings["DatabaseName"]));

// 注册 PLCOPC 连接
//var opcSettings = builder.Configuration.GetSection("PLC").Get<Dictionary<string, string>>();
//builder.Services.AddSingleton(new OPCScope(opcSettings["PLCIP"]));
//builder.Services.AddSingleton<Lazy<IPLCOperate>>(
//    serviceProvider => new Lazy<IPLCOperate>(() => serviceProvider.GetRequiredService<IPLCOperate>()));
// 注册 PLC 服务
//builder.Services.AddSingleton<IPLCOperate, PLCServer>();
// 添加PlcService作为后台服务
//builder.Services.AddHostedService<PLCServer>();

// 注入当前错误访问单例
builder.Services.AddSingleton<ICurrentErrorRepository, CurrentErrorRepository>();
// 注入历史错误访问单例
builder.Services.AddSingleton<IHistoryErrorRepository, HistoryErrorRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// 注入Furion基础组件
app.UseInject();

// 身份验证中间件
app.UseAuthentication();
app.UseAuthorization();

// MVC路由映射
app.MapDefaultControllerRoute();

app.Run();
