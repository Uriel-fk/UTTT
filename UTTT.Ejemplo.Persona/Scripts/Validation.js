function valid() {
    debugger;
    var claven = document.getElementById("txtClaveUnica").value;
    debugger;
    var nom = document.getElementById("txtNombre").value;
    debugger;
    var apaterno = document.getElementById("txtAPaterno").value;
    debugger;
    var amaterno = document.getElementById("txtAMaterno").value;
    debugger;
    var sex = document.getElementById("ddlSexo");
    debugger;
    var hermanos = document.getElementById("txtHermanos").value;
    debugger;
    var calendar = document.getElementById("hide");
    debugger;
    var sexl = sex.options[sex.selectedIndex].value;
    debugger;
    var fecha = calendar.defaultValue;
    debugger;
    var date = parseInt(("" + fecha.substr(6, 10)));
    debugger;
    var f = 2021 - date;
    debugger;
    var ban = true;
    debugger;
    var mensaje = "";
    debugger;
    if (claven == null || nom == null || apaterno == null || amaterno == null || date == null) {
        ban = false;
        mensaje = "Acceso denegado";
    } else if (!(/\d{3}$/.test(claven))) {
        mensaje = "Solo se admiten un maximo de 3 numeros";
        ban = false;

    } else if (!(/[A-z]{3}/.test(nom)) || !(/[A-z]{3}/.test(apaterno)) || !(/[A-z]{3}/.test(amaterno))) {
        mensaje = "Solo se admiten un maximo de 3 numeros";
        ban = false;

    } else if (f <= 17) {
        mensaje = "Debe ser mayor de edad";
        ban = false;

    } else if (isNaN(hermanos) == true) {
        mensaje = "Solo debe ingresar numeros";
        ban = false;


    } else if (sex1 < 0 || sexl > 2) {
        mensajes.push('Ingresa el setxo');
        ban = false;
    }
    alert(mensaje);
    return ban;
}
