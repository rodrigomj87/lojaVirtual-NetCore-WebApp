using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreWebApp.Models
{
    [Owned]
    public class Endereco
    {
        [Required(ErrorMessage = "O campo \"{0}\" é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O campo \"{0}\" deve ter no máximo {1} caracteres.")]
        [Display(Name = "Endereço Linha 1")]
        public string Logradouro { get; set; }

        [Required(ErrorMessage = "O campo \"{0}\" é obrigatório.")]
        [MaxLength(10, ErrorMessage = "O campo \"{0}\" deve ter no máximo {1} caracteres.")]
        [Display(Name = "Número")]
        public string Numero { get; set; }
        
        [MaxLength(100, ErrorMessage = "O campo \"{0}\" deve ter no máximo {1} caracteres.")]
        public string Complemento { get; set; }

        [Required(ErrorMessage = "O campo \"{0}\" é obrigatório.")]
        [MaxLength(50, ErrorMessage = "O campo \"{0}\" deve ter no máximo {1} caracteres.")]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "O campo \"{0}\" é obrigatório.")]
        [MaxLength(50, ErrorMessage = "O campo \"{0}\" deve ter no máximo {1} caracteres.")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "O campo \"{0}\" é obrigatório.")]
        [MaxLength(2, ErrorMessage = "O campo \"{0}\" deve ter no máximo {1} caracteres.")]
        public string Estado { get; set; }

        [Required(ErrorMessage = "O campo \"{0}\" é obrigatório.")]
        [MaxLength(8, ErrorMessage = "O campo \"{0}\" deve ter no máximo {1} caracteres.")]
        [RegularExpression(@"[0-9]{8}$", ErrorMessage = "O campo \"{0}\" deve ser preenchido com um CEP válido.")]
        [UIHint("_CepTemplate")]
        public string CEP { get; set; }

        [MaxLength(8, ErrorMessage = "O campo \"{0}\" deve ter no máximo {1} caracteres.")]
        [Display(Name = "Referência")]
        public string Referencia { get; set; }

        
        [MaxLength(100, ErrorMessage = "O campo \"{0}\" deve ter no máximo {1} caracteres.")]
        [Display(Name = "Endereço Linha 2")]
        public string Logradouro2 { get; set; }
    }
}