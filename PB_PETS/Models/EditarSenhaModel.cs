namespace PB_PETS.Models
{
    public class EditarSenhaModel
    {
        public int Id { get; set; }
        public string senhaAtual { get; set; }

        public string senha { get; set; }
        public string senha2 { get; set; }

        public bool erroSenhaAtual { get; set; }
        public bool erroSenhaNova { get; set; }


    }
}
