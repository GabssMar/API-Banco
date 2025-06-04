namespace ProjetoDsin.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public int CodigoAgente { get; set; }
        public int CodigoOrg { get; set; }

        public Usuario() { }

        public Usuario(int id, string nome, string email, string senha, int codigoAgente, int codigoOrg)
        {
            Id = id;
            Nome = nome;
            Email = email;
            Senha = senha;
            CodigoAgente = codigoAgente;
            CodigoOrg = codigoOrg;
        }
    }
}
