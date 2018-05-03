using BityBank.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using BityBank.App_Start.Identity;

//definir o tipo da classe de inicialização do owin
[assembly: OwinStartup(typeof(BityBank.Startup))]
namespace BityBank
{
    public class Startup
    {
        public void Configuration(IAppBuilder builder)
        {
            //criando o db context
            builder.CreatePerOwinContext<DbContext>(() =>
                new IdentityDbContext<Usuario>("conexao")
            );

            //criando o userSote, porem ele precisa do dbContext defino acima, vamos utilizar o owin para recuperar o dbContext
            builder.CreatePerOwinContext<IUserStore<Usuario>>((opcao, contextoOwin) =>
            {
                var dbContext = contextoOwin.Get<DbContext>();
                return new UserStore<Usuario>(dbContext);
            });

            //Criando o UserManager
            builder.CreatePerOwinContext<UserManager<Usuario>>((opcao, contextoOwin) =>
            {
                var userStore = contextoOwin.Get<IUserStore<Usuario>>();
                var userManager = new UserManager<Usuario>(userStore);
                //criando uma instancia de userValidator para criar validações
                var userValidator = new UserValidator<Usuario>(userManager);
                //informando que o email tem que ser unico
                userValidator.RequireUniqueEmail = true;

                //propriedade de validação dos usuarios recebendo uma instancia de validação
                userManager.UserValidator = userValidator;
                //atribuindo ao userManager.PasswordValidator a classe criada de validação
                userManager.PasswordValidator = new SenhaValidador()
                {
                    TamanhoRequerido = 10,
                    ObrigatorioCaracterEspecial = true,
                    ObrigatorioDigitos = true,
                    ObrigatorioLowerCase = true,
                    ObrigatorioUpperCase = true
                };
                return userManager;
            });
        }
    }
}