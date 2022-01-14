using APIUsuario.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIUsuario.Models
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [DataType(DataType.Text)]
        public string Nome { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Campo e-mail é obrigatório!")]
        public string email { get; set; }

        public string senha { get; set; }

        public DateTime CriadoEm { get; set; } = DateTime.Now;

        public DateTime ModificadoEm { get; set; } = DateTime.Now;

        public DateTime UltimoAcesso { get; set; }

        public virtual ICollection<Perfil> Perfis { get; set; } = new List<Perfil>();
    }
}
