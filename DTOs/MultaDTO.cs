namespace ProjetoDsin.DTOs
{
    public class MultaDTO
    {
        public VeiculoDTO DadosVeiculo { get; set; } = new VeiculoDTO();
        public ProprietarioDTO DadosProprietario { get; set; } = new ProprietarioDTO();
        public InfracaoDTO DetalhesInfracao { get; set; } = new InfracaoDTO();
        public AnexoDTO Anexos { get; set; } = new AnexoDTO();
    }

    public class VeiculoDTO
    {
        public string Placa { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public string Fabricante { get; set; } = string.Empty;
        public string Cor { get; set; } = string.Empty;
        public int Ano { get; set; }            // Alterado de string para int, ano é numérico
        public int IdUsuario { get; set; }      // O ID do usuário logado
    }

    public class ProprietarioDTO
    {
        public string Nome { get; set; } = string.Empty;
        public string Cnh { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
    }

    public class InfracaoDTO
    {
        public string TipoInfracao { get; set; } = string.Empty;
        public string CodigoInfracao { get; set; } = string.Empty;
        public string LocalInfracao { get; set; } = string.Empty;  // Corrigido o nome (de "LocaInfracao" para "LocalInfracao")
        public DateTime Data { get; set; }          // Corrigido o tipo para DateTime
        public string Hora { get; set; } = string.Empty;
        public string Gravidade { get; set; } = string.Empty;
        public int PontosCnh { get; set; }          // Corrigido para int, pontos geralmente são numéricos
    }

    public class AnexoDTO
    {
        public byte[]? Evidencia { get; set; }  // Corrigido tipo para byte[], para evidências binárias
        public string Comentarios { get; set; } = string.Empty;
    }
}
