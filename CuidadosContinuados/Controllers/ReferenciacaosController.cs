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
    public class ReferenciacaosController : Controller
    {
        private HospitalDb db = new HospitalDb();

        // GET: Referenciacaos
        public ActionResult Index()
        {
            return View(db.Dados.ToList());
        }

        // GET: Referenciacaos/Details/5
        public ActionResult Details(int? id)
        {
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

        // GET: Referenciacaos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Referenciacaos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UtenteId,Cuidador,EntidadeReferenciadora,DiagnosticoClinico,DataDeAlta,CriteriosDeTriagem,DependenciaAVD,Desnutricao,Deteorioracao,ProblemasSensoriais,DCronicas,NCCD,NTC,CP,NE,AND,ECE,CuidadorDetalhes,IRS,Criacao")] Referenciacao referenciacao)
        {
            if (ModelState.IsValid)
            {
                db.Dados.Add(referenciacao);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(referenciacao);
        }

        // GET: Referenciacaos/Edit/5
        public ActionResult Edit(int? id)
        {
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
        public ActionResult Edit([Bind(Include = "Id,UtenteId,Cuidador,EntidadeReferenciadora,DiagnosticoClinico,DataDeAlta,CriteriosDeTriagem,DependenciaAVD,Desnutricao,Deteorioracao,ProblemasSensoriais,DCronicas,NCCD,NTC,CP,NE,AND,ECE,CuidadorDetalhes,IRS,Criacao,Enfermeiro,MedicoOk,EnfermeiroOk,AssistOk")] Referenciacao referenciacao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(referenciacao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(referenciacao);
        }

        // GET: Referenciacaos/Delete/5
        public ActionResult Delete(int? id)
        {
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

        // POST: Referenciacaos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Referenciacao referenciacao = db.Dados.Find(id);
            db.Dados.Remove(referenciacao);
            db.SaveChanges();
            return RedirectToAction("Index");
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
