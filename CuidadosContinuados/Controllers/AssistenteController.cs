using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CuidadosContinuados.Models;

namespace CuidadosContinuados.Controllers
{
    public class AssistenteController : Controller
    {
        private HospitalDb db = new HospitalDb();


        [HttpGet]
        public ActionResult Index(string SearchString = null)
        {


            foreach (var d in db.tokens)
            {
                DateTime aux = d.crt;
                if (aux.AddHours(2).CompareTo(DateTime.Now) < 0) db.tokens.Remove(d);
            };

            db.SaveChanges();

            if (Session["role"] == null) return RedirectToAction("Login", "Home");

            Tokens token = (Tokens) db.tokens.ToList().Where(c => c.Token.Equals((String)Session["role"])).SingleOrDefault();
            if (token == null || token.Role != "Assistente" ) return RedirectToAction("Login", "Home");

            int id = token.UserId;
     

            if(SearchString != null || SearchString == "")
            {      
                List<Utente> lista = db.uts.ToList().Where(c => c.Name.Contains(SearchString) || c.HSE.Contains(SearchString)).ToList();
                MedicosViewPage dados1 = new MedicosViewPage { utentes = lista, refeDone = db.Dados.ToList().Where(c => {
                    for(int i = 0; i<lista.Count; i++)
                    {
                        if (lista[i].Id == c.UtenteId && c.AssistOk) return true;
                    }
                    return false;
                }).ToList().Take(20).ToList(),
                    refeNot = db.Dados.ToList().Where(c => {
                        for (int i = 0; i < lista.Count; i++)
                        {
                            if (lista[i].Id == c.UtenteId && c.AssistOk == false) return true;
                        }
                        return false;
                    }).ToList().Take(20).ToList(),
                    enfe = db.enfs.ToList(),
                    meds = db.meds.ToList()
                };
                
                return View(dados1);
            }
            MedicosViewPage dados2 = new MedicosViewPage { utentes = db.uts.ToList(), refeDone = db.Dados.ToList().Where(c => c.AssistOk).Take(20).ToList(), refeNot = db.Dados.ToList().Where(c => c.AssistOk == false).Take(20).ToList(), enfe = db.enfs.ToList(), meds = db.meds.ToList()};
            return View(dados2);
        }

        // GET: Referenciacaos/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["role"] == null) return RedirectToAction("Login", "Home");

            Tokens token = (Tokens)db.tokens.ToList().Where(c => c.Token.Equals((String)Session["role"])).SingleOrDefault();
            if (token == null || token.Role != "Assistente") return RedirectToAction("Login", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Referenciacao referenciacao = db.Dados.Find(id);
            
            if (referenciacao == null)
            {
                return HttpNotFound();
            }
            Utente utente = db.uts.Find(referenciacao.UtenteId);
            return View(new DetailsModel { refe = referenciacao, ut = utente });
        }

        

        // GET: Referenciacaos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["role"] == null) return RedirectToAction("Login", "Home");

            Tokens token = (Tokens)db.tokens.ToList().Where(c => c.Token.Equals((String)Session["role"])).SingleOrDefault();
            if (token == null || token.Role != "Assistente") return RedirectToAction("Login", "Home");


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Referenciacao referenciacao = db.Dados.Find(id);
            if (referenciacao == null)
            {
                return HttpNotFound();
            }
            return View(referenciacao);
        }

        // POST: Referenciacaos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Cuidador,CuidadorDetalhes,IRS")] Referenciacao referenciacao, int Id)
        {
            if (Session["role"] == null) return RedirectToAction("Login", "Home");

            Tokens token = (Tokens)db.tokens.ToList().Where(c => c.Token.Equals((String)Session["role"])).SingleOrDefault();
            if (token == null || token.Role != "Assistente") return RedirectToAction("Login", "Home");


            if (ModelState.IsValid)
            {
                var refe = db.Dados.Find(referenciacao.Id);
                if (refe != null)
                {
                    refe.Cuidador = referenciacao.Cuidador;
                    refe.IRS = referenciacao.IRS;
                    refe.CuidadorDetalhes = referenciacao.CuidadorDetalhes;
                    refe.AssistOk = true;

                    db.Entry(refe).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                
            }
            return View(referenciacao);
        }

        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
