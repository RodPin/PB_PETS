namespace PB_PETS.Models
{
    public class PublicacaoUsuarioModel
    {
        public int Id { get; set; }
        public int idUsuario { get; set; }
        public string foto { get; set; }
        public string texto { get; set; }
        public DateTime dataCriacao { get; set; }
        public string nome { get; set; }
        public string subrenome { get; set; }
        public string email { get; set; }
        public string telefone { get; set; }
        public string endereco { get; set; }
        public string senha { get; set; }

        public int idPublicacao { get; set; }

        public List<ComentarioUsuarioModel> comentarios { get; set; }
        public int curtidas { get; set; }
       
    }
}

