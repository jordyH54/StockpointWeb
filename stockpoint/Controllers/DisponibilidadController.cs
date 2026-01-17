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
    using System.Web.Mvc;
    using System.Collections.Generic;
    using System.Web.Http.Cors;
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DisponibilidadController : Controller
    {
        
        [HttpGet] // Acepta peticiones GET (como tu JavaScript)
        public JsonResult Comprobar(string name)
        {
            // ▶ 1. Lógica de ejemplo (personalízala)
            bool disponible = name.Length > 3; // true si el nombre tiene más de 3 caracteres
            List<string> alternativas = null;

            if (!disponible)
            {
                alternativas = new List<string> {
                name + "123",
                name + "_oficial",
                "mi_" + name
            };
            }

            // ▶ 2. Estructura JSON que espera tu JS
            var respuesta = new
            {
                disponible = disponible, // Propiedades en minúscula (como en tu JS)
                alternativas = alternativas  // Null si está disponible
            };

            return Json(respuesta, JsonRequestBehavior.AllowGet);
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