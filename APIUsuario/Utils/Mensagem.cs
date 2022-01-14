using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIUsuario.Models
{
     public class Mensagem
     {

          public string mensagem { get; set; }
          public Mensagem(string _mensagem)
          {
               mensagem = _mensagem;
          }
     }
}
