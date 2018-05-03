using BityBank.Models;
using BityBank.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BityBank.Controllers
{
    public class ContaController : Controller
    {
        //atribuindo o user manager a nosso controller
        private UserManager<Usuario> _userManager;
        
        public UserManager<Usuario> UserManager
        {
            get
            {
                if(_userManager == null)
                {
                    //pegando o contexto do owin dessa sessão
                    var contextoOwin = HttpContext.GetOwinContext();
                    _userManager = contextoOwin.GetUserManager<UserManager<Usuario>>();
                }
                return _userManager;
            }
            set
            {
                _userManager = value;
            }
        }

        [HttpGet]
        public ActionResult Cadastro()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Cadastro(UsuarioViewModel modelo)
        {
            if (ModelState.IsValid)
            {              
                var obj = new Usuario()
                {
                    NomeCompleto = modelo.NomeCompleto,
                    UserName = modelo.UserName,
                    Email = modelo.Email                    
                };
                //buscando um usuario que ja existe
                var usuario = UserManager.FindByEmail(modelo.Email);
                var usuarioJaExiste = usuario != null;
                if(usuarioJaExiste)
                    return RedirectToAction("Index", "Home");
                //Creando o usuario com a userStore e o userManager
                var resultado = await UserManager.CreateAsync(obj, modelo.Senha);
                //Se o usuario foi criado com sucesso
                if (resultado.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    //metodo que verifica se ocorreu erros
                    AdicionaErros(resultado);
                }
                
            }
            return View(modelo);
        }

        private void AdicionaErros(IdentityResult resultado)
        {
            //para cada erro encontrado ao criar o usuario
            foreach(var erro in resultado.Errors)
            {
                ModelState.AddModelError("", erro);
            }
        }
    }
}