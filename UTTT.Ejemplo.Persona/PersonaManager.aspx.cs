#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UTTT.Ejemplo.Linq.Data.Entity;
using System.Data.Linq;
using System.Linq.Expressions;
using System.Collections;
using UTTT.Ejemplo.Persona.Control;
using UTTT.Ejemplo.Persona.Control.Ctrl;
using System.Net.Configuration;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.Web.UI.MobileControls;

#endregion

namespace UTTT.Ejemplo.Persona
{
    public partial class PersonaManager : System.Web.UI.Page
    {
        #region Variables

        private SessionManager session = new SessionManager();
        private int idPersona = 0;
        private UTTT.Ejemplo.Linq.Data.Entity.Persona baseEntity;
        private DataContext dcGlobal = new DcGeneralDataContext();
        private int tipoAccion = 0;

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.Response.Buffer = true;
                this.session = (SessionManager)this.Session["SessionManager"];
                this.idPersona = this.session.Parametros["idPersona"] != null ?
                    int.Parse(this.session.Parametros["idPersona"].ToString()) : 0;
                if (this.idPersona == 0)
                {
                    this.baseEntity = new Linq.Data.Entity.Persona();
                    this.tipoAccion = 1;
                }
                else
                {
                    this.baseEntity = dcGlobal.GetTable<Linq.Data.Entity.Persona>().First(c => c.id == this.idPersona);
                    this.tipoAccion = 2;
                }

                if (!this.IsPostBack)
                {
                    if (this.session.Parametros["baseEntity"] == null)
                    {
                        this.session.Parametros.Add("baseEntity", this.baseEntity);
                    }
                    List<CatSexo> lista = dcGlobal.GetTable<CatSexo>().ToList();
                    CatSexo catTemp = new CatSexo();
                    catTemp.id = -1;
                    catTemp.strValor = "Seleccionar";
                    lista.Insert(0, catTemp);
                    this.ddlSexo.DataTextField = "strValor";
                    this.ddlSexo.DataValueField = "id";
                    this.ddlSexo.DataSource = lista;
                    this.ddlSexo.DataBind();

                    this.ddlSexo.SelectedIndexChanged += new EventHandler(ddlSexo_SelectedIndexChanged);
                    this.ddlSexo.AutoPostBack = true;
                    if (this.idPersona == 0)
                    {
                        this.lblAccion.Text = "Agregar";
                        DateTime tiempo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        this.FechaNaci.TodaysDate = tiempo;
                        this.FechaNaci.SelectedDate = tiempo;
                    }
                    else
                    {
                        this.lblAccion.Text = "Editar";
                        this.txtNombre.Text = this.baseEntity.strNombre;
                        this.txtAPaterno.Text = this.baseEntity.strAPaterno;
                        this.txtAMaterno.Text = this.baseEntity.strAMaterno;
                        this.txtClaveUnica.Text = this.baseEntity.strClaveUnica;

                        //fecha de nacimiento 
                        DateTime? fechaNacimiento = this.baseEntity.dteFechaNacimiento;
                        this.txtHermanos.Text = this.baseEntity.intNumHermanos.ToString();
                        //this.setItem(ref this.ddlSexo, baseEntity.CatSexo.strValor);
                        this.txtCorreo.Text = this.baseEntity.strCorreo;
                        this.txtCodigoP.Text = this.baseEntity.strCodigoP;
                        this.txtRfc.Text = this.baseEntity.strRfc;
                        this.setItem(ref this.ddlSexo, baseEntity.CatSexo.strValor);

                        //pase como 1 hora en solucionar este asunto 
                        ddlSexo.Items.FindByValue("-1").Enabled = false;
                        int valer = baseEntity.CatSexo.id;
                        if (valer==1)
                        {
                            ddlSexo.Items.FindByValue("2").Enabled = true;
                        }else
                        {
                            ddlSexo.Items.FindByValue("1").Enabled = true; 
                        }
                    }                
                }

            }
            catch (Exception _e)
            {
                this.showMessage("Ha ocurrido un problema al cargar la página");
                this.Response.Redirect("~/PersonaPrincipal.aspx", false);
            }

        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                DataContext dcGuardar = new DcGeneralDataContext();
                UTTT.Ejemplo.Linq.Data.Entity.Persona persona = new Linq.Data.Entity.Persona();
                if (this.idPersona == 0)
                {
                    persona.strClaveUnica = this.txtClaveUnica.Text.Trim();
                    persona.strNombre = this.txtNombre.Text.Trim();
                    persona.strAMaterno = this.txtAMaterno.Text.Trim();
                    persona.strAPaterno = this.txtAPaterno.Text.Trim();
                    persona.idCatSexo = int.Parse(this.ddlSexo.Text);
                    DateTime fechaNacimiento = this.FechaNaci.SelectedDate.Date;
                    persona.dteFechaNacimiento = fechaNacimiento;
                    persona.intNumHermanos = int.Parse(this.txtHermanos.Text);

                    persona.strCorreo = this.txtCorreo.Text.Trim();
                    persona.strCodigoP = this.txtCodigoP.Text.Trim();
                    persona.strRfc = this.txtRfc.Text.Trim();

                    dcGuardar.GetTable<UTTT.Ejemplo.Linq.Data.Entity.Persona>().InsertOnSubmit(persona);
                    dcGuardar.SubmitChanges();
                    this.showMessage("El registro se agrego correctamente." +
                        ":)");
                    this.Response.Redirect("~/PersonaPrincipal.aspx", false);
                    
                }
                if (this.idPersona > 0)
                {
                    persona = dcGuardar.GetTable<UTTT.Ejemplo.Linq.Data.Entity.Persona>().First(c => c.id == idPersona);
                    persona.strClaveUnica = this.txtClaveUnica.Text.Trim();
                    persona.strNombre = this.txtNombre.Text.Trim();
                    persona.strAMaterno = this.txtAMaterno.Text.Trim();
                    persona.strAPaterno = this.txtAPaterno.Text.Trim();
                    persona.idCatSexo = int.Parse(this.ddlSexo.Text);
                    DateTime fechaNacimiento = this.FechaNaci.SelectedDate.Date;
                    persona.dteFechaNacimiento = fechaNacimiento;
                    persona.intNumHermanos = int.Parse(this.txtHermanos.Text);

                    persona.strCorreo = this.txtCorreo.Text.Trim();
                    persona.strCodigoP = this.txtCodigoP.Text.Trim();
                    persona.strRfc = this.txtRfc.Text.Trim();

                    dcGuardar.SubmitChanges();
                    this.showMessage("El registro se edito correctamente. :)");
                    this.Response.Redirect("~/PersonaPrincipal.aspx", false);

                }
            }
            catch (Exception _e)
            {
                //Manejo envió de correos electrónicos al ocurrir una excepción y se enviara los datos de la excepción
                var smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
                string strHost = smtpSection.Network.Host;
                int port = smtpSection.Network.Port;
                string strUserName = smtpSection.Network.UserName;
                string strFromPass = smtpSection.Network.Password;

              
                SmtpClient smtp = new SmtpClient(strHost, port);
                MailMessage msg = new MailMessage();

     
                string body = "<h1>El Error Es: " + _e.Message + "</h1>";
                msg.From = new MailAddress(smtpSection.From, "Correo de Errores");
                msg.To.Add(new MailAddress("18300156@uttt.edu.mx"));
                msg.Subject = "Ocurrio un error"; ;
                msg.IsBodyHtml = true;
                msg.Body = body;

                smtp.Credentials = new NetworkCredential(strUserName, strFromPass);
                smtp.EnableSsl = true;
                smtp.Send(msg);
                Response.Redirect("~/ErrorFeo.aspx", false);

            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            try
            {              
                this.Response.Redirect("~/PersonaPrincipal.aspx", false);
            }
            catch (Exception _e)
            {
                this.showMessage("Ha ocurrido un error inesperado si continual este problema por favor " +
                    "Ponerse en contacto con el departamento de sistemas");
            }
        }

        protected void ddlSexo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int idSexo = int.Parse(this.ddlSexo.Text);
                Expression<Func<CatSexo, bool>> predicateSexo = c => c.id == idSexo;
                predicateSexo.Compile();
                List<CatSexo> lista = dcGlobal.GetTable<CatSexo>().Where(predicateSexo).ToList();
                CatSexo catTemp = new CatSexo();            
                this.ddlSexo.DataTextField = "strValor";
                this.ddlSexo.DataValueField = "id";
                this.ddlSexo.DataSource = lista;
                this.ddlSexo.DataBind();
            }
            catch (Exception)
            {
                this.showMessage("Ha ocurrido un error inesperado" +
                    "Ponerse en contacto con el departamento de sistemas");
            }
        }

        #endregion

        #region Metodos

        public void setItem(ref DropDownList _control, String _value)
        {
            foreach (ListItem item in _control.Items)
            {
                if (item.Value == _value)
                {
                    item.Selected = true;
                    break;
                }
            }
            _control.Items.FindByText(_value).Selected = true;
        }

        #endregion

        protected void FechaNaci_SelectionChanged(object sender, EventArgs e)
        {
            hide.Value = FechaNaci.SelectedDate.ToString();
        }  



        //Se Realizo validaciones a nivel código c# para los campos que tenemos en PersonaManager.aspx* :)
        public bool valer(UTTT.Ejemplo.Linq.Data.Entity.Persona per, ref String mensajes)
        {
            if (per.idCatSexo == -1)
            {
                mensajes = "Selecciona ya sea masculino o femenino si requieres de otro compo favor de ponerse en contacto con el departamento de sistemas ";
                return false;
            }
            int u = 0;
            if (int.TryParse(per.strClaveUnica, out u) == false)
            {
                mensajes = "En la clave unica deben ser numeros";
                return false;
            }
            if (per.strClaveUnica.Equals(String.Empty))
            {
                mensajes = "El campo de clave unica esta vacio";
                return false;
            }//la clave es de 3 numeros asi que no puede ser 99 pero puede ser 000
            if (int.Parse(per.strClaveUnica) < 000 || int.Parse(per.strClaveUnica) > 999)
            {
                mensajes= "La clave debe tener por lo menos 3 numeros";
                return false;
            }
            if (per.strNombre.Equals(String.Empty))
            {
                mensajes = "El campo nombre no debe estar vacio";
                return false;
            }
            if (per.strNombre.Length > 50)
            {
                mensajes = "El nombre no acepta mas de 50 caracteres";
                return false;
            }
            if (per.strAPaterno.Equals(String.Empty))
            {
                mensajes = "El A Paterno esta vacio favor de completar el ingreso de datos";
                return false;
            }
            if (per.strAPaterno.Length > 50)
            {
                mensajes = "Los caracteres permitidos para Apellido Paterno rebasan lo establecido";
                return false;
            }
            if (per.strAMaterno.Equals(String.Empty))
            {
                mensajes = "El AMaterno esta vacio favor de completar el ingreso de datos";
                return false;
            }
            if (per.strAPaterno.Length > 50)
            {
                mensajes = "Los caracteres permitidos para Apellido Materno rebasan lo establecido";
                return false;
            }
            if (int.TryParse(per.intNumHermanos.ToString(), out u) == false)
            {
                mensajes = "El numero de hermanos no es numero";
                return false;
            }
            //se puede ingresar 0 ya que no puede tener hermanos asi que se permite
            if (per.intNumHermanos < 0 || per.intNumHermanos > 20)
            {
                mensajes = "Los numeros de hermanos no deben ser menores a cero";
                return false;
            }
            //con las validaciones de js y estas ya nos mostrara cuales compos se necesita correjir
            if (per.strCorreo.Equals(String.Empty))
            {
                mensajes = "Correo Electronico esta vacio";
                return false;
            }
            if (per.strCorreo.Length > 50)
            {
                mensajes = "Los Caracteres Permitidos Para Correo Electronico Rebasan Lo Establecido";
                return false;
            }
            if (per.strRfc.Equals(String.Empty))
            {
                mensajes = "El Rfc esta vacio";
                return false;
            }
            if (per.strRfc.Length > 50)
            {
                mensajes = "Los caracteres permitidos para RFC rebasan lo establecido";
                return false;
            }
            if (int.TryParse(per.strCodigoP, out u) == false)
            {
                mensajes = "El codigo postal Son Numeros: Un Ejemplo Puede Ser 42380";
                return false;
            }
            if (per.strCodigoP.Equals(String.Empty))
            {
                mensajes = "Codigo Postal esta vacio";
                return false;
            }
            if (int.Parse(per.strCodigoP) < 0000 || int.Parse(per.strCodigoP) > 99999)
            {
                mensajes = "El codigo postal debe de constar de 5 numeros";
                return false;
            }
            TimeSpan timeSpan = DateTime.Now - per.dteFechaNacimiento.Value.Date;
            if (timeSpan.Days < 6570)
            {
                mensajes = "Un menor de edad no es aceptado";
                return false;
            }
            return true;
        }


        //Se Realizo validaciones a nivel Html para los campos que tenemos en PersonaManager.aspx* :)
        private bool valerParaHtml(ref String mensa)
        {
            ControlValer valida = new ControlValer();
            string mensajeFuncion = string.Empty;
            if (valida.htmlInyectionValida(this.txtNombre.Text.Trim(), ref mensajeFuncion, "Nombre", ref this.txtNombre))
            {
                mensa = mensajeFuncion;
                return false;
            }
            if (valida.htmlInyectionValida(this.txtAPaterno.Text.Trim(), ref mensajeFuncion, "A Paterno", ref this.txtAPaterno))
            {
                mensa = mensajeFuncion;
                return false;
            }
            //checar bien la escritura pase 30min buscando el error y era una letra que me faltaba 
            if (valida.htmlInyectionValida(this.txtAMaterno.Text.Trim(), ref mensajeFuncion, "A Materno", ref this.txtAMaterno))
            {
                mensa = mensajeFuncion;
                return false;
            }
            if (valida.htmlInyectionValida(this.txtCorreo.Text.Trim(), ref mensajeFuncion, "Correo Electronico", ref this.txtCorreo))
            {
                mensa = mensajeFuncion;
                return false;
            }
            if (valida.htmlInyectionValida(this.txtRfc.Text.Trim(), ref mensajeFuncion, "RFC", ref this.txtRfc))
            {
                mensa = mensajeFuncion;
                return false;
            }
            if (valida.htmlInyectionValida(this.txtClaveUnica.Text.Trim(), ref mensajeFuncion, "Clave Unica", ref this.txtClaveUnica))
            {
                mensa = mensajeFuncion;
                return false;
            }
            if (valida.htmlInyectionValida(this.txtCodigoP.Text.Trim(), ref mensajeFuncion, "Codigo Postal", ref this.txtCodigoP))
            {
                mensa = mensajeFuncion;
                return false;
            }
            return true;
        }
        //Se Realizo validaciones a nivel código Sql para los campos que tenemos en PersonaManager.aspx* :)
        private bool valerSql(ref String mensaa)
        {
            ControlValer valida = new ControlValer();
            string mensajeFuncion = string.Empty;
            if (valida.sqlInyectionValida(this.txtNombre.Text.Trim(), ref mensajeFuncion, "Nombre", ref this.txtNombre))
            {
                mensaa = mensajeFuncion;
                return false;
            }
            if (valida.sqlInyectionValida(this.txtAPaterno.Text.Trim(), ref mensajeFuncion, "A Paterno", ref this.txtAPaterno))
            {
                mensaa = mensajeFuncion;
                return false;
            }
            if (valida.sqlInyectionValida(this.txtAMaterno.Text.Trim(), ref mensajeFuncion, "A Materno", ref this.txtAMaterno))
            {
                mensaa = mensajeFuncion;
                return false;
            }
            if (valida.sqlInyectionValida(this.txtCorreo.Text.Trim(), ref mensajeFuncion, "Correo Electronico", ref this.txtCorreo))
            {
                mensaa = mensajeFuncion;
                return false;
            }
            if (valida.sqlInyectionValida(this.txtRfc.Text.Trim(), ref mensajeFuncion, "RFC", ref this.txtRfc))
            {
                mensaa = mensajeFuncion;
                return false;
            }

            return true;
        }
    }
}

