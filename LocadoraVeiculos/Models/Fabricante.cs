using System.Collections.Generic;

namespace LocadoraVeiculos.Models
{
    public class Fabricante
    {
        public int FabricanteId { get; set; }
        public string Nome { get; set; }
        public string PaisOrigem { get; set; }
        public ICollection<Veiculo> Veiculos { get; set; }
    }
}
