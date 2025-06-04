using System.ComponentModel.DataAnnotations;

namespace ProjetoDsin.Models
{
    public class DadosProprietario
    {
        public int Id { get; set; }

        public int IdDadosVeiculo { get; set; }
        public required DadosVeiculo DadosVeiculo { get; set; }

        public DadosProprietario() { }

        public DadosProprietario(int id, DadosVeiculo dadosVeiculo)
        {
            Id = id;
            DadosVeiculo = dadosVeiculo;
        }
    }
}

