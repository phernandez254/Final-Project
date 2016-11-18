using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HITEF.Models
{
    public class ContactoFormViewModel
    {
        [Required]
        [Display(Name = "Nombre:")]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Telefono:")]
        public string Telefono { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Correo Electronico:")]
        public string CorreoElectronico { get; set; }

        [Required]
        [Display(Name = "Mensaje:")]
        public string Mensaje { get; set; }
    }
}