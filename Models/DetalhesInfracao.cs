using System.ComponentModel.DataAnnotations;

namespace ProjetoDsin.Models
{
    public class DetalhesInfracao
    {
        public int Id { get; set; }

        public string TipoInfracao { get; set; } = string.Empty;
        [Required]
        public string CodigoInfracao { get; set; } = string.Empty;
        public string LocalInfracao { get; set; } = string.Empty;
        [Required, MaxLength(50)]
        public string Data { get; set; } = string.Empty;
        [Required]
        public string Hora { get; set; } = string.Empty;
        [Required]
        public string Gravidade { get; set; } = string.Empty;
        public int PontosCnh { get; set; }

        public int IdDadosVeiculo { get; set; }
        public required DadosVeiculo DadosVeiculo { get; set; }

        public DetalhesInfracao() { }

        public DetalhesInfracao(int id, string tipoInfracao, string codigoInfracao, string localInfracao,
            DateTime data, string hora, string gravidade, int pontosCnh, DadosVeiculo dadosVeiculo)
        {
            Id = id;
            TipoInfracao = tipoInfracao;
            CodigoInfracao = codigoInfracao;
            LocalInfracao = localInfracao;
            Data = data.ToString("yyyy-MM-dd");
            Hora = hora;
            Gravidade = gravidade;
            PontosCnh = pontosCnh;
            DadosVeiculo = dadosVeiculo;
        }
    }
}

