using APIUsuario.Data;
using APIUsuario.Dtos;
using APIUsuario.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIUsuario.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PerfilController : ControllerBase
    {
          private readonly APIUsuarioDBContext _contexto;

          public PerfilController(APIUsuarioDBContext contexto)
          {
               _contexto = contexto;
          }

          [HttpGet]
          public async Task<List<PerfilDto>> GetPerfis()
          {
               return await _contexto.perfis
                    .Select(x => new PerfilDto
                    {
                         Id = x.Id,
                         Nome = x.Nome,
                         Usuarios = x.Usuarios.Select(y => new UsuarioDto
                         {
                              Id = y.Id,
                              Nome = y.Nome,
                              email = y.email,
                              CriadoEm = y.CriadoEm,
                              ModificadoEm = y.ModificadoEm,
                         }).ToList()
                    }).ToListAsync();
          }

          [HttpPost]
          public async Task<ActionResult<Mensagem>> PostPerfil([FromBody] PerfilDto perfil)
          {
          
               string msg = "Erro ao cadastrar o registro do Perfil: ";

               try
               {
                    if (ModelState.IsValid)
                    {
                         var perfilEntity = new Perfil
                         {
                              Nome = perfil.Nome,
                         };
                         _contexto.perfis.Add(perfilEntity);

                         await _contexto.SaveChangesAsync();

                         return new Mensagem("Perfil cadastrado com sucesso!");
                    }
                    else
                    {
                         return new Mensagem(msg + BadRequest(ModelState));
                    }
               }
               catch (Exception e)
               {
                    return new Mensagem(msg + e.Message);
               }
          }

          [HttpPut]
          public async Task<ActionResult<Mensagem>> PutPerfil([FromBody] PerfilDto perfil)
          {
               string msg = "Erro ao alterar o registro do Perfil: ";
               try
               {
                    var perfilEntity = await _contexto.perfis.FindAsync(perfil.Id);

                    if (perfilEntity != null)
                    {
                         perfilEntity.Nome = perfil.Nome;

                         await _contexto.SaveChangesAsync();

                         msg = "Perfil alterado com sucesso!";
                    }
                    return new Mensagem(msg);
               }
               catch (Exception e)
               {
                    return new Mensagem(msg + e.Message);
               }
          }

          [HttpDelete("{id}")]
          public Mensagem DeletePerfil(Guid id)
          {
               string msg = "Erro ao excluir o registro do Perfil: ";
               try
               {
                    _contexto.perfis.Remove(new Perfil
                    {
                         Id = id,
                    });

                    int result = _contexto.SaveChanges();

                    if (result > 0)
                    {
                         msg = "Perfil excluído com sucesso!";
                    }

                    return new Mensagem(msg);
               }
               catch (Exception e)
               {
                    return new Mensagem(msg + e.Message);
               }
          }
    }
}
