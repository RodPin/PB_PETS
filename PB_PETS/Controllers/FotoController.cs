using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using PB_PETS.Models;
using System.Data;

namespace PB_PETS.Controllers
{
    public class FotoController : Controller
    {
        private string blobConnectionString = "DefaultEndpointsProtocol=https;AccountName=rodstorage212121;AccountKey=J65S5+UoCngu3gx1f1NDTfAxUaOMH6REVKpw6ItRjUWNLpBW2KxImHeLAk2S7e6WvJ8RSlqgPpJs+ASta9ILMg==;EndpointSuffix=core.windows.net";
        private string containerName = "containerfinalpb";
        private string bucketUrl = "https://rodstorage212121.blob.core.windows.net/containerfinalpb";
        private IDbConnection conexao;

        public FotoController(IDbConnection conexao)
        {
            this.conexao = conexao;
        }

        
    }
}
