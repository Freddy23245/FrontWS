const UtilsPdf = {
    ajaxPost: (Url,Model, callback, callbackError) => {
        $.post(Url,Model)
            .done(callback)
            .fail(callbackError);
    }
};
const UtilsShowPdf = {
    ajaxPost: (Url, Model,callback, callbackError) => {
        $.post(Url,Model)
            .done(callback)
            .fail(callbackError);
    }
};
const pdf = {

    init: () => {
        pdf.componentes = {
            Descargar: $("#Descargar"),
            UrlGenerarPDF: $("#UrlGenerarPDF"),
            UrlVerPDF: $("#UrlVerPDF"),
            
        };
        pdf.componentes.Descargar.on('click', pdf.acciones.botonClick)
    },

    componentes: {
        Descargar:undefined,
        UrlVerPDF: undefined,
        UrlGenerarPDF:undefined
    },

    acciones: {
        botonClick: () => {
            const Url = pdf.componentes.UrlGenerarPDF.val();
            console.log(Url);
            const Model = {

            };
            UtilsPdf.ajaxPost(Url,Model,
                (data) => {
                    const Url2 = pdf.componentes.UrlVerPDF.val();
                    const Model = {

                    };
                    UtilsShowPdf.ajaxPost(Url2, Model, (data) => {
                        console.log(Url2);
                        console.log("OK")
                    });
                });
        }
    }
}