using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HITEF.Models
{
    public class LoginFormViewModel
    {
        [Required]
        [Display(Name = "Usuario:")]
        public string Usuario { get; set; }

        [Required]
        [Display(Name = "Contraseña:")]
        public string Contraseña { get; set; }
    }
}