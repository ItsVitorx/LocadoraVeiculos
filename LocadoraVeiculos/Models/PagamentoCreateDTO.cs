using System;
using System.ComponentModel.DataAnnotations;

namespace LocadoraVeiculos.DTOs
{
    /// <summary>
    /// DTO utilizado para criar um novo pagamento.
    /// </summary>
    public class PagamentoCreateDTO
    {
        /// <summary>
        /// Identificador do aluguel relacionado ao pagamento.
        /// </summary>
        [Required(ErrorMessage = "AluguelId é obrigatório")]
        public int AluguelId { get; set; }

        /// <summary>
        /// Valor pago pelo cliente.
        /// </summary>
        [Required(ErrorMessage = "ValorPago é obrigatório")]
        [Range(0.01, double.MaxValue, ErrorMessage = "ValorPago deve ser maior que zero")]
        public decimal ValorPago { get; set; }

        /// <summary>
        /// Data do pagamento.
        /// </summary>
        [Required(ErrorMessage = "DataPagamento é obrigatória")]
        public DateTime DataPagamento { get; set; }
    }
}
