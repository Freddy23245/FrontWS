function mostrarModal() {
    var modal = document.getElementById('myModal');
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
const Utils = {
    ajaxPost: (Url, Model, callback, callbackError) => {
        $.post(Url, Model)
            .done(callback)
            .fail(callbackError);
    },
};
const create = {
    init:()=> {
        create.componentes ={
            Cuil: $("#cuil"),
            ApellidoYNombre: $("#ApellidoYNombre"),
            FechaNacimiento: $("#FechaNacimiento"),
            idzona: $("#idzona"),
            UrlPostPersonas: $("#UrlPostPersonas"),
            botonAgregar: $("#botonAgregar")
        };
        create.componentes.botonAgregar.on('click',create.acciones.botonClick)
    },
    componentes: {
        Cuil: undefined,
        ApellidoYNombre: undefined,
        FechaNacimiento: undefined,
        idzona: undefined,
        UrlPostPersonas: undefined,
        botonAgregar:undefined
    },
    acciones: {
        botonClick: () => {
            if (ValidarCampos()) {
                const Url = create.componentes.UrlPostPersonas.val();

                const Model = {
                    nuevo: {
                        Cuil: create.componentes.Cuil.val(),
                        ApellidoYNombre: create.componentes.ApellidoYNombre.val(),
                        FechaNacimiento: create.componentes.FechaNacimiento.val(),
                        idzona: create.componentes.idzona.val(),

                    }

                };
                console.log(Model);
                Utils.ajaxPost(Url, Model,
                    (data) => {
                        mostrarModal();
                    })
            }
          
        }
    }
};