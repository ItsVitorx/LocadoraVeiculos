using LocadoraVeiculos.Models;

public class Veiculo
{
    public int VeiculoId { get; set; }
    public string Modelo { get; set; }
    public int AnoFabricacao { get; set; }
    public int Quilometragem { get; set; }
    public string Placa { get; set; }
    public bool Disponivel { get; set; }

    public int FabricanteId { get; set; }

    public Fabricante? Fabricante { get; set; }

    public List<Aluguel>? Alugueis { get; set; }
}
