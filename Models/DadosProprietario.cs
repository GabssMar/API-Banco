using System.ComponentModel.DataAnnotations;

namespace ProjetoDsin.Models
{
    public class DadosProprietario
    {
        public int Id { get; set; }

        public string Nome { get; set; } = string.Empty;
        public string Cnh { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;

        public int IdDadosVeiculo { get; set; }
        public required DadosVeiculo DadosVeiculo { get; set; }

        public DadosProprietario() { }

        public DadosProprietario(int id, string nome, string cnh, string cpf)
        {
            Id = id;
            Nome = nome;
            Cnh = cnh;
            Cpf = cpf;
        }
    }
}

