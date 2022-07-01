using AspNetCoreWebApp.Data;
using AspNetCoreWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private const int tamanhoPagina = 12;

        private readonly ILogger<IndexModel> _logger;

        public ApplicationDbContext _context { get; set; }

        public IList<Produto> Produtos;

        public int PaginaAtual { get; set; }

        public int QuantidadePaginas { get; set; }

        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task OnGetAsync([FromQuery(Name = "q")] string termoBusca,
             [FromQuery(Name = "o")] int? ordem = 1, [FromQuery(Name = "p")] int? pagina = 1)
        {
            this.PaginaAtual = pagina.Value;

            var query = _context.Produto.AsQueryable();

            if (!string.IsNullOrEmpty(termoBusca))
            {
                query = query.Where(
                    p => p.Nome.ToLower()
                    .Replace("à", "a")
                    .Replace("â", "a")
                    .Replace("ä", "a")
                    .Replace("ç", "c")
                    .Replace("é", "e")
                    .Replace("è", "e")
                    .Replace("ê", "e")
                    .Replace("ë", "e")
                    .Replace("î", "i")
                    .Replace("ï", "i")
                    .Replace("ô", "o")
                    .Replace("ó", "o")
                    .Replace("ù", "u")
                    .Replace("û", "u")
                    .Replace("ü", "u")
                    .Contains(RemoveDiacritics(termoBusca).ToLower()));
            }


            if (ordem.HasValue)
            {
                switch (ordem.Value)
                {
                    case 1:
                        query = query.OrderBy(p => p.Nome.ToLower());
                        break;
                    case 2:
                        query = query.OrderBy(p => p.Preco);
                        break;
                    case 3:
                        query = query.OrderByDescending(p => p.Preco);
                        break;
                }
            }

            var queryCount = query;
            int qtdeProdutos = queryCount.Count();
            this.QuantidadePaginas = Convert.ToInt32(Math.Ceiling(qtdeProdutos * 1M / tamanhoPagina));

            query = query.Skip(tamanhoPagina * (this.PaginaAtual - 1)).Take(tamanhoPagina);

            Produtos = await query.ToListAsync();
        }

        static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
