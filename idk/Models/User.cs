using idk.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace idk.Models
{
    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Bio { get; set; }
        public virtual List<UserPic> Pics { get; set; } = new();

        public async Task LogIn(HttpContext httpContext)
        {
            var claims = new List<Claim>
            {
                new Claim("ID", ID.ToString()),
                new Claim(ClaimTypes.Name, Name)
            };
            var identity = new ClaimsIdentity(claims, "AuthCookie");
            var principal = new ClaimsPrincipal(identity);

            await httpContext.SignInAsync("AuthCookie", principal);
        }

        public List<User> GetNextUnmatchedUsers(DBContext context, int count = 1, bool includePics = true)
        {
            var queryResult = context.User.FromSql($"SELECT * FROM dbo.GetUnmatchedUsers({count}, {ID})");

            return includePics
                ? queryResult.GroupJoin(context.UserPic, x => x.ID, x => x.UserID, (user, pics) => new User
                  {
                      ID        = user.ID,
                      Name      = user.Name,
                      Email     = user.Email,
                      Password  = user.Password,
                      Bio       = user.Bio,
                      Pics      = pics.ToList()
                  }).ToList()
                : queryResult.ToList();
        }
    }

    public class UserViewModel
    {

        [StringLength(maximumLength: 50, MinimumLength = 2, ErrorMessage = "Name has to be between 2-50 characters long")]
        public string Name { get; set; }

        [MaxLength(100)]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,}$", ErrorMessage = "Incorrect email format")]
        public string Email { get; set; }

        [MinLength(6, ErrorMessage = "Password must contain at least 6 characters")]
        [MaxLength(50, ErrorMessage = "Password cannot be longer than 50 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
