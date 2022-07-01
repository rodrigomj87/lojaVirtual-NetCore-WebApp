using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using AspNetCoreWebApp.Data;
using AspNetCoreWebApp.Models;
using Microsoft.AspNetCore.Hosting;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using AspNetCoreWebApp.Utils;

namespace AspNetCoreWebApp.Pages.ProdutoCRUD
{
    public class CreateModel : PageModel
    {

        [BindProperty]
        public Produto Produto { get; set; }

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public string CaminhoImagem { get; set; }

        [BindProperty]
        [Display(Name = "Imagem do Produto")]
        [Required(ErrorMessage = "O campo \"{0}\" é de preenchimento obrigatório.")]
        public IFormFile ImagemProduto { get; set; }

        public CreateModel(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            CaminhoImagem = "~/img/produtos/sem_imagem.jpg";
        }

        public IActionResult OnGet()
        {
            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (ImagemProduto == null)
            {
                return Page();
            }

            var produto = new Produto();

            if (await TryUpdateModelAsync(produto, Produto.GetType(), nameof(Produto)))
            {
                _context.Produto.Add(produto);
                await _context.SaveChangesAsync();
                await AppUtils.ProcessarArquivoDeImagem(produto.ProdutoId,
                    ImagemProduto, _webHostEnvironment);
                return RedirectToPage("./Index");
            }

            return Page();
        }
    }
}
