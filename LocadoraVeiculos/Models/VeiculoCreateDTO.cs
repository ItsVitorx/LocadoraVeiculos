using System.ComponentModel.DataAnnotations;

public class VeiculoCreateDTO
{
    [Required(ErrorMessage = "Modelo é obrigatório")]
    public string Modelo { get; set; }

    [Range(1900, 2100, ErrorMessage = "Ano de fabricação inválido")]
    public int AnoFabricacao { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Quilometragem inválida")]
    public int Quilometragem { get; set; }

    [Required(ErrorMessage = "Placa é obrigatória")]
    public string Placa { get; set; }

    [Required(ErrorMessage = "FabricanteId é obrigatório")]
    public int FabricanteId { get; set; }
}
