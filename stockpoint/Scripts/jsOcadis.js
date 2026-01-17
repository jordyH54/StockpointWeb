
var url = "/../WebServices/";
var token = $('.sysToken').val() ||
    localStorage.getItem('sysToken') ||
    sessionStorage.getItem('currentToken');
var carrito = {};
var permisos = [];
var permisos2;
var idTipoPago;
$(document).ready(function () {
    actualizarVisibilidad();
    $('.sysToken').val(sessionStorage.getItem('currentToken'));
    $.ajaxSetup({
        beforeSend: function (xhr) {
            const token = obtenerToken();
            if (token) {
                xhr.setRequestHeader('Authorization', 'Bearer ' + token);
            }
        }
    });
    window.addEventListener('storage', function (e) {
        if (e.key === 'currentToken' || e.key === 'sysToken') {
            $('.sysToken').val(e.newValue || '');
            actualizarVisibilidad();
        }
    });

    $('select').each(function () {
        var cmb = '.' + $(this).attr('cmb')
        var textCombo = $(this).attr('catalogo')
        if ($(this).attr('catalogo') != undefined) {
            /*console.log($(this).attr('catalogo'));*/
            obj = {
                Catalogo: $(this).attr('catalogo'),   
                R: {
                    modulo: $('.sysPantalla').val(),
                    metodos: 'Cargar',
                    Token: token ,
                }
            };
            $.ajax({
                type: "POST",
                url: url + 'Elementos',
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(obj),
                async: true,
                processData: false,
                cache: false,
                success: function (Result) {
                    var opc = "<option value='0'>Selecciona</option>";
                    $.each(Result.Elementos, function (index, item) {
                        opc += "<option value='" + item.Valor + "'>" + item.Texto + "</option>"
                    });
                    $(cmb).html(opc)
                    
                },
                error: function (xhr) {
                    return false;
                }
            });

        }
    });

    $('.CatalogoOrigen').change(function () {
        var cmb = '.' + $(this).attr('cmbhijo');
        if ($(this).val() != undefined) {
            console.log($(this).attr('catalogo'));
            obj = {
                Catalogo: $(this).val(),
                R: {
                    modulo: $('.sysPantalla').val(),
                    metodos: 'Cargar',
                    Token: token,
                }
            };
            $.ajax({
                type: "POST",
                url: url + 'Elementos',
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(obj),
                async: true,
                processData: false,
                cache: false,
                success: function (Result) {
                    var opc = "<option value=''>Selecciona</option>";
                    $.each(Result.Elementos, function (index, item) {
                        opc += "<option value='" + item.Valor + "'>" + item.Texto + "</option>"
                    });
                    $(cmb).html(opc)

                },
                error: function (xhr) {
                    return false;
                }
            });

        }
    });
    
    $('.cmbCatalogos').change(function () {
        jsCargarCatalogo($(this).val());
    });
    //#region Login
    // Mostrar formulario de recuperación
    $('.cmdRecuperarDatos').click(function (e) {
        e.preventDefault();
        $('.Login').addClass('hidden');
        $('.divRecuperarclave').removeClass('hidden');
    });

    $('.cmdAccesar').click(function () {
        // 1. Obtener valores del formulario
        var obj = {
            CorreoElectronico: $('.CorreoElectronico').val(),
            ClaveAcceso: $('.Password').val(),
            R: {
                modulo: $('.sysPantalla').val(),
                metodos: 'Acceso',
                Token:token// Nota: Eliminamos Token: token aquí porque no es necesario enviarlo
            }
        };

        // 2. Realizar petición de login
        $.ajax({
            type: "POST",
            url: url + 'Usuarios',
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(obj),
            success: function (Resultado) {
                if (Resultado.R.Estatus == "Correcto") {
                    // 3. Procesar respuesta exitosa
                    var Rtoken = (Resultado.R.Token).trim();

                    // 4. Almacenar datos de sesión
                    sessionStorage.setItem('currentToken', Rtoken);
                    localStorage.setItem('sysToken', Rtoken); // Para persistencia
                    $('.sysToken').val(Rtoken);

                    // 5. Guardar menú recibido (ajusta según la estructura real de tu respuesta)
                    if (Resultado.Menu) {
                        sessionStorage.setItem('menuData', JSON.stringify(Resultado.Menu));
                        
                        
                    } else if (Resultado.R.Menu) {
                        sessionStorage.setItem('menuData', JSON.stringify(Resultado.R.Menu));
                    }

                    // 6. Limpiar formulario
                    $('.CorreoElectronico').val('');
                    $('.Password').val('');

                    // 7. Redireccionar
                    window.location.href = '/Home/Contact';

                } else {
                    // 8. Manejar error de autenticación
                    sessionStorage.removeItem('currentToken');
                    localStorage.removeItem('sysToken');
                    sessionStorage.removeItem('menuData');
                    $('.sysToken').val('');
                    alert(Resultado.R.mensaje);
                }
            },
            error: function (xhr) {
                // 9. Manejar error de conexión
                sessionStorage.removeItem('currentToken');
                localStorage.removeItem('sysToken');
                sessionStorage.removeItem('menuData');
                $('.sysToken').val('');

                alert("Error de conexión con el servidor");
            }
        });
    });
    // Validar y enviar
    $('.enviarDatos').click(function () {
        var email = $('.RecuperarCorreoElectronico').val().trim();
        var regx = /^[^\s@]+@[^\s@]+\.[^\s@]+$/i;

        if (!regx.test(email)) {
            
            alert('Ingresa un correo v\u00e1lido');
           
        }
    });

    // Cancelar
    $('.cancelarEnvioDatos').click(function () {
        $('.Login').removeClass('hidden');
        $('.divRecuperarclave').addClass('hidden');
        $('.RecuperarCorreoElectronico').val('');
        $('#error-message').hide();
    });


    $(".enviarDatos").click(function () {
        var obj = {
            CorreoElectronico:$('.RecuperarCorreoElectronico').val(),
            R: {
                modulo: 'Login',
                metodos: 'SolicitarClave',
                Token: token,
            }
        };
        console.log(obj);
        $.ajax({
            type: "POST",
            url: url + "Usuarios",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(obj),
            async: true,
            processData: false,
            cache: false,
            success: function (Resultado) {
                console.log(Resultado);
                alert(Resultado.R.mensaje);
                $(".Login").removeClass("hidden");

                $(".divRecuperarclave").addClass("hidden");
                $('.RecuperarCorreoElectronico').val('');

            },
            error: function (xhr) {
                return false;
            }

        });

    });
    //#endregion
    $('.hidden-switch').change(function () {
        const input = $('#switchInput');
        if (this.checked) {
            input.attr('placeholder', "Estado: ON");
            input.css('border-color', '#4CAF50');
        } else {
            input.attr('placeholder', "Estado: OFF");
            input.css('border-color', '');
        }
    });


    $('.cmbRol').change(function () {
        jsUsuarios('Cargar', $(this));  
    });
    $('.crearB').click(function () {
        jsCatalogo('Crear', $(this));
    });
    $('.cancelar').click(function () {
        jsUsuarios('Cancelar', $(this));
    });

    $('.cmbFiltroBitacora').change(function () {
        jsCargarResultados();
    });
    $('.guardarB').click(function () {
        jsUsuarios('Guardar', $(this));
    }); 
    //$('.cmbPantallas').click(function () {
    //    jsPantallas('Cargar', $(this));

    //    jsCmbPantalla();

    //});


    $('.crearBP').click(function () {
        if ($('.cmdEditarPantalla.active').attr('idpantalla') == 0) {
            alert('Primero se crea la pantalla despues se asignas permisos');
        }
        jsPantallas('Guardar');
    }); 
    $('.cancelar').click(function () {
        
        $('.nPantalla').val(0);
        $('.cmbOrigen').val(0);
        $('.cmbEstatus').val(0);
        $('.Descripcion').val('');
        $('.OrdenP').val('');
        $('.uPantalla').val('');
        $('.cPantalla').val('');
        $('.cToolTip').val('');
        $('.cMG').val('');
        $('.cME').val('');
        $('.cPE').val('');
        $('.uMPantalla').val('');
        $('.nPantalla').val('');

        

        

        $('.cmdEditarPantalla.active').attr('idpantalla', 0);
        //var cosa = $('.cmdEditarPantalla.active').attr('idpantalla');
        //console.log(cosa);
        jsCmbPantalla();
        
    });
    $('.btnAFP').click(function () {
        $('.framePantalla').removeClass('hidden');
        $('.framePermisos').addClass('hidden');
        
    }); 
    $('.btnAFR').click(function () {
        $('.framePantalla').addClass('hidden');
        $('.framePermisos').removeClass('hidden');
        elemento = $('.cmdEditarPantalla.active').attr('idpantalla');
        /*console.log(elemento);*/
        jsCargarRol(elemento);
    }); 
    $('.guardarPR').click(function () {
        jsPantallas('GuardarPermisos');
    }); 

    switch ($('.sysPantalla').val()) {
        case 'Login':
            $('.sysToken').val('Publico');
            console.log($('.sysToken').val())
            window.addEventListener('pageshow', function (event) {
                if (event.persisted) { // Si la página se carga desde el caché (al retroceder)
                    localStorage.removeItem('sysToken');
                    sessionStorage.removeItem('currentToken');
                    $('.sysToken').val(''); // Opcional: Borrar el input
                    console.log("Token eliminado al retroceder.");
                }
            });
            break;
        case 'Pantalla':
            jsCmbPantalla();
            jsCargartabla();
            break;
        case 'Producto':
            jsProductos('Cargar');
            $(document).on('keydown', function (e) {
                // Verificar Ctrl + L (keyCode 76) y que no esté ya en un campo de texto
                if (e.ctrlKey && e.keyCode === 66 && !$(e.target).is('input, textarea, [contenteditable]')) {
                    e.preventDefault(); // Bloquea el comportamiento por defecto del navegador
                    e.stopImmediatePropagation(); // Evita que otros eventos capturen esto

                    const $buscador = $('.txtBuscar');
                    $buscador.focus(); // Posiciona el cursor

                    // Selecciona el texto existente (opcional)
                    setTimeout(() => {
                        $buscador.select();

                        // Feedback visual (opcional)
                        $buscador.addClass('input-activo');
                        setTimeout(() => $buscador.removeClass('input-activo'), 300);
                    }, 10);
                }
            });

          


            $('.txtBuscar').on('keypress', function (e) {
                if (e.which === 13) { // Enter
                    e.preventDefault();
                    const codigoBuscado = $(this).val().trim();

                    if (!codigoBuscado) return;

                    try {
                        let productoEncontrado = null;
                        let idProducto = null;

                        // Buscar en las filas
                        $('.tblDatos tr.tblBitacora.hidden').each(function () {
                            const $fila = $(this);
                            const codigoProducto = $fila.find('td:eq(2)').text().trim();

                            if (codigoProducto === codigoBuscado) {
                                productoEncontrado = this;
                                idProducto = $fila.find('.cmdEditarCatalogo').attr('idcatalogo');
                                return false;
                            }
                        });

                        if (productoEncontrado) {
                            // 1. Marcar el producto en localStorage
                            localStorage.setItem('productoActivo', idProducto);

                            // 2. Disparar el click para editar
                            $(productoEncontrado).find('.cmdEditarCatalogo').trigger('click');

                            // 3. Recargar la tabla
                            jsProductos('Cargar');

                            $(this).val('').focus();
                        } else {
                            alert('Producto no encontrado con código: ' + codigoBuscado);
                            $(this).val('').focus();
                        }
                    } catch (error) {
                        console.error('Error al escanear:', error);
                        alert('Error al procesar el código');
                        $(this).val('').focus();
                    }
                }
            });
            
            $('.guardarBP').click(function () {
                jsProductos('Guardar', $('.cmdEditarCatalogo.active').attr('idcatalogo'));

            });
            $('.btnNuevo').click(function () {
                $('.cmdEditarCatalogo').removeClass('active');
                $('.cmbUnidad').val(0);
                $('.codigo').val('');
                $('.canMin').val('');
                $('.existencia').val('');
                $('.producto').val('');
                $('.descripcion').val('');
                $('.costoxCaja').val('');
                $('.piezaxCaja').val('');
                $('.piezaxPaquete').val('');
                $('.precioPublico').val('');

                $('.costoxPieza').val('');
                $('.costoxPaquete').val('');
                $('.uxProducto').val('');
                $('.uxCaja').val('');


            });
            break;
        case 'Venta':
            // Variable para controlar el carrito
            carrito = {};
            permisos = [];
            
            // Configuración inicial
            $(document).ready(function () {
                jsProductos('CargarV');
               /* $('.paga').text('$0.00');*/
                $('.cambio').text('$0.00');
                localStorage.removeItem('valorPaga');
                // Cargar valor de pago guardado si existe
                const valorGuardadoPaga = localStorage.getItem('valorPaga');
                if (valorGuardadoPaga) {
                    $('.paga').text(valorGuardadoPaga);
                    actualizarCampoCambio(valorGuardadoPaga);
                }
                $(document).on('keydown', function (e) {
                    // Verificar Ctrl + L (keyCode 76) y que no esté ya en un campo de texto
                    if (e.ctrlKey && e.keyCode === 66 && !$(e.target).is('input, textarea, [contenteditable]')) {
                        e.preventDefault(); // Bloquea el comportamiento por defecto del navegador
                        e.stopImmediatePropagation(); // Evita que otros eventos capturen esto

                        const $buscador = $('.txtBuscar');
                        $buscador.focus(); // Posiciona el cursor

                        // Selecciona el texto existente (opcional)
                        setTimeout(() => {
                            $buscador.select();

                            // Feedback visual (opcional)
                            $buscador.addClass('input-activo');
                            setTimeout(() => $buscador.removeClass('input-activo'), 300);
                        }, 10);
                    }
                });
            });
            $('.txtBuscar').on('input', function () {
                const codigoBuscado = $(this).val().trim();

                if (codigoBuscado === '') {
                    jsProductos('CargarV');
                }
            });
            // Manejo del escáner y búsqueda
            $('.txtBuscar').on('keypress', function (e) {
                if (e.which === 13) { // Enter
                    e.preventDefault();
                    const codigoBuscado = $(this).val().trim();

                    if (!codigoBuscado) return;

                    try {
                        // Buscar en la tabla (adaptado a tu estructura)
                        let productoEncontrado = null;

                        $('.tblDVatos tr[productos]').each(function () {
                            const codigoProducto = $(this).find('td:eq(1)').text().trim();
                            if (codigoProducto === codigoBuscado) {
                                productoEncontrado = this;
                                return false; // Rompe el each
                            }
                        });

                        if (productoEncontrado) {
                            agregarProducto(productoEncontrado);
                            
                            $(this).val('').focus();
                        } else {
                            alert('Producto no encontrado con código: ' + codigoBuscado);
                            $(this).val('').focus();
                        }
                    } catch (error) {
                        console.error('Error al escanear:', error);
                        alert('Error al procesar el código');
                        $(this).val('').focus();
                    }
                }
            });


            // Manejo de productos en el carrito
            $('.tblDVatos').on('click', '.producto', function () {
                agregarProducto(this);
            });

            // Manejo de cantidades en la tabla de venta
            $('.tblVenta')
                .on('click', '.btn-sumar', function () {
                    const id = $(this).closest('tr').data('id');
                    carrito[id].cantidad += 1;
                    jsProductos('CargarProducTabla');
                })
                .on('click', '.btn-restar', function () {
                    const id = $(this).closest('tr').data('id');
                    if (carrito[id].cantidad > 1) {
                        carrito[id].cantidad -= 1;
                    } else {
                        delete carrito[id];
                    }
                    jsProductos('CargarProducTabla');
                })
                .on('click', '.btn-eliminar', function () {
                    const id = $(this).closest('tr').data('id');
                    delete carrito[id];
                    jsProductos('CargarProducTabla');
                });

            // Manejo del pago
            $('.paga').on('click', function () {
                prepararEdicion();
            });

            $(document).on('keydown', function (e) {
                if (e.ctrlKey && e.keyCode === 80) { // Ctrl+P
                    e.preventDefault();
                    prepararEdicion();
                }
            });

            // Botones de guardar y liquidar venta
            $('.guardarUV').click(function () {
                jsVenta('GuardarUV', $('.cmdEditarDeuda.active').attr('idVenta'));
            });

            $('.liquidarV').click(function () {
                jsVenta('Guardar');
            });

            // Filtros
            $('.cmbFiltroVentas').change(function () {
                jsVenta('Cargar');
            });

            $('.cmbCliente').change(function () {
                jsProductos('CargarVCI', $(this).val());
            });

            // Navegación entre frames
            $('.btnFE').click(function () {
                $('.ventaFrame').addClass('hidden');
                $('.estadisFrame').removeClass('hidden');
            });

            $('.btnFV').click(function () {
                $('.ventaFrame').removeClass('hidden');
                $('.estadisFrame').addClass('hidden');
            });
            break;

        case 'Compra':
            carrito = {};
            permisos = [];
            
            jsCompra('Cargar');
            
            $('.btnFC').click(function () {
                $('.estadisFrame').addClass('hidden');
                $('.ventaFrame').removeClass('hidden');
            });
            $('.btnFCE').click(function () {
                $('.estadisFrame').removeClass('hidden');
                $('.ventaFrame').addClass('hidden');
                
            });
            
            $('.cmbFiltroCompras').change(function () {
                jsCompra('BCompra');

            });
            $(document).ready(function () {

                /* $('.paga').text('$0.00');*/
                $('.cambio').text('$0.00');
                localStorage.removeItem('valorPaga');
                // Cargar valor de pago guardado si existe
                const valorGuardadoPaga = localStorage.getItem('valorPaga');
                if (valorGuardadoPaga) {
                    $('.paga').text(valorGuardadoPaga);
                    actualizarCampoCambio(valorGuardadoPaga);
                }
                $(document).on('keydown', function (e) {
                    // Verificar Ctrl + L (keyCode 76) y que no esté ya en un campo de texto
                    if (e.ctrlKey && e.keyCode === 66 && !$(e.target).is('input, textarea, [contenteditable]')) {
                        e.preventDefault(); // Bloquea el comportamiento por defecto del navegador
                        e.stopImmediatePropagation(); // Evita que otros eventos capturen esto

                        const $buscador = $('.txtBuscar');
                        $buscador.focus(); // Posiciona el cursor

                        // Selecciona el texto existente (opcional)
                        setTimeout(() => {
                            $buscador.select();

                            // Feedback visual (opcional)
                            $buscador.addClass('input-activo');
                            setTimeout(() => $buscador.removeClass('input-activo'), 300);
                        }, 10);
                    }
                });
            });
            $('.txtBuscar').on('input', function () {
                const codigoBuscado = $(this).val().trim();

                if (codigoBuscado === '') {
                    jsCompra('Cargar');
                }
            });
            // Manejo del escáner y búsqueda
            $('.txtBuscar').on('keypress', function (e) {
                if (e.which === 13) { // Enter
                    e.preventDefault();
                    const codigoBuscado = $(this).val().trim();

                    if (!codigoBuscado) return;

                    try {
                        // Buscar en la tabla (adaptado a tu estructura)
                        let productoEncontrado = null;

                        $('.tblDCatos tr[productos]').each(function () {
                            const codigoProducto = $(this).find('td:eq(1)').text().trim();
                            if (codigoProducto === codigoBuscado) {
                                productoEncontrado = this;
                                return false; // Rompe el each
                            }
                        });

                        if (productoEncontrado) {
                            agregarProducto(productoEncontrado);

                            $(this).val('').focus();
                        } else {
                            alert('Producto no encontrado con código: ' + codigoBuscado);
                            $(this).val('').focus();
                        }
                        jsCompra('Cargar');

                    } catch (error) {
                        console.error('Error al escanear:', error);
                        alert('Error al procesar el código');
                        $(this).val('').focus();
                    }
                }
            });
            // Manejo de productos en el carrito
            $('.tblDCatos').on('click', '.productoC', function () {
                agregarProducto(this);
            });

            // Manejo de cantidades en la tabla de venta
            $('.tblVenta')
                .on('click', '.btn-sumar', function () {
                    const id = $(this).closest('tr').data('id');
                    carrito[id].cantidad += 1;
                    jsProductos('CargarProducTabla');
                })
                .on('click', '.btn-restar', function () {
                    const id = $(this).closest('tr').data('id');
                    if (carrito[id].cantidad > 1) {
                        carrito[id].cantidad -= 1;
                    } else {
                        delete carrito[id];
                    }
                    jsProductos('CargarProducTabla');
                })
                .on('click', '.btn-eliminar', function () {
                    const id = $(this).closest('tr').data('id');
                    delete carrito[id];
                    jsProductos('CargarProducTabla');
                });

            // Manejo del pago
            $('.paga').on('click', function () {
                prepararEdicion();
            });
            $('.liquidarV').click(function () {
                jsCompra('Guardar');
            });
            $(document).on('keydown', function (e) {
                if (e.ctrlKey && e.keyCode === 80) { // Ctrl+P
                    e.preventDefault();
                    prepararEdicion();
                }
            });
            break;
    }

    

    $('.cmbRol').change(function () {
        $('.idRol').val($(this).val());
    });
    $('.cmbidEstatus').change(function () {
        $('.idEstatus').val($(this).val());
    });
    
    $('.salirL').click(function (e) {
        e.preventDefault();
        sessionStorage.removeItem('currentToken');
        localStorage.removeItem('sysToken');
        sessionStorage.removeItem('menuData');
        $('.sysToken').val('');
        actualizarVisibilidad();
        window.location.href = '/Home/Login';
    });

});
function jsCargarCatalogo(catalogo = '') {
    var tokenn = obtenerToken();
   /* console.log('Token actual: ' + tokenn);*/
    var obj = {
        Catalogo: catalogo,
        R: {
            modulo: $('.sysPantalla').val(),
            metodos: 'Cargar',
            Token: tokenn,
        }
    };
    
    
    $.ajax({
        type: "POST",
        url: url + 'Catalogos',
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(obj),
        async: true,
        processData: false,
        cache: false,
        success: function (Result) {
            
            console.log(Result);
            var Cadena = "";

            $.each(Result.Catalogos, function (index, item) {
                
                Cadena = Cadena + '           <tr class="tblBitacora ReglonActivo ">\n';
                Cadena = Cadena + '                <td >\n';
                Cadena = Cadena + '                    <i obtener= "' + item.Valor + '" class="fas fa-edit cmdEditarCatalogo accion " idcatalogo=" ' + item.Valor + '" style="margin-left:8px;"' + item.Valor + '" title="Editar   ' + item.Texto + ' " aria-hidden="true"></i><span class="sr-only ">Editar  Web Master </span>\n';
                Cadena = Cadena + '                </td>\n';
                Cadena = Cadena + '                <td style="font-size:20px;"> ' + item.Texto + '</td>\n';
                Cadena = Cadena + '            </tr>\n';
            });

            $('.tblDatos').html(Cadena);

            jsPaginar();

            $('.cmdEditarCatalogo').click(function () {
                obj2 = {
                    id: $(this).attr('idcatalogo'),
                    R: {
                        modulo: $('.sysPantalla').val(),
                        metodos: 'Cargar2',
                        Token: tokenn,
                    }
                }
                $.ajax({
                    type: "POST",
                    url: url + 'DatosCat',
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(obj2),
                    async: true,
                    processData: false,
                    cache: false,
                    success: function (Resultado) {
                        console.log(Resultado);
                        $('#ipId').val(Resultado.catalogos[0].Id_Catalogo);
                        $('#ipOr').val(Resultado.catalogos[0].IdOrigen);
                       
                        $('#ipEs').val(Resultado.catalogos[0].Catalogo);
                        $('#ipCa').val(Resultado.catalogos[0].Catalogo);
                        $('#ipOrd').val(Resultado.catalogos[0].Orden);

                        // Manejar el switch
                        const isDefault = Resultado.catalogos[0].ValorDefault == 1;
                        $('#ipDe').prop('checked', isDefault);

                        const input = $('#switchInput');
                        if (isDefault) {
                            input.attr('placeholder', "Estado: ON");
                            input.css('border-color', '#4CAF50');
                        } else {
                            input.attr('placeholder', "Estado: OFF");
                            input.css('border-color', '');
                        }

                        $('#ipVa').val(Resultado.catalogos[0].Valor);
                        $('#ipTe').val(Resultado.catalogos[0].Descripcion);
                    },
                    error: function (xhr) {
                        return false;
                    }
                });
            });
        },
        error: function (xhr) {
            return false;
        }
    });
}
function jsUsuarios(caso, elemento) {  
    switch (caso) {
        case 'Cargar':
            if (elemento.val() != undefined) {  

                obj = {
                    idRol: elemento.val(), 
                    R: {
                        modulo: $('.sysPantalla').val(),
                        metodos: 'CargarU',
                        Token: token,
                    }
                };
                $.ajax({
                    type: "POST",
                    url: url + 'Catalogos',
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(obj),
                    async: true,
                    processData: false,
                    cache: false,
                    success: function (Result) {
                        var Cadena = "";

                        $.each(Result.Usuarios, function (index, item) {
                            Cadena = Cadena + '           <tr class="tblBitacora ReglonActivo ">\n';
                            Cadena = Cadena + '                <td >\n';
                            Cadena = Cadena + '                    <i obtener= "' + item.idUsuario + '" class="fas fa-edit cmdEditarCatalogo accion " idcatalogo=" ' + item.idUsuario + '" style="margin-left:8px;"' + item.idUsuario + '" title="Editar   ' + item.Nombre + ' " aria-hidden="true"></i><span class="sr-only ">Editar  Web Master </span>\n';
                            Cadena = Cadena + '                </td>\n';
                            Cadena = Cadena + '                <td style="font-size:20px;"> ' + item.Nombre + '</td>\n';
                            Cadena = Cadena + '            </tr>\n';

                        });
                        $('.tblDatos').html(Cadena);

                        jsPaginar();

                        $('.cmdEditarCatalogo').click(function () {
                            obj = {
                                idUsuario: $(this).attr('idcatalogo'),
                                R: {
                                    modulo: $('.sysPantalla').val(),
                                    metodos: 'CargarUData',
                                    Token: token,
                                }
                            };

                            $.ajax({
                                type: "POST",
                                url: url + 'Catalogos',
                                dataType: "json",
                                contentType: "application/json; charset=utf-8",
                                data: JSON.stringify(obj),
                                async: true,
                                processData: false,
                                cache: false,
                                success: function (Result) {
                                    console.log(Result);

                                    $('.idUsuario').val(Result.Usuarios[0].idUsuario); 
                                    $('.Nombre').val(Result.Usuarios[0].Nombre); 
                                    $('.cmbRol').val(Result.Usuarios[0].idRol); 
                                    $('.idRol').val(Result.Usuarios[0].idRol); 
                                    $('.idEstatus').val(Result.Usuarios[0].idEstatus); 
                                    $('.CorreoE').val(Result.Usuarios[0].CorreoElectronico); 
                                    $('.cmbidEstatus').val(Result.Usuarios[0].idEstatus); 
                                    $('.Contra').val(Result.Usuarios[0].ClaveAcceso); 
                                    $('.tel1').val(Result.Usuarios[0].Telefono1); 
                                    $('.tel2').val(Result.Usuarios[0].Telefono2); 
                                    
                                    

                                }
                            });
                        });
                    },
                    error: function (xhr) {
                        return false;
                    }
                });
            }
            break;
        case 'Guardar':
                obj = {
                    idUsuario: $('.idUsuario').val(),
                    Nombre: $('.Nombre').val(),
                    CorreoElectronico: $('.CorreoE').val(),
                    ClaveAcceso: $('.Contra').val(),
                    Telefono1: $('.tel1').val(),
                    Telefono2: $('.tel2').val(),
                    idRol: $('.idRol').val(),
                    idEstatus: $('.idEstatus').val(),
                    Token: null,
                    R: {
                        modulo: $('.sysPantalla').val(),
                        metodos: 'Guardar',
                        Token: token,
                    }
                };
                $.ajax({
                    type: "POST",
                    url: url + 'Usuarios',
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(obj),
                    async: true,
                    processData: false,
                    cache: false,
                    success: function (Result) {
                        /*console.log(Result);*/
                        if (Result.R.Estatus == "Correcto") {
                            alert("Usuario guardado correctamente");
                            jsUsuarios('Cargar', $('.cmbRol')); // Recargar usuarios
                            $('.idUsuario').val('');
                            $('.Nombre').val('');
                            $('.cmbRol').val('');
                            $('.idRol').val('');
                            $('.idEstatus').val('');
                            $('.CorreoE').val('');
                            $('.cmbidEstatus').val('');
                            $('.Contra').val('');
                            $('.tel1').val('');
                            $('.tel2').val('');
                        } else {
                            alert("Error al guardar el usuario: ");
                        }
                        
                    },
                    error: function (xhr) {
                        return false;
                    }
                });
            
            break;
        case 'Cancelar':
            $('.idUsuario').val('');
            $('.Nombre').val('');
            $('.cmbRol').val('');
            $('.idRol').val('');
            $('.idEstatus').val('');
            $('.CorreoE').val('');
            $('.cmbidEstatus').val('');
            $('.Contra').val('');
            $('.tel1').val('');
            $('.tel2').val('');
            break;
    }
}



function jsCargarRol(idpantalla) {
    if (idpantalla != undefined) {
        obj = {
            idPantalla: idpantalla,

            R: {
                modulo: $('.sysPantalla').val(),
                metodos: 'Cargar',
                Token: token,
            }
        };
        $.ajax({
            type: "POST",
            url: url + 'Pantallas',
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(obj),
            async: true,
            processData: false,
            cache: false,
            success: function (pantalla) {
            



                var listaHTML = '';

                listaHTML += '<div class="col-md-12 text-center" style="color:black; display: flex; flex-direction: column; align-items: center;">';
                listaHTML += '<div style ="background:rgb(67, 164, 223); width:30%; border-radius:2px"><h4 style="font-size:20px; ">Permisos</h4></div>';
                listaHTML += '  <ul class="list-group permission-list">';

                $.each(pantalla.Permisos, function (index, item) {
                    listaHTML += '<li class="list-group-item d-flex justify-content-between align-items-center ">';
                    listaHTML += '  <div class="form-check form-switch mb-0">';  // mb-0 para eliminar margen inferior
                    listaHTML += '    <input class="form-check-input permission-toggle rol " type="checkbox" role="switch"';
                    listaHTML += '           value="' + item.idRol + '"';
                    listaHTML += '           ' + (item.TienePermiso ? 'checked' : '') + '>';
                    listaHTML += '    <label class="form-check-label ms-3 mb-0 idROL" value="' + item.idRol + '">';  // mb-0 aquí también
                    listaHTML += '      <span class="permission-name align-middle">' + item.Texto + '</span>';  // align-middle
                    listaHTML += '    </label>';
                    listaHTML += '  </div>';
                    listaHTML += '</li>';
                });

                listaHTML += '  </ul>';
                listaHTML += '</div>';
                $('.lista').html(listaHTML);


            }
        });
    }
}
function jsCmbPantalla(elemento) {
    
    
    if (elemento == '0' || elemento == undefined) {
        $('.btnAFR').addClass('hidden');
    } else {
        $('.btnAFR').removeClass('hidden');
        
    }
}
function jsCargartabla() {
    obj = {
        R: {
            modulo: $('.sysPantalla').val(),
            metodos: 'obtenerPan',
            Token: token,
        }
    };

    $.ajax({
        type: "POST",
        url: url + 'Pantallas',
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(obj),
        async: true,
        processData: false,
        cache: false,
        success: function (response) {
            

            let text = '';
            const menuData = response.Menu || response.Menu2 || [];

            // Filtrar solo los menús principales (idOrigen = 0 o null)
            const menusPrincipales = menuData.filter(pantalla =>
                !pantalla.idOrigen || pantalla.idOrigen === 0
            );

            $.each(menusPrincipales, function (index, pantalla) {
                // Fila del menú principal
                text += `
                <tr class="pantalla-principal bg-light">
                    <td>
                        
                        <i class="fas fa-edit cmdEditarPantalla active float-right"
                           idpantalla="${pantalla.idPantalla}" 
                           title="Editar ${pantalla.Pantalla}"></i>
                        <strong>${ pantalla.Pantalla}</strong>
                        
                    </td>
                </tr>`;

                // Buscar submenús de este menú principal
                const subMenus = menuData.filter(sub =>
                    sub.idOrigen === pantalla.idPantalla
                );

                // Mostrar submenús
                $.each(subMenus, function (subIndex, subpantalla) {
                    text += `
                <tr class="subpantalla">
                    <td>
                           <i class="fas fa-edit cmdEditarPantalla float-right"
                           idpantalla="${subpantalla.idPantalla}" 
                           title="Editar ${subpantalla.Pantalla}"></i>
                        <span style="margin-left: 20px;"> ${subpantalla.Pantalla}</span>
                        
                    </td>
                </tr>`;
                });

                // Separador entre grupos (opcional)
                text += `<tr class="separador"><td></td></tr>`;
            });

            $('.tblPantallas').html(text);
            $('.cmdEditarPantalla').click(function () {
                $('.cmdEditarPantalla').removeClass('active');
                $(this).addClass('active');

                const idpantalla = $(this).attr('idpantalla');
                //console.log('This ' + idpantalla);
                //const active = $('.cmdEditarPantalla.active').attr('idpantalla');
                //console.log('Active ' + active);
                
                
                if ($('.cmdEditarPantalla.active').attr('idpantalla') == $(this).val()) {
                    idpantalla = $('.cmdEditarPantalla.active').attr('idpantalla') || $(this).val();
                }
                //const idpantalla = $(this).attr('idpantalla');
                //console.log(idpantalla);
                //return;
                jsPantallas('Cargar', idpantalla);
                jsCmbPantalla(idpantalla);
                $('.framePantalla').removeClass('hidden');
                $('.framePermisos').addClass('hidden');

            });

        },
        error: function (xhr, status, error) {
            console.error("Error en la petición AJAX:", status, error);
        }
    });
}
function jsPantallas(caso, elemento) {
    const token = obtenerToken();
    switch (caso) {
        case 'Cargar':
            if (elemento != undefined) {
                obj = {
                    idPantalla: elemento,
                    
                    R: {
                        modulo: $('.sysPantalla').val(),
                        metodos: 'Cargar',
                        Token: token,
                    }
                };
                $.ajax({
                    type: "POST",
                    url: url + 'Pantallas',
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(obj),
                    async: true,
                    processData: false,
                    cache: false,
                    success: function (pantalla) {
                        /*console.log(pantalla);*/
                        $('.cmbOrigen').val(pantalla.idOrigen);
                        $('.cmbEstatus').val(pantalla.idEstatus);
                        $('.nPantalla').val(pantalla.Pantalla);
                        $('.OrdenP').val(pantalla.Orden);
                        $('.uPantalla').val(pantalla.url);
                        $('.cPantalla').val(pantalla.Clase);
                        $('.Descripcion').val(pantalla.Descripcion);
                        $('.cToolTip').val(pantalla.ToolTip);
                        $('.cMG').val(pantalla.MsgGuardar);
                        $('.cME').val(pantalla.MsgEliminar);
                        $('.cPE').val(pantalla.PreguntaEliminar);
                        $('.uMPantalla').val(pantalla.urlMovil);
                    }
                });
            }
            break;
        case 'Guardar':
            elemento = $('.cmdEditarPantalla.active').attr('idpantalla');
            var origen = $('.cmbOrigen').val();
            var valor = $('.rol').is(':checked') ? 1 : 0;
            if (elemento != undefined) {
                obj = {
                    idPantalla: elemento,
                    idEstatus: $('.cmbEstatus').val(),
                    Descripcion :$('.Descripcion').val(),
                    Orden : $('.OrdenP').val(),
                    url : $('.uPantalla').val(),
                    Clase : $('.cPantalla').val(),
                    ToolTip : $('.cToolTip').val(),
                    MsgGuardar : $('.cMG').val(),
                    MsgEliminar : $('.cME').val(),
                    PreguntaEliminar : $('.cPE').val(),
                    idOrigen: origen ,
                    Pantalla: $('.nPantalla').val(),
                    urlMovil:$('.uMPantalla').val(),
                    R: {
                        modulo: $('.sysPantalla').val(),
                        metodos: 'Guardar',
                        Token: token,
                    }
                };
                $.ajax({
                    type: "POST",
                    url: url + 'Pantallas',
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(obj),
                    async: true,
                    processData: false,
                    cache: false,
                    success: function (pantalla) {

                        if (pantalla.idPantalla == 0) {
                            alert('Se creo pantalla ' + pantalla.Pantalla + ';');
                        } else {
                            alert('Se guardaron cambios en la pantalla ' + pantalla.Pantalla + ';');
                        }
                        
                        jsCargartabla();

                    }
                });
            }
            break;
        case 'GuardarPermisos':
                var permisos = [];
                $('.permission-toggle').each(function () {
                    permisos.push({
                        idRol: $(this).val(),
                        Estatusi: $(this).is(':checked') ? 1 : 0,
                        idPantalla: $('.cmdEditarPantalla.active').attr('idpantalla'),
                    });
                });
                obj = {
                    idPantalla: $('.cmdEditarPantalla.active').attr('idpantalla'),
                    Permisos: permisos,
                    R: {
                        modulo: $('.sysPantalla').val(),
                        metodos: 'GuardarPermisos',
                        Token: token,
                    }
                };
                $.ajax({
                    type: "POST",
                    url: url + 'Pantallas',
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(obj),
                    async: true,
                    processData: false,
                    cache: false,
                    success: function (pantalla) {
                        
                        alert('Se guardaron los Permisos a la pantalla: ' + pantalla.nombreP);
                        $('.framePermisos').addClass('hidden');
                        $('.framePantalla').removeClass('hidden');
                        jsPantallas('Cargar', $('.cmdEditarPantalla.active').attr('idpantalla'));

                    }
                });
            
            break;
    }
}







function agregarProducto(elementoProducto) {
    try {
        const $fila = $(elementoProducto).closest('tr');

        // Obtener datos de la fila (adaptado a tu estructura)
        const idProducto = $fila.attr('idproducto'); // Nota: es idproducto, no idProducto
        const nombre = $fila.attr('productos');
        const precio = parseFloat($fila.attr('preciopublico')) || 0;
        const codigo = $fila.find('td:eq(1)').text().trim(); // Asumiendo que el código está en la segunda columna

        // Validaciones
        if (!idProducto) throw new Error('El producto no tiene ID');
        if (!nombre) throw new Error('El producto no tiene nombre');

        // Agregar al carrito
        if (carrito[idProducto]) {
            carrito[idProducto].cantidad += 1;
        } else {
            carrito[idProducto] = {
                nombre: nombre,
                codigo: codigo,
                precio: precio,
                cantidad: 1
            };
        }

        // Actualizar la tabla de venta
        jsProductos('CargarProducTabla');

        // Resaltar producto (opcional)
        $fila.addClass('agregado').siblings().removeClass('agregado');
        jsProductos('CargarV');

    } catch (error) {
        console.error('Error al agregar producto:', error);
        alert('Error: ' + error.message);
    }
}

function actualizarTablaVenta() {
    var html = '';
    var total = 0;

    $.each(carrito, (id, item) => {
        const subtotal = item.precio * item.cantidad;
        total += subtotal;

        html += `
        <tr data-id="${id}">
            <td class="text-center">
                <div class="cantidad-container">
                    <button class="btn btn-sm btn-primary btn-restar">
                        <i class="fas fa-minus"></i>
                    </button>
                    <span class="cantidad">${item.cantidad}</span>
                    <button class="btn btn-sm btn-primary btn-sumar">
                        <i class="fas fa-plus"></i>
                    </button>
                </div>
            </td>
            <td>${item.nombre}</td>
            <td class="text-right">$${item.precio.toFixed(2)}</td>
            <td class="text-right">$${subtotal.toFixed(2)}</td>
            <td class="text-center">
                <button class="btn btn-sm btn-danger btn-eliminar">
                    <i class="fas fa-trash"></i>
                </button>
            </td>
        </tr>`;
    });

    $('.tblVenta').html(html || '<tr><td colspan="6" class="text-center">No hay productos</td></tr>');
    $('.totalV').text('$' + total.toFixed(2));

    // Actualizar cambio si hay pago
    const valorPaga = $('.paga').text().replace(/[^\d.]/g, '');
    if (parseFloat(valorPaga) > 0) {
        actualizarCampoCambio($('.paga').text());
    }
}

function prepararEdicion() {
    const elementoPaga = $('.paga');
    const valorOriginal = elementoPaga.text();

    elementoPaga.text(valorOriginal.replace('$', ''))
        .attr('contenteditable', true)
        .focus()
        .off('keydown')
        .on('keydown', function (e) {
            // Permitir solo números y punto decimal
            if (!((e.keyCode >= 48 && e.keyCode <= 57) || // números 0-9
                (e.keyCode >= 96 && e.keyCode <= 105) || // teclado numérico
                e.keyCode === 190 || e.keyCode === 110 || // punto
                e.keyCode === 8 || e.keyCode === 46 || // backspace, delete
                e.keyCode === 9 || e.keyCode === 27 || // tab, esc
                (e.keyCode >= 37 && e.keyCode <= 40))) { // flechas
                e.preventDefault();
            }

            if (e.keyCode === 27) { // ESC
                elementoPaga.text(valorOriginal)
                    .attr('contenteditable', false);
            }

            if (e.keyCode === 13) { // Enter
                e.preventDefault();
                finalizarEdicion();
            }
        });
}

function finalizarEdicion() {
    const elementoPaga = $('.paga');
    let valorIngresado = elementoPaga.text().trim();

    // Asegurar formato correcto
    if (!valorIngresado.includes('.')) {
        valorIngresado += '.00';
    }

    const valorNumerico = parseFloat(valorIngresado) || 0;

    if (valorNumerico <= 0) {
        alert('El valor debe ser mayor a cero');
        elementoPaga.focus();
        return;
    }

    const valorFormateado = '$' + valorNumerico.toFixed(2);

    elementoPaga.text(valorFormateado)
        .attr('contenteditable', false);

    actualizarCampoCambio(valorFormateado);
    localStorage.setItem('valorPaga', valorFormateado);
}

function actualizarCampoCambio(valor) {
    try {
        const total = parseFloat($('.totalV').text().replace(/[^\d.]/g, '')) || 0;
        const paga = parseFloat(valor.replace(/[^\d.]/g, '')) || 0;
        const cambio = paga - total;

        const simbolo = cambio < 0 ? '-' : '';
        const cambioAbs = Math.abs(cambio);
        const cambioFormateado = `${simbolo}$${cambioAbs.toFixed(2)}`;

        $('.cambio')
            .html(cambioFormateado)
            .css('color', cambio >= 0 ? 'green' : 'red')
            .fadeOut(100).fadeIn(100);

    } catch (error) {
        console.error("Error calculando cambio:", error);
        $('.cambio').text('$0.00').css('color', 'red');
    }
}
//function prepararEdicion() {
//    const elementoPaga = $('.paga');

//    // Guardar valor actual para restaurar si se presiona ESC
//    const valorOriginal = elementoPaga.text();

//    // Preparar para edición
//    elementoPaga.text('')
//        .attr('contenteditable', true)
//        .focus()
//        .off('keydown')
//        .on('keydown', function (e) {
//            // Cancelar edición con ESC
//            if (e.keyCode === 27) { // Tecla ESC
//                elementoPaga.text(valorOriginal)
//                    .attr('contenteditable', false);
//                return;
//            }

//            // Confirmar con Enter
//            if (e.keyCode === 13) { // Tecla Enter
//                e.preventDefault();
//                finalizarEdicion();
//            }
//        });
//}
//function finalizarEdicion() {
//    const elementoPaga = $('.paga');
//    const valorIngresado = elementoPaga.text().trim();

//    const valorNumerico = parseFloat(valorIngresado.replace(/[^\d.]/g, '')) || 0;

//    if (valorNumerico <= 0) {
//        alert('El valor debe ser mayor a cero');
//        elementoPaga.focus();
//        return;
//    }

//    const valorFormateado = '$' + valorNumerico.toFixed(2);

//    elementoPaga.text(valorFormateado)
//        .attr('contenteditable', false);

//    actualizarCampoCambio(valorFormateado);
//    localStorage.setItem('valorPaga', valorFormateado);
//}
//function actualizarCampoCambio(valor) {
//    try {
//        const total = parseFloat($('.totalV').text().replace(/[^\d.]/g, '')) || 0;
//        const paga = parseFloat(valor.replace(/[^\d.]/g, '')) || 0;
//        const cambio = paga - total;

//        // Formatear el cambio (ej: $-10.00 en rojo o $10.00 en verde)
//        const simbolo = cambio < 0 ? '-' : '';
//        const cambioAbs = Math.abs(cambio);
//        const cambioFormateado = `${simbolo}$${cambioAbs.toFixed(2)}`;

//        $('.cambio')
//            .html(cambioFormateado)
//            .css('color', cambio >= 0 ? 'green' : 'red')
//            .fadeOut(100).fadeIn(100);

//    } catch (error) {
//        console.error("Error calculando cambio:", error);
//        $('.cambio').text('$0.00').css('color', 'red');
//    }
//}
function jsProductos(caso, elemento) {
    switch (caso) {
        case 'Cargar':
            obj = {
                R: {
                    modulo: $('.sysPantalla').val(),
                    metodos: 'Cargar',
                    Token: token,
                }
            };

            $.ajax({
                type: "POST",
                url: url + 'Productos',
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(obj),
                async: true,
                processData: false,
                cache: false,
                success: function (response) {

                    //console.log(response.Productos);
                    //console.log(response.Producto);
                    
                    var Cadena = "";
                    /*console.log(response.Productos);*/
                    $.each(response.Producto, function (index, item) {
                        Cadena += `
                            <tr class="tblBitacora ReglonActivo">
                                <td>
                                    <i obtener="${item.idProducto}" 
                                       class="fas fa-edit cmdEditarCatalogo " 
                                       idcatalogo="${item.idProducto}" 
                                       title="Editar ${item.idProducto}" 
                                       aria-hidden="true">
                                    </i>
                                    <span class="sr-only">Editar ${item.Productos}</span>
                                </td>
                                <td style="font-size:20px;">${item.Productos}</td>
                                <td style="font-size:20px;">${item.Codigo}</td>
                            </tr>
                        `;
                    });

                    $('.tblDatos').html(Cadena);
                    jsPaginar();
                    

                    $('.cmdEditarCatalogo').click(function () {

                        $('.cmdEditarCatalogo').removeClass('active');
                        $(this).addClass('active');

                        //const idProducto = $(this).attr('idcatalogo');
                        ////console.log('This ' + idpantalla);
                        ////const active = $('.cmdEditarPantalla.active').attr('idpantalla');
                        ////console.log('Active ' + active);


                        //if ($('.cmdEditarCatalogo.active').attr('idcatalogo') == $(this).val()) {
                        //    idProducto = $('.cmdEditarCatalogo.active').attr('idcatalogo') || $(this).val();
                        //}
                        /*console.log($(this).attr('idcatalogo'));*/
                        
                        jsProductos('CargarI', $(this).attr('idcatalogo'));
                       

                    });
                    const idActivo = localStorage.getItem('productoActivo');
                    if (idActivo) {
                        $(`.cmdEditarCatalogo[idcatalogo="${idActivo}"]`).addClass('active');
                        localStorage.removeItem('productoActivo');
                    }


                },
                error: function (xhr, status, error) {
                    console.error("Error en la petición AJAX:", status, error);
                }
            });

            break;

        case 'CargarI':
            /*console.log(elemento);*/
            obj = {
                idProducto:elemento,
                R: {
                    modulo: $('.sysPantalla').val(),
                    metodos: 'Cargar',
                    Token: token,
                }
            };

            $.ajax({
                type: "POST",
                url: url + 'Productos',
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(obj),
                async: true,
                processData: false,
                cache: false,
                success: function (response) {

                    
                    console.log(response);
                    $('.imagen').val(response.Imagen);
                    $('.cmbCatePro').val(response.idCategoria);
                    $('.cmbEP').val(response.idEstatus);
                    $('.codigo').val(response.Codigo);
                    $('.canMin').val(response.CantidadMinima);
                    $('.existencia').val(response.Existencia);
                    $('.producto').val(response.Productos);
                    $('.descripcion').val(response.Descripcion);
                    $('.cmbUnidad').val(response.idUnidad);
                    $('.costoxCaja').val('$' + response.CostoXCaja);
                    $('.piezaxCaja').val(response.PiezasXcaja);
                    $('.piezaxPaquete').val(response.PiezasXPaquete);

                    $('.precioPublico').val('$' + response.PrecioPublico);

                    const resulCPI = (parseFloat($('.costoxCaja').val().replace('$', '')) / parseFloat($('.piezaxCaja').val())).toFixed(3);
                    $('.costoxPieza').val('$' + resulCPI);

                    const reulCPA = parseFloat($('.costoxPieza').val().replace('$', '')) * parseFloat($('.piezaxPaquete').val());

                    $('.costoxPaquete').val('$' + reulCPA);

                    const utixPro = parseFloat($('.precioPublico').val().replace('$','')) - parseFloat($('.costoxPaquete').val().replace('$',''));
                    $('.uxProducto').val('$' + utixPro.toFixed(3));

                    const utixCaja = (parseFloat($('.precioPublico').val().replace('$', '')) * 4) - parseFloat($('.costoxCaja').val().replace('$', ''));
                    $('.uxCaja').val('$' + utixCaja.toFixed(3));

                    
                    
                },
                error: function (xhr, status, error) {
                    console.error("Error en la petición AJAX:", status, error);
                }
            });

            break;

        case 'Guardar':
            /*console.log("Valor seleccionado en cmbUnidad:", elemento);*/
            obj = {
                idUnidad: parseInt($('.cmbUnidad').val()),
                idProducto: elemento || 0,
                Codigo:$('.codigo').val(),
                CantidadMinima: $('.canMin').val(),
                Existencia: $('.existencia').val(),
                Productos: $('.producto').val(),
                Descripcion: $('.descripcion').val(),
                CostoXCaja: $('.costoxCaja').val().replace('$', ''),
                PiezasXcaja: $('.piezaxCaja').val(),
                PiezasXPaquete: $('.piezaxPaquete').val(),
                PrecioPublico: $('.precioPublico').val().replace('$', ''),
                Imagen: $('.imagen').val() ,
                idCategoria: $('.cmbCatePro').val(),
                idEstatus: $('.cmbEP').val(),
                R: {
                    modulo: $('.sysPantalla').val(),
                    metodos: 'Guardar',
                    Token: token,
                }
            };
            /*console.log(obj);*/
            
            $.ajax({
                type: "POST",
                url: url + 'Productos',
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(obj),
                async: true,
                processData: false,
                cache: false,
                success: function (response) {

                    if (response.R.Estatus == "OK") {
                        alert(response.R.mensaje + ': ' + $('.producto').val());
                        
                        $('.cmbUnidad').val(0);
                        $('.cmdEditarCatalogo').removeClass('active');
                        $('.codigo').val('');
                        $('.canMin').val('');
                        $('.existencia').val('');
                            $('.producto').val('');
                            $('.descripcion').val('');
                            $('.costoxCaja').val('');
                            $('.piezaxCaja').val('');
                            $('.piezaxPaquete').val('');
                        $('.precioPublico').val('');
                        $('.costoxPieza').val('');
                        $('.costoxPaquete').val('');
                        $('.uxProducto').val('');
                        $('.uxCaja').val('');
                        
                        $('.imagen').val('');
                        $('.cmbCatePro').val(0);
                        $('.cmbEP').val(0);
                        jsProductos('Cargar');
                    }
                },
                error: function (xhr, status, error) {
                    console.error("Error en la petición AJAX:", status, error);
                }
            });
            break;
        case 'CargarV':
            obj = {
                R: {
                    modulo: $('.sysPantalla').val(),
                    metodos: 'CargarP',
                    Token: token,
                }
            };

            $.ajax({
                type: "POST",
                url: url + 'Venta',
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(obj),
                async: true,
                processData: false,
                cache: false,
                success: function (response) {

                    //console.log(response.Productos);
                    //console.log(response.Producto);

                    var Cadena = "";
                    console.log(response.Producto);
                    $.each(response.Producto, function (index, item) {
                        Cadena += `
                            <tr class="tblBitacora ReglonActivo producto" idProducto="${item.idProducto} " productos ="${item.Productos}" precioPublico = "${item.PrecioPublico}" >
                                <td style="font-size:20px;">${item.Productos}</td>
                                <td>
                                   ${item.Codigo}
                                </td>
                            </tr>
                        `;
                    });

                    $('.tblDVatos').html(Cadena);
                    jsPaginar();


                    

                },
                error: function (xhr, status, error) {
                    console.error("Error en la petición AJAX:", status, error);
                }
            });

            break;
        case 'CargarVCI':
            obj2 = {
                id: elemento ,
                R: {
                    modulo: $('.sysPantalla').val(),
                    metodos: 'Cargar2',
                    Token: token,
                }
            }
            $.ajax({
                type: "POST",
                url: url + 'DatosCat',
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(obj2),
                async: true,
                processData: false,
                cache: false,
                success: function (Resultado) {
                    /*console.log(Resultado);*/
                    $('.movilC').val(Resultado.catalogos[0].Descripcion);

                },
                error: function (xhr) {
                    return false;
                }
            });
            break;

        case 'CargarProducTabla':
            var html = '';
            var total = 0;
            
            $.each(carrito, (id, item) => {
                const subtotal = item.precio * item.cantidad;
                total += subtotal;

                        html += `
                <tr data-id="${id}">
                    <td class="text-center "  >
                        <div class="cantidad-container">
                            <button class="btn btn-sm  btn-primary btn-restar">
                                <i class="fas fa-minus"></i>
                            </button>
                            <span class="cantidad">${item.cantidad}</span>
                            <button class="btn btn-sm  btn-primary btn-sumar">
                                <i class="fas fa-plus"></i>
                            </button>
                        </div>
                    </td>
                    <td>${item.nombre}</td>
                    <td class="text-right">$${item.precio.toFixed(2)}</td>
                    <td class="text-right">$${subtotal.toFixed(2)}</td>
                    <td class="text-center">
                        <button class="btn btn-sm btn-danger btn-eliminar">
                            <i class="fas fa-trash"></i>
                        </button>
                    </td>
                </tr>`;
             });

            $('.tblVenta').html(html || '<tr><td colspan="6" class="text-center">No hay productos</td></tr>');
            const totalFormateado = '$' + total.toFixed(2);
            $('.totalV').text(totalFormateado);
            const valorPaga = $('.paga').text().replace(/[^\d.]/g, '') || 0;

            // Calcular y actualizar el cambio automáticamente
            if (parseFloat(valorPaga) > 0) {
                actualizarCampoCambio($('.paga').text()); // Forzar actualización
            } else {
                $('.cambio').text('$0.00'); // Resetear si no hay pago
            }
            break;


    }
}

function jsVenta(caso, elemento) {
    switch (caso) {

        case 'Guardar': 
            console.log(carrito);
            
            //console.log($('.cmbCliente').val());
            //return;
            var paga = parseFloat($('.paga').text().replace(/[^\d.]/g, ''));
            var total = parseFloat($('.totalV').text().replace(/[^\d.]/g, ''));
            
            //if ($('.cmbCliente').val() == 0) {
            //    $('.cmbCliente').val(15).trigger('change');
            //}
            if ($('.cmbTDP').val() == 0) {
                alert("Agrega un tipo de pago");
                return;
            }
            if (parseFloat($('.paga').text().replace(/[^\d.]/g, '')) == 0) {
                alert("No se agrego un pago");
                return;
            }
            if (paga > total) {
                $('.paga').text('$' + total.toString());
            }
            if (paga == total) {
                $('.paga').text('$' + total.toString());
            }
            /*console.log($('.cmbCliente').val());*/
            
            /*var permisos = [];*/
            $.each(carrito, (id, item) => {
                permisos.push({
                    idProducto:id,
                    cantidad:item.cantidad,
                });
            });
            
            obj = {
                idCliente: $('.cmbCliente').val(),
                Token: token,
                IdTipoPago: $('.cmbTDP').val(),
                Importe: $('.totalV').text().replace(/[^\d.]/g, ''),
                Pago: $('.paga').text().replace(/[^\d.]/g, ''),
                Existencias:permisos,
                R: {
                    modulo: $('.sysPantalla').val(),
                    metodos: 'Guardar',
                    Token: token,
                }
            };

            $.ajax({
                type: "POST",
                url: url + 'Venta',
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(obj),
                async: true,
                processData: false,
                cache: false,
                success: function (response) {

                   /*console.log(response);*/
                   
                    if (response.R.Estatus === "OK") {
                        $('.cmbCliente').val(0);
                        $('.cmbTDP').val(0);
                        $('.cambio').css('color', 'white');
                        $('.movilC').val('');
                        
                        $('.tblVenta tr').fadeOut(200, function () {
                            $(this).remove();
                        });

                        // Animación para los totales
                        $('.totalV, .paga, .cambio').animate({
                            opacity: 0
                        }, 200, function () {
                            $(this).text('$0.00').animate({ opacity: 1 }, 200);
                        });

                        // Opcional: Resetear variables globales
                        if (typeof carrito !== 'undefined') {
                            carrito = {};
                            permisos = [];
                        }
                    }
              



                },
                error: function (xhr, status, error) {
                    console.error("Error en la petición AJAX:", status, error);
                }
            });


            break;

        case 'Cargar':
            obj = {
                idCliente: $('.cmbCE').val() || '',
                FechaInicial: $('.fechaIniV').val() || '',
                FechaFinal: $('.fechaFinV').val() || '',
                IdTipoPago: $('.cmbTDPE').val() || '',
                R: {
                    modulo: $('.sysPantalla').val(),
                    metodos: 'Cargar',
                    Token: token,
                }
            };

            $.ajax({
                type: "POST",
                url: url + 'Venta',
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(obj),
                async: true,
                processData: false,
                cache: false,
                success: function (response) {

                    console.log(response.Ventas);

                    var Cadena = "";
                    /*console.log(response.Productos);*/
                    $.each(response.Ventas, function (index, item) {
                        let fechaFormateada = fechaF(item.Fecha);

                        Cadena += `
                            <tr class="tblBitacora ReglonActivo">
                                <td>
                                    <i obtener="${item.idVenta}" 
                                       class="fas fa-edit cmdEditarDeuda " 
                                       idVenta="${item.idVenta}" 
                                       title="Editar ${item.idVenta}" 
                                       aria-hidden="true">
                                    </i>
                                    <span class="sr-only">Editar ${item.Productos}</span>
                                </td>
                                <td style="font-size:20px;">${fechaFormateada}</td>
                                <td style="font-size:20px;">${item.Cliente}</td>
                                <td style="font-size:20px;">${item.Vendio}</td>
                                <td style="font-size:20px;">${item.Estatus}</td>
                                <td style="font-size:20px;">${item.Importe}</td>
                                <td style="font-size:20px;">${item.Saldo}</td>
                            </tr>
                        `;
                    });

                    $('.tblEV').html(Cadena);
                    $('.cmdEditarDeuda').click(function () {
                        /*jsVenta('uDeuda', $(this).attr('idVenta'));*/
                        /*$('.estadisFrame').addClass('hidden');*/
                        $('.inputNP').val('');
                        $('.inputC').val('');

                        $('.cmdEditarDeuda').removeClass('active');
                        $(this).addClass('active');

                        // Muestra el fondo oscuro
                        $('.estadisFrame').addClass('disabled');
                        $('.modal-backdrop').removeClass('hidden');
                        // Muestra el uDeudaFrame
                        $('.uDeudaFrame').removeClass('hidden');
                        jsVenta('uDeuda', $(this).attr('idVenta'));
                       
                    });


                    $('.modal-backdrop').click(function () {
                        $(this).addClass('hidden');
                        $('.uDeudaFrame').addClass('hidden');
                        $('.estadisFrame').removeClass('disabled');
                        $('body').css('overflow', 'auto');
                    });
                    $(document).keyup(function (e) {
                        if (e.key === "Escape" && !$('.modal-backdrop').hasClass('hidden')) {
                            $('.modal-backdrop').addClass('hidden');
                            $('.uDeudaFrame').addClass('hidden');
                            $('.estadisFrame').removeClass('disabled');
                            $('body').css('overflow', 'auto');
                        }
                    });



                },
                error: function (xhr, status, error) {
                    console.error("Error en la petición AJAX:", status, error);
                }
            });
            break;

        case 'uDeuda':
            obj = {
                idVenta:elemento,
                R: {
                    modulo: $('.sysPantalla').val(),
                    metodos: 'Cargar',
                    Token: token,
                }
            };

            $.ajax({
                type: "POST",
                url: url + 'Venta',
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(obj),
                async: true,
                processData: false,
                cache: false,
                success: function (response) {

                    /*console.log(response);*/
                    let nueva = formatFAM(response.Saldo)
                    $('.inputCambiar').val(nueva);
                    $('.inputNP').on('keyup change', function () {
                        // Formatear automáticamente mientras escribe
                        const rawValue = $(this).val();
                        if (!rawValue.startsWith('$')) {
                            $(this).val('$' + rawValue.replace(/[^\d.]/g, ''));
                        }

                        calcularCambio();
                    });
                    $('.inputNP').on('blur', function () {
                        $(this).val(formatFAM($(this).val()));
                    });

                },
                error: function (xhr, status, error) {
                    console.error("Error en la petición AJAX:", status, error);
                }
            });
            break;
        case 'GuardarUV':
            console.log(elemento);
            obj = {
                idVenta: elemento,
                Pago: $('.inputNP').val().replace('$', ''),
                Token:token,
                R: {
                    modulo: $('.sysPantalla').val(),
                    metodos: 'Guardar',
                    Token: token,
                }
            };

            $.ajax({
                type: "POST",
                url: url + 'Venta',
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(obj),
                async: true,
                processData: false,
                cache: false,
                success: function (response) {
                    console.log(response);
                    if (response.R.Estatus == 'OK') {
                        alert('Se actualizo deuda');
                        $('.estadisFrame').removeClass('disabled');
                        $('.modal-backdrop').addClass('hidden');
                        // Muestra el uDeudaFrame
                        $('.uDeudaFrame').addClass('hidden');
                        jsVenta('Cargar');
                        

                        

                    }
                },
                error: function (xhr, status, error) {
                    console.error("Error en la petición AJAX:", status, error);
                }
            });
            break;
    }
}
function fechaF(fechaf) {
    let fechaFormateada = "";
    if (fechaf) { // Valida que sí venga fecha
        let timestamp = parseInt(fechaf.replace(/\/Date\((\d+)\)\//, "$1"));
        if (!isNaN(timestamp)) {
            let Fecha = new Date(timestamp);
            return fechaFormateada = Fecha.toLocaleDateString("es-ES", {
                day: "2-digit",
                month: "2-digit",
                year: "numeric",
                hour: "2-digit",
                minute: "2-digit"
            });
        } else {
            return fechaFormateada = "Sin fecha";
        }
    } else {
        return fechaFormateada = "Sin fecha"; // Aquí si Fecha viene nula
    }
    return fechaFormateada;
}
function jsCatalogo(caso, elemento) {
    switch (caso) {
        case 'Cargar':
           
            break;
        case 'Crear':
            var idCatalogo = $('#ipId').val();
            var idOrigen = $('.idOrigen').val();
            var catalogo = $('#ipCa').val();
            var orden = $('#ipOrd').val();
            var valorDefault = $('#ipDe').is(':checked') ? 1 : 0;
            var valor = $('#ipVa').val();
            var descripcion = $('#ipTe').val();
            if (catalogo == "") {
                alert("El campo catálogo no puede estar vacío");
                return;
            }
            if (orden == "") {
                alert("El campo orden no puede estar vacío");
                return;
            }
            if (valor == "") {
                alert("El campo valor no puede estar vacío");
                return;
            }
            if (descripcion == "") {
                alert("El campo descripción no puede estar vacío");
                return;
            }

            var obj = {
                Id_Catalogo: idCatalogo,
                IdOrigen: idOrigen,
                Orden: orden,
                Catalogo: catalogo,
                Valor: valor,
                Descripcion: descripcion,
                ValorDefault: valorDefault,
                R:
                {
                    modulo: $('.sysPantalla').val(),
                    metodos: 'Guardar',
                    Token: token
                }
            };
            $.ajax({
                type: "POST",
                url: url + 'Catalogos',
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(obj),
                async: true,
                processData: false,
                cache: false,
                success: function (Resultado) {
                    console.log(Resultado);
                    return;
                },
                error: function (xhr) {
                    return false;
                }
            });
           
            break;

        case 'Cancelar': 
            $('#ipId').val("");
            $('#ipCa').val("");
            $('#ipOrd').val("");
            $('#ipDe').prop('checked', false);
            $('#ipVa').val("");
            $('#ipTe').val("");
            break;
    }
}
function formatFAM(num) {
    // Si es string, limpiar y convertir a float
    if (typeof num === 'string') {
        num = parseFloat(num.replace(/[^0-9.-]/g, ''));
    }

    // Verificar si es un número válido
    if (isNaN(num)) {
        console.error('El valor proporcionado no es un número válido');
        return '$0.00';
    }

    // Formatear a 2 decimales con signo $
    return '$' + num.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}
function formatMAFloat(currency) {
    // Eliminar símbolos y comas, luego convertir a float
    const num = parseFloat(currency.replace(/[^0-9.-]/g, ''));

    // Verificar si es un número válido
    if (isNaN(num)) {
        console.error('El formato de moneda no es válido');
        return 0.00;
    }

    return num;
}
function calcularCambio() {
    // 1. Obtener valores numéricos
    const deuda = formatMAFloat($('.inputCambiar').val());
    const pago = formatMAFloat($('.inputNP').val());
    const cambio = pago - deuda;

    $('.inputC').val(formatFAM(cambio));

    // Opcional: Resaltar si el pago es insuficiente
    if (cambio < 0) {
        $('.inputC').addClass('text-danger');
    } else {
        $('.inputC').removeClass('text-danger');
    }
}
function jsCargarResultados(metodos = 'Cargar' ) {
    var fechaIni = $('.fechaIni').val() || '';
    var fechaFin = $('.fechaFin').val() || '';
    var modulo = $('.cmbRPan').val() || '';
    var usuario = $('.cmbUsuarios').val() || '';
    var metodo = $('.cmbMetodos').val() || '';
    var estatus = $('.cmbEstatus').val() || '';



    var obj = {
        
        FechaInicial: fechaIni,
        FechaFinal: fechaFin,
        modulo: modulo,
        Usuario: usuario,
        metodos: metodo,
        Estatus: estatus,
        R: {
            modulo: $('.sysPantalla').val(),
            metodos: metodos,
            Token: token,
        }
    };
    
   
    $.ajax({
        type: "POST",
        url: url + 'ResultadosN',
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(obj),
        async: true,
        processData: false,
        cache: false,
        success: function (Result) {

            console.log(Result.Bitacora)
            var Cadena = "";

            $.each(Result.Bitacora, function (index, item) {
                let fechaFormateada = fechaF(item.fecha);

                Cadena += ` <tr class="tblBitacora ReglonActivo">\n';
                <td><i class="fas fa-edit cmdEditarResultado" idresultado="${item.Valor}" title="Editar"></i></td>
                <td>${item.idResultado || ''}</td>
                <td>${item.modulo || ''}</td>
                <td>${item.metodos || ''}</td>
                <td>${fechaFormateada}</td>
                <td>${item.Usuario || ''}</td>
                <td>${item.Estatus || ''}</td>
                <td>${item.mensaje || ''}</td>
                <td>${item.mensajeDeSistema || ''}</td>
            </tr>`;

                //if ($(this).attr('catalogo') != 'Tipo de catalogo') {

            });
            $('.tblDatos').html(Cadena);
            jsPaginar();
        },
        error: function (xhr) {
            return false;
        }
    });

}

function jsCompraG(metodo = '', url2 ='Compra') {
    return new Promise(function (resolve, reject) {
        obj = {

            FechaInicial: $('.fechaIniC').val() || '',
            FechaFinal: $('.fechaFinC').val() || '',



            
            idTipoPago: idTipoPago || '',
            PrecioTotal: $('.totalV').text().replace(/[^\d.]/g, ''),
            idProductos: permisos2,

            idProovedor: idProveedorC,
            Existencias: permisos,
            idProveedor: idProveedorC || '' ,
            id: idProveedorC,
            R: {
                modulo: $('.sysPantalla').val(),
                metodos: metodo,
                Token: token,
            }
        };

        $.ajax({
            type: "POST",
            url: url + url2,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(obj),
            async: true,
            processData: false,
            cache: false,
            success: function (response) {
                resolve(response); // Resolvemos la promesa con la respuesta
            },
            error: function (error) {
                reject(error);
            }
        });
    });
}

function jsCompra(metodo = '') {
    switch (metodo) {
        case 'Cargar':
            idProveedorC = $('.cmbProveedor').val() || '';
            console.log("Proveedor seleccionado:", idProveedorC);

            jsCompraG('Cargar').then(function (response) {
                console.log("Respuesta del servidor:", response);
                if (response && response.Producto) { 
                    var Cadena = "";
                    $.each(response.Producto, function (index, item) {
                        Cadena += `
                             <tr class="tblBitacora ReglonActivo productoC" idProducto="${item.idProducto}" productos ="${item.Productos}" precioPublico = "${item.CostoXCaja}" >
                                <td style="font-size:20px;">${item.Productos}</td>
                                <td>
                                   ${item.Codigo}
                                </td>
                            </tr>
                        `;
                    });
                    $('.tblDatos').html(Cadena);
                    jsPaginar();
                } else {
                    console.error("La respuesta no contiene datos válidos:", response);
                }
            }).catch(function (error) {
                console.error("Error en la solicitud:", error);
            });
           
            //jsCompraG('Cargar2', 'DatosCat').then(function (response) {
            //    console.log("Respuesta del servidor:", response);
            //    if (response) {
            //        $('.movilC').val(response.catalogos[0].Descripcion);
            //    } else {
            //        console.error("La respuesta no contiene datos válidos:", response);
            //    }
            //}).catch(function (error) {
            //    console.error("Error en la solicitud:", error);
            //});
            break;
        case 'Guardar':
            idProveedorC = $('.cmbPro').val();
            idTipoPago = $('.cmbTDP').val();
            var paga = parseFloat($('.paga').text().replace(/[^\d.]/g, ''));
            var total = parseFloat($('.totalV').text().replace(/[^\d.]/g, ''));

            //if ($('.cmbCliente').val() == 0) {
            //    $('.cmbCliente').val(15).trigger('change');
            //}
            if ($('.cmbTDP').val() == 0) {
                alert("Agrega un tipo de pago");
                return;
            }
            if (parseFloat($('.paga').text().replace(/[^\d.]/g, '')) == 0) {
                alert("No se agrego un pago");
                return;
            }
            if (paga > total) {
                $('.paga').text('$' + total);
            }
            if (paga == total) {
                $('.paga').text('$' + total.toString());
            }
            /*console.log($('.cmbCliente').val());*/
            $.each(carrito, (id, item) => {
                permisos.push({
                    idProducto: id,
                    cantidad: item.cantidad,
                });
            });

            /*console.log(permisos);*/
            permisos2 = permisos.map(function (item, index) {
                return item.idProducto.trim() + ":" + item.cantidad;
            }).join(",");
            /* console.log(permisos2);*/
            jsCompraG('Guardar').then(function (response) {
                console.log("Respuesta del servidor:", response);
                if (response.R.Estatus == 'OK') {
                    $('.cmbTDP').val(0);
                    $('.cmbPro').val(0);

                    $('.tblVenta tr').fadeOut(200, function () {
                        $(this).remove();
                    });

                    $('.totalV, .paga, .cambio').animate({
                        opacity: 0
                    }, 200, function () {
                        $(this).text('$0.00').animate({ opacity: 1 }, 200);
                    });

                    jsCompra('Cargar');

                } else {
                    console.error("La respuesta no contiene datos válidos:", response);
                }
                if (typeof carrito !== 'undefined') {
                    carrito = {};
                    permisos = [];
                }
                console.log(carrito);
            }).catch(function (error) {
                console.error("Error en la solicitud:", error);
            });
            break;
        case 'BCompra':
            idProveedorC = $('.cmbProveedorC').val() || '';
            idTipoPago = $('.cmbTDPEC').val() || '';
            console.log(idProveedorC, '..',idTipoPago);
            jsCompraG('BCompra').then(function (response) {
                console.log("Respuesta del servidor:", response);
                console.log("Respuesta del servidor:", response.Compra);
                var Cadena = '';
                if (response.R.Estatus == 'OK') {

                    $.each(response.Compra, function (index, item) {
                        let fechaFormateada = fechaF(item.Fecha);

                        Cadena += `
                            <tr class="tblBitacora ReglonActivo">
                                <td> ${item.idCompra}</td>
                                <td>
                                   ${fechaFormateada}
                                </td>
                                <td>${item.Proveedor}</td>
                                <td>${item.PrecioTotal}</td>
                                <td>${item.ProductosConCantidad}</td>
                            </tr>
                        `;
                    });
                    
                    $('.tblEV').html(Cadena);

                } else {
                    console.error("La respuesta no contiene datos válidos:", response);
                }
                
                
            }).catch(function (error) {
                console.error("Error en la solicitud:", error);
            });
            break;
    }
}

function cargarMenuDinamico() {
    const token = obtenerToken();
    const menuData = sessionStorage.getItem('menuData');

    // Limpiar menú dinámico anterior si existe
    $('.menu-dinamico').remove();

    if (token && menuData) {
        try {
            $('.eliminar').addClass('hidden');
            const menuItems = JSON.parse(menuData);
            let menuHTML = '';

            // Ordenar menús por propiedad Orden
            menuItems.sort((a, b) => a.Orden - b.Orden).forEach(item => {
                if (item.SubMenu && item.SubMenu.length > 0) {
                    menuHTML += `
                                <li class="dropdown menu-dinamico">
                                    <a href="#" class="dropdown-toggle" data-toggle="dropdown">
                                        ${item.Pantalla} <span class="caret"></span>
                                    </a>
                                    <ul class="dropdown-menu">`;

                    item.SubMenu.sort((a, b) => a.Orden - b.Orden).forEach(sub => {
                        menuHTML += `<li><a href="${sub.url}" title="${sub.ToolTip}">${sub.Pantalla}</a></li>`;
                    });

                    menuHTML += `</ul></li>`;
                } else {
                    menuHTML += `
                                <li class="menu-dinamico">
                                    <a href="${item.url}" title="${item.ToolTip}">${item.Pantalla}</a>
                                </li>`;
                }
            });

            // Insertar menú dinámico antes del elemento "About"
            $('ul.nav.navbar-nav li:contains("About")').first().before(menuHTML);

        } catch (e) {
            console.error("Error al cargar menú:", e);
        }
    }
}

function actualizarVisibilidad() {
    const token = obtenerToken();
   /* console.log("Token actual:", token);*/ // Para depuración

    // Controlar elementos de navegación
    $('.salirL').toggleClass('hidden', !token);  // Mostrar salir solo con token
    $('.hidenP').toggleClass('hidden', !!token); // Mostrar login solo sin token

    // Cargar menú dinámico
    cargarMenuDinamico();
}

function obtenerToken() {
    // Verifica múltiples fuentes y valida el formato del token
    const token = $('.sysToken').val() ||
        sessionStorage.getItem('currentToken') ||
        localStorage.getItem('sysToken');

    // Validación básica del token (debe ser un GUID válido)
    if (token && /^[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$/i.test(token)) {
        return token;
    }
    return null;
}
// Mover el evento del switch al archivo JS principal

