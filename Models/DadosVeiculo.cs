using System.ComponentModel.DataAnnotations;

namespace ProjetoDsin.Models
{
    public class DadosVeiculo
    {
        public int Id { get; set; }

        public string Placa { get; set; } = string.Empty;
        [Required, MaxLength(7)]
        public string Modelo { get; set; } = string.Empty;
        [Required, MaxLength(15)]
        public string Fabricante { get; set; } = string.Empty;
        [Required, MaxLength(10)]
        public string Cor { get; set; } = string.Empty;
        [Required]
        public int Ano { get; set; }

        [Required]
        public int IdUsuario { get; set; }
        public Usuario? Usuario { get; set; }

        public DadosVeiculo() { }

        public DadosVeiculo(int id, string placa, string modelo, string fabricante, string cor, int ano)
        {
            Id = id;
            Placa = placa;
            Modelo = modelo;
            Fabricante = fabricante;
            Cor = cor;
            Ano = ano;
        }
    }
}

