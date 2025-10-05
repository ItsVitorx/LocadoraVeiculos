using System;

namespace LocadoraVeiculos.Models
{
    /// <summary>
    /// Representa um pagamento realizado referente a um aluguel.
    /// </summary>
    public class Pagamento
    {
        /// <summary>
        /// Identificador único do pagamento.
        /// </summary>
        public int PagamentoId { get; set; }

        /// <summary>
        /// Identificador do aluguel ao qual o pagamento está associado.
        /// </summary>
        public int AluguelId { get; set; }

        /// <summary>
        /// Aluguel associado ao pagamento.
        /// </summary>
        public Aluguel Aluguel { get; set; }

        /// <summary>
        /// Valor pago.
        /// </summary>
        public decimal ValorPago { get; set; }

        /// <summary>
        /// Data em que o pagamento foi realizado.
        /// </summary>
        public DateTime DataPagamento { get; set; }
    }
}
