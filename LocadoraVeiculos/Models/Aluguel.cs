using System;
using System.Collections.Generic;

namespace LocadoraVeiculos.Models
{
    public class Aluguel
    {
        public int AluguelId { get; set; }

        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        public int VeiculoId { get; set; }
        public Veiculo Veiculo { get; set; }

        public DateTime DataInicio { get; set; }
        public DateTime DataFimPrevista { get; set; }
        public DateTime? DataDevolucao { get; set; }
        public double QuilometragemInicial { get; set; }
        public double? QuilometragemFinal { get; set; }
        public decimal ValorDiaria { get; set; }
        public decimal ValorTotal { get; set; }

        public ICollection<Pagamento> Pagamentos { get; set; }
    }
}
