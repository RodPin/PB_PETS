namespace PB_PETS.Models
{
    public class AmizadeModel
    {
        public int Id { get; set; }

        public UsuarioModel usuarioOrigem { get; set; }
        public UsuarioModel usuarioDestino { get; set; }

        public int statusDaSolicitacao { get; set; }
        public DateTime dataCriacao{ get; set; }
    }
}
