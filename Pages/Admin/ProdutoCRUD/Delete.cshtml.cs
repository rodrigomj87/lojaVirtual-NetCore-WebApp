using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AspNetCoreWebApp.Data;
using AspNetCoreWebApp.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace AspNetCoreWebApp.Pages.ProdutoCRUD
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public DeleteModel(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [BindProperty]
        public Produto Produto { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Produto = await _context.Produto.FirstOrDefaultAsync(m => m.ProdutoId == id);

            if (Produto == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
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
                        "img\\produtos",
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
