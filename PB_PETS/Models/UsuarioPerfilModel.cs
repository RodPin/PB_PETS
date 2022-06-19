namespace PB_PETS.Models
{
    public class UsuarioPerfilModel
    {
            public int Id { get; set; }

            public string nome { get; set; }
            public string sobrenome { get; set; }
            public string email { get; set; }
            public string telefone { get; set; }
            public string endereco { get; set; }
            public bool isAmigo { get; set; }
            public bool isSolicitado { get; set; }
            public string senha { get; set; }
        public DateTime dataCriacao { get; set; }


    }
}
