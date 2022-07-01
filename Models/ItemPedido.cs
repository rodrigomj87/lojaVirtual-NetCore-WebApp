using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetCoreWebApp.Models
{
    public class ItemPedido
    {
        [Required]
        public int PedidoId { get; set; }

        [Required]
        public int ProdutoId { get; set; }

        [Required(ErrorMessage = "O campo \"{0}\" é obrigatório.")]
        public float Quantidade { get; set; }

        [Required(ErrorMessage = "O campo \"{0}\" é obrigatório.")]
        [Display(Name = "Valor Unitário")]
        public double ValorUnitario { get; set; }

        [NotMapped]
        [Display(Name = "Valor do Item")]
        public double ValorItem
        {
            get
            {
                return Quantidade * ValorUnitario;
            }
        }

        [ForeignKey("PedidoId")]
        public Pedido Pedido { get; set; }

        [ForeignKey("ProdutoId")]
        public Produto Produto { get; set; }
    }
}