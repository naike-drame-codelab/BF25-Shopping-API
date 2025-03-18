using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopping.DAL;
using Shopping.DAL.Entities;

namespace Shopping.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    // autorisation globale pour tous les endpoints de ce controller
    // [Authorize(Roles = "Admin, Noob")]
    public class ArticleController(ShoppingContext context) : ControllerBase
    {
        [HttpGet]
        [Authorize] // doit être authentifié pour accéder à cette ressource
        public IActionResult Get()
        {
            //if(!User.IsInRole("Admin"))
            //{
            //    return Unauthorized(); // 401
            //}

            return Ok(context.Articles.ToList()); // 200
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]  // doit être authentifié et avoir le rôle Admin pour accéder à cette ressource
        public IActionResult Post([FromBody] Article article)
        {
            context.Articles.Add(article);
            context.SaveChanges();
            return Created(); // 201
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete([FromRoute] int id)
        {
            Article? articleToDelete = context.Articles.Find(id);

            if(articleToDelete == null)
            {
                return NotFound(); // 404
            }

            context.Articles.Remove(articleToDelete);
            context.SaveChanges();

            return NoContent(); // 204
        }

        [HttpDelete("all")]
        public IActionResult DeleteAll()
        {
            context.Articles.RemoveRange(context.Articles);
            context.SaveChanges();
            return NoContent(); // 204
        }
    }
}
