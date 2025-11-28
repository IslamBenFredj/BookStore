using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;

    public RolesController(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    //  Assignation d’un rôle : /api/roles/assign
    [HttpPost("assign")]
    [Authorize(Roles = "Admin")] // seul un Admin peut le faire
    public async Task<IActionResult> AssignRole(string userId, string role)

    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return NotFound("Utilisateur non trouvé");

        var result = await _userManager.AddToRoleAsync(user, role);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok($"Rôle '{role}' assigné à '{user.UserName}'");
    }

    [HttpGet("all-users")]
    [Authorize(Roles = "Admin")]
    public IActionResult GetAllUsers()
    {
        return Ok("Seulement les admins voient ceci !");
    }
    [HttpGet("me")]
    [Authorize(Roles = "User")]
    public IActionResult GetMyProfile()
    {
        return Ok("Uniquement les utilisateurs connectés");
    }
}
