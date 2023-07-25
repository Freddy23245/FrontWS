const Utils2 = {
    ajaxPost: (Url, Model, callback, callbackError) => {
        $.post(Url, Model)
            .done(callback)
            .fail(callbackError);
    }
};
const del = {

    init: () => {
        del.componentes = {
            btnID: $("#btnID"),
            UrlPostPersonasD: $("#UrlPostPersonasD")
        };
        del.componentes.btnID.on('click',del.acciones.botonClick)
    },
  
    componentes: {
        btnID: undefined,
        UrlPostPersonasD:undefined
    },

    acciones: {
        botonClick: () => {
            const Url = del.componentes.UrlPostPersonasD.val();
            console.log(Url);
            const Model = {

            };
            Utils2.ajaxPost(Url, Model,
                (data) => {
                    console.log("datos eliminados");
                });
        }
    }
}