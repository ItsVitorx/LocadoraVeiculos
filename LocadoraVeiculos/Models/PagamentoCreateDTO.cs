using System;
using System.ComponentModel.DataAnnotations;

namespace LocadoraVeiculos.DTOs
{
    public class PagamentoCreateDTO
    {
        public int AluguelId { get; set; }
        public decimal ValorPago { get; set; }
        public DateTime DataPagamento { get; set; }
    }
}

