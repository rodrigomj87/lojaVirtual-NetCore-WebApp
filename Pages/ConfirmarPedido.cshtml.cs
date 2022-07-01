using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AspNetCoreWebApp.Data;
using AspNetCoreWebApp.Models;

namespace QuitandaOnline.Pages
{
    [Authorize(Roles = "cliente")]
    public class ConfirmarPedidoModel : PageModel
    {
        private ApplicationDbContext _context;
        public string COOKIE_NAME
        {
            get { return ".AspNetCore.CartId"; }
        }

        public Pedido Pedido { get; set; }
        public Cliente Cliente { get; set; }

        public ConfirmarPedidoModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (Request.Cookies.ContainsKey(COOKIE_NAME))
            {
                var cartId = Request.Cookies[COOKIE_NAME];

                Pedido = await _context.Pedidos.Include(p => p.ItensPedido).ThenInclude(ip => ip.Produto).
                    FirstOrDefaultAsync(p => p.CarrinhoId == cartId);                

                if (Pedido != null)
                {
                    if ((Pedido.ItensPedido != null) && (Pedido.ItensPedido.Count > 0))
                    {
                        if (Pedido.Situacao == Pedido.SituacaoPedido.Carrinho)
                        {
                            Cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Email == User.Identity.Name);
                            Pedido.ClienteId = Cliente.ClienteId;
                            Pedido.Endereco = Cliente.Endereco;
                            Pedido.ValorTotal = Pedido.ItensPedido.Sum(x => x.Quantidade * Convert.ToDouble(x.ValorUnitario));
                            await _context.SaveChangesAsync();
                            return Page();
                        }
                    }                    
                }
            }

            return RedirectToPage("/Carrinho");
        }
    }
}