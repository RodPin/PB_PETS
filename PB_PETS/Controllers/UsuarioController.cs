using Azure.Storage.Blobs;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using PB_PETS.Helpers;
using PB_PETS.Models;
using System.Data;

namespace PB_PETS.Controllers
{
    public class UsuarioController : Controller
    {
        private IDbConnection conexao;
        private string blobConnectionString = "DefaultEndpointsProtocol=https;AccountName=rodstorage212121;AccountKey=J65S5+UoCngu3gx1f1NDTfAxUaOMH6REVKpw6ItRjUWNLpBW2KxImHeLAk2S7e6WvJ8RSlqgPpJs+ASta9ILMg==;EndpointSuffix=core.windows.net";
        private string containerName = "containerfinalpb";
        private string bucketUrl = "https://rodstorage212121.blob.core.windows.net/containerfinalpb";
        private LoggedUser loggedUser = new LoggedUser();

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
                List<AmizadeModel> amizades = (List<AmizadeModel>)conexao.Query<AmizadeModel>("SELECT * FROM Amizade where IdUsuarioDestino=@id AND idUsuarioOrigem=@meuId OR IdUsuarioDestino=@meuId AND idUsuarioOrigem=@id", new { id = int.Parse(id), meuId = loggedUser.getLoggedUser() });
                var usuario = usuarios[0];

                usuario.isSolicitado = false;
                usuario.isAmigo = false;
                usuario.isMine = usuario.Id == loggedUser.getLoggedUser() ? true : false;
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
            changeSenha.Id = loggedUser.getLoggedUser();
            return View(changeSenha);
        }

        [HttpPost]
        public ActionResult EditarSenha(EditarSenhaModel usuarioNovaSenha)
        {

            var changeSenha = new EditarSenhaModel();
            conexao.Open();
            List<UsuarioPerfilModel> usuarios = (List<UsuarioPerfilModel>)conexao.Query<UsuarioPerfilModel>("SELECT * FROM Usuario where Id=@id", new { id = usuarioNovaSenha.Id });
            conexao.Close();

            if (usuarios.Count == 0)
            {
                return RedirectToAction("/");
            };
            changeSenha.senhaAtual = usuarioNovaSenha.senhaAtual;
            changeSenha.senha = usuarioNovaSenha.senha;
            changeSenha.senha2 = usuarioNovaSenha.senha2;

            var usuario = usuarios[0];
            if (usuario.senha != usuarioNovaSenha.senhaAtual)
            {
                changeSenha.erroSenhaAtual = true;
                return View(changeSenha);
            }
            if (usuarioNovaSenha.senha != usuarioNovaSenha.senha2)
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

        [HttpPost]

        public async Task<ActionResult> putFoto()
        {
            var file = this.HttpContext.Request.Form.Files.FirstOrDefault();

            var dict = this.HttpContext.Request.Form.ToDictionary(x => x.Key, x => x.Value.ToString());
            int id = int.Parse(dict["id"]);

            var foto = "";

            if (file != null)
            {
                if (file == null)
                {
                    return BadRequest();
                }
                else if (file.ContentType != "image/jpg" && file.ContentType != "image/png" && file.ContentType != "image/jpeg" && file.ContentType != "image/bmp" && file.ContentType != "image/gif")
                {
                    return BadRequest();
                }

                using (var stream = file.OpenReadStream())
                {
                    string fileName = Guid.NewGuid().ToString("N") + "." + file.ContentType.Split('/')[1];

                    // Conecta no Azure Blob Storage
                    BlobServiceClient client = new BlobServiceClient(blobConnectionString);
                    // Pega a referencia do container
                    var containerClient = client.GetBlobContainerClient(containerName);
                    // Cria a referencia do nome do arquivo
                    BlobClient blob = containerClient.GetBlobClient(fileName);
                    // Sobe para Azure
                    await blob.UploadAsync(stream);
                    foto = $"{bucketUrl}/{fileName}";
                }
            }

            conexao.Open();
            conexao.Query<UsuarioModel>("UPDATE Usuario SET foto = @foto where Id = @idUsuario", new { idUsuario = id, foto = foto });
            conexao.Close();
            return Redirect("/usuario/EditarPerfil" + "?Id=" + id.ToString());
        }

        
         [HttpPost]
        public ActionResult deleteFoto(UsuarioModel usuario)
        {
            conexao.Open();
            conexao.Query<UsuarioPerfilModel>("UPDATE Usuario SET foto=@foto where Id =@id", new
            {
                id = usuario.Id,
                foto=""
            });
            conexao.Close();


            return Redirect("/usuario/EditarPerfil" + "?Id=" + usuario.Id);
        }

        [HttpPost]

        public ActionResult redirectPerfil()
        {
            return Redirect("/usuario/Perfil" + "?Id=" + loggedUser.getLoggedUser());

        }
    }
}
    