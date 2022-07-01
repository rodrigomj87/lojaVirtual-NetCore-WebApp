using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AspNetCoreWebApp.Data;
using AspNetCoreWebApp.Models;
using AspNetCoreWebApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace AspNetCoreWebApp.Pages
{
    public class RecuperarSenhaModel : PageModel
    {
        private UserManager<AppUser> _userManager;
        private readonly IEmailSender _emailSender;

        public class DadosEmail
        {
            [Required(ErrorMessage = "O campo \"{0}\" � de preenchimento obrigat�rio.")]
            [EmailAddress]
            [Display(Name = "E-mail")]
            [DataType(DataType.EmailAddress)]
            public string Email { get; set; }
        }

        public RecuperarSenhaModel(ApplicationDbContext context,
            UserManager<AppUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public DadosEmail Dados { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                AppUser usuario = await _userManager.FindByNameAsync(Dados.Email);
                if (usuario != null)
                {
                    string token = await _userManager.GeneratePasswordResetTokenAsync(usuario);
                    token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
                    var urlResetarSenha = Url.Page("/RedefinirSenha",
                        null, new { token }, Request.Scheme);

                    StringBuilder msg = new StringBuilder();
                    msg.Append("<h1>Digital Store :: Recupera��o de Senha</h1>");
                    msg.Append($"<p>Por favor, redefina sua senha <a href='{HtmlEncoder.Default.Encode(urlResetarSenha)}'>clicando aqui</a>.</p>");
                    msg.Append("<p>Atenciosamente<br>Equipe de Suporte Digital Store</p>");
                    await _emailSender.SendEmailAsync(usuario.Email, "Recupera��o de Senha", "", msg.ToString());
                    return RedirectToPage("/EmailRecuperacaoEnviado");
                }
                else
                {
                    //N�o � seguro informar ao usu�rio que o e-mail informado 
                    return RedirectToPage("/EmailRecuperacaoEnviado");
                    //ModelState.AddModelError("Dados.Email", "Nenhum usu�rio foi encontrado com este e-mail. " +
                    //    "Confira e tente novamente.");                    
                }
            }

            return Page();
        }
    }
}
