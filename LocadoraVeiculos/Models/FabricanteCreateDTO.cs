using LocadoraVeiculos.Models;
using System.ComponentModel.DataAnnotations;

namespace LocadoraVeiculos.DTOs
{
    /// <summary>
    /// DTO para criação de um fabricante.
    /// </summary>
    public class FabricanteCreateDTO
    {
        /// <summary>
        /// Nome do fabricante.
        /// </summary>
        [Required(ErrorMessage = "Nome é obrigatório")]
        public string Nome { get; set; }

        /// <summary>
        /// País de origem do fabricante.
        /// </summary>
        [Required(ErrorMessage = "PaisOrigem é obrigatório")]
        public string PaisOrigem { get; set; }
    }
}
