namespace PB_PETS.Models
{
    public class AmizadeModel
    {
        public int Id { get; set; }

        public int idUsuarioOrigem { get; set; }
        public int idUsuarioDestino { get; set; }
        public int statusDaSolicitacao { get; set; }
        public string nome { get; set; }
        public string sobrenome { get; set; }
        public DateTime dataCriacao{ get; set; }
    }
}
