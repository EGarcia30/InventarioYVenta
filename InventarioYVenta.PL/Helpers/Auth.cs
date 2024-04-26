using System.Security.Claims;
using InventarioYVenta.Models.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace InventarioYVenta.PL.Helpers;

public class Auth{
    public async static Task CreateCookie(HttpContext httpContext, User user, string rol){
        var claims = new List<Claim>
        {
            new Claim("UserId", user.UserId.ToString()!),
            new Claim(ClaimTypes.Name, user.Name!.ToString()),
            new Claim(ClaimTypes.Gender, user.Gender!.ToString()),
            new Claim("BirthDate", user.Birthdate.ToString()!),
            new Claim(ClaimTypes.StreetAddress, user.Address!.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Dui!.ToString()),
            new Claim(ClaimTypes.OtherPhone, user.PhoneNumber!.ToString()),
            new Claim("Username", user.Username!.ToString()),
            new Claim(ClaimTypes.Email, user.Email!.ToString()),
            new Claim(ClaimTypes.Role, rol),
            new Claim("CreatedAt", user.CreatedAt.ToString()!)
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
    }

    public async static Task DeleteCookie(HttpContext httpContext){
        await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}