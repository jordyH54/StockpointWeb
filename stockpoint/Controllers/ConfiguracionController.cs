using static stockpoint.Models.cOcadis;

namespace stockpoint.Controllers
{
    using stockpoint.Models;
    using System.Web.Mvc;
    public class ConfiguracionController : Controller
    {
        public ActionResult Usuarios()
        {
            
            return View();
        }
        public ActionResult Catalogo()
        {
            
            return View();
        }
        public ActionResult Resultado()
        {
            //cResul objR = new cResul()
            //{
            //    Pantalla = null,
            //    metodos = null,
            //    fecha =null,



            //};
            //objR.Cargar();
            //ViewBag.oResul = objR;
            //objR = null;
            //cUsuario obU=new cUsuario()
            //{
            //    CorreoElectronico= "201064137@tecnmtlahuac.onmicrosoft.com",
            //    ClaveAcceso = "Primoch18$",
            //    Pantalla = "Bitacora",
            //    R = new cResul()
            //    {
            //        Pantalla = "Bitacora",
            //        metodos = "Acceso"
            //    }
            //};
            //obU.Actualizar();

            //cResul oResul = new cResul()
            //{
            //    idResultado = 0,
            //    idUsuario = obU.idUsuario,
            //    mensaje = "Consulta de bitacora correcta de "+ obU.Nombre,
            //    Pantalla =obU.Pantalla,
            //    metodos = "Consulta Bitacora",
            //    fecha = DateTime.Now,
            //    Id_Catalogo = 39,
            //    Estatus = "Es correcto",


            //};
            //oResul.Guardar();
            //ViewBag.oResul = oResul;
            //System.Diagnostics.Debug.WriteLine("Token: " + obU);

            return View();
        }
        public ActionResult Pantalla()
        {
            ViewBag.sysPage = "Pantalla";

            return View();
        }
        //public ActionResult Producto()
        //{

        //    return View();
        //}
        
    }
}