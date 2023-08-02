$("#hide").click(function () {
    $("#myModal").modal("hide");
});
function ModalEliminar() {
    var modal = document.getElementById('myModal');
    $(modal).modal('show');
}
function HabilitarCampos() {
    document.getElementById("cuil").readOnly = false;
    document.getElementById("ApellidoYNombre").readOnly = false;
    document.getElementById("FechaNacimiento").readOnly = false;
    document.getElementById("idzona").disabled = false;
    document.getElementById("idzona").readOnly = false;
    document.getElementById("botonAgregar").style.display = "";

    document.getElementById('habilitar').style.display = 'none';

}

var myCarousel = document.querySelector('#car12')
var carousel = new bootstrap.Carousel(myCarousel, {
    interval:2000,
})