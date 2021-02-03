function valid() {
    var claven = document.getElementById("txtClaveUnica").value;
    var nom = document.getElementById("txtNombre").value;
    var apaterno = document.getElementById("txtAPaterno").value;
    var amaterno = document.getElementById("txtAMaterno").value;
    var sex = document.getElementById("ddlSexo"); 
    var hermanos = document.getElementById("txtHermanos").value;
    var calendar = document.getElementById("hide");
    var sexl = sex.options[sex.selectedIndex].value;
    var fecha = calendar.defaultValue;
    var date = parseInt(("" + fecha.substr(6, 10)));
    var f = 2021 - date;
    var ban = true;
    var mensaje = "";

    // si ponemos == null no respeta la primera mensaje asi que lo deje como ==""
    if (claven == null || nom == null || apaterno == null || amaterno == null || date == null) {
        ban = false;
        mensaje = "Acceso denegado"; 

    } else if (!(/\d{3}$/.test(claven))) {
        mensaje = "Solo se admiten letras";
        ban = false;

    } else if (!(/[A-z]{3}/.test(nom)) || !(/[A-z]{3}/.test(apaterno)) || !(/[A-z]{3}/.test(amaterno))) {
        mensaje = "Solo se admiten un maximo de 3 numeros";
        ban = false;

    } else if (f <= 17) {
        mensaje = "El usuario debe ser mayor de edad";
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