using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreWebApp.Data;
using AspNetCoreWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreWebApp.Pages.ClienteCRUD
{
    [Authorize(Policy = "isAdmin")]
    public class AlterarModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public Cliente Cliente { get; set; }
        public AlterarModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound("O Código de cliente não foi enviado");
            }

            Cliente = await _context.Clientes.FirstOrDefaultAsync(_ => _.ClienteId == id);
            if (Cliente == null)
            {
                return NotFound("Cliente não encontrado");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //para garantir que o CPF e o E-mail não serão atualizados
            var cliente = await _context.Clientes.Select(m => new { m.ClienteId, m.Email, m.CPF }).FirstOrDefaultAsync(m => m.ClienteId == Cliente.ClienteId);
            Cliente.Email = cliente.Email;
            Cliente.CPF = cliente.CPF;

            //ModelState.ClearValidationState("Cliente.Email");
            //ModelState.ClearValidationState("Cliente.CPF");

            if (ModelState.Keys.Contains("Cliente.Email"))
            {
                ModelState["Cliente.Email"].Errors.Clear();
                ModelState.Remove("Cliente.Email");
            }
            if (ModelState.Keys.Contains("Cliente.CPF"))
            {
                ModelState["Cliente.CPF"].Errors.Clear();
                ModelState.Remove("Cliente.CPF");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Cliente).State = EntityState.Modified;
            _context.Attach(Cliente.Endereco).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteAindaExiste(Cliente.ClienteId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Listar");
        }

        private bool ClienteAindaExiste(int id)
        {
            return _context.Clientes.Any(c => c.ClienteId == id);
        }
    }
}
