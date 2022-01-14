using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIUsuario.Dtos
{
     public class UsuarioDto
     {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string email { get; set; }
        public string senha { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime ModificadoEm { get; set; }
        public DateTime UltimoAcesso { get; set; }
        public virtual ICollection<PerfilUsuarioDto> Perfis { get; set; } = new List<PerfilUsuarioDto>();
    }
}
