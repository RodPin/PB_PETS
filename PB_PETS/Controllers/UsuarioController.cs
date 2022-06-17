using Microsoft.AspNetCore.Mvc;
using PB_PETS.Models;

namespace PB_PETS.Controllers
{
    public class UsuarioController :Controller
    {
        public ActionResult Cadastrar()
        {
            var viewModel = new UsuarioModel();

            return View(viewModel);
        }

        [HttpPost]

        public ActionResult Cadastrar(UsuarioModel dadosCliente)
        { 
            return RedirectToAction("listar");
        }
    }
}
    