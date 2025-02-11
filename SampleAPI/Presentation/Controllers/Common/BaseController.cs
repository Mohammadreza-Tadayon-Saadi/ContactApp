using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebConfig.Filters;

namespace API.Controllers.Common;

[ApiController]
[AllowAnonymous]
[ApiResultFilter]
[ApiVersion("1")]
[Route("/api/v{version:apiVersion}/[controller]")]// api/v1/[controller]/[action]
public class BaseController : ControllerBase;