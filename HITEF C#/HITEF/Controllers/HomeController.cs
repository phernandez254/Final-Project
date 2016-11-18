using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using HITEF.Models;

namespace HITEF.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult QuienesSomos()
        {
            ViewBag.Message = "Quienes Somos";

            return View();
        }

        public ActionResult Cursos()
        {
            ViewBag.Message = "Cursos";

            return View();
        }

        public ActionResult ServiciosEmpresariales()
        {
            ViewBag.Message = "Servicios Empresariales";

            return View();
        }

        public ActionResult SitiosDeInteres()
        {
            ViewBag.Message = "Sitios de Interes";

            return View();
        }

        public ActionResult ContactoForm()
        {
            ViewBag.Message = "Contacto";

            return View("ContactoForm");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ContactoForm(ContactoFormViewModel myModel)
        {
			string emailBody = "<html><head><title></title></head><body><div><p>Hola Pablo, una nueva solicitud ha sido creada.</p><p>Por favor, vea la siguiente tabla para mas informacion:</p><table class='table table-bordered'><tr class='row'><td>Nombre:</td><td>Telefono:</td><td>Correo Electronico:</td><td>Mensaje:</td></tr><tr class='row'><td>@myModel.Nombre</td><td>@myModel.Telefono</td><td>@myModel.CorreoElectronico</td><td>@myModel.Mensaje</td></tr></table><p>Recuerde que tambien puede acceder al sitio web como administrador<br/>para ver todas las solicitudes en un portal bien organizado.</p></div></body></html>";

			/*
			var message = await UseEmailTemplate("EmailTemplate");
            message = message.Replace("@ViewBag.Nombre", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(myModel.Nombre));
			*/
            await EmailHandler.SendEmailAsync("HITEF-Solicitud", emailBody);

			DBConnectionHandler.AddModel(myModel);

            return View("ContactoConfirmation", myModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ContactoConfirmation()
        {
            ViewBag.Message = "Confirmacion";

            return View();
        }

        public ActionResult AdminLogin()
        {
            ViewBag.Message = "Administrador";

            return View();
        }

		[HttpPost]
		[AllowAnonymous]
		public ActionResult AdminLogin(LoginFormViewModel myModel)
		{
			if (DBConnectionHandler.ValidateLogIn(myModel) == true)
			{
				return RedirectToAction("AdminPortal");
			}
			else
			{
				return RedirectToAction("AdminLogInError");
            }
		}

        public ActionResult AdminLogInError()
        {
            ViewBag.Message = "Error:";

            return View();
        }


        public ActionResult AdminPortal()
        {
            ViewBag.Message = "Portal de administrador";

			List<ContactoFormViewModel> myList = new List<ContactoFormViewModel>();

			myList = DBConnectionHandler.GetAllModels();

			return View(myList);
        }

		public ActionResult EmailTemplate(ContactoFormViewModel myModel)
		{
			ViewBag.Message = "Portal de administrador";

			return View(myModel);
		}

		public static async Task<string> UseEmailTemplate(string templateName)
        {
            var templateFilePath = HostingEnvironment.MapPath("~/Views/Home/" + templateName + ".cshtml");
            StreamReader objstreamreaderfile = new StreamReader(templateFilePath);

            var emailBody = await objstreamreaderfile.ReadToEndAsync();
            return emailBody;
        }
    }
}