using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;

namespace Giris_Sayfasi.Models.Jobs
{
    public class jobEPosta : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Helpers.clsEPostaHelper ePostaHelper = new Helpers.clsEPostaHelper();
            Giris_SayfasiEntities2 db = new Giris_SayfasiEntities2();
            foreach (tblMail mail in db.tblMail.Where(x => x.DurumFlg == 0).ToList())
            {
                string hataMesaji = null;

                bool sonuc = ePostaHelper.EPostaGonder(mail, ref hataMesaji);

                if (!sonuc)
                {
                    mail.DurumFlg = 2;
                    mail.HataMesaji = hataMesaji;
                }
                else
                {
                    mail.GondermeTarihi = DateTime.Now;
                    mail.DurumFlg = 1;
                }

                db.SaveChanges();
            }

            return null;
        }

    }

}