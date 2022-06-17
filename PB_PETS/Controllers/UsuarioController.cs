using Dapper;
using Microsoft.AspNetCore.Mvc;
using PB_PETS.Models;
using System.Data;

namespace PB_PETS.Controllers
{
    public class UsuarioController :Controller
    {
        private IDbConnection conexao;

        public UsuarioController(IDbConnection conexao)
        {
            this.conexao = conexao;
        }
        public ActionResult Perfil(string id)
        {
            conexao.Open();
            List<UsuarioModel> usuarios = (List<UsuarioModel>)conexao.Query<UsuarioModel>("SELECT * FROM Usuario where Id=@id", new {id= int.Parse(id)});
            conexao.Close();
            if (usuarios.Count > 0)
            {
                return View(usuarios[0]);
            };
            return View();
        }
        [HttpPost]

        public ActionResult Cadastrar(UsuarioModel dadosCliente)
        { 
            return RedirectToAction("listar");
        }
    }
}
    