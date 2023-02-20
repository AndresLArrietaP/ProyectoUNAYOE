using ProyectoBiblioteca.Logica;
using ProyectoBiblioteca.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entidades;
using AccesoDatos;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using ProyectoUNAYOE.Logica;

namespace ProyectoUNAYOE.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        /*public ActionResult Index(string correo, string clave)
        {

            Persona oUsuario = PersonaLogica.Instancia.Listar().Where(u => u.Correo == correo && u.Clave == clave && u.oTipoPersona.IdTipoPersona != 3).FirstOrDefault();

            if (oUsuario == null)
            {
                ViewBag.Error = "Usuario o contraseña no correcta";
                return View();
            }

            Session["Usuario"] = oUsuario;

            return RedirectToAction("Index", "Admin");
        }*/
        public ActionResult Index(Persona oUsuario) 
        {
            using (SqlConnection oConexion = new SqlConnection(ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["cnnSql"]].ConnectionString)) 
            {
                SqlCommand cmd = new SqlCommand("sp_ValidarUsuario", oConexion);
                cmd.Parameters.AddWithValue("Correo", oUsuario.Correo);
                cmd.Parameters.AddWithValue("Clave", oUsuario.Clave);
                cmd.CommandType = CommandType.StoredProcedure;

                oConexion.Open();
                oUsuario.IdPersona = Convert.ToInt32(cmd.ExecuteScalar().ToString());
            }
            if (oUsuario.IdPersona != 0)
            {
                Session["Usuario"] = oUsuario;
                return RedirectToAction("Index", "Admin");
            }
            else 
            {
                ViewBag.Error = "Usuario o contraseña no correcta";
                return View();
            }
        }

    }
}