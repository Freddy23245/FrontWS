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
$("#hide").click(function () {
    $("#myModal").modal("hide");
});