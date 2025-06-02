namespace ProjetoDsin.DTOs
{
    public class MultaDTO
    {
        public VeiculoDTO DadosVeiculo { get; set; }
        public ProprietarioDTO DadosProprietario { get; set; }
        public InfracaoDTO DetalhesInfracao { get; set; }
        public AnexoDTO Anexos { get; set; }
    }

    public class VeiculoDTO
    {
        public string Placa { get; set; }
        public string Modelo { get; set; }
        public string Fabricante { get; set; }
        public string Cor { get; set; }
        public string Ano { get; set; }
        public int IdUsuario { get; set; } // O ID do usuário logado
    }

    public class ProprietarioDTO
    {
        public string Nome { get; set; }
        public string Cnh { get; set; }
        public string Cpf { get; set; }
    }

    public class InfracaoDTO
    {
        public string TipoInfracao { get; set; }
        public string CodigoInfracao { get; set; }
        public string LocaInfracao { get; set; }
        public string Data { get; set; }
        public string Hota { get; set; }
        public string Gravidade { get; set; }
        public string PontosCnh { get; set; }
    }

    public class AnexoDTO
    {
        public string Evidencia { get; set; }
        public string Comentarios { get; set; }
    }
}

