using Giris_Sayfasi.Models;
using Giris_Sayfasi.Models.Helpers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace Giris_Sayfasi.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        Giris_SayfasiEntities2 db = new Giris_SayfasiEntities2();
        
        
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
        [HttpPost]
        public ActionResult Login(tblKullanici p)
        {
            var bilgiler = db.tblKullanici.FirstOrDefault(x => x.Email == p.Email && x.Sifre == p.Sifre && x.GecerliFlg == p.GecerliFlg && x.OlusturmaTarihi == p.OlusturmaTarihi);
            if (bilgiler != null)
            {
                FormsAuthentication.SetAuthCookie(bilgiler.Email, false);
                Session["Mail"] = bilgiler.Email.ToString();
                Session["Ad"] = bilgiler.Ad.ToString();
                Session["Soyad"] = bilgiler.Soyad.ToString();
                return RedirectToAction("Index", "Home");

            }
            else
            {
                ViewBag.Hata = "Kullanıcı Adı veya Şifre Hatalı";
            }

            return View();
        }
       
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Register(mdlRegister data)
        {
            if (ModelState.IsValid)
            {
                tblKullanici yeni =  new tblKullanici()
                {   Ad = data.Ad,
                    Email = data.Email,
                    GecerliFlg = data.GecerliFlg,
                    GuncellemeTarihi = data.GuncellemeTarihi,
                    GuncelleyenAdi = data.GuncelleyenAdi,
                    KullaniciAd = data.KullaniciAd,
                    OlusturanAdi = data.OlusturanAdi,
                    OlusturmaTarihi = data.OlusturmaTarihi,
                    Rol = data.Rol,
                    Sifre = data.Sifre,
                    Soyad = data.Sifre
                };

                yeni.Rol = "U";

                db.tblKullanici.Add(yeni);
                
                db.SaveChanges();
                return Json(new{ Status = "OK" }, JsonRequestBehavior.AllowGet);
                
            }
            else
            {
                var errors = string.Join("<br/>", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));

                return Json(new { Status = "ERROR", ErrorMessage = errors }, JsonRequestBehavior.AllowGet);
            }


        }
        public ActionResult Guncelle()
        {
            var kullanicilar = (string)Session["Mail"];
            var degerler = db.tblKullanici.FirstOrDefault(x => x.Email == kullanicilar);
            return View(degerler);
        }
        [HttpPost]
        public ActionResult Guncelle(tblKullanici data)
        {
            var kullanicilar = (string)Session["Mail"];
            var user = db.tblKullanici.Where(x => x.Email == kullanicilar).FirstOrDefault();
            user.Ad = data.Ad;
            user.Soyad = data.Soyad;
            user.Email = data.Email;
            user.KullaniciAd = data.KullaniciAd;
            user.Sifre = data.Sifre;
            db.SaveChanges();
            return RedirectToAction("Index","Home");
        }
        public ActionResult SifreReset()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SifreReset(tblKullanici eposta)
        {
            var mail= db.tblKullanici.FirstOrDefault(x => x.Email == eposta.Email);
            
            if (mail!= null)
            {
                Random rnd = new Random();
                int yenisifre = rnd.Next();
                tblKullanici sifre = new tblKullanici();
                mail.Sifre = Crypto.Hash(Convert.ToString(yenisifre), "MD5");
                db.SaveChanges();
                
                clsEPostaHelper epostaHelper = new clsEPostaHelper();
                epostaHelper.EPostaKayit("Şifre Hatırlatma", "Şifreniz:" + yenisifre, eposta.Email, eposta.Ad + " " + eposta.Soyad);
                

                /*WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.EnableSsl = true;
                WebMail.UserName = "kilicmelikbugra@gmail.com";
                WebMail.Password = "rfzggvqtfjohhxho";
                WebMail.SmtpPort = 587;

                WebMail.Send(eposta.Email, "Giriş şifreniz", "Şifreniz:" + yenisifre);*/

                ViewBag.uyari = "Şifreniz Başarıyla Gönderilmiştir";
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.uyari = "Hata Oluştu Tekrar Deneyiniz";
            }
            return View();
        }
        
    }
}