namespace stockpoint.Models
{
    using Microsoft.Ajax.Utilities;
    using stockpoint.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Data.Common.CommandTrees.ExpressionBuilder;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.EnterpriseServices;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Net.Mime;
    using System.Net.NetworkInformation;
    using System.Reflection.Emit;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Services.Description;
    using System.Web.UI.WebControls;
    using System.Xml.Linq;
    using ZXing.Aztec.Internal;
    using ZXing.Client.Result;
    using static stockpoint.Models.cOcadis;
    using static stockpoint.Models.cOcadis.cResul;
    using static stockpoint.Models.cOcadis.cUsuario;
    using static System.Net.Mime.MediaTypeNames;

    public class cOcadis
    {
        


        #region clasesadicionales
        public class cElemento
        {
            public int id { get; set; }
            public string Catalogo { get; set; }
            public string Valor { get; set; }
            public string Texto { get; set; }
            public int idRol { get; set; }

            public string idOrigen { get; set; }

            public string ValorDefault { get; set; }
            public List<cElemento> Elementos { get; set; }
            public List<cElemento> SubEelementos { get; set; }
            public List<tblCatalogo> catalogos { get; set; }
            public string Descripcion { get; set; }

            public List<vwUsuario> Usuarios { get; set; }

            public string Token { get; set; }
            public string modulo { get; set; }
            public cResul R { get; set; }
            private dbstockpointEntities db;


            public void Actualizar()
            {

                try
                {
                    
                    db = new dbstockpointEntities();

                    if (R.TieneAcceso())
                    {
                        switch (R.metodos)
                        {
                            case "Cargar":
                                Cargar();
                                break;
                            case "Cargar2":
                                Cargar2();
                                break;
                            
                        }
                    }
                    








                }
                catch (Exception e)
                {
                    this.R = new cResul()
                    {
                        Estatus = "Error de sistema",
                        metodos = "Guardar",
                        mensaje = e.Message,
                        fecha = DateTime.Now,
                    };

                }
            }
            private void Cargar()
            {
                try
                {
                    db = new dbstockpointEntities();
                    Elementos = (from item in db.uspGetElementosCatalogo(Catalogo, idOrigen, ValorDefault)
                                 select new cElemento
                                 {
                                     Valor = item.Valor,
                                     Texto = item.Texto,
                                     ValorDefault = item.ValorDefault

                                 }).ToList();
                    Usuarios = (from item in db.vwUsuario.Where(u => u.idRol.Equals(idRol))
                                select new vwUsuario
                                {
                                    Nombre = item.Nombre,
                                    idUsuario = item.idUsuario,
                                    Estatus = item.Estatus

                                }).ToList();
                    db.Dispose();
                    db = null;
                }catch (Exception e)
                {
                    this.R = new cResul()
                    {
                        Estatus = "Error de sistema",
                        mensaje = e.Message,
                        fecha = DateTime.Now,
                    };
                }
            }
           

            private void Cargar2()
            {
                try
                {
                        


                    db = new dbstockpointEntities();


                    catalogos = db.tblCatalogo.ToList();
                    catalogos = catalogos.Where(c => c.Id_Catalogo.Equals(id)).ToList();

                    catalogos = (from item in catalogos
                                 select new tblCatalogo
                                 {
                                     Id_Catalogo = item.Id_Catalogo,
                                     Valor = item.Valor,
                                     IdOrigen = item.IdOrigen,
                                     Catalogo = item.Catalogo,
                                     Orden = item.Orden,
                                     Descripcion = item.Descripcion,
                                     ValorDefault = item.ValorDefault

                                 }).ToList();

                    
                    db.Dispose();
                    db = null;


                }
                catch (Exception e)
                {
                    this.R = new cResul()
                    {
                        Estatus = "Error de sistema",
                        mensaje = e.Message,
                        fecha = DateTime.Now,
                    };
                }
            }

            


        }
       
       
        public class cResultados : vwResultado
        {

            //public string idResultado { get; set; }
            //public string Nombre { get; set; }
            //public string modulo { get; set; }
            //public string Texto { get; set; }
            //public DateTime Fecha { get; set; }
            //public string mensajeDeSistema { get; set; }

        }
        public class cUsuarion
        {
            private dbstockpointEntities db;
            public string IdUsuario { get; set; }
            public string Nombre { get; set; }
            public string CorreoElectronico { get; set; }
            public string ClaveAcceso { get; set; }
            public string Telefono1 { get; set; }
            public string Telefono2 { get; set; }
            public string IdRol { get; set; }
            public string IdEstatus { get; set; }
            public string Token { get; set; }
        }
        #endregion


        #region DB
        public class cCatalogo : tblCatalogo
        {
            #region propertys
            public List<cElemento> Catalogos { get; set; }
            public string modulo { get; set; }
            public string Token { get; set; }
            public int idRol { get; set; }
            public int idUsuario { get; set; }
            public vwUsuario User { get; set; }
            public List<vwUsuario> Usuarios { get; set; }
           
            #endregion

            //primer metodo que se ejecuta cuado creo un metodo
            #region constructor
            public cResul R { get; set; }
            private dbstockpointEntities db;
            private cEncriptar oCode = new cEncriptar();
            #endregion

            #region metodos
            public void Actualizar()
            {
                try
                {
                    db = new dbstockpointEntities();
                    if (R.TieneAcceso())
                    {
                        switch (R.metodos)
                        {
                            case "Cargar":
                                Cargar();
                                break;
                            case "Guardar":
                                Guardar();
                                break;
                            case "CargarU":
                                CargarU();
                                break;
                            case "CargarUData":
                                CargarUData();
                                break;

                        }
                    }
                    
                    
                }
                catch (Exception e)
                {
                    this.R = new cResul()
                    {
                        Estatus = "Error de sistema",
                        metodos = e.Message,
                        fecha = DateTime.Now,
                    };
                }
            }
           
            public void Cargar()
            {
                try
                {
                    

                    Catalogos = (from item in db.uspGetElementosCatalogo(Catalogo, IdOrigen.ToString(), ValorDefault.ToString())
                                 select new cElemento
                                 {
                                     Texto = item.Texto,
                                     Valor = item.Valor,
                                     ValorDefault = item.ValorDefault
                                 }
                                 ).ToList();
                    

                               
                    this.R = new cResul()
                    {
                        Estatus = "Correcto",
                        metodos = R.metodos,
                        mensaje = "Se cargo correctamente",
                        fecha = DateTime.Now,
                        modulo = R.modulo
                    };
                }
                catch (Exception e)
                {
                    this.R = new cResul()
                    {
                        Estatus = "Error de sistema",
                        mensaje = e.Message,
                        fecha = DateTime.Now,
                    };
                }
            }
            private void CargarU()
            {
                try
                {
                    db = new dbstockpointEntities();
                    Usuarios = db.vwUsuario.ToList();
                    Usuarios = Usuarios.Where(u => u.idRol.Equals(idRol)).ToList();
                    Usuarios = (from item in Usuarios
                                select new vwUsuario
                                {
                                    Nombre = item.Nombre,
                                    idUsuario = item.idUsuario,
                                    idEstatus = item.idEstatus,
                                   
                                }).ToList();
                    db.Dispose();
                    db = null;
                }
                catch (Exception e)
                {
                    this.R = new cResul()
                    {
                        Estatus = "Error de sistema",
                        mensaje = e.Message,
                        fecha = DateTime.Now,
                    };
                }
            }
            private void CargarUData()
            {

                try
                {
                    db = new dbstockpointEntities();


                    vwUsuario obj = db.vwUsuario.Where(u => u.idUsuario.Equals(idUsuario)).FirstOrDefault();

                   

                    Usuarios = db.vwUsuario.ToList();
                    Usuarios = Usuarios.Where(u => u.idUsuario.Equals(idUsuario)).ToList();
                    Usuarios = (from item in Usuarios
                                select new vwUsuario
                                {
                                    Nombre = item.Nombre,
                                    idUsuario = item.idUsuario,
                                    idEstatus = item.idEstatus,
                                    CorreoElectronico = item.CorreoElectronico,
                                    ClaveAcceso = oCode.Desencriptar(item.ClaveAcceso),
                                    Telefono1 = item.Telefono1,
                                    Telefono2 = item.Telefono2,
                                    idRol = item.idRol,
                                    Rol = item.Rol,
                                    Estatus = item.Estatus,
                                }).ToList();
                    db.Dispose();
                    db = null;
                }
                catch (Exception e)
                {
                    this.R = new cResul()
                    {
                        Estatus = "Error de sistema",
                        mensaje = e.Message,
                        fecha = DateTime.Now,
                    };
                }
            }
            private  void Guardar()
            {
                try
                {
                    tblCatalogo obj = (from item in db.uspCatalogo(Id_Catalogo, IdOrigen, Catalogo, Valor, Orden, ValorDefault, Descripcion)
                                       select new tblCatalogo
                                       {
                                           Id_Catalogo = Id_Catalogo,
                                           IdOrigen = item.IdOrigen,
                                           Orden = item.Orden,
                                           Catalogo = item.Catalogo,
                                           Valor = item.Valor,
                                           Descripcion = item.Descripcion,
                                           ValorDefault = item.ValorDefault

                                       }
                                 ).FirstOrDefault();
                    this.Id_Catalogo = 0;
                    this.IdOrigen = 0;
                    this.Catalogo = string.Empty;
                    this.Orden = 1;
                    this.Valor = string.Empty;
                    this.Descripcion = string.Empty;
                    this.ValorDefault = 0;




                    this.R = new cResul()
                    {
                        Estatus = "Correcto",
                        metodos = string.Empty,
                        mensaje = "Se guardo correctamente",
                        fecha = DateTime.Now,
                    };
                }

                catch (Exception e)
                {
                    this.R = new cResul()
                    {
                        Estatus = "Error de sistema",
                        mensaje = e.Message,
                        fecha = DateTime.Now,
                    };
                }
            }

            //public string Configuracion(string clave)
            //{
            //    // Simulamos la obtención de la configuración del servidor de correo
            //    if (clave == "Web Mail")
            //    {
            //        return "smtp.gmail.com,465,aron2014123456@gmail.com,Primoch18$,true,,aron2014123456@gmail.com";
            //    }
            //    return string.Empty;
            //}
            public string Configuracion(string valor)
            {
                try
                {
                    dbstockpointEntities db = new dbstockpointEntities();
                    tblCatalogo obj = db.tblCatalogo.Where(c => c.Catalogo.Equals("Configuración") && c.Valor.Equals(valor)).FirstOrDefault();
                    valor = string.Empty;
                    if (obj != null)
                    {
                        valor = obj.Descripcion;
                    }

                    obj = null;
                    db.Dispose();
                    db = null;
                    return valor;

                }
                catch (Exception e)
                {
                    this.R = new cResul
                    {
                        idResultado = 0,
                        idUsuario = 1,
                        modulo = "Usuario",
                        metodos = "Guardar",
                        idEstatus = 11,
                        mensaje = e.Message,
                        Estatus = "Error de sistema",
                        fecha = DateTime.Now
                    };
                    return "";

                }

            }
            #endregion
        }
        public class cResul : vwResultado
        {
            #region propertys
           
            public int Pagina { get; set; } 
            public int RegistrosPorPagina { get; set; } 
            public List<cResul> Resultados { get; set; }
            public List<vwResultado> Bitacora { get; set; }
            public List<cPermisoporpantalla> Menu { get; set; }
            public string Token { get; set; }
            public DateTime FechaFinal { get; set; }
            public DateTime FechaInicial { get; set; }
            public vwUsuario User { get; set; }
            public int Movil { get; set; }
            public int cantidad { get; set; }

            #endregion

            #region constructor
            public cResul R { get; set; } 
            private dbstockpointEntities db;
            
            #endregion

            #region metodos
            public void Actualizar()
            {
                try
                {
                    db = new dbstockpointEntities();
                    if (R.TieneAcceso())
                    {
                        switch (R.metodos)
                        {
                            case "Cargar":
                                CargarBitacora();
                                break;
                            case "Guardar":
                                Guardar();
                                break;
                            case "TieneAceso":
                                TieneAcceso();
                                break;

                        }
                    }
                   
                   
                }
                catch (Exception e)
                {
                    this.R = new cResul()
                    {
                        Estatus = "Error de sistema",
                        metodos = "Guardar",
                        mensaje = e.Message,
                        fecha = DateTime.Now,
                    };
                }





            }
            public class cPermisoporpantalla : vwPantallaPorPermiso
            {
                public List<cPermisoporpantalla> SubMenu { get; set; }
            }
            private void Cargar()
            {

                try
                {
                    db = new dbstockpointEntities();
                    //tblResultado obj = db.tblResultado.Where(c => c.idResultado == idResultado).FirstOrDefault();

                    //if (obj == null)
                    //{

                    //    this.idResultado = obj.idResultado;
                    //    this.idUsuario = obj.idUsuario;
                    //    this.modulo = obj.modulo;
                    //    this.metodos = obj.metodos;
                    //    this.Id_Catalogo = obj.idEstatus;
                    //    this.mensaje = obj.mensaje;
                    //    this.Estatus = obj.mensajeDeSistema;
                    //    this.fecha = DateTime.Now;

                    //}
                    //else
                    //{
                    //    this.idResultado = 0;
                    //    this.idUsuario = 0;
                    //    this.modulo = string.Empty;
                    //    this.metodos = string.Empty;
                    //    this.Id_Catalogo = 0;
                    //    this.mensaje = string.Empty;
                    //    this.Estatus = string.Empty;
                    //    this.fecha = new DateTime();
                    //}


                    Resultados = (from item in db.uspGetElementosResultados(modulo, metodos, fecha, idUsuario)
                                  select new cResul
                                  {




                                      modulo = item.modulo,
                                      metodos = item.metodos,


                                      Usuario = item.Usuario,

                                      mensaje = item.mensaje,



                                  }).ToList();



                    //obj = null;
                }
                catch (Exception e)
                {
                    this.R = new cResul()
                    {
                        Estatus = "Error de sistema",
                        mensaje = e.Message,
                        metodos = "Guardar",
                        fecha = DateTime.Now,
                    };
                }
            }




            public void Guardar()
            {
                try
                {
                    db = new dbstockpointEntities();

                    // Crear un nuevo objeto tblResultado
                    tblResultado nuevoResultado = new tblResultado
                    {
                        idResultado = this.idResultado,
                        idUsuario = this.idUsuario,
                        modulo = this.modulo,
                        metodos = this.metodos,
                        idEstatus = this.idEstatus,
                        mensaje = this.mensaje,
                        mensajeDeSistema = this.Estatus,
                        fecha = DateTime.Now,
                    };
                    // Agregar el objeto a la base de datos
                    db.usptblResultado(nuevoResultado.idResultado, nuevoResultado.idUsuario, nuevoResultado.modulo, nuevoResultado.metodos, nuevoResultado.idEstatus, nuevoResultado.mensaje, nuevoResultado.mensajeDeSistema, nuevoResultado.fecha);
                    db.SaveChanges(); // Guardar los cambios en la base de datos
                    db.Dispose(); //Matar la bd
                    db = null;
                    // Asignar el resultado a R
                    //this.R = new vwResultado
                    //{
                    //    Estatus = "Correcto",
                    //    metodos = "Guardar",
                    //    mensaje = "El resultado se guardó correctamente",
                    //    fecha = DateTime.Now,
                    //};
                }
                catch (Exception e)
                {
                    // Manejar errores
                    //this.R = new vwResultado
                    //{
                    //    Estatus = "Error de sistema",
                    //    metodos = "Guardar",
                    //    mensaje = e.Message,
                    //    fecha = DateTime.Now,
                    //};
                }
                finally
                {
                    // Liberar recursos
                    if (db != null)
                    {
                        db.Dispose();
                        db = null;
                    }
                }
            }
            //private void CargarBitacora()
            //{
            //    try
            //    {


            //        Bitacora = db.vwResultado.ToList();

            //        if (FechaInicial != DateTime.MinValue)
            //        {
            //            if (FechaFinal == DateTime.MinValue)
            //            {

            //                FechaFinal = FechaInicial.AddHours(24);
            //            }
            //            else
            //            {

            //                FechaFinal = FechaFinal.AddHours(24);
            //            }

            //            Bitacora = Bitacora.Where(o => o.fecha > FechaInicial && o.fecha < FechaFinal).ToList();



            //        }
            //        if (Movil == 1)
            //        {
            //            if (!string.IsNullOrEmpty(modulo))
            //            {
            //                Bitacora = Bitacora.Where(r => r.modulo.Equals(modulo)).ToList();
            //            }
            //            if (!string.IsNullOrEmpty(Usuario))
            //            {
            //                Bitacora = Bitacora.Where(r => r.Usuario.Equals(Usuario)).ToList();
            //            }
            //            if (!string.IsNullOrEmpty(metodos))
            //            {
            //                Bitacora = Bitacora.Where(r => r.metodos.Equals(metodos)).ToList();
            //            }
            //            if (!string.IsNullOrEmpty(Estatus))
            //            {
            //                Bitacora = Bitacora.Where(r => r.Estatus.Equals(Estatus)).ToList();
            //            }
            //            Bitacora = Bitacora.OrderByDescending(x => x.fecha).Take(8000).ToList();
            //        }
            //        else
            //        {
            //            if (!string.IsNullOrEmpty(modulo) && modulo != "0")
            //            {
            //                Bitacora = Bitacora.Where(r => r.modulo.Equals(modulo)).ToList();
            //            }

            //            if (!string.IsNullOrEmpty(Usuario) && Usuario != "0")
            //            {
            //                Bitacora = Bitacora.Where(r => r.Usuario.Equals(Usuario)).ToList();
            //            }

            //            if (!string.IsNullOrEmpty(metodos) && metodos != "0")
            //            {
            //                Bitacora = Bitacora.Where(r => r.metodos.Equals(metodos)).ToList();
            //            }

            //            if (!string.IsNullOrEmpty(Estatus) && Estatus != "0")
            //            {
            //                Bitacora = Bitacora.Where(r => r.Estatus.Equals(Estatus)).ToList();
            //            }
            //        }


            //        //if (!string.IsNullOrEmpty(modulo))
            //        //{
            //        //    Bitacora = Bitacora.Where(r => r.modulo.Equals(modulo)).ToList();
            //        //}
            //        //if (!string.IsNullOrEmpty(Usuario))
            //        //{
            //        //    Bitacora = Bitacora.Where(r => r.Usuario.Equals(Usuario)).ToList();
            //        //}
            //        //if (!string.IsNullOrEmpty(metodos))
            //        //{
            //        //    Bitacora = Bitacora.Where(r => r.metodos.Equals(metodos)).ToList();
            //        //}
            //        //if (!string.IsNullOrEmpty(Estatus))
            //        //{
            //        //    Bitacora = Bitacora.Where(r => r.Estatus.Equals(Estatus)).ToList();
            //        //}








            //    }
            //    catch (Exception e)
            //    {
            //        this.R = new cResul()
            //        {
            //            idResultado = 0,
            //            idUsuario = 13,
            //            mensaje = "Consulta de bitacora incorrecto no hay registros",
            //            modulo = modulo,
            //            metodos = "Consulta Bitacora",
            //            fecha = DateTime.Now,
            //            idEstatus = 37,
            //            mensajeDeSistema = "Se relizaron los filtros de " + FechaInicial + ", " + FechaFinal + ", " + modulo + ", " + Usuario + ", " + metodos + ", " + Estatus + ". ",
            //            Estatus = "Incorrecto" + e,

            //        };
            //    }




            //}

            


            private void  CargarBitacora()
            {
                try
                {
                    if (Movil == 1)
                    {
                        Pagina = (Pagina < 1) ? 1 : Pagina;  // Solo esto para inicializar bien
                        RegistrosPorPagina = 8;

                        int skip = (Pagina - 1) * RegistrosPorPagina;

                        IQueryable<vwResultado> query = db.vwResultado;

                        if (FechaInicial != DateTime.MinValue)
                        {
                            var fechaFinalReal = FechaFinal == DateTime.MinValue ? FechaInicial.AddDays(1) : FechaFinal;
                            query = query.Where(o => o.fecha >= FechaInicial && o.fecha <= fechaFinalReal);
                        }
                        if (!string.IsNullOrEmpty(modulo))
                        {
                            query = query.Where(r => r.modulo.Equals(modulo));
                        }
                        if (!string.IsNullOrEmpty(Usuario))
                        {
                            query = query.Where(r => r.Usuario.Equals(Usuario));
                        }
                        if (!string.IsNullOrEmpty(metodos))
                        {
                            query = query.Where(r => r.metodos.Equals(metodos));
                        }
                        if (!string.IsNullOrEmpty(Estatus))
                        {
                            query = query.Where(r => r.Estatus.Equals(Estatus));
                        }

                        cantidad = query.Count();

                        Bitacora = query
                           .OrderByDescending(x => x.fecha)
                           .Skip(skip)
                           .Take(RegistrosPorPagina)
                           .ToList();
                    }
                    else
                    {

                        Bitacora = db.vwResultado.ToList();

                        if (FechaInicial != DateTime.MinValue)
                        {
                            if (FechaFinal == DateTime.MinValue)
                            {

                                FechaFinal = FechaInicial.AddHours(24);
                            }
                            else
                            {

                                FechaFinal = FechaFinal.AddHours(24);
                            }

                            Bitacora = Bitacora.Where(o => o.fecha > FechaInicial && o.fecha < FechaFinal).ToList();



                        }
                        if (!string.IsNullOrEmpty(modulo) && modulo != "0")
                        {
                            Bitacora = Bitacora.Where(r => r.modulo.Equals(modulo)).ToList();
                        }

                        if (!string.IsNullOrEmpty(Usuario) && Usuario != "0")
                        {
                            Bitacora = Bitacora.Where(r => r.Usuario.Equals(Usuario)).ToList();
                        }

                        if (!string.IsNullOrEmpty(metodos) && metodos != "0")
                        {
                            Bitacora = Bitacora.Where(r => r.metodos.Equals(metodos)).ToList();
                        }

                        if (!string.IsNullOrEmpty(Estatus) && Estatus != "0")
                        {
                            Bitacora = Bitacora.Where(r => r.Estatus.Equals(Estatus)).ToList();
                        }
                    }


                    //if (!string.IsNullOrEmpty(modulo))
                    //{
                    //    Bitacora = Bitacora.Where(r => r.modulo.Equals(modulo)).ToList();
                    //}
                    //if (!string.IsNullOrEmpty(Usuario))
                    //{
                    //    Bitacora = Bitacora.Where(r => r.Usuario.Equals(Usuario)).ToList();
                    //}
                    //if (!string.IsNullOrEmpty(metodos))
                    //{
                    //    Bitacora = Bitacora.Where(r => r.metodos.Equals(metodos)).ToList();
                    //}
                    //if (!string.IsNullOrEmpty(Estatus))
                    //{
                    //    Bitacora = Bitacora.Where(r => r.Estatus.Equals(Estatus)).ToList();
                    //}








                }
                catch (Exception e)
                {
                    this.R = new cResul()
                    {
                        idResultado = 0,
                        idUsuario = 13,
                        mensaje = "Consulta de bitacora incorrecto no hay registros",
                        modulo = modulo,
                        metodos = "Consulta Bitacora",
                        fecha = DateTime.Now,
                        idEstatus = 37,
                        mensajeDeSistema = "Se relizaron los filtros de " + FechaInicial + ", " + FechaFinal + ", " + modulo + ", " + Usuario + ", " + metodos + ", " + Estatus + ". ",
                        Estatus = "Incorrecto" + e,

                    };
                }




            }


            
            public bool TieneAcceso()
            {
                db = new dbstockpointEntities();
                User = db.vwUsuario.Where(U => U.Token.Equals(Token)).FirstOrDefault();
                if (User == null)
                {
                    User = db.vwUsuario.Where(U => U.Nombre.Equals("Público")).FirstOrDefault();

                }
                idResultado = 0;
                idUsuario = User.idUsuario;
                idEstatus = db.tblCatalogo.Where(c => c.Catalogo.Equals("Estatus de Resultados") && c.Valor.Equals("Actualizacion de Pantalla denegada")).FirstOrDefault().Id_Catalogo;
                fecha = DateTime.Now;
                mensaje = "El usuario " + User.Nombre + " no tiene acceso al " + modulo;
                mensajeDeSistema = "";
                bool Acceso = false;
                if ((int)db.uspPermisoToken(modulo, User.Token).FirstOrDefault() == 1)
                {

                    idEstatus = db.tblCatalogo.Where(c => c.Catalogo.Equals("Estatus de Resultados") && c.Valor.Equals("Es correcto")).FirstOrDefault().Id_Catalogo;
                    fecha = DateTime.Now;
                    mensaje = "El usuario " + User.Nombre + " Ingreso al " + modulo;
                    Acceso = true;
                }
                db.usptblResultado(idResultado, idUsuario, modulo, metodos, idEstatus, mensaje, mensajeDeSistema, fecha);

                db.Dispose(); //Matar la bd
                db = null;


                return Acceso;
            }
            #endregion
        }
        public class cUsuario : vwUsuario
        {
            #region propertys
            public List<cUsuarion> Usuarions { get; set; }
            private cEncriptar oCode = new cEncriptar();
            public List<vwUsuario> UsuarioS { get; set; }
            public List<cPermisoporpantalla> Menu { get; set; }
            public List<cPermisoporpantalla> SubMenu { get; set; }
            #endregion

            #region constructor
            public cResul R { get; set; } = new cResul();
            private dbstockpointEntities db;
            public string modulo { get; set; }
            public string ClaveAcceso2 { get; set; }

            #endregion

            #region metodos
            public void Actualizar()
            {

                try
                {
                    db = new dbstockpointEntities();
                    if (R.TieneAcceso())
                    {
                        switch (R.metodos)
                        {
                            case "Cargar":
                                Cargar();
                                break;
                            case "Guardar":
                                Guardar();
                                break;
                            case "Entrar":
                                Entrar(CorreoElectronico, ClaveAcceso);
                                break;
                            case "Acceso":
                                Acceso();
                                break;
                            case "Token":
                                aToken();
                                break;
                            case "AccesoToken":
                                accesoToken();
                                break;
                            case "SolicitarClave":
                                solicitarClave();
                                break;
                            case "CambiarContra":
                                cambiarContra();
                                break;
                            case "RecuperarDataToken":
                                recuperarDataToken();
                                break;

                        }

                    }// Verificar si el método tiene acceso

                    
                    



                }
                catch (Exception e)
                {
                    this.R = new cResul()
                    {
                        Estatus = "Error de sistema",
                        metodos = "Guardar",
                        mensaje = e.Message,
                        fecha = DateTime.Now,
                    };

                }
            }
            private void Cargar()
            {

                try
                {
                    // UsuarioS = db.vwUsuario.Where(p => p.idUsuario == this.idUsuario).FirstOrDefault();

                    //if (UsuarioS != null)
                    //{
                    //    this.Estatus = UsuarioS.Estatus;
                    //    this.Rol = UsuarioS.Rol;
                    //    this.idUsuario = UsuarioS.idUsuario;
                    //    this.Nombre = UsuarioS.Nombre;
                    //    this.CorreoElectronico = UsuarioS.CorreoElectronico;
                    //    this.ClaveAcceso = oCode.Desencriptar(UsuarioS.ClaveAcceso);
                    //    this.Telefono1 = UsuarioS.Telefono1;
                    //    this.Telefono2 = UsuarioS.Telefono2;
                    //    this.idRol = UsuarioS.idRol;
                    //    this.idEstatus = UsuarioS.idEstatus;




                    //}
                    //else
                    //{
                    //    this.Estatus = string.Empty;
                    //    this.Rol = string.Empty;
                    //    this.idUsuario = 0;
                    //    this.Nombre = string.Empty;
                    //    this.CorreoElectronico = string.Empty;
                    //    this.ClaveAcceso = string.Empty;
                    //    this.Telefono1 = string.Empty;
                    //    this.Telefono2 = string.Empty;
                    //    this.idRol = 0;
                    //    this.idEstatus = 0;

                    //}

                    //R.mensaje = Nombre + "Fue guardado correctamente";

                    //db.SaveChanges();

                    //this.R = new cResul()
                    //{
                    //    Estatus = "Correctos",
                    //    metodos = "Guardar",
                    //    mensaje = R.mensaje,
                    //    fecha = DateTime.Now,
                    //    modulo = R.modulo
                    //};
                    //UsuarioS = null;
                    db = new dbstockpointEntities();
                    vwUsuario obj = db.vwUsuario.Where(u => u.idUsuario.Equals(idUsuario)).FirstOrDefault();
                    if (obj != null) { 
                        Nombre = obj.Nombre;
                        CorreoElectronico = obj.CorreoElectronico;
                        ClaveAcceso = oCode.Desencriptar(obj.ClaveAcceso);
                        Telefono1 = obj.Telefono1;
                        Telefono2 = obj.Telefono2;
                        idRol = obj.idRol;
                        idEstatus = obj.idEstatus;
                        Rol = obj.Rol;
                        Estatus = obj.Estatus;

                    }

                    UsuarioS = db.vwUsuario.ToList();
                    UsuarioS = UsuarioS.Where(c => c.idUsuario.Equals(idUsuario)).ToList();

                    UsuarioS = (from item in UsuarioS
                                select new vwUsuario
                                 {
                                    Estatus = item.Estatus,
                                    Rol = item.Rol,
                                    idUsuario = item.idUsuario,
                                    Nombre = item.Nombre,
                                    CorreoElectronico = item.CorreoElectronico,
                                    ClaveAcceso = oCode.Desencriptar(item.ClaveAcceso),
                                    Telefono1 = item.Telefono1,
                                    Telefono2 = item.Telefono2,
                                    idRol = item.idRol,
                                    idEstatus = item.idEstatus

                                }).ToList();
                    db.Dispose();
                    db = null;

                }
                catch (Exception e)
                {
                    this.R = new cResul()
                    {
                        Estatus = "Error de sistema",
                        metodos = "Guardar",
                        mensaje = e.Message,
                        fecha = DateTime.Now,
                    };

                }
            }




            private void Guardar()
            {
                try
                {
                    // Inicializar el objeto de resultado
                    R = new cResul
                    {
                        Estatus = "Correcto", // Inicializar el estado como correcto
                        metodos = "Guardar",  // Asignar el método
                        modulo = "Usuario", // Asignar la modulo
                        fecha = DateTime.Now // Asignar la fecha actual
                    };

                    db = new dbstockpointEntities();
                    // Llamar al procedimiento almacenado para guardar el usuario
                    vwUsuario obj = (from item in db.uspUsuarios2(idUsuario, Nombre, CorreoElectronico, oCode.Encriptar(ClaveAcceso), Telefono1, Telefono2, idRol, idEstatus, Token)
                                   select new vwUsuario
                                   {
                                       Rol = item.Rol,
                                       Estatus = item.Estatus,
                                       idUsuario = item.idUsuario,
                                       Nombre = item.Nombre,
                                       CorreoElectronico = item.CorreoElectronico,
                                       idRol = item.idRol,
                                       idEstatus = item.idEstatus,
                                       ClaveAcceso = item.ClaveAcceso,
                                       Telefono1 = item.Telefono1,
                                       Telefono2 = item.Telefono2,
                                       Token = item.Token,
                                   }).FirstOrDefault();

                        if (obj != null)
                        {
                            // Actualizar las propiedades del usuario
                            Rol = obj.Rol;
                            Estatus = obj.Estatus;
                            idUsuario = obj.idUsuario;
                            Nombre = obj.Nombre;
                            CorreoElectronico = obj.CorreoElectronico;
                            idRol = obj.idRol;
                            idEstatus = obj.idEstatus;
                            ClaveAcceso = oCode.Desencriptar(obj.ClaveAcceso);
                            Telefono1 = obj.Telefono1;
                            Telefono2 = obj.Telefono2;
                            Token = obj.Token;
                        }
                        
                        // Asignar valores adicionales al resultado
                        R.idUsuario = idUsuario;
                        R.mensaje = R.mensaje;
                        R.fecha = DateTime.Now;

                        // Guardar el resultado en la base de datos utilizando cResul
                        cResul resultado = new cResul
                        {
                            idResultado = 0,
                            idUsuario = idUsuario,
                            modulo = "Usuario",
                            metodos = "Guardar",
                            idEstatus = idEstatus,
                            mensaje = R.mensaje,
                            Estatus = R.Estatus,
                            fecha = DateTime.Now
                        };
                    db.Dispose();
                    db = null;
                        resultado.Guardar(); // Llamar al método Guardar de cResul
                    
                }
                catch (Exception e)
                {
                    // Manejar errores inesperados
                    R.Estatus = "Error de sistema";
                    R.mensaje = e.Message;
                    R.fecha = DateTime.Now;

                    // Guardar el error en la base de datos utilizando cResul
                    cResul resultadoError = new cResul
                    {
                        idResultado = 0,
                        idUsuario = idUsuario,
                        modulo = "Usuario",
                        metodos = "Guardar",
                        idEstatus = idEstatus,
                        mensaje = e.Message,
                        Estatus = "Error de sistema",
                        fecha = DateTime.Now
                    };

                    resultadoError.Guardar(); // Llamar al método Guardar de cResul
                }
                finally
                {
                    // Liberar recursos
                    if (db != null)
                    {
                        db.Dispose();
                        db = null;
                    }
                }
            }
            private void Entrar(string correo, string clave)
            {
                using (var db = new dbstockpointEntities()) // Usar "using" para liberar recursos
                {
                    try
                    {
                        tblUsuario usuario = db.tblUsuario.FirstOrDefault(u => u.CorreoElectronico == correo);

                        if (usuario != null)
                        {
                            if (oCode.Desencriptar(usuario.ClaveAcceso) == clave)
                            {
                                // Crear el resultado para el login exitoso
                                var resultado = new tblResultado
                                {
                                    idResultado = 0,
                                    idUsuario = usuario.idUsuario,
                                    mensaje = "Contraseña correcta de " + usuario.CorreoElectronico,
                                    metodos = "Login",
                                    modulo = "Login",
                                    fecha = DateTime.Now,
                                    idEstatus = usuario.idEstatus,
                                };

                                // Llamar al procedimiento almacenado
                                db.usptblResultado(
                                    resultado.idResultado,
                                    resultado.idUsuario,
                                    resultado.modulo,
                                    resultado.metodos,
                                    resultado.idEstatus,
                                    resultado.mensaje,
                                    resultado.mensaje,
                                    resultado.fecha
                                );

                                // Guardar cambios en la base de datos
                                db.SaveChanges();
                                this.R = new cResul()
                                {
                                    Estatus = "Correcto",
                                    Usuario = usuario.Nombre,
                                    metodos = string.Empty,
                                    mensaje = "El resultado se guardo correctamente",
                                    fecha = DateTime.Now,
                                };
                            }
                            else
                            {
                                // En caso de contraseña incorrecta, buscar un WebMaster
                                var webMasterA = db.tblUsuario.Where(u => u.idRol == 4).OrderBy(u => Guid.NewGuid()).FirstOrDefault();

                                if (webMasterA != null)
                                {
                                    // Insertar el error de contraseña incorrecta y asignar WebMaster
                                    var resultado = new tblResultado
                                    {
                                        idResultado = 0,
                                        idUsuario = webMasterA.idUsuario,
                                        mensaje = "Contraseña incorrecta de " + usuario.CorreoElectronico + " se asignó WebMaster",
                                        metodos = "Login",
                                        modulo = "Login",
                                        fecha = DateTime.Now,
                                        idEstatus = usuario.idEstatus,
                                    };

                                    // Llamar al procedimiento almacenado
                                    db.usptblResultado(
                                        resultado.idResultado,
                                        resultado.idUsuario,
                                        resultado.modulo,
                                        resultado.metodos,
                                        resultado.idEstatus,
                                        resultado.mensaje,
                                        resultado.mensaje,
                                        resultado.fecha
                                    );

                                    // Guardar cambios en la base de datos
                                    db.SaveChanges();

                                    this.R = new cResul()
                                    {
                                        Estatus = "Contraseña incorrecta",
                                        metodos = string.Empty,
                                        mensaje = "El resultado se guardo correctamente",
                                        fecha = DateTime.Now,
                                    };
                                }
                                else
                                {

                                    this.R = new cResul()
                                    {
                                        Estatus = "Error: No se encontró WebMaster",
                                        metodos = string.Empty,
                                        mensaje = "El resultado no se guardo correctamente",
                                        fecha = DateTime.Now,
                                    };
                                }
                            }
                        }
                        else
                        {

                            this.R = new cResul()
                            {
                                Estatus = "Error:El correo no existe",
                                metodos = string.Empty,
                                mensaje = "El resultado no se guardo correctamente",
                                fecha = DateTime.Now,
                            };

                        }
                    }
                    catch (Exception e)
                    {
                        // Manejar errores inesperados
                        this.R = new cResul()
                        {
                            Estatus = "Error",
                            metodos = e.Message,
                            mensaje = "El resultado no se guardo correctamente",
                            fecha = DateTime.Now,
                        };
                    }
                }
            }


            private void Acceso()
            {
                //ClaveAcceso = oCode.Desencriptar("127029019222136092049183183172158153158015150156165075061237099103193150188068232031091013102147");
                //ClaveAcceso = ClaveAcceso;
                vwUsuario obj = db.vwUsuario.Where(u => u.CorreoElectronico.Equals(this.CorreoElectronico)).FirstOrDefault();
                if (obj != null)
                {
                    string newClave = oCode.Desencriptar(obj.ClaveAcceso);
                    if (ClaveAcceso != newClave)
                    {
                        var webMasterA = db.tblUsuario.Where(u => u.idRol == 4).OrderBy(u => Guid.NewGuid()).FirstOrDefault();

                        R.idResultado = 0;
                        R.idUsuario = webMasterA.idUsuario;
                        R.mensaje = "Contraseña incorrecta de " + obj.CorreoElectronico + " se asignó WebMaster";
                        R.modulo =R.modulo;
                        R.metodos= R.metodos;
                        R.fecha = DateTime.Now;
                        R.idEstatus = obj.idEstatus;
                        R.Estatus = "Error";
                        R.Guardar();
                        
                    }
                    else
                    {
                        idUsuario = obj.idUsuario;
                        Nombre = obj.Nombre;

                        Token = Guid.NewGuid().ToString();
                        db.uspUPDToken(idUsuario, Token);



                        Menu = (from item in db.uspGetPantallaByToken(Token, 0)
                                select new cPermisoporpantalla
                                {
                                    Descripcion = item.Descripcion,
                                    idPermisoPorPantalla = item.idPermisoPorPantalla,
                                    idPantalla = item.idPantalla,
                                    idOrigen = item.idOrigen,
                                    Orden = item.Orden,
                                    Rol = item.Rol,
                                    Pantalla = item.Pantalla,
                                    ToolTip = item.ToolTip,
                                    url = item.url,
                                    Clase = item.Clase,
                                    MsgGuardar = item.MsgGuardar,
                                    MsgEliminar = item.MsgEliminar,
                                    PreguntaEliminar = item.PreguntaEliminar,
                                    urlMovil = item.urlMovil
                                }).ToList();
                        foreach (cPermisoporpantalla Pantalla in Menu)
                        {
                            List<cPermisoporpantalla> miNuevoSubMenu = (from itemsb in
                                db.uspGetPantallaByToken(Token, Pantalla.idPantalla)
                                                                        select new cPermisoporpantalla
                                                                        {
                                                                            Descripcion = itemsb.Descripcion,
                                                                            idPermisoPorPantalla = itemsb.idPermisoPorPantalla,
                                                                            idPantalla = itemsb.idPantalla,
                                                                            idOrigen = itemsb.idOrigen,
                                                                            Orden = itemsb.Orden,
                                                                            Rol = itemsb.Rol,
                                                                            Pantalla = itemsb.Pantalla,
                                                                            ToolTip = itemsb.ToolTip,
                                                                            url = itemsb.url,
                                                                            Clase = itemsb.Clase,
                                                                            MsgGuardar = itemsb.MsgGuardar,
                                                                            MsgEliminar = itemsb.MsgEliminar,
                                                                            PreguntaEliminar = itemsb.PreguntaEliminar,
                                                                            urlMovil = itemsb.urlMovil
                                                                        }).ToList();
                            Pantalla.SubMenu = miNuevoSubMenu;
                            miNuevoSubMenu = null;

                        }

                        //Token = oCode.Encriptar(Token);
                        R.mensaje = Nombre + " Ingresó al sistema.";
                        R.Estatus = "Correcto";
                        R.User = obj;
                        R.Menu = Menu;




                        R.Token = Token;
                        R.idResultado = 0;
                        R.mensaje = "Contraseña correcta de " + obj.CorreoElectronico ;
                        R.idUsuario = obj.idUsuario;
                        R.modulo = R.modulo;
                        R.metodos = R.metodos;
                        R.idEstatus = obj.idEstatus;
                        R.Estatus = "Correcto";
                        R.fecha = DateTime.Now;
                        R.Guardar();

                    }

                }

                #endregion


            }

           
            private void aToken()
            {
                vwUsuario obj = db.vwUsuario.Where(t => t.Token.Equals(this.Token)).FirstOrDefault();
                if (obj != null)
                {




                    idUsuario = obj.idUsuario;
                    Nombre = obj.Nombre;
                    CorreoElectronico = obj.CorreoElectronico;






                    R.mensaje = "Token correcto de " + CorreoElectronico ;
                    R.idUsuario = idUsuario;
                    R.idEstatus = idEstatus;
                    R.Estatus = R.mensaje;
                    Token = Guid.NewGuid().ToString();
                    db.uspUPDToken(idUsuario, Token);



                }
                else
                {
                    var webMasterA = db.tblUsuario.Where(u => u.idRol == 4).OrderBy(u => Guid.NewGuid()).FirstOrDefault();


                    R.idUsuario = webMasterA.idUsuario;
                    R.mensaje = "Token incorrecto se asignó WebMaster ";
                    R.modulo = R.modulo;
                    R.Estatus = R.mensaje;
                    R.idEstatus = R.idEstatus;


                }
                this.R = new cResul()
                {
                    idUsuario = R.idUsuario,
                    idEstatus = 1,
                    metodos = "Token",
                    mensaje = R.mensaje,
                    modulo = R.modulo,
                    fecha = DateTime.Now,
                    Estatus = R.mensaje
                };
                R.Guardar();


            }

            public void accesoToken()
            {
                try
                {
                    // Llamar al procedimiento almacenado y obtener el resultado
                    vwUsuario ne = (from item in db.uspTokenUsuarioN(this.modulo, this.Token)
                                    select new vwUsuario
                                    {
                                        Nombre = item.Nombre,
                                        idUsuario = item.idUsuario,
                                        idRol = item.idRol,
                                        Rol = item.Rol,
                                        Estatus = item.Estatus
                                    }).FirstOrDefault();

                    // Verificar si el usuario es válido y no es "Público"
                    if (ne != null && ne.Nombre != "Publico")
                    {

                        R.mensaje = "Acceso autorizado a modulo " + this.modulo + " el usuario: " + ne.Nombre;
                        R.idUsuario = ne.idUsuario;
                        R.idEstatus = ne.idRol;
                        R.Estatus = ne.Estatus;
                        R.modulo = this.modulo;
                    }
                    else
                    {
                        var webMasterA = db.tblUsuario.Where(u => u.idRol == 4).OrderBy(u => Guid.NewGuid()).FirstOrDefault();


                        R.mensaje = "Acceso denegado a modulo " + this.modulo + " el usuario: " + (ne != null ? ne.Nombre : "Público");
                        R.idUsuario = webMasterA.idUsuario;
                        R.idEstatus = ne.idRol;  // Cambiado a 7 para coincidir con el procedimiento almacenado
                        R.Estatus = ne.Estatus;
                        R.modulo = this.modulo;
                    }
                }
                catch (Exception ex)
                {
                    var webMasterA = db.tblUsuario.Where(u => u.idRol == 4).OrderBy(u => Guid.NewGuid()).FirstOrDefault();

                    // En caso de error, enviar el mensaje de error al objeto R
                    R.mensaje = "Error al verificar el acceso: " + ex.Message;
                    R.idUsuario = webMasterA.idUsuario;
                    R.idEstatus = 7;
                    R.Estatus = "Error";
                    R.modulo = this.modulo;
                }

                // Crear un nuevo objeto cResul y guardar el resultado
                this.R = new cResul()
                {
                    idUsuario = R.idUsuario,
                    idEstatus = R.idEstatus,
                    Estatus = R.Estatus,
                    metodos = "Entrar modulo",
                    mensaje = R.mensaje,
                    modulo = R.modulo,
                    fecha = DateTime.Now,
                };

                // Guardar el resultado
                R.Guardar();
            }


            private void solicitarClave()
            {
                try
                {
                    R = new cResul
                    {
                        Estatus = "Correcto", // Inicializar el estado como correcto
                        metodos = "SolicitarClave",  // Asignar el método
                        modulo = "Login", // Asignar la modulo
                        fecha = DateTime.Now // Asignar la fecha actual
                    };

                    
                    
                    vwUsuario obj = db.vwUsuario.Where(u=>u.CorreoElectronico.Equals(CorreoElectronico)).FirstOrDefault();
                    if (obj == null)
                    {
                         R = new cResul
                        {
                            idResultado = 0,
                            idUsuario = idUsuario,
                            modulo = "Login",
                            metodos = "SolicitarClave",
                            idEstatus = idEstatus,
                            mensaje = "No hay una cuenta asociada con este correo", 
                            Estatus = "Error",
                            fecha = DateTime.Now
                        };
                        return;
                        
                    }else
                    {
                        cOcadisINI.Val val = new cOcadisINI.Val();
                        
                        string token = Guid.NewGuid().ToString();
                        db.usptblTokenRecuperar(token,obj.idUsuario);
                        string htmlCorreo = val.LeerArchivo("~/Content/RecurperarClave.html");
                        DateTime fechaExpiracion = DateTime.Now.AddHours(1);
                        htmlCorreo = htmlCorreo.Replace("{usuario}", obj.Nombre); // Asegúrate de que obj tenga esta propiedad
                        htmlCorreo = htmlCorreo.Replace("{link}", GenerarLinkRecuperacion(token)); // Método para generar el link
                        htmlCorreo = htmlCorreo.Replace("{fechaExpiracion}", fechaExpiracion.ToString("dd-MMM-yyyy HH:mm:ss"));
                        
                        cOcadisINI.cMail OCorreo = new cOcadisINI.cMail()
                        {
                            Cuerpo = htmlCorreo,
                            Para = obj.CorreoElectronico,
                            Asunto = "Restablecimiento de contraseña - Válido por 1 hora" // Agregar asunto
                        };

                        OCorreo.EnviarCorreo();

                        val = null;
                        OCorreo = null;

                        R.idResultado = 0;
                        R.idUsuario = obj.idUsuario;
                        R.idEstatus = obj.idEstatus;
                        R.mensaje = "Se a enviado datos a " + obj.CorreoElectronico;
                        R.Estatus = "Correcto";
                        R.fecha = DateTime.Now;
                        
                        R.Guardar();
                    }   
                    obj =null;
                   

                }catch(Exception e)
                {
                    R = new cResul
                    {
                        idResultado = 0,
                        idUsuario = idUsuario,
                        idEstatus = idEstatus,
                        mensaje = e.Message,
                        Estatus = "Error de sistema" + e.Message,
                        fecha = DateTime.Now
                    };
                    
                }
            }
            private void recuperarDataToken()
            {
                var datosTo = db.vwtblTokenRecuperar.Where(t => t.Token == this.Token &&
                                            t.FechaExpiracion > DateTime.Now).FirstOrDefault();
                if (datosTo == null)
                {
                    R.Estatus = "Error";
                    R.mensaje = "El token no es válido o ha expirado";
                    R.fecha = DateTime.Now;
                    R.modulo = this.modulo;
                    R.metodos = "RecuperarDataToken";

                    return;
                }
                else
                {

                    R.idUsuario = datosTo.idUsuario;
                    R.Usuario = datosTo.Nombre;
                    R.modulo = R.modulo;
                    R.metodos = R.metodos;
                    R.Estatus = "Correcto";
                    R.fecha = DateTime.Now;
                    R.Actualizar();
                    return;
                }

            }


            
            private void cambiarContra()
            {
                try
                {
                    
                    R = new cResul();
                    
                    // 1. Validar el token (corregido Include y relación)
                    db = new dbstockpointEntities();
                    var tokenValido = db.vwtblTokenRecuperar
                                      .Where(t => t.Token == this.Token &&
                                            t.FechaExpiracion > DateTime.Now)
                                      .FirstOrDefault();

                    if (tokenValido == null)
                    {

                        R.Estatus = "Error";
                        R.mensaje = "El token no es válido o ha expirado";
                        R.fecha = DateTime.Now;
                        R.modulo = this.modulo;
                        R.metodos = this.R?.metodos ?? "cambiarContra";

                        return;
                    }

                    // 2. Validar coincidencia de contraseñas
                    if (this.ClaveAcceso != this.ClaveAcceso2)
                    {

                        R.Estatus = "Error";
                        R.mensaje = "Las contraseñas no coinciden";
                        R.fecha = DateTime.Now;
                        R.modulo = this.modulo;
                        R.metodos = this.R?.metodos ?? "cambiarContra";

                        return;
                    }

                    // 3. Validar fortaleza de contraseña (corregido el return faltante)
                    if (!ValidarClaveAcceso(this.ClaveAcceso))
                    {

                        R.Estatus = "Error";
                        R.mensaje = "La contraseña debe tener más de 8 caracteres, " +
                                 "contener al menos un número, un carácter especial y una mayúscula.";
                        R.fecha = DateTime.Now;
                        R.modulo = this.modulo;
                        R.metodos = this.R?.metodos ?? "cambiarContra";

                        return;
                    }

                    // 4. Obtener usuario (corregido - buscar por idUsuario del token)
                    var usuario = db.tblUsuario.Find(tokenValido.idUsuario);
                    if (usuario == null)
                    {

                        R.Estatus = "Error";
                        R.mensaje = "Usuario no encontrado";
                        R.fecha = DateTime.Now;
                        R.modulo = this.modulo;
                        R.metodos = this.R?.metodos ?? "cambiarContra";

                        return;
                    }

                    // 5. Actualizar contraseña
                    usuario.ClaveAcceso = oCode.Encriptar(this.ClaveAcceso);

                    // 6. Marcar token como utilizado (si tu tabla tiene este campo)
                    db.uspDelTokenRecuperacion(usuario.idUsuario);

                    cOcadisINI.Val val = new cOcadisINI.Val();

                    cCatalogo oCatalogo = new cCatalogo();
                    string baseUrl = oCatalogo.Configuracion("DominioW");

                    string htmlCorreo = val.LeerArchivo("~/Content/SeCamboContraseña.html");
                    DateTime fechaCambio = DateTime.Now;
                    htmlCorreo = htmlCorreo.Replace("{usuario}", usuario.Nombre); // Asegúrate de que obj tenga esta propiedad
                    htmlCorreo = htmlCorreo.Replace("{fechaCambio}", fechaCambio.ToString("dd-MMM-yyyy HH:mm:ss"));
                    htmlCorreo = htmlCorreo.Replace("{linkAcceso}", baseUrl);

                    cOcadisINI.cMail OCorreo = new cOcadisINI.cMail()
                    {
                        Cuerpo = htmlCorreo,
                        Para = usuario.CorreoElectronico,
                        Asunto = "Se cambio contraseña en tu cuenta de StockPoint" // Agregar asunto
                    };

                    OCorreo.EnviarCorreo();

                    val = null;
                    OCorreo = null;


                    db.SaveChanges();

                    // 7. Respuesta exitosa

                    R = new cResul
                    {
                        idResultado = 0,
                        mensaje = "Contraseña cambiada correctamente",
                        idUsuario = usuario.idUsuario,
                        Usuario = usuario.Nombre,
                        modulo = this.modulo ?? "Autenticación",
                        metodos = this.R?.metodos ?? "cambiarContra",
                        idEstatus = usuario.idEstatus,
                        Estatus = "Correcto",
                        fecha = DateTime.Now
                    };

                    // Guardar antes de liberar recursos
                    R.Guardar();
                    db.Dispose();
                    db = null;
                    return;
                }
                catch (Exception e)
                {
                    R = new cResul
                    {
                        Estatus = "Error de sistema",
                        mensaje = "Error al cambiar la contraseña: " + e.Message,
                        fecha = DateTime.Now,
                        modulo = this.modulo ?? "Autenticación",
                        metodos = this.R?.metodos ?? "cambiarContra"
                    };

                    // Log adicional del error
                    System.Diagnostics.Debug.WriteLine($"Error en cambiarContra: {e}");
                }
                finally
                {
                    try
                    {
                        if (R != null)
                        {
                            R.Guardar();
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error al guardar resultado: {ex}");
                    }

                    // Liberar recursos
                    if (db != null)
                    {
                        db.Dispose();
                    }
                }
            }
            public bool ValidarClaveAcceso(string clave)
            {
                if (string.IsNullOrWhiteSpace(clave) || clave.Length < 8)
                    return false;

                // Verificar al menos un número
                if (!clave.Any(char.IsDigit))
                    return false;

                // Verificar al menos una mayúscula
                if (!clave.Any(char.IsUpper))
                    return false;

                // Verificar al menos un carácter especial
                var caracteresEspeciales = "!@#$%^&*()-_+=".ToCharArray();
                if (!clave.Any(c => caracteresEspeciales.Contains(c)))
                    return false;

                return true;
            }

            
            private string GenerarLinkRecuperacion(string token)
            {
                // Asegúrate de que esta URL sea accesible desde el exterior
                cCatalogo oCatalogo = new cCatalogo();

                string baseUrl = oCatalogo.Configuracion("DominioW");

                // Cambiar por tu dominio real
                return $"{baseUrl}Home/RecuperarContra?token={token}";
               
            }
            
            













        }
        public class cPantalla : vwPantallas
        {
            #region propertys
            public List<tblPantalla> Pantallas { get; set; }
            public List<cPantalla> Permisos { get; set; }
            public string Token { get; set; }
            public string modulo { get; set; }
            public string metodos { get; set; }
            public int idRol { get; set; }
            public string Texto { get; set; }
            public int TienePermiso { get; set; }
            public int Estatusi { get; set; }
            public string nombreP { get; set; }
            public List<vwPantallas> Menu { get; set; }
            public List<vwPantallas> Menu2 { get; set; }
            //public List<vwPantallas> SubMenu { get; set; }
            #endregion

            #region constructor
            public cResul R { get; set; }
            private dbstockpointEntities db;

            #endregion

            #region Metodos
            public void Actualizar()
            {
                try
                {
                    db = new dbstockpointEntities();
                    if (R.TieneAcceso())
                    {
                        switch (R.metodos)
                        {
                            case "Cargar":
                                Cargar();
                                break;
                            case "Guardar":
                                Guardar();
                                break;
                            case "GuardarPermisos":
                                GuardarPermisos();
                                break;
                            case "obtenerPan":
                                obtenerPan();
                                break;

                        }
                    }
                }
                catch (Exception e)
                {
                    R = new cResul
                    {
                        Estatus = "Error de sistema",
                        mensaje = e.Message,
                        fecha = DateTime.Now,
                        modulo = "Pantalla",
                        metodos = "Actualizar"
                    };
                }
            }

            private void Cargar()
            {
                
                try
                {
                    db = new dbstockpointEntities();
                    Permisos = (from item in db.uspGetPantallaCheck(idPantalla).ToList()
                                select new cPantalla
                                {
                                    idRol = item.idRol,
                                    Texto = item.Texto,
                                    TienePermiso = item.TienePermiso,
                                }).ToList();
                    vwPantallas obj = db.vwPantallas.Where(p => p.idPantalla.Equals(idPantalla)).FirstOrDefault();
                    
                    if (obj != null) { 
                        
                        this.idPantalla = obj.idPantalla;
                        this.Descripcion = obj.Descripcion;
                        this.Orden = obj.Orden;
                        this.url = obj.url;
                        this.Clase = obj.Clase;
                        this.ToolTip = obj.ToolTip;
                        this.MsgGuardar = obj.MsgGuardar;
                        this.MsgEliminar = obj.MsgEliminar;
                        this.PreguntaEliminar = obj.PreguntaEliminar;
                        this.idOrigen = obj.idOrigen;
                        this.idEstatus = obj.idEstatus;
                        this.Estatus = obj.Estatus;
                        this.Pantalla = obj.Pantalla;
                        this.Clase = obj.Clase;
                        this.urlMovil = obj.urlMovil;



                        

                        R.mensaje = "Pantalla cargada correctamente";
                        R.Estatus = "Correcto";
                        R.idEstatus = 1; // Asignar un valor por defecto o el estatus actual si es necesario
                    }
                    else
                    {
                        R.mensaje = "No se encontró la pantalla con el ID especificado";
                        R.Estatus = "Error";
                        R.idEstatus = 2;
                    }
                    
                    R.fecha = DateTime.Now;
                    R.idResultado= 0;
                    R.idUsuario = idPantalla; // Asignar un valor por defecto o el usuario actual si es necesario
                    R.modulo = modulo;
                    R.metodos = metodos;
                    R.idEstatus = R.idEstatus;
                    R.mensajeDeSistema = R.Estatus;
                    R.Guardar();
                    db.Dispose();
                    db = null;
                }
                catch (Exception e)
                {
                    R = new cResul
                    {
                        Estatus = "Error de sistema",
                        mensaje = e.Message,
                        fecha = DateTime.Now,
                        modulo = "Pantalla",
                        metodos = "Cargar"
                    };
                }
            }

            private void Guardar()
            {
                try
                {





                    db = new dbstockpointEntities();
                    tblPantalla pantallas =new tblPantalla
                    { idEstatus = this.idEstatus,
                        Descripcion = this.Descripcion,
                        Orden = this.Orden,
                        url = this.url,
                        Clase = this.Clase,
                        ToolTip = this.ToolTip,
                        MsgGuardar = this.MsgGuardar,
                        MsgEliminar = this.MsgEliminar,
                        PreguntaEliminar = this.PreguntaEliminar,
                        idOrigen = this.idOrigen,
                        Pantalla = this.Pantalla,
                        idPantalla = this.idPantalla ,
                        urlMovil=this.urlMovil,
                    };

                    //foreach(var permiso in this.Permisos)
                    //{
                    //    db.uspPermisoPorPantalla(pantallas.idPantalla, permiso.idRol, permiso.Estatusi);
                    //}
                    db.uspPantalla(pantallas.idOrigen,pantallas.Orden,pantallas.Pantalla,pantallas.url,pantallas.Clase,pantallas.ToolTip,pantallas.Descripcion,pantallas.idEstatus, pantallas.MsgGuardar, pantallas.MsgEliminar, pantallas.PreguntaEliminar,pantallas.urlMovil);

                    //db.uspPermisoPorPantalla(idPantalla, idRol, idEstatus);
                    db.SaveChanges();
                    R = new cResul { Estatus = "OK", mensaje = "Pantalla guardada correctamente" };
                    db.Dispose();
                    db = null;

                }
                catch (Exception e)
                {
                    R = new cResul
                    {
                        Estatus = "Error de sistema",
                        mensaje = e.Message,
                        fecha = DateTime.Now,
                        modulo = "Pantalla",
                        metodos = "Actualizar"
                    };
                }
            }
            //this.idPantalla = obj.idPantalla;
            //            this.Descripcion = obj.Descripcion;
            //            this.Orden = obj.Orden;
            //            this.url = obj.url;
            //            this.Clase = obj.Clase;
            //            this.ToolTip = obj.ToolTip;
            //            this.MsgGuardar = obj.MsgGuardar;
            //            this.MsgEliminar = obj.MsgEliminar;
            //            this.PreguntaEliminar = obj.PreguntaEliminar;
            //            this.idOrigen = obj.idOrigen;
            //            this.idEstatus = obj.idEstatus;
            //            this.Estatus = obj.Estatus;
            //            this.Pantalla = obj.Pantalla;
            //            this.Clase = obj.Clase;
            private void obtenerPan()
            {
                db = new dbstockpointEntities();
                Menu = db.vwPantallas.ToList();
                Menu2 = db.vwPantallas.ToList();
                Menu = (from item in Menu
                        select new vwPantallas
                        {
                            Descripcion = item.Descripcion,
                            Estatus = item.Estatus,
                            idPantalla = item.idPantalla,
                            idOrigen = item.idOrigen,
                            Orden = item.Orden,
                            idEstatus = item.idEstatus,
                            Pantalla = item.Pantalla,
                            ToolTip = item.ToolTip,
                            url = item.url,
                            Clase = item.Clase,
                            MsgGuardar = item.MsgGuardar,
                            MsgEliminar = item.MsgEliminar,
                            PreguntaEliminar = item.PreguntaEliminar,
                            urlMovil = item.urlMovil
                        }).ToList();
                foreach (vwPantallas Pantalla in Menu)
                {
                    List<vwPantallas> miNuevoSubMenu = (from itemsb in
                        Menu2.Where(p=>p.idOrigen.Equals(Pantalla.idPantalla))
                                                                select new vwPantallas
                                                                {
                                                                    Descripcion = itemsb.Descripcion,
                                                                    Estatus = itemsb.Estatus,
                                                                    idPantalla = itemsb.idPantalla,
                                                                    idOrigen = itemsb.idOrigen,
                                                                    Orden = itemsb.Orden,
                                                                    idEstatus = itemsb.idEstatus,
                                                                    Pantalla = itemsb.Pantalla,
                                                                    ToolTip = itemsb.ToolTip,
                                                                    url = itemsb.url,
                                                                    Clase = itemsb.Clase,
                                                                    MsgGuardar = itemsb.MsgGuardar,
                                                                    MsgEliminar = itemsb.MsgEliminar,
                                                                    PreguntaEliminar = itemsb.PreguntaEliminar,
                                                                    urlMovil = itemsb.urlMovil
                                                                }).ToList();
                    Pantalla.SubMenu = miNuevoSubMenu;
                    miNuevoSubMenu = null;

                }
                db.Dispose();
                db = null;
            }

            
            private void GuardarPermisos()
            {
                try
                {
                    db = new dbstockpointEntities();
                    if (Permisos != null && Permisos.Count > 0)
                    {
                        foreach (var permiso in Permisos)
                        {
                            db.uspPermisoPorPantalla(permiso.idPantalla, permiso.idRol, permiso.Estatusi);
                        }
                        db.SaveChanges();
                        vwPantallas pantalla = db.vwPantallas.Where(p => p.idPantalla.Equals(idPantalla)).FirstOrDefault();
                        nombreP = pantalla.Pantalla;
                        R = new cResul { Estatus = "OK", mensaje = "Permisos guardados correctamente" };
                    }
                    else
                    {
                        R = new cResul { Estatus = "Error", mensaje = "No hay permisos para guardar" };
                    }
                    db.Dispose();
                    db = null;
                }
                catch (Exception e)
                {
                    R = new cResul
                    {
                        Estatus = "Error de sistema",
                        mensaje = e.Message,
                        fecha = DateTime.Now,
                        modulo = "Pantalla",
                        metodos = "GuardarPermisos"
                    };
                }
            }
            #endregion
        }
        public class cCompra :vwtblCompraDetallada
        {
            #region propertys
            public List<tblProducto> Producto { get; set; }
            public List<vwtblCompraDetallada> Compra { get; set; }
            public string idProductos { get; set; }
            public DateTime FechaFinal { get; set; }
            public DateTime FechaInicial { get; set; }
            public List<cVenTa> Existencias { get; set; }
            //public int idTipoPago { get; set; }
            //public int idCompra { get; set; }
            //public int idProovedor { get; set; }
            //public string idProductos { get; set; }
            public int movil { get; set; }
            public double? PrecioPublico { get; set; }
            public string Nombre { get; set; }
            #endregion

            #region constructor
            public cResul R { get; set; }
            private dbstockpointEntities db;

            #endregion



            #region Metodos
            public void Actualizar()
            {
                try
                {
                    //db = new dbstockpointEntities();
                    if (R.TieneAcceso())
                    {
                        switch (R.metodos)
                        {
                            case "Cargar":
                                Cargar();
                                break;
                            case "Guardar":
                                Guardar();
                                break;
                            case "BCompra":
                                BCompra();
                                break;

                        }
                    }


                }
                catch (Exception e)
                {
                    R = new cResul
                    {
                        Estatus = "Error de sistema",
                        mensaje = e.Message,
                        fecha = DateTime.Now,
                        modulo = "Pantalla",
                        metodos = "Actualizar"
                    };
                }
            }


            public void Cargar()
            {
                db = new dbstockpointEntities();
                Producto = db.tblProducto.ToList();
                
                Producto = Producto.Where(c => c.Existencia < c.CantidadMinima).ToList();
                Producto = (from item in Producto
                            select new tblProducto
                            {
                                Descripcion = item.Descripcion,
                                idProducto = item.idProducto,
                                PiezasXcaja = item.PiezasXcaja,
                                idUnidad = item.idUnidad,
                                PiezasXPaquete = item.PiezasXPaquete,
                                CantidadMinima = item.CantidadMinima,
                                Existencia = item.Existencia,
                                CostoXCaja = item.CostoXCaja,
                                PrecioPublico = item.PrecioPublico,
                                Productos = item.Productos,
                                Codigo = item.Codigo,
                                
                            }).ToList();

                db.Dispose();




            }

            public void Guardar()
            {
                try
                {
                    db = new dbstockpointEntities();
                    tblCompra compra = new tblCompra
                    {
                        idCompra = this.idCompra,
                        idProovedor = this.idProveedor,
                        idTipoPago = this.idTipoPago,
                        PrecioTotal = this.PrecioTotal,
                        Fecha = DateTime.Now,
                        idProductos = this.idProductos,
                        

                    };
                    
                    db.usptblCompra(compra.idProovedor, compra.Fecha, compra.PrecioTotal, compra.idTipoPago, compra.idProductos);
                    
                    if (Existencias != null && Existencias.Count > 0)
                    {
                        foreach (var existencias in Existencias)
                        {
                            var valoe = -existencias.cantidad;
                            db.uspUEProducto(existencias.idProducto, valoe);
                        }
                    }
                    db.SaveChanges();
                    R = new cResul { Estatus = "OK", mensaje = "Compra guardada correctamente" };
                    db.Dispose();
                    db = null;
                }
                catch (Exception e)
                {
                    R = new cResul
                    {
                        Estatus = "Error de sistema",
                        mensaje = e.Message,
                        fecha = DateTime.Now,
                        modulo = "Compra",
                        metodos = "Guardar"
                    };
                }
            }

            public void BCompra()
            {
                db = new dbstockpointEntities();
                Compra = db.vwtblCompraDetallada.ToList();

                if (!string.IsNullOrEmpty(idProveedor.ToString()) && idProveedor != 0)
                {
                    Compra = Compra.Where(r => r.idProveedor.Equals(idProveedor)).ToList();
                }
                if (FechaInicial != DateTime.MinValue)
                {
                    if (FechaFinal == DateTime.MinValue)
                    {
                        FechaFinal = FechaInicial.AddHours(24);
                    }
                    else
                    {
                        FechaFinal = FechaFinal.AddHours(24);
                    }
                    Compra = Compra.Where(o => o.Fecha >= FechaInicial && o.Fecha < FechaFinal).ToList();
                }
                if (!string.IsNullOrEmpty(idTipoPago.ToString()) &&  idTipoPago != 0)
                {
                    Compra = Compra.Where(o => o.idTipoPago >= idTipoPago).ToList();
                }
                R = new cResul { Estatus = "OK", mensaje = "Compras cargadas correctamente" };

                db.Dispose();
                db = null;


            }


            #endregion
        }
        public class cProducto : vwProducto
        {
            #region propertys
            public List<vwProducto> Producto { get; set; }
           
            public int movil { get; set; }
            public double? PrecioPublico { get; set; }
            public string Nombre { get; set; }
            #endregion

            #region constructor
            public cResul R { get; set; }
            private dbstockpointEntities db;

            #endregion

            #region Metodos
            public void Actualizar()
            {
                try
                {
                    //db = new dbstockpointEntities();
                    if (R.TieneAcceso())
                    {
                        switch (R.metodos)
                        {
                            case "Cargar":
                                Cargar();
                                break;
                            case "Guardar":
                                Guardar();
                                break;
                            

                        }
                    }


                }
                catch (Exception e)
                {
                    R = new cResul
                    {
                        Estatus = "Error de sistema",
                        mensaje = e.Message,
                        fecha = DateTime.Now,
                        modulo = "Pantalla",
                        metodos = "Actualizar"
                    };
                }
            }

            private void Guardar()
            {
                try
                {

                    db = new dbstockpointEntities();
                    vwProducto productog = new vwProducto
                    {
                        idUnidad = this.idUnidad,
                        idProducto = this.idProducto,
                        Descripcion = this.Descripcion,
                        PiezasXcaja = this.PiezasXcaja,
                        PiezasXPaquete = this.PiezasXPaquete,
                        CantidadMinima = this.CantidadMinima,
                        Existencia = this.Existencia,
                        CostoXCaja = this.CostoXCaja,
                        Precio = this.PrecioPublico,
                        Productos = this.Productos,
                        Codigo = this.Codigo,
                        Imagen = this.Imagen,
                        idCategoria=this.idCategoria,
                        idEstatus = this.idEstatus
                    };

                    db.uspProducto(productog.Descripcion, productog.idProducto, productog.PiezasXcaja, productog.idUnidad,productog.PiezasXPaquete, productog.CantidadMinima, productog.Existencia,
                        productog.idCategoria, productog.CostoXCaja, productog.Precio,productog.Productos, productog.Codigo, productog.Imagen, productog.idEstatus);
                    db.SaveChanges();
                    R = new cResul { Estatus = "OK", mensaje = "Producto guardada correctamente" };
                    db.Dispose();
                    db = null;

                }
                catch(Exception e)
                {
                    R = new cResul
                    {
                        Estatus = "Error de sistema",
                        mensaje = e.Message,
                        fecha = DateTime.Now,
                        modulo = "Producto",
                        metodos = "Guardar"
                    };
                }
                


            }

            private void Cargar()
            {
                try
                {
                    db = new dbstockpointEntities();

                    if (movil == 1)
                    {
                        Producto = db.vwProducto.ToList();
                        Producto = Producto.Where(c => c.idProducto.Equals(idProducto)).ToList();

                        Producto = (from item in Producto
                                    select new vwProducto
                                    {
                                          Descripcion = item.Descripcion,
                                          idProducto = item.idProducto,
                                          PiezasXcaja = item.PiezasXcaja,
                                          idUnidad = item.idUnidad,
                                          PiezasXPaquete = item.PiezasXPaquete,
                                          CantidadMinima = item.CantidadMinima,
                                          Existencia = item.Existencia,
                                          CostoXCaja = item.CostoXCaja,
                                          Precio = item.Precio,
                                          Productos = item.Productos,
                                          Codigo = item.Codigo,
                                          idCategoria = item.idCategoria,
                                          Imagen = item.Imagen,
                                          idEstatus =item.idEstatus
                                      }).ToList();
                    }
                    else
                    {
                        Producto = (from item in db.vwProducto.ToList()
                                    select new vwProducto
                                    {
                                        Descripcion = item.Descripcion,
                                        idProducto = item.idProducto,
                                        PiezasXcaja = item.PiezasXcaja,
                                        idUnidad = item.idUnidad,
                                        PiezasXPaquete = item.PiezasXPaquete,
                                        CantidadMinima = item.CantidadMinima,
                                        Existencia = item.Existencia,
                                        CostoXCaja = item.CostoXCaja,
                                        Precio = item.Precio,
                                        Productos = item.Productos,
                                        Codigo = item.Codigo,
                                        idCategoria = item.idCategoria,
                                        Imagen = item.Imagen,
                                        idEstatus = item.idEstatus
                                    }).ToList();


                        vwProducto obj = db.vwProducto.Where(p => p.idProducto.Equals(idProducto)).FirstOrDefault();

                        if (obj != null)
                        {

                            Descripcion = obj.Descripcion;
                            idProducto = obj.idProducto;
                            PiezasXcaja = obj.PiezasXcaja;
                            idUnidad = obj.idUnidad;
                            PiezasXPaquete = obj.PiezasXPaquete;
                            CantidadMinima = obj.CantidadMinima;
                            Existencia = obj.Existencia;
                            CostoXCaja = obj.CostoXCaja;
                            Precio = obj.Precio;
                            Productos = obj.Productos;
                            Codigo = obj.Codigo;
                            idCategoria = obj.idCategoria;
                            Imagen = obj.Imagen;
                            idEstatus = obj.idEstatus;


                            R.mensaje = "Producto cargada correctamente";
                            R.Estatus = "Correcto";
                            R.idEstatus = 1; // Asignar un valor por defecto o el estatus actual si es necesario
                        }
                        else
                        {
                            R.mensaje = "No se encontró el Producto con el ID especificado";
                            R.Estatus = "Error";
                            R.idEstatus = 2;
                        }

                        R.fecha = DateTime.Now;
                        R.idResultado = 0;
                        R.idUsuario = idProducto; // Asignar un valor por defecto o el usuario actual si es necesario
                        R.modulo = Nombre;
                        R.metodos = "Cargar";
                        R.idEstatus = R.idEstatus;
                        R.mensajeDeSistema = R.Estatus;
                        R.Guardar();
                    }
                       
                   
                    db.Dispose();
                    db = null;


                }
                catch (Exception e)
                {
                    R = new cResul
                    {
                        Estatus = "Error de sistema",
                        mensaje = e.Message,
                        fecha = DateTime.Now,
                        modulo = "Pantalla",
                        metodos = "Actualizar"
                    };
                }
            }

            
        
           


            
            #endregion
        }
        public class cVenTa:vwVenta
        {
            #region propertys
            public DateTime FechaFinal { get; set; }
            public DateTime FechaInicial { get; set; }
            //public string Cliente { get; set; }
            public string Token { get; set; }
            public int idProducto { get; set; }
            public int cantidad { get; set; }
            public int idCliente { get; set; }
            
            public List<cVenTa> Existencias { get; set; }
            public List<vwVenta> Ventas { get; set; }
            public List<tblProducto> Producto { get; set; }
            
            #endregion

            #region constructor
            public cResul R { get; set; }
            private dbstockpointEntities db;
            #endregion


            #region Metodos

            public void Actualizar()
            {
                try
                {
                    db = new dbstockpointEntities();
                    //db = new dbstockpointEntities();
                    if (R.TieneAcceso())
                    {
                        switch (R.metodos)
                        {
                            
                            case "Guardar":
                                Guardar();
                                break;
                            case "Cargar":
                                Cargar();
                                break;
                            case "CargarP":
                                CargarP();
                                break;
                            case "GuardarCT":
                                GuardarCT();
                                break;
                            case "CargarITD":
                                CargarITD();
                                break;
                        }
                    }
                    db.Dispose();
                    db = null;

                }
                catch (Exception e)
                {
                    R = new cResul
                    {
                        Estatus = "Error de sistema",
                        mensaje = e.Message,
                        fecha = DateTime.Now,
                        modulo = "Venta",
                        metodos = "Actualizar"
                    };
                }
            }
            private void GuardarCT()
            {
                var datosVentas = db.vwVenta.Where(v=>v.idCliente.Equals(this.idCliente) && v.Estatus.Equals("Deuda")).ToList();
                decimal? PagoU = this.Pago;
                 
                    foreach (var venta in datosVentas)
                    {
                    if (PagoU > 0)
                    {
                        decimal? pagoFin = PagoU < venta.Saldo ? PagoU + venta.Pago : venta.Pago + venta.Saldo;
                        db.uspVenta(venta.idVenta, venta.Fecha, venta.Importe, venta.idUsuario, pagoFin, venta.idTipoPago, venta.Cliente, venta.Movil);
                        PagoU = PagoU - venta.Saldo;
                    }
                            
                        
                    db.SaveChanges();
                }
                R = new cResul { Estatus = "OK", mensaje = "Deuda Total actualizada correctamente" };
            }
            public void CargarITD()
            {
                var datost = db.vwVenta.Where(v => v.Cliente.Equals(this.Cliente) && v.Estatus.Equals("Deuda")).ToList();
                decimal? total = 0;
                
                foreach (var venta in datost)
                {
                    total = total + venta.Saldo;

                }
                int id = datost[0].idCliente;
                vwVenta obj = db.vwVenta.Where(p => p.idCliente.Equals(id)).FirstOrDefault();
                if (obj != null) {
                    Saldo = total;
                    idCliente = obj.idCliente;
                }
                
            }
            public void Guardar()
                {
                    try
                    {
                        
                        
                        vwVenta objVU = new vwVenta();
                        var datosTo = db.vwUsuario.Where(t => t.Token == this.Token).FirstOrDefault();
                        if (this.idVenta > 0)
                        {
                            objVU = db.vwVenta.Where(v => v.idVenta == this.idVenta).FirstOrDefault();
                        }
                        var idcliente = this.idCliente > 0 ? this.idCliente : objVU.idCliente;
                        var datosCliente =db.tblCatalogo.Where(c=>c.Catalogo.Equals("Cliente") && c.Id_Catalogo== idcliente).FirstOrDefault();
                        
                    vwVenta venta = new vwVenta
                        {
                            idVenta = this.idVenta,
                            idCliente = this.idCliente > 0 ? this.idCliente : objVU.idCliente,
                            idUsuario = datosTo.idUsuario,
                            idTipoPago = this.idTipoPago > 0 ? this.idTipoPago : objVU.idTipoPago,
                            Importe = this.Importe > 0 ? this.Importe : objVU.Importe,
                            Pago = this.idVenta > 0 ? objVU.Pago + this.Pago : this.Pago,
                            Fecha = DateTime.Now,
                            Cliente = datosCliente.Valor != "" ? datosCliente.Valor :objVU.Cliente ,
                            Movil = datosTo.Telefono1,

                        };
                        db.uspVenta(venta.idVenta, venta.Fecha, venta.Importe, venta.idUsuario, venta.Pago, venta.idTipoPago,venta.Cliente,venta.Movil);

                        if (Existencias != null && Existencias.Count > 0)
                        {
                            foreach (var existencias in Existencias)
                            {
                                db.uspUEProducto(existencias.idProducto, existencias.cantidad);
                            }
                        }
                        db.SaveChanges();
                        R = new cResul { Estatus = "OK", mensaje = "Venta guardada correctamente" };
                        
                    }
                    catch (Exception e)
                    {
                        R = new cResul
                        {
                            Estatus = "Error de sistema",
                            mensaje = e.Message,
                            fecha = DateTime.Now,
                            modulo = "Venta",
                            metodos = "Cargar"
                        };
                    }
                }

            public void Cargar()
            {
                
                Ventas = db.vwVenta.ToList();
                Ventas= Ventas.Where(v=> v.Pago < v.Importe).ToList();


                if (idVenta != 0)
                {
                    vwVenta obj = db.vwVenta.Where(p => p.idVenta.Equals(idVenta)).FirstOrDefault();

                    if (obj != null)
                    {

                        this.idVenta = obj.idVenta;
                        this.idCliente = obj.idCliente;
                        this.idTipoPago = obj.idTipoPago;
                        this.Importe = obj.Importe;
                        this.Pago = obj.Pago;
                        this.Fecha = obj.Fecha;
                        this.Saldo = obj.Saldo;
                        this.Vendio = obj.Vendio;

                        R.mensaje = "Venta Editar cargada correctamente";
                        R.Estatus = "Correcto";
                        R.idEstatus = 1; // Asignar un valor por defecto o el estatus actual si es necesario
                    }
                }
                else
                {
                    if (idCliente != 0)
                    {
                        Ventas = Ventas.Where(r => r.idCliente.Equals(idCliente)).ToList();
                    }
                    if (FechaInicial != DateTime.MinValue)
                    {
                        if (FechaFinal == DateTime.MinValue)
                        {
                            FechaFinal = FechaInicial.AddHours(24);
                        }
                        else
                        {
                            FechaFinal = FechaFinal.AddHours(24);
                        }
                        Ventas = Ventas.Where(o => o.Fecha >= FechaInicial && o.Fecha < FechaFinal).ToList();
                    }
                    if (idTipoPago != 0)
                    {
                        Ventas = Ventas.Where(o => o.idTipoPago >= idTipoPago).ToList();
                    }
                }
               



            }

            public void CargarP()
            {
                
               
                Producto = (from item in  db.tblProducto.Where(p => p.idEstatus == 1).ToList()
                            select new tblProducto
                            {
                                Descripcion = item.Descripcion,
                                idProducto = item.idProducto,
                                PiezasXcaja = item.PiezasXcaja,
                                idUnidad = item.idUnidad,
                                PiezasXPaquete = item.PiezasXPaquete,
                                CantidadMinima = item.CantidadMinima,
                                Existencia = item.Existencia,
                                CostoXCaja = item.CostoXCaja,
                                PrecioPublico = item.PrecioPublico,
                                Productos = item.Productos,
                                Codigo = item.Codigo,
                                idCategoria = item.idCategoria,
                                Imagen = item.Imagen,
                                idEstatus = item.idEstatus
                            }).ToList();
               
            }

            #endregion

        }

        #endregion


        #region Clases correo
        public class cMail
        {
            public string Server { get; set; }
            public string User { get; set; }
            public string Password { get; set; }
            public int Port { get; set; }
            public string Para { get; set; }
            public string De { get; set; }
            public string Asunto { get; set; }
            public string Copias { get; set; }
            public string Cuerpo { get; set; }
            public string Archivos { get; set; }
            public bool SSL { get; set; } = true;
            public cResul R { get; set; } = new cResul();
            public cMail()
            {

                Val oVal = new Val();
                cCatalogo oCatalogo = new cCatalogo();
                string[] config = oCatalogo.Configuracion("Web Mail").Split(',');
                Server = config[0];
                Port = oVal.Entero(config[1]);
                User = config[2];
                Password = config[3];
                if (config[4].Equals("true"))
                    SSL = true;
                Copias = config[6];
                oCatalogo = null;
            }

            #region Metods
            public cResul EnviarCorreo()
            {

                cResul R = new cResul();
                System.Net.Mail.MailMessage correo = new System.Net.Mail.MailMessage();

                if (Para.IndexOf('@') == -1 || Para.IndexOf('.') == -1)
                {
                    R.mensaje = "Formato de correo no válido";
                    return R;
                }
                try
                {
                    correo.From = new System.Net.Mail.MailAddress(User);
                    correo.To.Clear();
                    correo.Bcc.Clear();
                    correo.To.Add(Para);
                    correo.Bcc.Add(User);
                    if (!string.IsNullOrEmpty(Copias))
                    {
                        String[] arrCC = Copias.Split(';');

                        for (int i = 0; i < arrCC.Length; i++)
                        {
                            if (arrCC[i].ToString().Trim() != "")
                            {
                                correo.CC.Add(arrCC[i].ToString());
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(Archivos))
                    {
                        string[] arr = Archivos.Split(';');
                        foreach (string str in arr)
                        {
                            System.Net.Mail.Attachment oAttch = new System.Net.Mail.Attachment(str, MediaTypeNames.Application.Octet);
                            correo.Attachments.Add(oAttch);

                        }
                    }
                    correo.Subject = Asunto;
                    correo.Body = Cuerpo;
                    correo.IsBodyHtml = true;
                    correo.Priority = System.Net.Mail.MailPriority.High;
                    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                    smtp.Host = Server;
                    smtp.Port = Port;
                    smtp.EnableSsl = SSL;
                    correo.DeliveryNotificationOptions = System.Net.Mail.DeliveryNotificationOptions.OnSuccess;
                    smtp.Credentials = new System.Net.NetworkCredential(User, Password);
                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    smtp.Send(correo);

                    R.mensaje = "Mensaje enviado a " + Para;
                }
                catch (Exception ex)
                {
                    R.mensaje = "ERROR: " + ex.Message;

                }
                return R;

            }

            #endregion

        }
        private class cEncriptar
        {
            public string KEY { get; set; }
            public string Dominio { get; set; }
            public string Usuario { get; set; }
            public string Mac { get; set; }
            public string Server { get; set; }
            public string UserDB { get; set; }
            public string PswDB { get; set; }
            public string DB { get; set; }
            public string CN { get; set; }
            public static byte[] EncriptarR(string strEncriptar, byte[] bytPK)
            {
                Rijndael miRijndael = Rijndael.Create();
                byte[] encrypted = null;
                byte[] returnValue = null;

                try
                {
                    miRijndael.Key = bytPK;
                    miRijndael.GenerateIV();

                    byte[] toEncrypt = System.Text.Encoding.UTF8.GetBytes(strEncriptar);
                    encrypted = (miRijndael.CreateEncryptor()).TransformFinalBlock(toEncrypt, 0, toEncrypt.Length);

                    returnValue = new byte[miRijndael.IV.Length + encrypted.Length];
                    miRijndael.IV.CopyTo(returnValue, 0);
                    encrypted.CopyTo(returnValue, miRijndael.IV.Length);
                }
                catch { }
                finally { miRijndael.Clear(); }

                return returnValue;
            }
            public cEncriptar()
            {
                IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
                string[] arrUser = GetUser().Split('\\');

                Dominio = computerProperties.DomainName;
                Usuario = arrUser[1];
                Mac = GetMac();
                KEY = "www.ocadis";

            }
            public static string Desencriptar(byte[] bytDesEncriptar, byte[] bytPK)
            {
                Rijndael miRijndael = Rijndael.Create();
                byte[] tempArray = new byte[miRijndael.IV.Length];
                byte[] encrypted = new byte[bytDesEncriptar.Length - miRijndael.IV.Length];
                string returnValue = string.Empty;

                try
                {
                    miRijndael.Key = bytPK;

                    Array.Copy(bytDesEncriptar, tempArray, tempArray.Length);
                    Array.Copy(bytDesEncriptar, tempArray.Length, encrypted, 0, encrypted.Length);
                    miRijndael.IV = tempArray;

                    returnValue = System.Text.Encoding.UTF8.GetString((miRijndael.CreateDecryptor()).TransformFinalBlock(encrypted, 0, encrypted.Length));
                }
                catch { }
                finally { miRijndael.Clear(); }

                return returnValue;
            }
            private string GetMac()
            {

                IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

                string Micadena = "";
                foreach (NetworkInterface adapter in nics)
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties(); //  .GetIPInterfaceProperties();


                    PhysicalAddress address = adapter.GetPhysicalAddress();
                    byte[] bytes = address.GetAddressBytes();

                    for (int i = 0; i < bytes.Length; i++)
                    {
                        // Display the physical address in hexadecimal.
                        Micadena = Micadena + bytes[i].ToString("X2");

                        // Insert a hyphen after each byte, unless we are at the end of the  
                        // address. 
                        if (i != bytes.Length - 1)
                        {
                            Micadena = Micadena + "-";
                        }
                    }
                    return Micadena;


                }
                return Micadena;
            }
            private static string GetUser()//Usuario + dominio
            {
                string UsName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;//Dominio + Login
                                                                                            //string UsName = Environment.UserName;//Sólo login
                return UsName;
            }
            public string Encriptar(string strEncriptar, string strPK)
            {
                byte[] bytes = EncriptarR(strEncriptar, (new PasswordDeriveBytes(strPK, null)).GetBytes(32));

                StringBuilder TextoEncriptado = new StringBuilder();
                foreach (byte element in bytes)
                {
                    TextoEncriptado.Append(element.ToString().PadLeft(3, '0'));
                    //Console.WriteLine("{0} = {1}", element, (char)element);
                }

                return TextoEncriptado.ToString().Substring(0, (TextoEncriptado.ToString().Length));

            }
            public string Encriptar(string strEncriptar)
            {
                if (strEncriptar == null || strEncriptar.Equals(""))
                    return "";
                byte[] bytes = EncriptarR(strEncriptar, (new PasswordDeriveBytes(KEY, null)).GetBytes(32));

                StringBuilder TextoEncriptado = new StringBuilder();
                foreach (byte element in bytes)
                {
                    TextoEncriptado.Append(element.ToString().PadLeft(3, '0'));
                    //Console.WriteLine("{0} = {1}", element, (char)element);
                }

                return TextoEncriptado.ToString().Substring(0, (TextoEncriptado.ToString().Length));

            }
            public string Desencriptar(string strDesEncriptar, string strPK)
            {

                byte[] bytDesEncriptar = null;

                //string[] split = strDesEncriptar.Split(new Char[] { '-' });

                byte Valor;
                int I = 0, I3 = 0, IRow = 0;
                //foreach (string s in split)
                //{
                //    I = I + 1;
                //}
                I = strDesEncriptar.Length / 3;
                bytDesEncriptar = new byte[I];


                for (I = 0; I < strDesEncriptar.Length; I++)
                {
                    I3 = I3 + 1;
                    if (I3 == 3)
                    {
                        Valor = Convert.ToByte(strDesEncriptar.Substring(I - 2, 3));
                        bytDesEncriptar[IRow] = Valor;
                        I3 = 0;
                        IRow = IRow + 1;
                    }
                }


                return Desencriptar(bytDesEncriptar, (new PasswordDeriveBytes(strPK, null)).GetBytes(32));

                #region Metodo Split
                //byte[] bytDesEncriptar = null;

                //string[] split = strDesEncriptar.Split(new Char[] { '-' });

                //byte Valor;
                //int I = 0;
                //foreach (string s in split)
                //{
                //    I = I + 1;
                //}
                //bytDesEncriptar = new byte[I];
                //I = 0;

                //foreach (string s in split)
                //{
                //    Valor = Convert.ToByte(s);
                //    bytDesEncriptar[I] = Valor;
                //    I = I + 1;
                //}

                //return Desencriptar(bytDesEncriptar, (new PasswordDeriveBytes(strPK, null)).GetBytes(32));
                #endregion
            }
            public string Desencriptar(string strDesEncriptar)
            {
                try
                {
                    if (strDesEncriptar == null)
                        return "";
                    if (strDesEncriptar == "")
                        return "";
                    byte[] bytDesEncriptar = null;

                    //string[] split = strDesEncriptar.Split(new Char[] { '-' });

                    byte Valor;
                    int I = 0, I3 = 0, IRow = 0;
                    //foreach (string s in split)
                    //{
                    //    I = I + 1;
                    //}
                    I = strDesEncriptar.Length / 3;
                    bytDesEncriptar = new byte[I];


                    for (I = 0; I < strDesEncriptar.Length; I++)
                    {
                        I3 = I3 + 1;
                        if (I3 == 3)
                        {
                            Valor = Convert.ToByte(strDesEncriptar.Substring(I - 2, 3));
                            bytDesEncriptar[IRow] = Valor;
                            I3 = 0;
                            IRow = IRow + 1;
                        }
                    }


                    return Desencriptar(bytDesEncriptar, (new PasswordDeriveBytes(KEY, null)).GetBytes(32));
                }
                catch (Exception e)
                {
                    return "";
                }
            }
            public bool IsNumeric(String sValue)
            {
                Double d;
                try
                {
                    d = Convert.ToDouble(sValue);
                    return (true);
                }
                catch
                {
                    return (false);
                }
            }
            public string GenerarPsd()
            {
                string sClave = "";
                ToExcelIndexCol(DateTime.Now.Second * 896, ref sClave);
                ToExcelIndexCol(DateTime.Now.Millisecond, ref sClave);
                sClave = sClave + DateTime.Now.Millisecond.ToString();
                ToExcelIndexCol(DateTime.Now.Second * 887, ref sClave);
                ToExcelIndexCol(DateTime.Now.Day, ref sClave);
                ToExcelIndexCol(DateTime.Now.Millisecond * 788, ref sClave);
                ToExcelIndexCol(DateTime.Now.Minute, ref sClave);
                return sClave;
            }
            public void ToExcelIndexCol(int n, ref string res)
            {
                if (n == 0)
                {
                    //Console.Write("Z");
                    return;
                }
                else
                {
                    int r = n % 59;
                    n = n / 59;
                    if (r == 0)
                        ToExcelIndexCol(n - 1, ref res);
                    else
                        ToExcelIndexCol(n, ref res);
                    if (r == 0)
                    {
                        res += "Z";
                        if (n == 1)
                            return;
                    }
                    switch (r)
                    {
                        case 1:
                            res += "A";
                            break;
                        case 2:
                            res += "B";
                            break;
                        case 3:
                            res += "C";
                            break;
                        case 4:
                            res += "D";
                            break;
                        case 5:
                            res += "E";
                            break;
                        case 6:
                            res += "F";
                            break;
                        case 7:
                            res += "G";
                            break;
                        case 8:
                            res += "H";
                            break;
                        case 9:
                            res += "I";
                            break;
                        case 10:
                            res += "J";
                            break;
                        case 11:
                            res += "K";
                            break;
                        case 12:
                            res += "L";
                            break;
                        case 13:
                            res += "M";
                            break;
                        case 14:
                            res += "N";
                            break;
                        case 15:
                            res += "O";
                            break;
                        case 16:
                            res += "P";
                            break;
                        case 17:
                            res += "Q";
                            break;
                        case 18:
                            res += "R";
                            break;
                        case 19:
                            res += "S";
                            break;
                        case 20:
                            res += "T";
                            break;
                        case 21:
                            res += "U";
                            break;
                        case 22:
                            res += "V";
                            break;
                        case 23:
                            res += "W";
                            break;
                        case 24:
                            res += "X";
                            break;
                        case 25:
                            res += "Y";
                            break;
                        case 26:
                            res += "Z";
                            break;
                        case 27:
                            res += "a";
                            break;
                        case 28:
                            res += "b";
                            break;
                        case 29:
                            res += "c";
                            break;
                        case 30:
                            res += "d";
                            break;
                        case 31:
                            res += "e";
                            break;
                        case 32:
                            res += "f";
                            break;
                        case 33:
                            res += "g";
                            break;
                        case 34:
                            res += "h";
                            break;
                        case 35:
                            res += "i";
                            break;
                        case 36:
                            res += "j";
                            break;
                        case 37:
                            res += "k";
                            break;
                        case 38:
                            res += "l";
                            break;
                        case 39:
                            res += "m";
                            break;
                        case 40:
                            res += "n";
                            break;
                        case 41:
                            res += "o";
                            break;
                        case 42:
                            res += "p";
                            break;
                        case 43:
                            res += "q";
                            break;
                        case 44:
                            res += "r";
                            break;
                        case 45:
                            res += "s";
                            break;
                        case 46:
                            res += "t";
                            break;
                        case 47:
                            res += "u";
                            break;
                        case 48:
                            res += "v";
                            break;
                        case 49:
                            res += "w";
                            break;
                        case 50:
                            res += "x";
                            break;
                        case 51:
                            res += "y";
                            break;
                        case 52:
                            res += "z";
                            break;
                        case 53:
                            res += "1";
                            break;
                        case 54:
                            res += "2";
                            break;
                        case 55:
                            res += "3";
                            break;
                        case 56:
                            res += "4";
                            break;
                        case 57:
                            res += "5";
                            break;
                        case 58:
                            res += "6";
                            break;
                        case 59:
                            res += "7";
                            break;
                        case 60:
                            res += "8";
                            break;
                    }
                }
            }

        }

        public class Val
        {
            public int Entero(string valor)
            {
                int resultado;
                if (int.TryParse(valor, out resultado))
                {
                    return resultado;
                }
                return 0; // Retorna 0 si la conversión falla
            }
        }

        #endregion
    }
}