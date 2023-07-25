function mostrarModalUpdate() {
    var modal = document.getElementById('myModalUpdate');
    $(modal).modal('show');
}
function ValidarCampos() {
    var modal = document.getElementById('myModalError');
    if ($("#cuil").val() === "" || $("#cuil").val() == null || $("#ApellidoYNombre").val() === "" || $("#ApellidoYNombre").val() == null || $("#FechaNacimiento").val() === "" || $("#FechaNacimiento").val() == null || $("#idzona").val() === "" || $("#idzona").val() == null) {
        $(modal).modal('show');
        return false;
    }
    return true;
}
const Utils1 = {
    ajaxPost: (Url, Model, callback, callbackError) => {
        $.post(Url, Model)
            .done(callback)
            .fail(callbackError);
    },
};

const Update = {
    init: () => {
        Update.componentes = {
            Cuil: $("#cuil"),
            ApellidoYNombre: $("#ApellidoYNombre"),
            FechaNacimiento: $("#FechaNacimiento"),
            idzona: $("#idzona"),
            UrlPostPersonas: $("#UrlPostPersonas"),
            botonAgregar: $("#botonAgregar")
        };
        Update.componentes.botonAgregar.on('click',Update.acciones.botonClick)
    },
    componentes: {
        Cuil: undefined,
        ApellidoYNombre: undefined,
        FechaNacimiento: undefined,
        idzona: undefined,
        UrlPostPersonas: undefined,
        botonAgregar: undefined
    },
    acciones: {
        botonClick: () => {
            if (ValidarCampos()) {
                const Url = Update.componentes.UrlPostPersonas.val();
                const Model = {

                    modifica: {
                        Cuil: Update.componentes.Cuil.val(),
                        ApellidoYNombre: Update.componentes.ApellidoYNombre.val(),
                        FechaNacimiento: Update.componentes.FechaNacimiento.val(),
                        idzona: Update.componentes.idzona.val()
                    }
                };

                Utils1.ajaxPost(Url, Model,
                    (data) => {
                        mostrarModalUpdate();
                    })
            }
        }
    }
};