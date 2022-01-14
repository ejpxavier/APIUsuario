using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace APIUsuario.Utils
{
     public class CodificarSenha
     {

          private HashAlgorithm hash;

          public string senha { get; set; }

          public CodificarSenha(HashAlgorithm _senha) 
          {
               hash = _senha;     
          }

          public string CriptografarSenha(string senha)
          {
               var codificarValor = Encoding.UTF8.GetBytes(senha);
               var codificarSenhaId = hash.ComputeHash(codificarValor);

               var _senha = new StringBuilder();
               foreach (var caracter in codificarSenhaId)
               {
                    _senha.Append(caracter.ToString("X2"));
               }

               return _senha.ToString();
          }
     }
}
