using APIUsuario.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIUsuario.Data
{
     public class APIUsuarioDBContext : DbContext
     {
          public APIUsuarioDBContext()
          {
          }

          public APIUsuarioDBContext(DbContextOptions<APIUsuarioDBContext> options) : base(options)
          {
               Database.EnsureCreated();
          }
          public DbSet<Perfil> perfis { get; set; }

          public DbSet<Usuario> usuarios { get; set; }

     }
}
