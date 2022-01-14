using System;
using System.Collections.Generic;

namespace APIUsuario.Dtos
{
     public class PerfilDto
     {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public virtual ICollection<UsuarioDto> Usuarios { get; set; } = new List<UsuarioDto>();
    }
}