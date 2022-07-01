using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreWebApp.Data;
using AspNetCoreWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetCoreWebApp.Pages.ClienteCRUD
{
    [Authorize(Policy = "isAdmin")]
    public class IncluirModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public Cliente Cliente { get; set; }
        public IncluirModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var cliente = new Cliente();
            cliente.Endereco = new Endereco();
            //novos clientes sempre iniciam com essa situação
            cliente.Situacao = Cliente.SituacaoCliente.Cadastrado;

            if (await TryUpdateModelAsync(cliente, Cliente.GetType(), nameof(Cliente)))
            {
                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Listar");
            }

            return Page();
        }

    }
}
