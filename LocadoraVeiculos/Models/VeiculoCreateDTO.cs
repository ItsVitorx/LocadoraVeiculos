using System.ComponentModel.DataAnnotations;

namespace LocadoraVeiculos.DTOs
{
    /// <summary>
    /// DTO para criar um novo veículo na locadora.
    /// </summary>
    public class VeiculoCreateDTO
    {
        /// <summary>
        /// Modelo do veículo.
        /// </summary>
        [Required(ErrorMessage = "Modelo é obrigatório")]
        public string Modelo { get; set; }

        /// <summary>
        /// Ano de fabricação do veículo.
        /// </summary>
        [Range(1900, 2100, ErrorMessage = "Ano de fabricação inválido")]
        public int AnoFabricacao { get; set; }

        /// <summary>
        /// Quilometragem inicial do veículo.
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Quilometragem inválida")]
        public int Quilometragem { get; set; }

        /// <summary>
        /// Placa do veículo.
        /// </summary>
        [Required(ErrorMessage = "Placa é obrigatória")]
        public string Placa { get; set; }

        /// <summary>
        /// Identificador do fabricante do veículo.
        /// </summary>
        [Required(ErrorMessage = "FabricanteId é obrigatório")]
        public int FabricanteId { get; set; }
    }
}
