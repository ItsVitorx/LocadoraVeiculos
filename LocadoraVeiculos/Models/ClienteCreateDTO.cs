using System.ComponentModel.DataAnnotations;

public class ClienteCreateDTO
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "CPF é obrigatório")]
    [RegularExpression(@"\d{11}", ErrorMessage = "CPF deve ter 11 dígitos")]
    public string CPF { get; set; }

    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Formato de email inválido")]
    public string Email { get; set; }

    [StringLength(20)]
    public string Telefone { get; set; }
}
