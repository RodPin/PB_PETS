namespace PB_PETS.Models
{
    public class ComentarioModel
    {
         public int Id { get; set; }

        public int idUsuario{ get; set; }
        public int idPublicacao{ get; set; }
        public string texto { get; set; }
        public DateTime dataCriacao { get; set; }
    }
}
