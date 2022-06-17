namespace PB_PETS.Models
{
    public class PublicacaoModel
    {
        public int Id { get; set; }
        public int idUsuario { get; set; }
        public string foto { get; set; }
        public string texto { get; set; }
        public DateTime dataCriacao { get; set; }
    }
}
