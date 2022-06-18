using Dapper;
using Microsoft.AspNetCore.Mvc;
using PB_PETS.Models;
using System.Data;

namespace PB_PETS.Controllers
{
    public class AmizadeController:Controller
    {
        private IDbConnection conexao;

        public AmizadeController(IDbConnection conexao)
        {
            this.conexao = conexao;
        }

        [HttpPost]
        public ActionResult Adicionar(UsuarioModel usuario)
        {
            conexao.Open();
            conexao.Query("INSERT INTO Amizade (idUsuarioOrigem,idUsuarioDestino,dataCriacao) values (@idUsuarioOrigem,@idUsuarioDestino,@dataCriacao)", new { idUsuarioOrigem = 1, idUsuarioDestino = usuario.Id,  dataCriacao = DateTime.Now });
            conexao.Close();
            return Redirect("/Usuario/perfil?Id="+usuario.Id);
        }
    }
}
