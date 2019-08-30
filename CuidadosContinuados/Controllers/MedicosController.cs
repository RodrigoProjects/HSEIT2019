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
    public class MedicosController : Controller
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
            if (token == null || token.Role != "Médico" ) return RedirectToAction("Login", "Home");

            int id = token.UserId;
     

            if(SearchString != null || SearchString == "")
            {      
                List<Utente> lista = db.uts.ToList().Where(c => c.Name.Contains(SearchString) || c.HSE.Contains(SearchString)).ToList();
                MedicosViewPage dados1 = new MedicosViewPage { utentes = lista, refeDone = db.Dados.ToList().Where(c => {
                    for(int i = 0; i<lista.Count; i++)
                    {
                        if (lista[i].Id == c.UtenteId && c.MedicoOk) return true;
                    }
                    return false;
                }).ToList().Take(20).ToList(),
                    refeNot = db.Dados.ToList().Where(c => {
                        for (int i = 0; i < lista.Count; i++)
                        {
                            if (lista[i].Id == c.UtenteId && c.MedicoOk == false) return true;
                        }
                        return false;
                    }).ToList().Take(20).ToList(),
                    enfe = db.enfs.ToList(),
                    meds = db.meds.ToList()
                };
                
                return View(dados1);
            }
            MedicosViewPage dados2 = new MedicosViewPage { utentes = db.uts.ToList(), refeDone = db.Dados.ToList().Where(c => c.MedicoOk).Take(20).ToList(), refeNot = db.Dados.ToList().Where(c => c.MedicoOk == false).Take(20).ToList(), enfe = db.enfs.ToList(), meds = db.meds.ToList()};
            return View(dados2);
        }

        public ActionResult LogOut()
        {
            Session["role"] = null;
            return RedirectToAction("Login", "Home");
        }

        // GET: Referenciacaos/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["role"] == null) return RedirectToAction("Login", "Home");

            Tokens token = (Tokens)db.tokens.ToList().Where(c => c.Token.Equals((String)Session["role"])).SingleOrDefault();
            if (token == null || token.Role != "Médico") return RedirectToAction("Login", "Home");

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
            if (token == null || token.Role != "Médico") return RedirectToAction("Login", "Home");


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
        public ActionResult Edit([Bind(Include = "Id,Cuidador,EntidadeReferenciadora,DiagnosticoClinico,DataDeAlta,CriteriosDeTriagem,DependenciaAVD,Desnutricao,Deteorioracao,ProblemasSensoriais,DCronicas,NCCD,NTC,CP")] Referenciacao referenciacao, int Id)
        {
            if (Session["role"] == null) return RedirectToAction("Login", "Home");

            Tokens token = (Tokens)db.tokens.ToList().Where(c => c.Token.Equals((String)Session["role"])).SingleOrDefault();
            if (token == null || token.Role != "Médico") return RedirectToAction("Login", "Home");


            if (ModelState.IsValid)
            {
                var refe = db.Dados.Find(referenciacao.Id);
                if (refe != null)
                {
                    refe.EntidadeReferenciadora = referenciacao.EntidadeReferenciadora;
                    refe.DiagnosticoClinico = referenciacao.DiagnosticoClinico;
                    refe.DataDeAlta = referenciacao.DataDeAlta;
                    refe.CriteriosDeTriagem = referenciacao.CriteriosDeTriagem;
                    refe.DependenciaAVD = referenciacao.DependenciaAVD;
                    refe.Desnutricao = referenciacao.Desnutricao;
                    refe.Deteorioracao = referenciacao.Deteorioracao;
                    refe.ProblemasSensoriais = referenciacao.ProblemasSensoriais;
                    refe.DCronicas = referenciacao.DCronicas;
                    refe.NCCD = referenciacao.NCCD;
                    refe.NTC = referenciacao.NTC;
                    refe.CP = referenciacao.CP;
                    refe.MedicoOk = true;
                    refe.Medico = db.tokens.ToList().Where(c => c.Token.Equals(Session["role"].ToString())).SingleOrDefault().UserId;

                    db.Entry(refe).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                
            }
            return View(referenciacao);
        }

        // GET: Referenciacaos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["role"] == null) return RedirectToAction("Login", "Home");

            Tokens token = (Tokens)db.tokens.ToList().Where(c => c.Token.Equals((String)Session["role"])).SingleOrDefault();
            if (token == null || token.Role != "Médico") return RedirectToAction("Login", "Home");


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Referenciacao referenciacao = db.Dados.Find(id);
            if (referenciacao == null)
            {
                return HttpNotFound();
            }
            DeleteModel viewTemplate = new DeleteModel { refe = referenciacao, utente = db.uts.Where(c => c.Id == referenciacao.UtenteId).SingleOrDefault()};
            return View(viewTemplate);
        }

        // POST: Referenciacaos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["role"] == null) return RedirectToAction("Login", "Home");

            Tokens token = (Tokens)db.tokens.ToList().Where(c => c.Token.Equals((String)Session["role"])).SingleOrDefault();
            if (token == null || token.Role != "Médico") return RedirectToAction("Login", "Home");


            Referenciacao referenciacao = db.Dados.Find(id);
            db.Dados.Remove(referenciacao);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Create(string SearchString = null)
        {
            if (Session["role"] == null) return RedirectToAction("Login", "Home");

            Tokens token = (Tokens)db.tokens.ToList().Where(c => c.Token.Equals((String)Session["role"])).SingleOrDefault();
            if (token == null || token.Role != "Médico") return RedirectToAction("Login", "Home");


            var refes = db.Dados.ToList();
            var lista = db.uts.ToList().Where(c => {
                for (int i = 0; i < refes.Count; i++)
                {
                    if (refes[i].UtenteId == c.Id) return false;
                }
                return true;
            }).ToList();

            if (SearchString != null)
            {
                lista = lista.Where(c => c.Name.Contains(SearchString) || c.HSE.Contains(SearchString)).Take(20).ToList();
                return View(lista);
            }

            return View(lista.Take(20));
        }

        // POST: Referenciacaos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(int Id)
        {

            if (Session["role"] == null) return RedirectToAction("Login", "Home");

            Tokens token = (Tokens)db.tokens.ToList().Where(c => c.Token.Equals((String)Session["role"])).SingleOrDefault();
            if (token == null || token.Role != "Médico") return RedirectToAction("Login", "Home");

            Referenciacao referenciacao = new Referenciacao { UtenteId = Id };
            referenciacao.Cuidador = new Cuidador { Name = "", EstadoCivil = "", GrauDeParentesco = "", Morada = "", Nascimento = new DateTime(), NumeroTelefone = "" };
            referenciacao.DCronicas = new DCER();
            referenciacao.NCCD = new NCCD();
            referenciacao.NTC = new NTC();
            referenciacao.NE = new NE();
            referenciacao.IRS = new IRS();
            referenciacao.CP = new CP();
            referenciacao.ECE = new ECE();
            referenciacao.CuidadorDetalhes = new CuidadorDetalhes();
            referenciacao.Criacao = new DateTime();
            referenciacao.DataDeAlta = new DateTime();


            db.Dados.Add(referenciacao);
            db.SaveChanges();
            return RedirectToAction("CreateEnf", new { Id = referenciacao.Id });



        }

        public ActionResult CreateEnf(int? Id, string SearchString)
        {
            if (Session["role"] == null) return RedirectToAction("Login", "Home");

            Tokens token = (Tokens)db.tokens.ToList().Where(c => c.Token.Equals((String)Session["role"])).SingleOrDefault();
            if (token == null || token.Role != "Médico") return RedirectToAction("Login", "Home");


            ViewBag.Message = Id;
            if (SearchString != null)
            {
                var lista = db.enfs.Where(c => c.Name.Contains(SearchString)).Take(20).ToList();
                return View(lista);
            }

            return View(db.enfs.ToList().Take(20));
        }

        [HttpPost]
        public ActionResult CreateEnf(int Id, int EnfId)
        {

            if (Session["role"] == null) return RedirectToAction("Login", "Home");

            Tokens token = (Tokens)db.tokens.ToList().Where(c => c.Token.Equals((String)Session["role"])).SingleOrDefault();
            if (token == null || token.Role != "Médico") return RedirectToAction("Login", "Home");


            var refe = db.Dados.Find(Id);
            if (refe != null)
            {
                refe.Enfermeiro = EnfId;

                refe.Medico = token.UserId;
                db.Entry(refe).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", new { Id = refe.Id });
            }
            return View();

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
