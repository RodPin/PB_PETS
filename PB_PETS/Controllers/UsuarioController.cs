using Dapper;
using Microsoft.AspNetCore.Mvc;
using PB_PETS.Models;
using System.Data;

namespace PB_PETS.Controllers
{
    public class UsuarioController : Controller
    {
        private IDbConnection conexao;

        public UsuarioController(IDbConnection conexao)
        {
            this.conexao = conexao;
        }
        public ActionResult Perfil(string id)
        {
            conexao.Open();
            List<UsuarioPerfilModel> usuarios = (List<UsuarioPerfilModel>)conexao.Query<UsuarioPerfilModel>("SELECT * FROM Usuario where Id=@id", new { id = int.Parse(id) });
            conexao.Close();

            if (usuarios.Count > 0)
            {
                List<AmizadeModel> amizades = (List<AmizadeModel>)conexao.Query<AmizadeModel>("SELECT * FROM Amizade where IdUsuarioDestino=@id AND idUsuarioOrigem=1  OR IdUsuarioDestino=1 AND idUsuarioOrigem=@id", new { id = int.Parse(id) });
                var usuario = usuarios[0];

                usuario.isSolicitado = false;
                usuario.isAmigo = false;
                if (amizades.Count > 0)
                {
                    usuario.isSolicitado = true;
                    if (amizades[0].statusDaSolicitacao != 0)
                    {
                        usuario.isAmigo = true;
                    }
                }
                return View(usuarios[0]);
            };
            return RedirectToAction("/Publicacao/Listar");
        }

        public ActionResult EditarPerfil(string id)
        {
            if (int.Parse(id) != 1)
            {
                return RedirectToAction("/Publicacao/Listar");
            }
            conexao.Open();
            List<UsuarioPerfilModel> usuarios = (List<UsuarioPerfilModel>)conexao.Query<UsuarioPerfilModel>("SELECT * FROM Usuario where Id=@id", new { id = int.Parse(id) });
            conexao.Close();

            if (usuarios.Count > 0)
            {
                var usuario = usuarios[0];

                return View(usuario);
            };
            return RedirectToAction("/Publicacao/Listar");
        }

        [HttpPost]
        public ActionResult EditarPerfil(UsuarioModel usuario)
        {
            conexao.Open();
            conexao.Query<UsuarioPerfilModel>("UPDATE Usuario SET nome=@nome, sobrenome =@sobrenome, email=@email, telefone =@telefone, endereco =@endereco where Id = @Id", new
            {
                id = usuario.Id,
                nome = usuario.nome,
                sobrenome = usuario.sobrenome,
                email = usuario.email,
                telefone = usuario.telefone,
                endereco = usuario.endereco
            });
            conexao.Close();


            return Redirect("/usuario/Perfil" + "?Id=" + usuario.Id);
        }

        public ActionResult EditarSenha()  
        {
            var changeSenha = new EditarSenhaModel();
            return View(changeSenha);
        }

        [HttpPost]
        public ActionResult EditarSenha(EditarSenhaModel usuarioNovaSenha)
        {

            var changeSenha = new EditarSenhaModel();
            conexao.Open();
            List<UsuarioPerfilModel> usuarios = (List<UsuarioPerfilModel>)conexao.Query<UsuarioPerfilModel>("SELECT * FROM Usuario where Id=@id", new { id =usuarioNovaSenha.Id });
            conexao.Close();

            if (usuarios.Count == 0)
            {
                return RedirectToAction("/");
            };
            changeSenha.senhaAtual = usuarioNovaSenha.senhaAtual;
            changeSenha.senha = usuarioNovaSenha.senha;
            changeSenha.senha2 = usuarioNovaSenha.senha2;

            var usuario = usuarios[0];
            if(usuario.senha != usuarioNovaSenha.senhaAtual)
            {
                changeSenha.erroSenhaAtual = true;
                return View(changeSenha);
            }
            if(usuarioNovaSenha.senha != usuarioNovaSenha.senha2)
            {
                changeSenha.erroSenhaNova = true;
                return View(changeSenha);
            }

            conexao.Query<UsuarioPerfilModel>("UPDATE Usuario SET senha=@senha where Id = @id", new
            {
                id = usuarioNovaSenha.Id,
                senha = usuarioNovaSenha.senha
            });
            conexao.Close();


            return Redirect("/usuario/Perfil" + "?Id=" + usuario.Id);
        }
    }
}
    