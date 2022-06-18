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
            List<UsuarioPerfilModel> usuarios = (List<UsuarioPerfilModel>)conexao.Query<UsuarioPerfilModel>("SELECT * FROM Usuario where Id=@id", new {id= int.Parse(id)});
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
            return RedirectToAction("Listar");
        }
    }
}
    