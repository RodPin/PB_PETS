namespace PB_PETS.Models
{
        public class ComentarioUsuarioModel
        {
            public int Id { get; set; }

            public int idUsuario { get; set; }
            public int idPublicacao { get; set; }
            public string texto { get; set; }
            public string nome { get; set; }
            public string foto { get; set; }
        public DateTime dataCriacao { get; set; }
        }
    }

