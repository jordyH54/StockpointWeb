using stockpoint.Models;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Web.Mvc;
using static stockpoint.Models.cOcadis;
using static stockpoint.Models.cOcadisINI;
using Newtonsoft.Json;
using System.Web.Http;
using System.Net.NetworkInformation;
using System.Web.Services.Description;
using System.Drawing.Printing;
namespace stockpoint.Controllers
{
    
    
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            //cUsuario obj = new cUsuario()
            //{
            //    idUsuario = 14,

            //    ClaveAcceso = "060202",
            //    R = new cResul()
            //    {
            //        metodos = "Guardar",
            //    }
            //};
            //obj.Actualizar();

            // Asignar valores al nuevo usuario

            


            // Cambiar el método a "Guardar" para guardar el nuevo usuario
            
            return View();
        }
        //cElemento obj = new cElemento()
        //{
        //    Catalogo= "Rol"
        //};
        //ViewBag.oElementos = obj;
        //obj.Cargar();
        //obj = null;

        //var obj= new cResul()
        //{
        //    Pantalla = "Login",
        //    Nombre= "Esteban",
        //    metodos = "Login incorrecto",
        //    R = new cResul()
        //    {
        //        metodos = "CargarBitacora"
        //    }

        //};
        //obj.Actualizar();
        //ViewBag.oResul = obj;
        //obj = null;




        

        //// Llamar al método Actualizar para guardar el nuevo usuario
        //obj.Actualizar();


        //cUsuario obj = new cUsuario()
        //{
        //    CorreoElectronico = "201064137@tecnmtlahuac.onmicrosoft.com",
        //    ClaveAcceso = "060202",
        //    R = new cResul()
        //    {
        //        metodos = "Acceso"
        //    }
        //};
        //obj.Actualizar();


        //cUsuario obj=new cUsuario()
        //{
        //    Token= "26ec7c11-a777-43ef-a41d-83e0141798a4",
        //    Pantalla= "Catalogo",
        //    R=new cResul()
        //    {  
        //        metodos= "AccesoToken"
        //    }
        //};
        //obj.Actualizar();





        //cUsuario obj = new cUsuario()
        //{
        //    Token = "26ec7c11-a777-43ef-a41d-83e0141798a4",
        //    Pantalla = "Catalogo",
        //    R = new cResul()
        //    {
        //        metodos = "AccesoToken"
        //    }
        //};
        //obj.Actualizar(); // Llama al método para verificar el acceso

        //cUsuario obj = new cUsuario()
        //{
        //    Token= "f3904f47-1a32-4f5e-8dfd-e99191fb9c72",
        //    R=new cResul()
        //    {
        //        metodos="Token"
        //    }
        //};
        //obj.Actualizar();




        //cMail correo = new cMail()
        //{
        //    Para = "201064137@tecnmtlahuac.onmicrosoft.com",
        //    Asunto = "CORREO PRUEBA",
        //    Cuerpo = "<h1>Este es un correo de prueba</h1>",

        //};
        //correo.EnviarCorreo();

        //string resultado = correo.EnviarCorreo(
        //para: "201064137@tecnmtlahuac.onmicrosoft.com",
        //asunto: "Prueba Ultima",
        //cuerpo: "<h1>Este es un correo de prueba</h1>",
        //archivos: "" // Opcional: Ruta de archivos adjuntos
        // );

        //correo.Para = "201064137@tecnmtlahuac.onmicrosoft.com"; // Correo del destinatario
        //correo.Asunto = "Prueba de correo3";
        //correo.Cuerpo = "<h1>Este es un correo de prueba</h1>";
        //correo.Archivos = ""; // Opcional: Ruta de archivos adjuntos

        // Enviar el correo


        // Mostrar el resultado
        //Console.WriteLine(resultado.ToString());
        //if (resultado.Equals("Mensaje enviado a "))
        //{
        //    Console.WriteLine("El correo se envió correctamente.");
        //}
        //else
        //{
        //    Console.WriteLine("Hubo un error al enviar el correo.");
        //}



        //cOcadis.cUsuario obj = new cOcadis.cUsuario()
        //{
        //    CorreoElectronico = "201064137@tecnmtlahuac.onmicrosoft.com",


        //};
        //obj.solicitarClave();


        public ActionResult About()
        {
            

            return View();
        }

        public ActionResult Contact()
        {
            

            return View();
        }
        public ActionResult RecuperarContra()
        {
            ViewBag.sysPage = "Login";

            return View();
        }
        public ActionResult Login()
        {
            ViewBag.sysPage = "Login";
            return View();
        }
        
        public ActionResult Catalogo()
        {
            return View();
        }
    }
}