using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WY.Data.Utility;
using WY.Data.Utility._MD5;
using WY.IService;
using WY.Jwt.Models.Login;
using WY.Model.Entity;

namespace WY.Jwt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthoizeController : Controller
    {
        private readonly IAdminService _adminService;
        public AuthoizeController(IAdminService adminService)
        {
            _adminService = adminService;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromQuery] LoginRequestModel model)
        {
            var resp = new AjaxResult<LoginResponseModel>() { Status = AjaxStatus.Normal };

           // string pwd = MD5Helper.MD5Encrpt32(model.Pwd);
            try
            {
                //var admin = await _adminService.GetAdminByUserNameAndPwd(model.UserName, model.Pwd);
                var admin = new Admin { Name = "yuyang", UserName = "yuyang", Pwd = "1" };
                if (admin != null)
                {
                    var claims = new Claim[]
                    {
                    new Claim(ClaimTypes.Name,admin.Name),
                    new Claim("Id",admin.Id.ToString()),
                    new Claim("UserName",admin.UserName),
                    };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SDMC-CJAS1-SAD-DFSFA-SADHJVF-VF"));
                    var token = new JwtSecurityToken(
                        issuer: "http://localhost:6060",
                        audience: "http://localhost:5000",
                        claims: claims,
                        notBefore: DateTime.Now,
                        expires: DateTime.Now.AddHours(1),
                        signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                        );
                    var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                    resp.Data.Token = jwtToken;
                }
                else
                {
                    resp.Status = AjaxStatus.Error;
                    resp.Message = "账号或密码错误";
                }
            }
            catch(Exception ex)
            {
                resp.Status = AjaxStatus.Error;
                resp.Message = ex.Message;
            }
            return Json(resp);
        }
    }
}
