using Dapper;
using Microsoft.AspNetCore.Mvc;
using PB_PETS.Models;
using System.Data;

namespace PB_PETS.Controllers
{
    public class PublicacaoController : Controller
    {
        private IDbConnection conexao;

        public PublicacaoController(IDbConnection conexao)
        {
            this.conexao = conexao;
        }
        public ActionResult Listar()
        {
            conexao.Open();
            var publicacoes = conexao.Query<PublicacaoUsuarioModel>("select Publicacao.texto,Publicacao.Id as idPublicacao, Publicacao.dataCriacao as publicacaoDataCriacao,Usuario.nome,Usuario.id as idUsuario from Publicacao INNER JOIN Usuario ON Publicacao.idUsuario=Usuario.Id ORDER BY publicacaoDataCriacao ASC");

            foreach (var publicacao in publicacoes)
            {
                publicacao.comentarios = (List<ComentarioUsuarioModel>)conexao.Query<ComentarioUsuarioModel>("SELECT * FROM Comentario INNER JOIN Usuario ON Usuario.Id=Comentario.idUsuario where idPublicacao=@idPublicacao", new { idPublicacao = publicacao.idPublicacao, idUsuario = publicacao.idUsuario });
                var curtidas = conexao.Query<CurtidaModel>("SELECT * FROM Curtidas where idPublicacao=@idPublicacao", new { idPublicacao = publicacao.idPublicacao });
                publicacao.curtidas = curtidas.Count();
            };

            conexao.Close();

            return View(publicacoes);
        }

        [HttpPost]
        public ActionResult Publicar(PublicacaoModel publicacao)
        {
            conexao.Open();
            conexao.Query<PublicacaoModel>("INSERT INTO Publicacao (idUsuario,texto,foto,dataCriacao) values(@idUsuario,@texto,@foto,@dataCriacao)", new { idUsuario = 1, texto= publicacao.texto,foto="AOSKDOASD",dataCriacao = DateTime.Now });
            conexao.Close();
            return Redirect("Listar");
        }


        [HttpPost]
        public ActionResult Comentar(ComentarioModel comentario)
        {
            conexao.Open();
            conexao.Query<ComentarioModel>("INSERT INTO Comentario (idUsuario,idPublicacao,texto,dataCriacao) values(@idUsuario,@idPublicacao,@texto,@dataCriacao)", new { idUsuario = 2, texto = comentario.texto, idPublicacao = comentario.idPublicacao, dataCriacao = DateTime.Now });
            conexao.Close();
            return Redirect("Listar");
        }


        [HttpPost]
        public ActionResult Curtir(PublicacaoModel publicacao)
        {
            conexao.Open();
            List<CurtidaModel>curtida = (List<CurtidaModel>)conexao.Query<CurtidaModel>("SELECT * FROM Curtidas where idPublicacao=@idPublicacao and idUsuario=@idUsuario", new { idPublicacao = publicacao.Id,idUsuario = 1 });

            if(curtida.Count == 0)
            {
                conexao.Query<PublicacaoModel>("INSERT INTO Curtidas (idUsuario,idPublicacao,dataCriacao) values(@idUsuario,@idPublicacao,@dataCriacao)", new { idUsuario = 1, idPublicacao = publicacao.Id, dataCriacao = DateTime.Now });
            }
            else
            {
                conexao.Query<PublicacaoModel>("DELETE FROM Curtidas WHERE idPublicacao=@idPublicacao and idUsuario=@idUsuario", new { idUsuario = 1, idPublicacao = publicacao.Id, dataCriacao = DateTime.Now });
            }

            conexao.Close();
            return Redirect("Listar");
        }

        [HttpPost]
        public ActionResult Deletar(PublicacaoModel publicacao)
        {
            conexao.Open();
            conexao.Query<PublicacaoModel>("DELETE FROM Publicacao WHERE Id=@idPublicacao", new { idPublicacao = publicacao.Id});
            conexao.Close();
            return Redirect("Listar");
        }
    }
}

