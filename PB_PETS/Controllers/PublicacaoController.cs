using Azure.Storage.Blobs;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using PB_PETS.Models;
using System.Data;

namespace PB_PETS.Controllers
{
    public class PublicacaoController : Controller
    {
        private IDbConnection conexao;
        private string blobConnectionString = "DefaultEndpointsProtocol=https;AccountName=rodstorage212121;AccountKey=J65S5+UoCngu3gx1f1NDTfAxUaOMH6REVKpw6ItRjUWNLpBW2KxImHeLAk2S7e6WvJ8RSlqgPpJs+ASta9ILMg==;EndpointSuffix=core.windows.net";
        private string containerName = "containerfinalpb";
        private string bucketUrl = "https://rodstorage212121.blob.core.windows.net/containerfinalpb";


        public PublicacaoController(IDbConnection conexao)
        {
            this.conexao = conexao;
        }
        public ActionResult Listar()
        {
            conexao.Open();
            var publicacoes = conexao.Query<PublicacaoUsuarioModel>("select Publicacao.texto,Publicacao.foto , Publicacao.Id as idPublicacao, Publicacao.dataCriacao as publicacaoDataCriacao,Usuario.nome,Usuario.foto as fotoUsuario,Usuario.id as idUsuario from Publicacao INNER JOIN Usuario ON Publicacao.idUsuario=Usuario.Id ORDER BY publicacaoDataCriacao ASC");

            foreach (var publicacao in publicacoes)
            {
                publicacao.comentarios = (List<ComentarioUsuarioModel>)conexao.Query<ComentarioUsuarioModel>("SELECT * FROM Comentario INNER JOIN Usuario ON Usuario.Id=Comentario.idUsuario where idPublicacao=@idPublicacao", new { idPublicacao = publicacao.idPublicacao, idUsuario = publicacao.idUsuario });
                var curtidas = conexao.Query<CurtidaModel>("SELECT * FROM Curtidas where idPublicacao=@idPublicacao", new { idPublicacao = publicacao.idPublicacao });
                publicacao.curtidas = curtidas.Count();
                publicacao.isMine = publicacao.idUsuario == 1 ? true : false;
            };

            conexao.Close();

            return View(publicacoes);
        }

        [HttpPost]
        public async Task<ActionResult> PublicarAsync()
        {
            var file = this.HttpContext.Request.Form.Files.FirstOrDefault();

            var dict = this.HttpContext.Request.Form.ToDictionary(x => x.Key, x => x.Value.ToString());
            string texto = dict["texto"];

            var foto = "";

            if (file != null)
            {
                FotoController fotoController = new FotoController(conexao);
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
            conexao.Query<PublicacaoModel>("INSERT INTO Publicacao (idUsuario,texto,foto,dataCriacao) values(@idUsuario,@texto,@foto,@dataCriacao)", new { idUsuario = 1, texto= texto,foto=foto,dataCriacao = DateTime.Now });
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

        [HttpPost]
        public ActionResult putFoto(FotoModel publicacao)
        {
            conexao.Open();
            conexao.Query<PublicacaoModel>("UPDATE publicacao SET foto = @foto WHERE Id = @id", new { foto = publicacao.foto, id = publicacao.Id});
            conexao.Close();
            return Redirect("Listar");
        }
    }
}

