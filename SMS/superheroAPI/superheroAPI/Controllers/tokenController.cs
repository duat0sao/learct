using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace superheroAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class tokenController : ControllerBase
    {

    }
}
