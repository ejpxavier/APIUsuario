using APIUsuario.Data;
using APIUsuario.Dtos;
using APIUsuario.Models;
using APIUsuario.Utils;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace APIUsuario.Controllers
{

     [ApiController]
     [Route("api/[controller]")]
     public class UsuarioController : ControllerBase
     {
          private readonly APIUsuarioDBContext _contexto;
          private readonly IConfiguration _config;

          public UsuarioController(APIUsuarioDBContext contexto, IConfiguration configuration)
          {
               _contexto = contexto;
               _config = configuration;
          }

          [HttpGet]
          public List<Usuario> GetUsuario()
          {
               List<Usuario> lista = _contexto.usuarios.ToList();

               return lista;
          }

          [HttpPost]
          public IActionResult PostUsuario(UsuarioDto usuario)
          {
               bool exiteEmail = false;

               string msg = "Erro ao cadastrar usuário: ";

               using (NpgsqlConnection conexaoNpgsql = new NpgsqlConnection(_config.GetConnectionString("APIUsuarioDBContext")))
               {
                    try
                    {
                         conexaoNpgsql.Open();
                         var query = "SELECT * FROM usuarios where Upper(email) = Upper('" + usuario.email + "') ";

                         Usuario _usuario = conexaoNpgsql.Query<Usuario>(query).FirstOrDefault();
                         
                         exiteEmail = (_usuario != null);

                         conexaoNpgsql.Close();
                    }
                    catch (Exception e)
                    {
                         return new ObjectResult(new Mensagem(msg + e.Message));
                    }
               }             
              
               if (exiteEmail)
               {
                    return new ObjectResult(new Mensagem("E-mail já existente."));
               }
               else
               {
                    var hash = new CodificarSenha(SHA512.Create());

                    var usuarioEntity = new Usuario
                    {
                         Nome = usuario.Nome,
                         email = usuario.email,
                         senha = hash.CriptografarSenha(usuario.senha)
                    };

                    _contexto.usuarios.Add(usuarioEntity);

                    foreach (var perfilUsuarioDto in usuario.Perfis)
                    {
                         var perfilEntity = _contexto.perfis.Find(perfilUsuarioDto.Id);
                         if (perfilEntity != null)
                         {
                              perfilEntity.Usuarios.Add(usuarioEntity);
                              perfilUsuarioDto.Nome = perfilEntity.Nome;
                         }
                    }

                    _contexto.SaveChanges();

                    usuario.Id = usuarioEntity.Id;
                    usuario.CriadoEm = usuarioEntity.CriadoEm;
                    usuario.ModificadoEm = usuarioEntity.ModificadoEm;

               }

               return Ok(usuario);
          }

          [HttpPut]
          public Mensagem PutUsuario(UsuarioDto usuario)
          {
               string msg = "Erro ao alterar o registro do Usuário: ";
               try
               {
                    var hash = new CodificarSenha(SHA512.Create());

                    var usuarioEntity = new Usuario
                    {
                         Nome = usuario.Nome,
                         email = usuario.email,
                         senha = hash.CriptografarSenha(usuario.senha),
                         ModificadoEm = DateTime.Now,
                    };

                    _contexto.usuarios.Update(usuarioEntity);

                    int result = _contexto.SaveChanges();

                    if (result > 0)
                    {
                         msg = "Usuário alterado com sucesso!";
                    }

                    return new Mensagem(msg);

               }
               catch (Exception e)
               {
                    return new Mensagem(msg + e.Message);
               }
          }

          [HttpDelete("{id}")]
          public Mensagem DeleteUsuario(Guid id)
          {
            
               string msg = "Erro ao excluir o registro do Usuário!";
               try 
               {
                    _contexto.usuarios.Remove(new Usuario
                    {
                         Id = id,
                    });

                    int result = _contexto.SaveChanges();
                    
                    if (result > 0)
                    {
                         msg = "Usuário excluído com sucesso!";
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
