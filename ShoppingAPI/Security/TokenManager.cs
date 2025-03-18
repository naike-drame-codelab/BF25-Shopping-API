using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Shopping.API.Security
{
    public class TokenManager(TokenManager.Config config) : ITokenManager
    {
        #region Class interne
        // class interne très liéé à la class TokenManager et utilisée qu'à cet endroit
        public class Config
        {
            public string Secret { get; set; } = null!;
            public int Duration { get; set; } // en secondes
            public string Issuer { get; set; } = null!;
            public string Audience { get; set; } = null!;
        }
        #endregion

        public string CreateToken(int id, string email, string role)
        {
            JwtSecurityTokenHandler handler = new();
            JwtSecurityToken token = new(
                // issuer
                config.Issuer,
                // audience
                config.Audience,
                // claims
                CreateClaims(id, email, role),
                // start
                DateTime.Now,
                // expires
                DateTime.Now.AddSeconds(config.Duration),
                //  signingKey
                new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Secret)),
                    SecurityAlgorithms.HmacSha256
                ));
            return handler.WriteToken(token);
        }

        // un IEnumerable est une collection d'éléments qui peut être parcourue par une boucle foreach
        // on peut retourner un tableau, une liste, un dictionnaire, etc.
        private IEnumerable<Claim> CreateClaims(int id, string email, string role)
        {
            // yield return permet de retourner un élément à la fois
            // tableau chaîné de données
            yield return new Claim(ClaimTypes.Email, email);
            yield return new Claim(ClaimTypes.Role, role);
            yield return new Claim(ClaimTypes.NameIdentifier, id.ToString(), ClaimValueTypes.Integer32);

            #region équivalent
            // return
            // [
            //    new Claim(ClaimTypes.Email,email),
            //    new Claim(ClaimTypes.Role, role),
            //    new Claim(ClaimTypes.NameIdentifier, id.ToString())
            //]; 
            #endregion
        }
    }
}
