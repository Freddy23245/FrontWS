const Utils01 = {
    ajaxPost: (Url, model, callback, callbackError) => {
        $.post(Url, model)
            .done(callback)
            .fail(callbackError);
    },
};
const buscar = {

    init:()=> {
        buscar.componentes = {
            ApellidoYNombre: $("#ApellidoYNombre"),
            botonBuscar: $("#botonBuscar"),
            UrlPostBuscarPersonas: $("#UrlPostBuscarPersonas")
        }
        buscar.componentes.botonBuscar.on('click', buscar.acciones.botonClick)
    },
    componentes: {
        ApellidoYNombre: undefined,
        botonBuscar: undefined,
        UrlPostBuscarPersonas:undefined
    },
    acciones: {
        botonClick: () => {
            const url = buscar.componentes.UrlPostBuscarPersonas.val();

            const Model = {
                ApellidoYNombre: buscar.componentes.ApellidoYNombre.val()
            };

            Utils01.ajaxPost(url, Model, (data) => {
                const Resultados = $('#resultados');
                Resultados.empty();
        
                for (let i = 0; i < data.length; i++) {

                    const Personas = data[i];
                    var fecha = new Date(Personas.fechaNacimiento);
                    var año = fecha.getFullYear();
                    var mes = fecha.getMonth() +1;
                    var dia = fecha.getDate();
                    if (mes < 10) {
                        var fechaFormateada = `${dia}/0${mes}/${año} `;
                    } else {
                        var fechaFormateada = `${dia}/${mes}/${año} `;
                    }

                    Resultados.append('<tr><td>' + Personas.cuil + '</td><td>' + Personas.apellidoYNombre + ' </td><td>' + fechaFormateada + ' </td><td>' + Personas.zona + ' </td><td> <a href="/Home/Detalle/'+ Personas.id +'" class="btn btn-primary">Ver detalle</a></td></tr>');
              
                }
                console.log(data);
            })
        }
    }
}