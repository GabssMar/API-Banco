namespace ProjetoDsin.Models
{
    public class Anexos
    {
        public int Id { get; set; }

        public string Evidencia { get; set; } = string.Empty;
        public string Comentarios { get; set; } = string.Empty;

        public int IdDadosVeiculo { get; set; }
        public required DadosVeiculo DadosVeiculo { get; set; }

        public Anexos() { }

        public Anexos(int id, string evidencia, string comentarios, DadosVeiculo dadosVeiculo)
        {
            Id = id;
            Evidencia = evidencia;
            Comentarios = comentarios;
            DadosVeiculo = dadosVeiculo;
        }
    }
}

