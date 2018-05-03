using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BityBank.ViewModels
{
    public class UsuarioViewModel
    {
        [Display(Name = "Nome Completo"), Required(ErrorMessage = "Nome Completo Obrigatório")]
        public string NomeCompleto { get; set; }
        [Display(Name = "Nome de Usuario"), Required(ErrorMessage = "Usuario obrigatório")]
        public string UserName { get; set; }
        [EmailAddress(ErrorMessage = "Email Invalido"), Required(ErrorMessage = "Email Obrigatório")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Senha obrigatória"), DataType(DataType.Password)]
        public string Senha { get; set; }
    }
}