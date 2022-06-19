namespace PB_PETS.Models
{
    public class AmizadeList
    {
        public IEnumerable<AmizadeModel> novasAmizades { get; set; }

        public IEnumerable<PublicacaoUsuarioModel> postsAmigos { get; set; }
    }
}
