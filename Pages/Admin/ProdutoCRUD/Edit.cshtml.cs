using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AspNetCoreWebApp.Data;
using AspNetCoreWebApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using AspNetCoreWebApp.Utils;

namespace AspNetCoreWebApp.Pages.ProdutoCRUD
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        [BindProperty]
        public Produto Produto { get; set; }

        public string CaminhoImagem { get; set; }

        [BindProperty]
        [Display(Name = "Imagem do Produto")]
        public IFormFile ImagemProduto { get; set; }

        public EditModel(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Produto = await _context.Produto.FirstOrDefaultAsync(p => p.ProdutoId == id);

            if (Produto == null)
            {
                return NotFound();
            }
            CaminhoImagem = $"~/img/produtos/{Produto.ProdutoId:D6}.jpg";
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Produto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                if (ImagemProduto != null)
                {
                    await AppUtils.ProcessarArquivoDeImagem(Produto.ProdutoId,
                        ImagemProduto, _webHostEnvironment);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoExists(Produto.ProdutoId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ProdutoExists(int id)
        {
            return _context.Produto.Any(e => e.ProdutoId == id);
        }
    }
}
