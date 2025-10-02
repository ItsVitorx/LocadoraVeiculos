using System;
using System.ComponentModel.DataAnnotations;

public class AluguelCreateDTO
{
    [Required]
    public int ClienteId { get; set; }

    [Required]
    public int VeiculoId { get; set; }

    [Required]
    public DateTime DataInicio { get; set; }

    [Required]
    public DateTime DataFimPrevista { get; set; }

    [Range(0, int.MaxValue)]
    public int QuilometragemInicial { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal ValorDiaria { get; set; }
}
