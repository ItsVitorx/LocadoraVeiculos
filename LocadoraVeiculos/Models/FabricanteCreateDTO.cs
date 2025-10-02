using LocadoraVeiculos.Models;
using System.ComponentModel.DataAnnotations;

public class FabricanteCreateDTO
{
    [Required]
    public string Nome { get; set; }

    [Required]
    public string PaisOrigem { get; set; }

}
