using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Giris_Sayfasi.Models
{
    public class mdlRegister : tblKullanici
    {
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "E-Posta Tekrar Girilmeli")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Compare("Email", ErrorMessage = "The email and confirmation do not match.")]
        public string Emailtekrar { get; set; }


        [Display(Name = "Şifre Tekrar")]
        [Required(ErrorMessage = "The Sifretekrar address is required")]
        [Compare("Sifre", ErrorMessage = "The Sifre and Sifretekrar do not match.")]
        public string Sifretekrar { get; set; }

        public class tblKullanici
        {
        }
    }
}