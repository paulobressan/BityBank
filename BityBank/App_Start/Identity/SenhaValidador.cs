using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace BityBank.App_Start.Identity
{
    //classe criada para definir validações de senhas
    public class SenhaValidador : IIdentityValidator<string>
    {
        //propriedades de validações
        public int TamanhoRequerido { get; set; }
        public bool ObrigatorioCaracterEspecial { get; set; }
        public bool ObrigatorioLowerCase { get; set; }
        public bool ObrigatorioUpperCase { get; set; }
        public bool ObrigatorioDigitos { get; set; } //numericos
        //metodo implementado pela interface
        public async Task<IdentityResult> ValidateAsync(string item)
        {
            var erros = new List<string>();
            //if (ObrigatorioCaracterEspecial)
            //    erros.Add("A senha deve conter caracteres especiais");

            //verificar se tudo esta valido, se não adicionar um erro
            if (ObrigatorioDigitos && !VerificarDigitos(item))
                erros.Add("A senha deve conter no minimo um digito");
            if (ObrigatorioLowerCase && !VerificarLowerCase(item))
                erros.Add("A senha deve conter no minimo um caracter minusculo");
            if (ObrigatorioUpperCase && !VerificarUpperCase(item))
                erros.Add("A senha deve conter no minimo um caracter maiusculo");
            if (!VerificarTamanhoRequerido(item))
                erros.Add($"A senha deve conter no minimo {TamanhoRequerido} caracter");

            //existe algum item na lista de erros?
            if (erros.Any())
                //se sim, retornar o identityResult com falhas
                return IdentityResult.Failed(erros.ToArray());
            else
                return IdentityResult.Success;
        }

        //metodo com arrow operator
        private bool VerificarTamanhoRequerido(string senha) =>
             senha?.Length >= TamanhoRequerido;

        //private bool VerificarCaracteresEspeciais(string senha) =>
        //    Regex.IsMatch(senha, EXPRESSAO REGULAR)

            //verificar se exeiste caracter lower case na senha
        private bool VerificarLowerCase(string senha) =>
            senha.Any(char.IsLower);

        //verificar se existe upper case na senha
        private bool VerificarUpperCase(string senha)=>
            senha.Any(char.IsUpper);

        //verificar se existe digitos na senha
        private bool VerificarDigitos(string senha) =>
            senha.Any(char.IsDigit);
    }
}