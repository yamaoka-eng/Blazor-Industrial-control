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

// ע�������֤����
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
// �����֤����
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(o =>
{
    o.Events.OnRedirectToLogin = context =>
    {
        return Task.CompletedTask;
    };
});
// ��Ȩ����
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("admin", policy => policy.RequireRole("admin"));
    options.AddPolicy("superadmin", policy => policy.RequireRole("superadmin"));
});
// ע�������
builder.Services.AddControllers().AddInject();
// ע��bootstrap���
builder.Services.AddBootstrapBlazor();

// ע�� MongoDB ����
//var mongoSettings = builder.Configuration.GetSection("MongoDatabase").Get<Dictionary<string, string>>();
//builder.Services.AddSingleton(new MongoDBConnection(mongoSettings["ConnectionString"], mongoSettings["DatabaseName"]));

// ע�� PLCOPC ����
//var opcSettings = builder.Configuration.GetSection("PLC").Get<Dictionary<string, string>>();
//builder.Services.AddSingleton(new OPCScope(opcSettings["PLCIP"]));
//builder.Services.AddSingleton<Lazy<IPLCOperate>>(
//    serviceProvider => new Lazy<IPLCOperate>(() => serviceProvider.GetRequiredService<IPLCOperate>()));
// ע�� PLC ����
//builder.Services.AddSingleton<IPLCOperate, PLCServer>();
// ���PlcService��Ϊ��̨����
//builder.Services.AddHostedService<PLCServer>();

// ע�뵱ǰ������ʵ���
builder.Services.AddSingleton<ICurrentErrorRepository, CurrentErrorRepository>();
// ע����ʷ������ʵ���
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

// ע��Furion�������
app.UseInject();

// �����֤�м��
app.UseAuthentication();
app.UseAuthorization();

// MVC·��ӳ��
app.MapDefaultControllerRoute();

app.Run();
