using APIUsuario.Data;
using APIUsuario.Dtos;
using APIUsuario.Models;
using APIUsuario.Utils;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace APIUsuario.Controllers
{
     [Route("api/[controller]")]
     [ApiController]
     public class LoginController : ControllerBase
     {
          private readonly APIUsuarioDBContext _contexto;
          private readonly IConfiguration _config;

          public LoginController(APIUsuarioDBContext contexto, IConfiguration configuration)
          {
               _contexto = contexto;
               _config = configuration;
          }

          [HttpPost]
          public IActionResult Autenticar([FromBody] Login login)
          {
               var _login = login;

               Usuario usuarioLogado = null;

               bool exiteEmail = false;

               bool senhaConferida = false;

               var hash = new CodificarSenha(SHA512.Create());

               string msg = "Erro ao executar login: ";

               using (NpgsqlConnection conexaoNpgsql = new NpgsqlConnection(_config.GetConnectionString("APIUsuarioDBContext")))
               {
                    try
                    {
                         conexaoNpgsql.Open();

                         var queryLogin = "SELECT * FROM usuarios where Upper(email) = Upper('" + _login.email + "') ";

                         Usuario _usuarioLogin = conexaoNpgsql.Query<Usuario>(queryLogin).FirstOrDefault();

                         exiteEmail = (_usuarioLogin != null);

                         conexaoNpgsql.Close();
                     
                         if (_usuarioLogin != null) 
                         {
                              senhaConferida = (_usuarioLogin.senha.Equals(hash.CriptografarSenha(_login.senha)));

                              usuarioLogado = _usuarioLogin;
                         }
                    }
                    catch (Exception e)
                    {
                         return new ObjectResult(new Mensagem(msg + e.Message));
                    }
               }

               if (!exiteEmail)
               {
                    return BadRequest(new Mensagem("Usuário e / ou senha inválidos"));
               }
               else {
                    if (!senhaConferida) 
                    {
                         return new UnauthorizedObjectResult(new { message = "401: Usuário e / ou senha inválidos" });
                    }
                    else
                    {
                         
                         _contexto.usuarios.Update(new Usuario
                         {
                              Id = usuarioLogado.Id,
                              senha = usuarioLogado.senha,
                              Nome = usuarioLogado.Nome,
                              email = usuarioLogado.email,
                              CriadoEm = usuarioLogado.CriadoEm,
                              ModificadoEm = usuarioLogado.ModificadoEm,
                              UltimoAcesso = DateTime.Now,
                         });

                         _contexto.SaveChanges();

                         Usuario usuarioRetorno = _contexto.usuarios.Find(usuarioLogado.Id);

                         var token = Token.GerarToken(usuarioRetorno);                    

                         return Ok(new { message ="Logado", Object = "token: " + token.ToString(), usuarioRetorno });
                    }
                    
               }
          }
     }
}
