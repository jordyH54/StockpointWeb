using static stockpoint.Models.cOcadis;

namespace stockpoint.Controllers
{
    using stockpoint.Models;
    using System;
    using System.Net.NetworkInformation;
    using System.Web.Mvc;
    using System.Web.UI;

    public class AdministracionController : Controller
    {
        public ActionResult Usuario()
        {
            
            return View();
        }
        public ActionResult Producto()
        {
            //cProducto obj = new cOcadis.cProducto()
            //{
            //    Descripcion = "Coca cola 600ml",
            //    idProducto = 1,
            //    PiezasXcaja = 1,
            //    idUnidad = 8,
            //    PiezasXPaquete = 1,
            //    CantidadMinima = 10,
            //    Existencia = 1,
            //    idCategoria = 88,
            //    idEstatus = 99,
            //    CostoXCaja = 16.04,
            //    PrecioPublico = 20,
            //    Productos = "Coca cola 600ml",
            //    Codigo = "75007614",
            //    Imagen = "../Productos/Logo.jpg",

            //    R = new cOcadis.cResul
            //    {
            //        modulo = "Producto",
            //        Token = "53f2d96b-47d8-4de0-9215-d92682144e21",
            //        metodos = "Guardar"
            //    }

            //};
            //obj.Actualizar();

            ViewBag.sysPage = "Producto";
            return View();
        }
       
        
public ActionResult Compra()
        {
            ViewBag.sysPage = "Compra";
            return View();
        }
        public ActionResult Venta()
        {
            cVenTa obj = new cOcadis.cVenTa()
            {
                idCliente = 103,
                Pago = 20,
                R = new cOcadis.cResul
                {
                    modulo = "Venta",
                    Token = "f8d417ae-675c-4b06-87d9-e6b726b2c806",
                    metodos = "GuardarCT"
                }

            };
            obj.Actualizar();
            //cVenTa obj = new cOcadis.cVenTa()
            //{
            //    idVenta = 0,
            //    idCliente = 103,
            //    Token = "e65647e4-172b-433d-abbe-57e88073d0db",
            //    idTipoPago = 78,
            //    Importe = 50,
            //    Pago = 40,



            //    R = new cOcadis.cResul
            //    {
            //        modulo = "Producto",
            //        Token = "e65647e4-172b-433d-abbe-57e88073d0db",
            //        metodos = "Guardar"
            //    }

            //};
            //obj.Actualizar();



            ViewBag.sysPage = "Venta";
            return View();
        }
    }
}