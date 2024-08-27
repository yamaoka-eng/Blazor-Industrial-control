using Furion.DataEncryption;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace WHITE_20.Controller
{
    public class LoginController : IDynamicApiController
    {
        // 从请求体中接收登录信息
        public async Task<object> Post([FromBody] User user)
        {
            // 检查用户名是否为空
            if (string.IsNullOrEmpty(user.UserName)) return new { code = 500, message = "用户名不能为空" };
            // 检查密码是否为空
            if (string.IsNullOrEmpty(user.Password)) return new { code = 500, message = "密码不能为空" };
            // 检查账号密码是否错误
            if ((user.UserName == "admin" && user.Password == "123456") || (user.UserName == "superadmin" && user.Password == "xr123456"))
            {
                // 使用MD5加密用户输入的密码
                var password = MD5Encryption.Encrypt(user.Password);

                // 从数据库中查询用户信息，包括其角色信息


                // 创建一个新的身份认证标识，使用Cookie认证方案
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                // 添加用户名和角色信息到身份认证标识中
                identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName!));
                identity.AddClaim(new Claim(ClaimTypes.Role, user.UserName!));
                // 执行登录操作，设置持久性和过期时间(如果用户点了记住我，保留3个月否则30分钟后过期)
                await Furion.App.HttpContext.SignInAsync(new ClaimsPrincipal(identity), new AuthenticationProperties() { IsPersistent = true, ExpiresUtc = user.RememberMe ? DateTimeOffset.Now.AddDays(90) : DateTimeOffset.Now.AddMinutes(30) });

                return new { code = 200, message = "登录成功" };
            }
            else
            {
                return new { code = 500, message = "账号或密码错误" };
            }
        }

        // 登出操作
        public async Task<object> Get()
        {
            try
            {
                await Furion.App.HttpContext.SignOutAsync();
                return new { code = 200, message = "账号退出成功" };
            }
            catch (Exception)
            {
                return new { code = 500, message = "账号退出失败" };
            }
            
        }
    }
}
