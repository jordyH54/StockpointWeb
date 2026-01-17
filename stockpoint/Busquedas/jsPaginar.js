$(document).ready(function () {
    $('.txtBuscar').keypress(function (e) {
        tabla = $(this).attr('table');
        tabla = '.tblDatos.' + $(this).attr('table') + ' tr';
        $(tabla).addClass('hidden');
        var Texto = $(this).val() + String.fromCharCode(e.which);
        $(tabla).each(function () {
            var html = $(this).html();
            html = html.toLowerCase();
            if (html.indexOf(Texto.toLowerCase()) != -1) {
                $(this).addClass('ReglonActivo');
                $(this).removeClass('hidden');
            }
            else {
                $(this).removeClass('ReglonActivo');
                $(this).addClass('hidden');
            }
        });
        jsPaginar();


    });
})
function jsPaginar() {
    //#region inicial
    tabla = '.tblDatos.' + $('.tblDatos').attr('table') + ' tr';
    ReglonActivo = '.' + $('.tblDatos').attr('table') + '.ReglonActivo';
    tfood = '.tfood.' + $('.tblDatos').attr('table');
    Cadena = "";
    Cadena = Cadena + '                        <span class=" cmdAnterior "><i class="fa-solid fa-circle-left"></i></span>\n';
    Cadena = Cadena + '                        <span item = "1" class="NoPag1 NoPag NoPagActivo">1</span>\n';
    Cadena = Cadena + '                        <span item = "2" class="NoPag2 NoPag hidden">2</span>\n';
    Cadena = Cadena + '                        <span item = "3" class="NoPag3 NoPag hidden">3</span>\n';
    Cadena = Cadena + '                        <span item = "4" class="NoPag4 NoPag hidden">4</span>\n';
    Cadena = Cadena + '                        <span item = "5" class="NoPag5 NoPag hidden">5</span>\n';
    Cadena = Cadena + '                        <span item = "" class="cmdSiguiente "><i class="fa-solid fa-circle-right"></i></span>\n';
    $(tfood).html(Cadena);
    var Reciduo = $(ReglonActivo).length % 10;
    var NoPagina = Reciduo == 0 ? $(ReglonActivo).length / 10 : (($(ReglonActivo).length - Reciduo) / 10) + 1;
    var Min = ($('.NoPagActivo').html() * 10) - 11;
    var Max = Min + 11;
    $(ReglonActivo).each(function (index) {
        index > Min && index < Max ? $(this).removeClass('hidden') : $(this).addClass('hidden');
    });
    Min = Min + 2;
    $('.thTable').html('Mostrando ' + Min + ' a ' + Max + ' de ' + $(ReglonActivo).length + ' Registros,');
    $(".NoPag").attr("onclick", "").unbind("click");
    $(".cmdSiguiente ").attr("onclick", "").unbind("click");
    $(".cmdAnterior").attr("onclick", "").unbind("click");
    //#endregion
    $('.NoPag').click(function () {
        $('.NoPag').removeClass('NoPagActivo');
        $(this).addClass('NoPagActivo');
        var min = ($(this).html() * 10) - 11;
        var max = min + 10;
        $(tabla).each(function (index) {
            index > min && (index < max || index == max) ? $(this).removeClass('hidden') : $(this).addClass('hidden');
        });
    });
    $('.cmdAnterior').click(function () {
        NoPagActivo = $('.NoPagActivo').attr('item') == 1 ? 5 : NoPagActivo = eval($('.NoPagActivo').attr('item')) - 1;
        var Btn = '.NoPag' + NoPagActivo;
        $('.NoPag').removeClass('NoPagActivo');
        $('.cmdSiguiente').removeClass('hidden');
        var Btn = '.NoPag' + NoPagActivo;
        $(Btn).addClass('NoPagActivo');
        pagHtml = eval($('.NoPagActivo').html());
        if (pagHtml == 1) {
            $('.cmdAnterior').addClass('hidden');

        }
        else {
            $('.cmdAnterior').removeClass('hidden');
            if (pagHtml % 5 == 0) {
                pagHtml = pagHtml - 9;
                for (i = 1; i < 6; i++) {
                    var Btn = '.NoPag' + i;
                    $(Btn).html(pagHtml);
                    pagHtml++;
                    $(Btn).removeClass('hidden');

                }
            }
        }
        pagHtml = eval($('.NoPagActivo').html());

        var Min = (pagHtml * 10) - 10;
        var Max = Min + 9;
        $(tabla).each(function (index) {
            (index > Min || index == Min) && (index < Max || index == Max) ? $(this).removeClass('hidden') : $(this).addClass('hidden');
        });
        $('.thTable').html('Mostrando ' + Min + ' a ' + Max + ' de ' + $(ReglonActivo).length + ' Registros,');
    });
    $('.cmdSiguiente').click(function () {

        NoPagActivo = $('.NoPagActivo').attr('item') == 6 ? 1 : NoPagActivo = eval($('.NoPagActivo').attr('item')) + 1;
        pagHtml = eval($('.NoPagActivo').html());

        Reciduo = $(ReglonActivo).length % 10;
        var TotalPagina = (($(ReglonActivo).length - Reciduo) / 10) + 1;
        TotalPagina = $(ReglonActivo).length % 10 == 0 ? $(ReglonActivo).length / 10 : (($(ReglonActivo).length - Reciduo) / 10) + 1;
        $('.NoPag').removeClass('NoPagActivo');
        var Btn = '.NoPag' + NoPagActivo;
        $(Btn).addClass('NoPagActivo');
        $('.cmdAnterior').removeClass('hidden');

        if (NoPagActivo % 6 == 0) {
            for (i = 1; i < 6; i++) {
                NoPagActivo = NoPagActivo + i;
                var Btn = '.NoPag' + i;
                if (i == 1) {
                    $(Btn).addClass('NoPagActivo');

                }
                Pag = pagHtml + i;
                if (Pag < TotalPagina || Pag == TotalPagina) {
                    $(Btn).removeClass('hidden');
                }
                else {
                    $(Btn).addClass('hidden');
                }
                Pag < TotalPagina || Pag == TotalPagina ? $(Btn).removeClass('hidden') : $(Btn).addClass('hidden');
                $(Btn).html(Pag);
            }
        }

        pagHtml = eval($('.NoPagActivo').html());
        pagHtml < TotalPagina ? $(this).removeClass('hidden') : $(this).addClass('hidden');
        var Max = (pagHtml * 10) - 1;
        var Min = Max - 10;
        $(tabla).each(function (index) {
            index > Min && (index < Max || index == Max) ? $(this).removeClass('hidden') : $(this).addClass('hidden');
        });
        Min = Min + 2;
        Max++;
        $('.thTable').html('Mostrando ' + Min + ' a ' + Max + ' de ' + $(ReglonActivo).length + ' Registros,');
    });

    var noBtn = NoPagina > 5 ? 5 : NoPagina;
    for (i = 1; i < noBtn || i == noBtn; i++) {
        var Btn = '.NoPag' + i;
        $(Btn).removeClass('hidden');
    }
}
