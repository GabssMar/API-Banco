using System.ComponentModel.DataAnnotations;

namespace ProjetoDsin.Models
{
    public class DetalhesInfracao
    {
        public int Id { get; set; }

        public string TipoInfracao { get; set; } = string.Empty;
        [Required]
        public string CodigoInfracao { get; set; } = string.Empty;
        public string LocaInfracao { get; set; } = string.Empty;
        [Required, MaxLength(50)]
        public string Data { get; set; } = string.Empty;
        [Required]
        public string Hota { get; set; } = string.Empty;
        [Required]
        public string Gravidade { get; set; } = string.Empty;
        public string PontosCnh { get; set; } = string.Empty;

        public int IdDadosVeiculo { get; set; }
        public required DadosVeiculo DadosVeiculo { get; set; }

        public DetalhesInfracao() { }

        public DetalhesInfracao(int id, string tipoInfracao, string codigoInfracao, string locaInfracao,
            string data, string hota, string gravidade, string pontosCnh, DadosVeiculo dadosVeiculo)
        {
            Id = id;
            TipoInfracao = tipoInfracao;
            CodigoInfracao = codigoInfracao;
            LocaInfracao = locaInfracao;
            Data = data;
            Hota = hota;
            Gravidade = gravidade;
            PontosCnh = pontosCnh;
            DadosVeiculo = dadosVeiculo;
        }
    }
}

