using LocadoraVeiculos.Models;
using System.Collections.Generic;

namespace LocadoraVeiculos.Models
{
    /// <summary>
    /// Representa um veículo disponível para aluguel na locadora.
    /// </summary>
    public class Veiculo
    {
        /// <summary>
        /// Identificador único do veículo.
        /// </summary>
        public int VeiculoId { get; set; }

        /// <summary>
        /// Modelo do veículo.
        /// </summary>
        public string Modelo { get; set; }

        /// <summary>
        /// Ano de fabricação do veículo.
        /// </summary>
        public int AnoFabricacao { get; set; }

        /// <summary>
        /// Quilometragem atual do veículo.
        /// </summary>
        public int Quilometragem { get; set; }

        /// <summary>
        /// Placa do veículo.
        /// </summary>
        public string Placa { get; set; }

        /// <summary>
        /// Indica se o veículo está disponível para aluguel.
        /// </summary>
        public bool Disponivel { get; set; }

        /// <summary>
        /// Identificador do fabricante do veículo.
        /// </summary>
        public int FabricanteId { get; set; }

        /// <summary>
        /// Referência ao fabricante do veículo.
        /// </summary>
        public Fabricante? Fabricante { get; set; }

        /// <summary>
        /// Lista de aluguéis associados a este veículo.
        /// </summary>
        public List<Aluguel>? Alugueis { get; set; }
    }
}
