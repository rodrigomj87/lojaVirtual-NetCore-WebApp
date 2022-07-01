using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreWebApp.Data;
using AspNetCoreWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreWebApp.Pages.ClienteCRUD
{
    [Authorize(Policy = "isAdmin")]
    public class ListarModel : PageModel
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _usr;
        private readonly RoleManager<IdentityRole> _rMngr;

        public IList<Cliente> Clientes { get; set; }
        public IList<string> EmailsAdmins { get; set; }
        public ListarModel(ApplicationDbContext context, UserManager<AppUser> usr, RoleManager<IdentityRole> rMngr)
        {
            _context = context;
            _usr = usr;
            _rMngr = rMngr;
        }
        public async Task OnGetAsync()
        {
            EmailsAdmins = (await _usr.GetUsersInRoleAsync("admin")).
                Select(a => a.Email).ToList();
            Clientes = await _context.Clientes.ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int? id) 
        {
            if (id == null)
            {
                return NotFound("Id do cliente não encotrado");
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
                if (await _context.SaveChangesAsync() > 0)
                {
                    AppUser usuario = await _usr.FindByEmailAsync(cliente.Email);
                    
                    if (usuario != null) 
                        await _usr.DeleteAsync(usuario);                    
                }
            }

            return RedirectToPage("./Listar");
        }

        public async Task<IActionResult> OnPostDelAdminAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente != null)
            {
                AppUser usuario = await _usr.FindByNameAsync(cliente.Email);
                if (usuario != null)
                {
                    await _usr.RemoveFromRoleAsync(usuario, "admin");
                }
            }

            return RedirectToPage("./Listar");
        }

        public async Task<IActionResult> OnPostSetAdminAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente != null)
            {
                AppUser usuario = await _usr.FindByNameAsync(cliente.Email);
                if (usuario != null)
                {
                    if (!await _rMngr.RoleExistsAsync("admin"))
                        await _rMngr.CreateAsync(new IdentityRole("admin"));

                    await _usr.AddToRoleAsync(usuario, "admin");
                }
            }

            return RedirectToPage("./Listar");
        }
    }
}
