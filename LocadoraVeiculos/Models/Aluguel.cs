using System;
using System.Collections.Generic;

namespace LocadoraVeiculos.Models
{
    /// <summary>
    /// Representa um aluguel de veículo realizado por um cliente.
    /// </summary>
    public class Aluguel
    {
        /// <summary>
        /// Identificador único do aluguel.
        /// </summary>
        public int AluguelId { get; set; }

        /// <summary>
        /// Identificador do cliente que realizou o aluguel.
        /// </summary>
        public int ClienteId { get; set; }

        /// <summary>
        /// Cliente associado ao aluguel.
        /// </summary>
        public Cliente Cliente { get; set; }

        /// <summary>
        /// Identificador do veículo alugado.
        /// </summary>
        public int VeiculoId { get; set; }

        /// <summary>
        /// Veículo associado ao aluguel.
        /// </summary>
        public Veiculo Veiculo { get; set; }

        /// <summary>
        /// Data de início do aluguel.
        /// </summary>
        public DateTime DataInicio { get; set; }

        /// <summary>
        /// Data prevista para término do aluguel.
        /// </summary>
        public DateTime DataFimPrevista { get; set; }

        /// <summary>
        /// Data de devolução do veículo (pode ser nula se não devolvido).
        /// </summary>
        public DateTime? DataDevolucao { get; set; }

        /// <summary>
        /// Quilometragem do veículo no momento do início do aluguel.
        /// </summary>
        public double QuilometragemInicial { get; set; }

        /// <summary>
        /// Quilometragem do veículo no momento da devolução (pode ser nula se não devolvido).
        /// </summary>
        public double? QuilometragemFinal { get; set; }

        /// <summary>
        /// Valor da diária do aluguel.
        /// </summary>
        public decimal ValorDiaria { get; set; }

        /// <summary>
        /// Valor total do aluguel calculado.
        /// </summary>
        public decimal ValorTotal { get; set; }

        /// <summary>
        /// Pagamentos associados a este aluguel.
        /// </summary>
        public ICollection<Pagamento> Pagamentos { get; set; }
    }
}
