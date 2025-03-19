using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopping.API.DTO;
using Shopping.API.Security;
using Shopping.DAL;
using Shopping.DAL.Entities;

namespace Shopping.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController(ShoppingContext ctx, ITokenManager tokenManager) : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginFormDTO dto)
        {
            // se connecter à la db
            // chercher l'utilisateur dont le username correspond à dto.Username
            User? user = ctx.Users.SingleOrDefault(u => u.Username == dto.Username);

            // si l'utilisateur n'est pas trouvé
            if (user is null)
            {
                // 400 // 401
                return BadRequest();
                // return Unauthorized();
            }

            // si l'utilisateur est trouvé
            // vérifier si le mot de passe est correct
            // si le mot de passe n'est pas correct
            if (Encoding.UTF8.GetString(SHA512.HashData(Encoding.UTF8.GetBytes(dto.Password + user.Salt))) != user.Password)
            {
                // 400 // 401
                return BadRequest();
            }

            // si le mot de passe est correct
            // générer et retourner un token
            return Ok(new
            {
                Token = tokenManager.CreateToken(user.Id, user.Email, user.Role)
            }
            );
        }

        [HttpGet]
        public IActionResult RefreshToken([FromQuery] string token)
        {
            try
            {
                int id = tokenManager.ValidateTokenWithoutLifeTime(token);
                //vérifier le user existe pour pouvoir passer ses données dans le nouveau token de rafraîchissement
                User? user = ctx.Users.Find(id);
                if (user is null)
                {
                    return Unauthorized();
                }
                string newToken = tokenManager.CreateToken(user.Id, user.Email, user.Role);
                return Ok(new
                {
                    Token = newToken
                });
            }
            catch (Exception)
            {
                return Unauthorized();
            }

        }
    }
}
