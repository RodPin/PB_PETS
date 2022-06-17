namespace PB_PETS.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }

        public string nome { get; set; }
        public string sobrenome { get; set; }
        public string email { get; set; }
        public string telefone { get; set; }
        public string endereco { get; set; }
        public string senha { get; set; }
        public DateTime dataCriacao { get; set; }

    }
}
