using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AspNetCoreWebApp.Data;
using AspNetCoreWebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Linq;
using System;

namespace AspNetCoreWebApp.Pages.ProdutoCRUD
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public IndexModel(ApplicationDbContext context, IWebHostEnvironment env )
        {
            _context = context;
            _env = env;
        }

        public IList<Produto> Produto { get;set; }

        public async Task OnGetAsync()
        {
            Produto = await _context.Produto.OrderBy(p => p.Nome).ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int? id)
        {
            if (id == null)
            {
                return NotFound("Id do produto não encotrado");
            }

            var produto = await _context.Produto.FindAsync(id);

            if (produto != null)
            {
                _context.Produto.Remove(produto);
                if (await _context.SaveChangesAsync() > 0)
                {
                    var imgPath = Path.Combine(
                        _env.WebRootPath, 
                        "img\\produtos\\", 
                        produto.ProdutoId.ToString("D6") + ".jpg");
                    if (System.IO.File.Exists(imgPath))
                    {
                        System.IO.File.Delete(imgPath);
                    }
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
