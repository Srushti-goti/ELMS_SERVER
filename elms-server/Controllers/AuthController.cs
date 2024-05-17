using EmployeeLeaveManagementApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Employee;

namespace EmployeeLeaveManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly EmployeeDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(EmployeeDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<Employee>> Register(Employee employee)
        {
            var user = await _context.Employees.FirstOrDefaultAsync(e => e.Email == employee.Email);
            if(user != null)
            {
                return BadRequest(new { message = "User Already Exist!" });
            }
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Employee Register successfully", data = employee });
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginRequest model)
        {
            var user = await _context.Employees.FirstOrDefaultAsync(e => e.Email == model.Email);
            if(user == null)
            {
                return BadRequest(new { message = "No User Found" });
            }
            if(user.Password  != model.Password)
            {
                return BadRequest(new { message = "Invalid Password"});
            }
            var token = GenerateJwtToken(user);
            return Ok(new { message = "Login successful", token, user });
        }
        #region Private method
        private string GenerateJwtToken(Employee user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("8d92f47ae8794b7e9c384c89e81a05ff1e798263da8fbfbec5b6d20b9947l"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
        {
           new Claim(ClaimTypes.Name, user.EmployeeId.ToString()),


        };



            var token = new JwtSecurityToken(
                issuer: "https://localhost:7062",
                audience: "https://localhost:7062",
                claims: claims,
                expires: DateTime.Now.AddHours(10),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
        #endregion
    }
