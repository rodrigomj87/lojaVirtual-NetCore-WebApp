using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetCoreWebApp.Models
{
    public class Pedido
    {
        public enum SituacaoPedido 
        {
            Cancelado,
            Realizado,
            Verificado,
            Atendido,
            Entregue,
            Carrinho
        }

        [Key]
        [Display(Name = "Código")]
        public int PedidoId { get; set; }

        [Required(ErrorMessage = "O campo \"{0}\" é obrigatório.")]
        [Display(Name = "Data/Hora")]
        public DateTime DataHoraPedido { get; set; }

        [Required(ErrorMessage = "O campo \"{0}\" é obrigatório.")]
        [Display(Name = "Valor Total")]
        public double ValorTotal { get; set; }

        [Required(ErrorMessage = "O campo \"{0}\" é obrigatório.")]
        [Display(Name = "Situação")]
        public SituacaoPedido Situacao { get; set; }

        public int? ClienteId { get; set; }

        public string CarrinhoId { get; set; }

        [ForeignKey("ClienteId")]
        public Cliente Cliente { get;}

        public Endereco Endereco { get; set; }

        public ICollection<ItemPedido> ItensPedido { get; set; }

    }
}