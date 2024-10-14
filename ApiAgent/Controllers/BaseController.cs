using Microsoft.AspNetCore.Mvc;
namespace ApiAgent.Controllers;
public class BaseController<T>: ControllerBase
{
    

    private ILogger<T> _logger;
    public ILogger<T> logger => _logger ??= HttpContext.RequestServices.GetService<ILogger<T>>();




}