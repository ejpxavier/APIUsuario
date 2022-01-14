using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace APIUsuario.Models
{
     public class Login
     {
          [Required (ErrorMessage ="Por favor digite o e-mail!")]
          [DataType(DataType.EmailAddress)]
          [Display(Name = "E-mail")]
          public string email { get; set; }

          [Required(ErrorMessage = "Por favor digite a senha!")]
          [DataType(DataType.Password)]
          [Display(Name = "Senha")]
          public string senha { get; set; }
     }
}
