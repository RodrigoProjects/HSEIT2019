using CuidadosContinuados.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace CuidadosContinuados.Controllers
{
    public class HomeController : Controller
    {
        private HospitalDb db = new HospitalDb();

        [HttpPost]
        public ActionResult Login(string username, string password)
        {

            Medicos med = (Medicos) db.meds.Where(c => c.Name.Equals(username) && c.Password.Equals(password)).SingleOrDefault();
            Enfermeiros enf = (Enfermeiros)db.enfs.Where(c => c.Name.Equals(username) && c.Password.Equals(password)).SingleOrDefault();
            AssistentesSociais asist = (AssistentesSociais) db.assis.Where(c => c.Name.Equals(username) && c.Password.Equals(password)).SingleOrDefault();

            if(med != null)
            {
                var token = Crypto.HashPassword(med.Password);
                db.tokens.Add(new Tokens { Role = "Médico", UserId = med.Id, Token= token, crt= DateTime.Now});
                db.SaveChanges();
                Session["role"] = token;
                return RedirectToAction("Index", "Medicos");

            }

            else if(enf != null)
            {
                var token = Crypto.HashPassword(enf.Password);
                db.tokens.Add(new Tokens { Role = "Enfermeiro", UserId = enf.Id, Token = token, crt = DateTime.Now });
                db.SaveChanges();
                Session["role"] = token;
                return RedirectToAction("Index", "Enfermeiros");

            } else if(asist != null)
            {
                var token = Crypto.HashPassword(asist.Password);
                db.tokens.Add(new Tokens { Role = "Assistente", UserId = asist.Id, Token = token, crt = DateTime.Now });
                db.SaveChanges();
                Session["role"] = token;
                return RedirectToAction("Index", "Assistente");

            }
            
            return View();
        }

        public ActionResult Login()
        {

            return View();
        }
    }
}