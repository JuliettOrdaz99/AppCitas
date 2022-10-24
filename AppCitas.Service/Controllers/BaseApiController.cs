using AppCitas.Service.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace AppCitas.Service.Controllers;

[ServiceFilter(typeof(LogUserActivity))]
[ApiController]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
}
