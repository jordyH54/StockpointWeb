using System.Reflection;
using System.Security.Policy;
using System;
using ZXing;

namespace stockpoint.Controllers
{
    
    using stockpoint.Models;
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using static stockpoint.Models.cOcadis;
    using Newtonsoft.Json;
    public class WebServicesController : Controller
    {
        public JsonResult Usuarios(cOcadis.cUsuario obj)
        {
            
            obj.Actualizar();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UsuariosA(cOcadis.cUsuario obj)
        {

            obj.Actualizar();
            return Json(obj.UsuarioS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Pantallas(cOcadis.cPantalla obj)
        {

            obj.Actualizar();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Productos(cOcadis.cProducto obj)
        {
            

            obj.Actualizar();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ProductosA(cOcadis.cProducto obj)
        {

            obj.Actualizar();
            return Json(obj.Producto, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Compra(cOcadis.cCompra obj)
        {

            obj.Actualizar();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        //[HttpGet]
        //public JsonResult ObtenerRoles()
        //{
        //    try
        //    {
        //        var elemento = new cElemento
        //        {
        //            Catalogo = "Tipo de catalogo",
        //            R = new cResul { metodos = "Cargar" }
        //        };
        //        elemento.Actualizar();
        //        return Json(elemento.Elementos,JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new
        //        {
        //            Error = true,
        //            Mensaje = ex.Message
        //        },JsonRequestBehavior.AllowGet);
        //    }
        //}
        public JsonResult Elementos(cOcadis.cElemento obj) {
            //Cargar
            obj.Actualizar();
            //obj.Cargar();
            return Json(obj,JsonRequestBehavior.AllowGet);
        }
        public JsonResult ElementosA(cOcadis.cElemento obj)
        {
            
            obj.Actualizar();
            return Json(obj.Elementos, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DatosCat(cOcadis.cElemento obj)
        {
            obj.Actualizar();
            //obj.Cargar2();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DatosCatA(cOcadis.cElemento obj)
        {
            obj.Actualizar();
            //obj.Cargar2();
            return Json(obj.catalogos, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Catalogos(cOcadis.cCatalogo obj)
        {
            obj.Actualizar();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CatalogosA(cOcadis.cCatalogo obj)
        {
            obj.Actualizar();
            return Json(obj.Catalogos, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ResultadosN(cOcadis.cResul obj)
        {
            
           
            obj.Actualizar();

            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ResultadosNA(cOcadis.cResul obj)
        {
            

            obj.Actualizar();
            //return Json(obj, JsonRequestBehavior.AllowGet);

            return Json(new
            {
                Data = obj.Bitacora,
                Total = obj.cantidad
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Venta(cOcadis.cVenTa obj)
        {


            obj.Actualizar();

            return Json(obj, JsonRequestBehavior.AllowGet);
        }



    }


}

//$('.cmbCatalogos').change(function() {
//    obj = {
//    Catalogo: $(this).val(),
//            R:
//        {
//        metodos: 'Cargar'
//            }
//    }
//    ;
//        $.ajax({
//    type: "POST",
//            url: url + 'Catalogos',
//            dataType: "json",
//            contentType: "application/json; charset=utf-8",
//            data: JSON.stringify(obj),
//            async: true,
//            processData: false,
//            cache: false,
//            success: function(Result) {
//            console.log(Result);
//            var tData = "";
//                $.each(Result.Catalogos, function(index, item) {

//                tData += '<tr class="tblCatalogo ReglonActivo">\n';
//                tData += ' <td style="width:30px"><i class="fas fa-edit cmdEditarCatalogo" id="7" aria-hidden="true" onclick=""></i></td>\n';
//                tData += '    <td style="width:30px"></td>\n';
//                tData += '<td>' + item.Valor + '</td>\n';
//                tData += '</tr>\n'
//                });
//                $('.tData').html(tData);
//        },
//            error: function(xhr) {
//            return false;
//        }
//    });

//});