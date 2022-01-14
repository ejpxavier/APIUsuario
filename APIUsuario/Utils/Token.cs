using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using APIUsuario.Models;

namespace APIUsuario.Utils
{
     public static class Token
     {
          public static string GerarToken(Usuario usuario) 
          {
               var token = new JwtSecurityTokenHandler();

               var key = Encoding.ASCII.GetBytes(Utils.ConfiguracaoChave.Chave);

               var tokenD = new SecurityTokenDescriptor
               {
                    Subject = new ClaimsIdentity(new Claim[] 
                    { 
                         new Claim(ClaimTypes.Email, usuario.email.ToString ()),
                         new Claim(ClaimTypes.Name, usuario.Nome.ToString ())
                    }),
                    Expires = DateTime.UtcNow.AddHours (2),
                    SigningCredentials = new SigningCredentials(
                         new SymmetricSecurityKey(key), 
                         SecurityAlgorithms.HmacSha256Signature)

               };

               var tokenGerado = token.CreateToken(tokenD);

               return token.WriteToken(tokenGerado);

          }
     }
}
