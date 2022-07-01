using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreWebApp.Models
{
    public class Cliente 
    { 
        public enum SituacaoCliente 
        {
            Bloqueado,
            Cadastrado,
            Aprovado,
            Especial
        }
    
        [Key]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "O campo \"{0}\" é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O campo \"{0}\" deve ter no máximo {1} caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo \"{0}\" é obrigatório.")]
        [DataType(DataType.Date, ErrorMessage = "O campo \"{0}\" deve ter uma data válida.")]
        [DisplayName("Data de Nascimento")]        
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "O campo \"{0}\" é obrigatório.")]        
        [RegularExpression(@"[0-9]{11}$", ErrorMessage = "o campo \"{0}\" deve conter apenas números e ter 11 dígitos.")]
        [MaxLength(11, ErrorMessage = "O campo \"{0}\" deve ter no máximo {1} caracteres.")]
        [MinLength(11, ErrorMessage = "O campo \"{0}\" deve ter no mínimo {1} caracteres.")]
        [UIHint("_CustomCPF")]
        public string CPF { get; set; }

        [Required(ErrorMessage = "O campo \"{0}\" é obrigatório.")]
        [RegularExpression(@"[0-9]{11}$", ErrorMessage = "o campo \"{0}\" deve conter apenas números e ter 11 dígitos.")]
        [MaxLength(11, ErrorMessage = "O campo \"{0}\" deve ter no máximo {1} caracteres.")]
        [MinLength(11, ErrorMessage = "O campo \"{0}\" deve ter no mínimo {1} caracteres.")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "O campo \"{0}\" é obrigatório.")]
        [DisplayName("E-mail")]
        [EmailAddress(ErrorMessage = "O campo \"{0}\" dete conter um endereço de e-mail válido.")]
        [MaxLength(50, ErrorMessage = "O campo \"{0}\" deve ter no máximo {1} caracteres.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo \"{0}\" é de preenchimento obrigatório.")]
        [DisplayName("Situação")]
        public SituacaoCliente Situacao { get; set; }

        public Endereco Endereco { get; set; }

        public ICollection<Pedido> Pedidos { get; set; }

    }
}
