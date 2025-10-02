using System;

namespace LocadoraVeiculos.Models
{
    public class Pagamento
    {
        public int PagamentoId { get; set; }

        public int AluguelId { get; set; }
        public Aluguel Aluguel { get; set; }

        public decimal ValorPago { get; set; }
        public DateTime DataPagamento { get; set; }
    }
}
