namespace MultiPurposeProject.Controllers;

using Microsoft.AspNetCore.Mvc;
using MultiPurposeProject.Authorization;
using MultiPurposeProject.Entities;
using MultiPurposeProject.Models.Users;
using MultiPurposeProject.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{

    private IUserService _userService;

    public UsersController(
        IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Authenticate the user passed.
    /// </summary>
    /// <returns>One token</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /Todo
    ///     {
    ///        "username": "admin",
    ///        "password": "******"
    ///     }
    ///
    /// </remarks>
    [AllowAnonymous]
    [HttpPost("authenticate")]
    public ActionResult Authenticate(AuthenticateRequest model)
    {
        try
        {
            AuthenticateResponse response = _userService.Authenticate(model);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public ActionResult Register(RegisterRequest model)
    {
        try
        {
            User user = _userService.Register(model);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    public ActionResult GetAll()
    {
        var users = _userService.GetAll();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public ActionResult GetById(int id)
    {
        try
        {
            var user = _userService.GetById(id);
            return Ok(user);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public ActionResult Update(int id, UpdateRequest model)
    {
        try
        {
            _userService.Update(id, model);
            return Ok(new { message = "User updated successfully" });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        try
        {
            _userService.Delete(id);
            return Ok(new { message = "User deleted successfully" });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}

