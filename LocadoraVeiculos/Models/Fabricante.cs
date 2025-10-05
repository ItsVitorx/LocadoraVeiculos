using System.Collections.Generic;

namespace LocadoraVeiculos.Models
{
    /// <summary>
    /// Representa um fabricante de veículos.
    /// </summary>
    public class Fabricante
    {
        /// <summary>
        /// Identificador único do fabricante.
        /// </summary>
        public int FabricanteId { get; set; }

        /// <summary>
        /// Nome do fabricante.
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// País de origem do fabricante.
        /// </summary>
        public string PaisOrigem { get; set; }

        /// <summary>
        /// Lista de veículos associados a este fabricante.
        /// </summary>
        public ICollection<Veiculo> Veiculos { get; set; }
    }
}
