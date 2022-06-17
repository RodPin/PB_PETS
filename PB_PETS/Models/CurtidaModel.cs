namespace PB_PETS.Models
{
    public class CurtidaModel
    {
        public int Id { get; set; }

        public UsuarioModel usuario { get; set; }
        public PublicacaoModel publicacao { get; set; }
        public DateTime dataCriacao { get; set; }

    }
}
