using Dapper;
using Microsoft.AspNetCore.Mvc;
using PB_PETS.Helpers;
using PB_PETS.Models;
using System.Data;

namespace PB_PETS.Controllers
{
    public class AmizadeController : Controller
    {
        private IDbConnection conexao;
        private LoggedUser loggedUser = new LoggedUser();
        public AmizadeController(IDbConnection conexao)
        {
            this.conexao = conexao;
        }

        [HttpPost]
        public ActionResult Adicionar(UsuarioModel usuario)
        {
            conexao.Open();
            conexao.Query("INSERT INTO Amizade (idUsuarioOrigem,idUsuarioDestino,dataCriacao) values (@idUsuarioOrigem,@idUsuarioDestino,@dataCriacao)", new { idUsuarioOrigem = loggedUser.getLoggedUser(), idUsuarioDestino = usuario.Id, dataCriacao = DateTime.Now });
            conexao.Close();
            return Redirect("/Usuario/perfil?Id=" + usuario.Id);
        }

        [HttpPost]
        public ActionResult Aceitar(AmizadeModel amizade)
        {
            conexao.Open();
            conexao.Query("UPDATE Amizade SET statusDaSolicitacao = 1 where Id = @id", new { id = @amizade.Id });
            conexao.Close();
            return RedirectToAction("listar");
        }

        [HttpPost]

        public ActionResult Recusar(AmizadeModel amizade)
        {
            conexao.Open();
            conexao.Query("DELETE Amizade where Id = @id", new { id = @amizade.Id });
            conexao.Close();
            return Redirect("listar");
        }


        public ActionResult Listar()
        {
            AmizadeList amizadeList = new AmizadeList();
            conexao.Open();

            var publicacoesAmigos = conexao.Query<PublicacaoUsuarioModel>("SELECT Usuario.foto as fotoUsuario, Publicacao.texto,Publicacao.foto,Usuario.nome,Publicacao.dataCriacao FROM Publicacao INNER JOIN Usuario ON Usuario.Id = Publicacao.idUsuario WHERE idUsuario IN( SELECT idUsuarioDestino as Id FROM Amizade WHERE idUsuarioOrigem=@id AND statusDaSolicitacao = 1 UNION SELECT idUsuarioOrigem as Id FROM Amizade WHERE idUsuarioDestino=@id AND statusDaSolicitacao = 1)", new { id = loggedUser.getLoggedUser() });
            var i = 0;
            foreach (var publicacao in publicacoesAmigos)
            {
                publicacao.comentarios = (List<ComentarioUsuarioModel>)conexao.Query<ComentarioUsuarioModel>("SELECT * FROM Comentario INNER JOIN Usuario ON Usuario.Id=Comentario.idUsuario where idPublicacao=@idPublicacao", new { idPublicacao = publicacao.idPublicacao, idUsuario = publicacao.idUsuario });
                var curtidas = conexao.Query<CurtidaModel>("SELECT * FROM Curtidas where idPublicacao=@idPublicacao", new { idPublicacao = publicacao.idPublicacao });
                publicacao.curtidas = curtidas.Count();
                publicacao.isMine = publicacao.idUsuario == loggedUser.getLoggedUser() ? true : false;
                i++;
            };
            amizadeList.novasAmizades = conexao.Query<AmizadeModel>("SELECT Amizade.Id,Usuario.nome,Usuario.sobrenome,Amizade.dataCriacao FROM Amizade INNER JOIN Usuario ON Amizade.idUsuarioDestino=Usuario.Id  WHERE Amizade.idUsuarioDestino=@id and Amizade.statusDaSolicitacao=0", new { id = loggedUser.getLoggedUser() });
            amizadeList.postsAmigos = publicacoesAmigos;

            conexao.Close();
            return View(amizadeList);
        }
    } 
}

