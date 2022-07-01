using AspNetCoreWebApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreWebApp.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarrinhoApiController : ControllerBase
    {
        private ApplicationDbContext _context;

        public CarrinhoApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("AtualizarItemCarrinho")]
        public async Task<JsonResult> AtualizarItemCarrinho([FromForm] string CarrinhoId,
            [FromForm] int? ProdutoId, [FromForm] int? quantidade)
        {
            if (string.IsNullOrEmpty(CarrinhoId) || (ProdutoId <= 0) ||
                (quantidade <= 0)) return new JsonResult(false);

            var pedido = await _context.Pedidos.Include("ItensPedido").
                FirstOrDefaultAsync(p => p.CarrinhoId == CarrinhoId);

            if (pedido != null)
            {
                if (pedido.Situacao == Models.Pedido.SituacaoPedido.Carrinho)
                {
                    var itemPedido = pedido.ItensPedido.FirstOrDefault(ip => ip.ProdutoId == ProdutoId);

                    if (itemPedido != null)
                    {
                        itemPedido.Quantidade = quantidade.Value;

                        if (_context.SaveChanges() > 0)
                        {
                            double valorPedido = pedido.ItensPedido.Sum(ip => ip.ValorItem);
                            var item = pedido.ItensPedido.Select(
                                x => new { id = x.ProdutoId, q = x.Quantidade, v = x.ValorItem }).
                                FirstOrDefault(ip => ip.id == ProdutoId);
                            var jsonRes = new JsonResult(new { v = valorPedido, item });
                            return jsonRes;
                        }

                    }
                }
            }
            return new JsonResult(false);
        }

        [HttpPost("ExcluirItemCarrinho")]
        public async Task<JsonResult> ExcluirItemCarrinho([FromForm] string CarrinhoId,
            [FromForm] int? ProdutoId = 0)
        {
            if (string.IsNullOrEmpty(CarrinhoId) || (ProdutoId <= 0)) return new JsonResult(false);

            var pedido = await _context.Pedidos.Include("ItensPedido").
                FirstOrDefaultAsync(p => p.CarrinhoId == CarrinhoId);

            if (pedido != null)
            {
                if (pedido.Situacao == Models.Pedido.SituacaoPedido.Carrinho)
                {
                    var itemPedido = pedido.ItensPedido.FirstOrDefault(ip => ip.ProdutoId == ProdutoId);
                    if (itemPedido != null)
                    {
                        pedido.ItensPedido.Remove(itemPedido);

                        if (_context.SaveChanges() > 0)
                        {
                            double valorPedido = pedido.ItensPedido.Sum(ip => ip.ValorItem);
                            var jsonRes = new JsonResult(new { v = valorPedido, id = ProdutoId });
                            return jsonRes;
                        }
                    }
                }
            }
            return new JsonResult(false);
        }
    }
}
