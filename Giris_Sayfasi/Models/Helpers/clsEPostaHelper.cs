using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using Giris_Sayfasi.Models;

namespace Giris_Sayfasi.Models.Helpers
{
    public class clsEPostaHelper
    {
        public void EPostaKayit(string Konu, string Govde, string Alici, string Kullanici)
        {
            tblMail mail = new tblMail()
            {
                Alici = Alici,
                Govde = Govde,
                Konu = Konu,
                Creuser = string.IsNullOrWhiteSpace(Kullanici) ? "Sistem" : Kullanici,
                GondermeTarihi=DateTime.Now,
                GecerliFlg = true,
                DurumFlg = 0,
                Credate = DateTime.Now
            };

            using (Giris_SayfasiEntities2 db = new Giris_SayfasiEntities2())
            {
                db.tblMail.Add(mail);
                db.SaveChanges();
            }
        }

        public bool EPostaGonder(tblMail mail, ref string HataMesaji)
        {
            try
            {
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
                client.Credentials = new NetworkCredential("kilicmelikbugra@gmail.com", "xbvufnnpgdqlftiz");
                client.EnableSsl = true;
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                message.To.Add(mail.Alici);
                message.Subject = mail.Konu;
                message.Body = mail.Govde;
                message.IsBodyHtml = true;
                message.From = new System.Net.Mail.MailAddress("kilicmelikbugra@gmail.com");
                client.Send(message);

                /*WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.EnableSsl = true;
                WebMail.UserName = "kilicmelikbugra@gmail.com";
                WebMail.Password = "rfzggvqtfjohhxho";
                WebMail.SmtpPort = 587;

                WebMail.Send(mail.Alici, mail.Konu, mail.Govde);*/

                return true;
            }
            catch (Exception ex)
            {
                HataMesaji = ex.Message;
                return false;
            }
        }
    }
}