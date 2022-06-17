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
            var publicacoes = conexao.Query<PublicacaoUsuarioModel>("select Publicacao.texto,Publicacao.Id as idPublicacao,Usuario.nome,Usuario.id as idUsuario from Publicacao INNER JOIN Usuario ON Publicacao.idUsuario=Usuario.Id");

            foreach (var publicacao in publicacoes)
            {
                publicacao.comentarios = (List<ComentarioUsuarioModel>)conexao.Query<ComentarioUsuarioModel>("SELECT * FROM Comentario INNER JOIN Usuario ON idUsuario=@idUsuario where idPublicacao=@idPublicacao", new { idPublicacao = publicacao.idPublicacao,idUsuario = publicacao.idUsuario });
                publicacao.curtidas = (List<CurtidaModel>)conexao.Query<CurtidaModel>("SELECT * FROM Curtidas where idPublicacao=@idPublicacao", new { idPublicacao = publicacao.idPublicacao });
            };

            conexao.Close();

            return View(publicacoes);
        }
    }
}
