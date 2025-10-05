using System;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// DTO utilizado para criar um novo aluguel.
/// </summary>
public class AluguelCreateDTO
{
    /// <summary>
    /// Identificador do cliente que realizará o aluguel.
    /// </summary>
    [Required(ErrorMessage = "ClienteId é obrigatório.")]
    public int ClienteId { get; set; }

    /// <summary>
    /// Identificador do veículo que será alugado.
    /// </summary>
    [Required(ErrorMessage = "VeiculoId é obrigatório.")]
    public int VeiculoId { get; set; }

    /// <summary>
    /// Data de início do aluguel.
    /// </summary>
    [Required(ErrorMessage = "DataInicio é obrigatória.")]
    public DateTime DataInicio { get; set; }

    /// <summary>
    /// Data prevista para término do aluguel.
    /// </summary>
    [Required(ErrorMessage = "DataFimPrevista é obrigatória.")]
    public DateTime DataFimPrevista { get; set; }

    /// <summary>
    /// Quilometragem inicial do veículo no início do aluguel.
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "QuilometragemInicial inválida.")]
    public int QuilometragemInicial { get; set; }

    /// <summary>
    /// Valor da diária do aluguel.
    /// </summary>
    [Range(0.01, double.MaxValue, ErrorMessage = "ValorDiaria deve ser maior que 0.")]
    public decimal ValorDiaria { get; set; }
}
