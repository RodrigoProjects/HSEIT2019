using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CuidadosContinuados.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace CuidadosContinuados.Controllers
{
    public class EnfermeirosController : Controller
    {
        private HospitalDb db = new HospitalDb();

        // GET: Enfermeiros
        [HttpGet]
        public ActionResult Index(string searchString = null)
        {
            foreach (var d in db.tokens)
            {
                DateTime aux = d.crt;
                if (aux.AddHours(2).CompareTo(DateTime.Now) < 0) db.tokens.Remove(d);
            };

            db.SaveChanges();

            if (Session["role"] == null) return RedirectToAction("Login", "Home");

            Tokens token = (Tokens)db.tokens.ToList().Where(c => c.Token.Equals((String)Session["role"])).SingleOrDefault();
            if (token == null || token.Role != "Enfermeiro") return RedirectToAction("Login", "Home");

            int id = token.UserId;




            if (searchString != null || searchString == "")
            {
                List<Utente> lista = db.uts.ToList().Where(c => c.Name.Contains(searchString) || c.HSE.Contains(searchString)).ToList();
                MedicosViewPage dados1 = new MedicosViewPage
                {
                    utentes = lista,
                    refeDone = db.Dados.ToList().Where(c => {
                        for (int i = 0; i < lista.Count; i++)
                        {
                            if (lista[i].Id == c.UtenteId && c.Enfermeiro == id && c.EnfermeiroOk) return true;
                        }
                        return false;
                    }).ToList().Take(20).ToList(),
                    refeNot = db.Dados.ToList().Where(c => {
                        for (int i = 0; i < lista.Count; i++)
                        {
                            if (lista[i].Id == c.UtenteId && c.Enfermeiro == id && c.EnfermeiroOk == false) return true;
                        }
                        return false;
                    }).ToList().Take(20).ToList(),
                    enfe = db.enfs.ToList(),
                    meds = db.meds.ToList()
                };

                return View(dados1);
            }

            return View(new MedicosViewPage { refeNot = db.Dados.ToList().Where(c => c.Enfermeiro == id && c.EnfermeiroOk == false).Take(20).ToList(), refeDone = db.Dados.ToList().Where(c => c.Enfermeiro == id && c.EnfermeiroOk).Take(20).ToList(), enfe = db.enfs.ToList().Where(c => c.Id == id).ToList(), utentes = db.uts.ToList(), meds = db.meds.ToList() }) ;
        }

        [HttpGet]
        public ActionResult Create(string SearchString = null)
        {
            if (Session["role"] == null) return RedirectToAction("Login", "Home");

            Tokens token = (Tokens)db.tokens.ToList().Where(c => c.Token.Equals((String)Session["role"])).SingleOrDefault();
            if (token == null || token.Role != "Enfermeiro") return RedirectToAction("Login", "Home");


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
            if (token == null || token.Role != "Enfermeiro") return RedirectToAction("Login", "Home");

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
            if (token == null || token.Role != "Enfermeiro") return RedirectToAction("Login", "Home");


            ViewBag.Message = Id;
            if (SearchString != null)
            {
                var lista = db.meds.Where(c => c.Name.Contains(SearchString)).Take(20).ToList();
                return View(lista);
            }

            return View(db.meds.ToList().Take(20));
        }

        [HttpPost]
        public ActionResult CreateEnf(int Id, int EnfId)
        {

            if (Session["role"] == null) return RedirectToAction("Login", "Home");

            Tokens token = (Tokens)db.tokens.ToList().Where(c => c.Token.Equals((String)Session["role"])).SingleOrDefault();
            if (token == null || token.Role != "Enfermeiro") return RedirectToAction("Login", "Home");


            var refe = db.Dados.Find(Id);
            if (refe != null)
            {
                refe.Medico = EnfId;
               
                refe.Enfermeiro = token.UserId;
                db.Entry(refe).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", new { Id = refe.Id });
            }
            return View();

        }

        // GET: Enfermeiros/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["role"] == null) return RedirectToAction("Login", "Home");

            Tokens token = (Tokens)db.tokens.ToList().Where(c => c.Token.Equals((String)Session["role"])).SingleOrDefault();
            if (token == null || token.Role != "Enfermeiro") return RedirectToAction("Login", "Home");

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

        // GET: Enfermeiros/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["role"] == null) return RedirectToAction("Login", "Home");

            Tokens token = (Tokens)db.tokens.ToList().Where(c => c.Token.Equals((String)Session["role"])).SingleOrDefault();
            if (token == null || token.Role != "Enfermeiro") return RedirectToAction("Login", "Home");

            if (id == null)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Referenciacao referenciacao = db.Dados.Find(id);
            if (referenciacao == null)
            {
                return HttpNotFound();
            }
            ViewBag.Escolaridade = db.uts.ToList().Where(c => c.Id == referenciacao.UtenteId).SingleOrDefault().Escolaridade;
            return View(referenciacao);
        }

        // POST: Enfermeiros/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,NE,AND,ECE")] Referenciacao referenciacao, FormCollection forms)
        {
            if (Session["role"] == null) return RedirectToAction("Login", "Home");

            Tokens token = (Tokens)db.tokens.ToList().Where(c => c.Token.Equals((String)Session["role"])).SingleOrDefault();
            if (token == null || token.Role != "Enfermeiro") return RedirectToAction("Login", "Home");

            if(forms["Escolaridade"].Replace(" ", string.Empty) == "")
            {
                ViewBag.Message = "Por favor insira a Escolaridade do Utente.";
                return View(referenciacao);
            }

            if (ModelState.IsValid)
            {

                var toSave = db.Dados.Find(referenciacao.Id);
                db.uts.ToList().Where(c => c.Id == toSave.UtenteId).SingleOrDefault().Escolaridade = forms["Escolaridade"].ToString();
                toSave.NE = referenciacao.NE;
                toSave.AND = referenciacao.AND;
                toSave.ECE = referenciacao.ECE;
                toSave.EnfermeiroOk = true;
                db.Entry(toSave).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            
            return View(referenciacao);
        }

        public ActionResult EditMed(int? id)
        {
            if (Session["role"] == null) return RedirectToAction("Login", "Home");

            Tokens token = (Tokens)db.tokens.ToList().Where(c => c.Token.Equals((String)Session["role"])).SingleOrDefault();
            if (token == null || token.Role != "Enfermeiro") return RedirectToAction("Login", "Home");


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
        public ActionResult EditMed([Bind(Include = "Id,Cuidador,EntidadeReferenciadora,DiagnosticoClinico,DataDeAlta,CriteriosDeTriagem,DependenciaAVD,Desnutricao,Deteorioracao,ProblemasSensoriais,DCronicas,NCCD,NTC,CP")] Referenciacao referenciacao, int Id)
        {
            if (Session["role"] == null) return RedirectToAction("Login", "Home");

            Tokens token = (Tokens)db.tokens.ToList().Where(c => c.Token.Equals((String)Session["role"])).SingleOrDefault();
            if (token == null || token.Role != "Enfermeiro") return RedirectToAction("Login", "Home");


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
                    

                    db.Entry(refe).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            return View(referenciacao);
        }

        public ActionResult EditAssist(int? id)
        {
            if (Session["role"] == null) return RedirectToAction("Login", "Home");

            Tokens token = (Tokens)db.tokens.ToList().Where(c => c.Token.Equals((String)Session["role"])).SingleOrDefault();
            if (token == null || token.Role != "Enfermeiro") return RedirectToAction("Login", "Home");


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
        public ActionResult EditAssist([Bind(Include = "Id,Cuidador,CuidadorDetalhes,IRS")] Referenciacao referenciacao, int Id)
        {
            if (Session["role"] == null) return RedirectToAction("Login", "Home");

            Tokens token = (Tokens)db.tokens.ToList().Where(c => c.Token.Equals((String)Session["role"])).SingleOrDefault();
            if (token == null || token.Role != "Enfermeiro") return RedirectToAction("Login", "Home");


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

        public ActionResult ImprimePDF(int id)
        {

            Byte[] bytes = GerarPDF(id);


            return new FileContentResult(bytes, "application/pdf");


        }

        private Byte[] GerarPDF(int id)
        {
            Referenciacao referenciacao = db.Dados.Find(id);
            Utente utente = db.uts.Where(x => x.Id == referenciacao.UtenteId).FirstOrDefault();

            Byte[] bytes;
            using (var ms = new MemoryStream())
            {
                using (var pdfDoc = new Document(PageSize.A4, 25, 25, 25, 50))
                {
                    using (var pdfWriter = PdfWriter.GetInstance(pdfDoc, ms))
                    {
                        pdfDoc.Open();

                        try
                        {
                            pdfDoc.Open();

                            String FONT_CHECKBOX = "c:/windows/fonts/WINGDING.TTF";
                            string Ischecked = "\u00fe";
                            string NotChecked = "o";

                            BaseFont bf = BaseFont.CreateFont(FONT_CHECKBOX, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                            Font f = new Font(bf, 8);

                            Font textoColuna = FontFactory.GetFont("Arial", 9, Font.NORMAL, BaseColor.BLACK);
                            Font textoOutras = FontFactory.GetFont("Arial", 7, Font.ITALIC, BaseColor.BLACK);
                            Font textoCabecalho = FontFactory.GetFont("Arial", 9, Font.BOLD, BaseColor.BLACK);

                            PdfPCell cell = new PdfPCell();
                            PdfPTable table = new PdfPTable(1);
                            Paragraph p = new Paragraph();
                            p.Add(new Chunk("\n"));
                            string imagepath = Server.MapPath("~/Static");
                            Image jpg = Image.GetInstance(imagepath + "/RRCCI.jpg");
                            //jpg.ScalePercent(80f);
                            jpg.ScaleAbsolute(60, 60);
                            p.Add(new Chunk(jpg, 0, -50));
                            imagepath = Server.MapPath("~/Static");
                            Image png = Image.GetInstance(imagepath + "/barra_azul.png");
                            png.ScaleAbsolute(10, 70);
                            p.Add(new Chunk(png, 0, -50));
                            p.Add(new Chunk("Estrutura de Missão - Rede Regional de Cuidados Continuados \n\n                     Integrados\n\n\n\n", FontFactory.GetFont("Arial", 12, Font.NORMAL, new BaseColor(102, 178, 255).Darker())));
                            cell = new PdfPCell(p);
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cell.Border = 0;
                            table.AddCell(cell);
                            pdfDoc.Add(table);

                            /*
                            PdfPCell cell = new PdfPCell();
                            PdfPTable table = new PdfPTable(1);                   
                            Paragraph titulo = new Paragraph();
                            string imagepath = Server.MapPath("~/Images");
                            Image jpg = Image.GetInstance(imagepath + "/RRCCI.jpg");
                            jpg.ScalePercent(20f);
                            titulo.Add(new Chunk(jpg, 0, 0));
                            Chunk chunk = new Chunk("Estrutura de Missão - Rede Regional de Cuidados Continuados \nIntegrados", FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLUE));
                            titulo.Add(chunk);
                            //titulo.Alignment = Element.ALIGN_LEFT;
                            cell = new PdfPCell(titulo);
                            table.AddCell(cell);
                            pdfDoc.Add(table);*/

                            Chunk modelo1 = new Chunk("MODELO 1. REFERENCIAÇÃO\n\n", FontFactory.GetFont("Arial", 11, Font.BOLD, new BaseColor(93, 188, 210).Darker()));
                            Paragraph paraMod1 = new Paragraph(modelo1);
                            paraMod1.Alignment = Element.ALIGN_CENTER;
                            pdfDoc.Add(paraMod1);



                            /**
                             * Dados utente 
                             */

                            /* TABELA COM 3 COLUNAS */
                            //PdfPTable tableUtente = new PdfPTable(3);
                            //PdfPCell cabecalho = new PdfPCell(new Phrase(" 1.IDENTIFICAÇÃO DO UTENTE", textoCabecalho));
                            //cabecalho.Colspan = 3;
                            //cabecalho.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            //tableUtente.AddCell(cabecalho);

                            //PdfPCell cell = new PdfPCell(new Phrase("  Nome: ", textoColuna));                     
                            //tableUtente.AddCell(cell);
                            //cell = new PdfPCell(new Paragraph(utente.Name, textoColuna));
                            //cell.Colspan = 3;
                            //cell.BorderWidthLeft = 0;
                            //tableUtente.AddCell(cell);


                            PdfPTable tableUtente = new PdfPTable(1);
                            PdfPCell cabecalho = new PdfPCell(new Phrase(" 1.IDENTIFICAÇÃO DO UTENTE", textoCabecalho));
                            tableUtente.AddCell(cabecalho);

                            cell = new PdfPCell(new Phrase("  Nome:  " + utente.Name, textoColuna));
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableUtente.AddCell(cell);

                            cell = new PdfPCell(new Paragraph("  Morada:  " + utente.Morada, textoColuna));
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableUtente.AddCell(cell);

                            cell = new PdfPCell(new Paragraph("  Freguesia:  " + utente.Freguesia, textoColuna));
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableUtente.AddCell(cell);

                            cell = new PdfPCell(new Paragraph("  Telefone:  " + utente.NumeroTelefone, textoColuna));
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableUtente.AddCell(cell);

                            string dataNascimento = utente.Nascimento.ToShortDateString();
                            cell = new PdfPCell(new Paragraph("  Data de nascimento:  " + dataNascimento, textoColuna));
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableUtente.AddCell(cell);

                            cell = new PdfPCell(new Paragraph("  Estado civil:  " + utente.EstadoCivil, textoColuna));
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableUtente.AddCell(cell);

                            cell = new PdfPCell(new Paragraph("  Escolaridade:  " + utente.Escolaridade, textoColuna));
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableUtente.AddCell(cell);

                            cell = new PdfPCell(new Paragraph("  Nº de beneficiário:  " + utente.Beneficiario, textoColuna));
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableUtente.AddCell(cell);

                            pdfDoc.Add(tableUtente);

                            /**
                             * Dados cuidador
                             */
                            PdfPTable tableCuidador = new PdfPTable(1);
                            cabecalho = new PdfPCell(new Phrase(" 2.IDENTIFICAÇÃO DO CUIDADOR PRINCIPAL", textoCabecalho));
                            tableCuidador.AddCell(cabecalho);

                            cell = new PdfPCell(new Phrase("  Nome:  " + referenciacao.Cuidador.Name, textoColuna));
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableCuidador.AddCell(cell);

                            cell = new PdfPCell(new Paragraph("  Morada:  " + referenciacao.Cuidador.Morada, textoColuna));
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableCuidador.AddCell(cell);

                            cell = new PdfPCell(new Paragraph("  Telefone:  " + referenciacao.Cuidador.NumeroTelefone, textoColuna));
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableCuidador.AddCell(cell);

                            string dataNascimentoCuidador = referenciacao.Cuidador.Nascimento.ToShortDateString();
                            cell = new PdfPCell(new Paragraph("  Data de nascimento:  " + dataNascimentoCuidador, textoColuna));
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableCuidador.AddCell(cell);

                            cell = new PdfPCell(new Paragraph("  Estado civil:  " + referenciacao.Cuidador.EstadoCivil, textoColuna));
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableCuidador.AddCell(cell);

                            cell = new PdfPCell(new Paragraph("  Grau de parentesco:  " + referenciacao.Cuidador.GrauDeParentesco, textoColuna));
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableCuidador.AddCell(cell);

                            pdfDoc.Add(tableCuidador);

                            /**
                             * Entidade Referenciadora até Problemas Sensoriais
                             */

                            PdfPTable tableUtenteDetalhes = new PdfPTable(1);
                            cell = new PdfPCell(new Phrase(" 3.ENTIDADE REFERENCIADORA:  " + referenciacao.EntidadeReferenciadora, textoCabecalho));
                            tableUtenteDetalhes.AddCell(cell);

                            cell = new PdfPCell(new Phrase(" 4.DIAGNÓSTICO CLÍNICO:  " + referenciacao.DiagnosticoClinico, textoCabecalho));
                            tableUtenteDetalhes.AddCell(cell);

                            string dataAlta = referenciacao.DataDeAlta.ToShortDateString();
                            cell = new PdfPCell(new Phrase(" 4.1.Previsão de alta:  " + dataAlta, textoCabecalho));
                            tableUtenteDetalhes.AddCell(cell);

                            cabecalho = new PdfPCell(new Phrase(" 5.CRITÉRIOS DE TRIAGEM PARA CUIDADOS CONTINUADOS", textoCabecalho));
                            tableUtenteDetalhes.AddCell(cabecalho);

                            Paragraph paragraphCheckBox = new Paragraph();
                            paragraphCheckBox.Add(new Chunk(" 5.1.Dependência nas AVD ", textoCabecalho));
                            _ = referenciacao.DependenciaAVD ? paragraphCheckBox.Add(new Paragraph(Ischecked, f)) : paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            //if (referenciacao.DependenciaAVD) paragraphCheckBox.Add(new Paragraph(Ischecked, f));
                            //else paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            cell = new PdfPCell(paragraphCheckBox);
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableUtenteDetalhes.AddCell(cell);

                            paragraphCheckBox = new Paragraph();
                            paragraphCheckBox.Add(new Chunk(" 5.2.Desnutrição ", textoCabecalho));
                            _ = referenciacao.Desnutricao ? paragraphCheckBox.Add(new Paragraph(Ischecked, f)) : paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            cell = new PdfPCell(paragraphCheckBox);
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableUtenteDetalhes.AddCell(cell);

                            paragraphCheckBox = new Paragraph();
                            paragraphCheckBox.Add(new Chunk(" 5.3.Deterioração cognitiva ", textoCabecalho));
                            _ = referenciacao.Deteorioracao ? paragraphCheckBox.Add(new Paragraph(Ischecked, f)) : paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            cell = new PdfPCell(paragraphCheckBox);
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableUtenteDetalhes.AddCell(cell);

                            paragraphCheckBox = new Paragraph();
                            paragraphCheckBox.Add(new Chunk(" 5.4.Problemas sensoriais ", textoCabecalho));
                            _ = referenciacao.ProblemasSensoriais ? paragraphCheckBox.Add(new Paragraph(Ischecked, f)) : paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            cell = new PdfPCell(paragraphCheckBox);
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableUtenteDetalhes.AddCell(cell);

                            pdfDoc.Add(tableUtenteDetalhes);

                            /**
                            * Doenças Crónicas com Episódios de Reagudização
                            */
                            PdfPTable tableDoencasCronicas = new PdfPTable(1);
                            cabecalho = new PdfPCell(new Phrase(" 5.5.Doenças crónicas com episódios de reagudização:", textoCabecalho));
                            tableDoencasCronicas.AddCell(cabecalho);

                            paragraphCheckBox = new Paragraph();
                            paragraphCheckBox.Add(new Chunk("         DPOC ", textoColuna));
                            _ = referenciacao.DCronicas.DPOC ? paragraphCheckBox.Add(new Paragraph(Ischecked, f)) : paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            cell = new PdfPCell(paragraphCheckBox);
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableDoencasCronicas.AddCell(cell);

                            paragraphCheckBox = new Paragraph();
                            paragraphCheckBox.Add(new Chunk("         ICC ", textoColuna));
                            _ = referenciacao.DCronicas.ICC ? paragraphCheckBox.Add(new Paragraph(Ischecked, f)) : paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            cell = new PdfPCell(paragraphCheckBox);
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableDoencasCronicas.AddCell(cell);

                            paragraphCheckBox = new Paragraph();
                            paragraphCheckBox.Add(new Chunk("         Doença Cérebro Vascular ", textoColuna));
                            _ = referenciacao.DCronicas.DCV ? paragraphCheckBox.Add(new Paragraph(Ischecked, f)) : paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            cell = new PdfPCell(paragraphCheckBox);
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableDoencasCronicas.AddCell(cell);


                            paragraphCheckBox = new Paragraph();
                            paragraphCheckBox.Add(new Chunk("         Outras ", textoColuna));
                            if (referenciacao.DCronicas.Outras != null)
                            {
                                paragraphCheckBox.Add(new Paragraph(Ischecked, f));
                                paragraphCheckBox.Add(new Chunk("  " + referenciacao.DCronicas.Outras, textoOutras));
                            }
                            else paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            cell = new PdfPCell(paragraphCheckBox);
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                            tableDoencasCronicas.AddCell(cell);

                            pdfDoc.Add(tableDoencasCronicas);

                            /**
                             * Necessidade de Continuidade de Cuidados no Domicílio
                             */
                            PdfPTable tableNCCD = new PdfPTable(1);
                            cabecalho = new PdfPCell(new Phrase(" 5.6.Necessidade de Continuidade de Cuidados no Domicílio:", textoCabecalho));
                            tableNCCD.AddCell(cabecalho);

                            paragraphCheckBox = new Paragraph();
                            paragraphCheckBox.Add(new Chunk("         Tratamento de feridas/úlceras por pressão ", textoColuna));
                            _ = referenciacao.NCCD.TratamentoDeFeridas ? paragraphCheckBox.Add(new Paragraph(Ischecked, f)) : paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            cell = new PdfPCell(paragraphCheckBox);
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableNCCD.AddCell(cell);

                            paragraphCheckBox = new Paragraph();
                            paragraphCheckBox.Add(new Chunk("         Reabilitação ", textoColuna));
                            _ = referenciacao.NCCD.Reabilitacao ? paragraphCheckBox.Add(new Paragraph(Ischecked, f)) : paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            cell = new PdfPCell(paragraphCheckBox);
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableNCCD.AddCell(cell);

                            paragraphCheckBox = new Paragraph();
                            paragraphCheckBox.Add(new Chunk("         Manutenção de dispositivos ", textoColuna));
                            _ = referenciacao.NCCD.ManutencaoDeDispositivos ? paragraphCheckBox.Add(new Paragraph(Ischecked, f)) : paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            cell = new PdfPCell(paragraphCheckBox);
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableNCCD.AddCell(cell);

                            paragraphCheckBox = new Paragraph();
                            paragraphCheckBox.Add(new Chunk("         Gestão de regime terapêutico ", textoColuna));
                            _ = referenciacao.NCCD.GestaoDeRegimeTerapeutico ? paragraphCheckBox.Add(new Paragraph(Ischecked, f)) : paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            cell = new PdfPCell(paragraphCheckBox);
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableNCCD.AddCell(cell);

                            paragraphCheckBox = new Paragraph();
                            paragraphCheckBox.Add(new Chunk("         Outras ", textoColuna));
                            if (referenciacao.NCCD.Outras != null)
                            {
                                paragraphCheckBox.Add(new Paragraph(Ischecked, f));
                                paragraphCheckBox.Add(new Chunk("  " + referenciacao.NCCD.Outras, textoOutras));
                            }
                            else paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            cell = new PdfPCell(paragraphCheckBox);
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                            tableNCCD.AddCell(cell);

                            pdfDoc.Add(tableNCCD);

                            /**
                             * Necessidade de tratamentos complexos
                             */
                            PdfPTable tableNTC = new PdfPTable(1);
                            cabecalho = new PdfPCell(new Phrase(" 5.7.Necessidade de tratamentos complexos:", textoCabecalho));
                            tableNTC.AddCell(cabecalho);

                            paragraphCheckBox = new Paragraph();
                            paragraphCheckBox.Add(new Chunk("         Úlceras por pressão múltiplas ", textoColuna));
                            _ = referenciacao.NTC.UlcerasPorPressaoMultiplas ? paragraphCheckBox.Add(new Paragraph(Ischecked, f)) : paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            cell = new PdfPCell(paragraphCheckBox);
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableNTC.AddCell(cell);

                            paragraphCheckBox = new Paragraph();
                            paragraphCheckBox.Add(new Chunk("         Portadores de SNG/PEG ", textoColuna));
                            _ = referenciacao.NTC.PortadoresDeSNG_PEG ? paragraphCheckBox.Add(new Paragraph(Ischecked, f)) : paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            cell = new PdfPCell(paragraphCheckBox);
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableNTC.AddCell(cell);

                            paragraphCheckBox = new Paragraph();
                            paragraphCheckBox.Add(new Chunk("         Ventilação assistida ", textoColuna));
                            _ = referenciacao.NTC.VentilacaoAssistida ? paragraphCheckBox.Add(new Paragraph(Ischecked, f)) : paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            cell = new PdfPCell(paragraphCheckBox);
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableNTC.AddCell(cell);

                            paragraphCheckBox = new Paragraph();
                            paragraphCheckBox.Add(new Chunk("         Outras ", textoColuna));
                            if (referenciacao.NTC.Outras != null)
                            {
                                paragraphCheckBox.Add(new Paragraph(Ischecked, f));
                                paragraphCheckBox.Add(new Chunk("  " + referenciacao.NTC.Outras, textoOutras));
                            }
                            else paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            cell = new PdfPCell(paragraphCheckBox);
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                            tableNTC.AddCell(cell);

                            pdfDoc.Add(tableNTC);

                            /**
                             * Cuidados Paliativos
                             */
                            PdfPTable tableCP = new PdfPTable(1);
                            cabecalho = new PdfPCell(new Phrase(" 5.8.Cuidados Paliativos", textoCabecalho));
                            tableCP.AddCell(cabecalho);

                            paragraphCheckBox = new Paragraph();
                            paragraphCheckBox.Add(new Chunk("         Controlo de sintomas ", textoColuna));
                            _ = referenciacao.CP.ControloDeSintomas ? paragraphCheckBox.Add(new Paragraph(Ischecked, f)) : paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            cell = new PdfPCell(paragraphCheckBox);
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableCP.AddCell(cell);

                            paragraphCheckBox = new Paragraph();
                            paragraphCheckBox.Add(new Chunk("         Processo de luto ", textoColuna));
                            _ = referenciacao.CP.ProcessoDeLuto ? paragraphCheckBox.Add(new Paragraph(Ischecked, f)) : paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            cell = new PdfPCell(paragraphCheckBox);
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableCP.AddCell(cell);

                            paragraphCheckBox = new Paragraph();
                            paragraphCheckBox.Add(new Chunk("         Outros ", textoColuna));
                            if (referenciacao.CP.Outras != null)
                            {
                                paragraphCheckBox.Add(new Paragraph(Ischecked, f));
                                paragraphCheckBox.Add(new Chunk("  " + referenciacao.CP.Outras, textoOutras));
                            }
                            else paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            cell = new PdfPCell(paragraphCheckBox);
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                            tableCP.AddCell(cell);

                            pdfDoc.Add(tableCP);

                            /**
                             * Necessidade de ensino
                             */
                            PdfPTable tableNE = new PdfPTable(1);
                            cabecalho = new PdfPCell(new Phrase(" 5.9.Necessidade de ensino", textoCabecalho));
                            tableNE.AddCell(cabecalho);

                            paragraphCheckBox = new Paragraph();
                            paragraphCheckBox.Add(new Chunk("         Execução de técnicas ", textoColuna));
                            _ = referenciacao.NE.ExecucaoDeTecnicas ? paragraphCheckBox.Add(new Paragraph(Ischecked, f)) : paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            cell = new PdfPCell(paragraphCheckBox);
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableNE.AddCell(cell);

                            paragraphCheckBox = new Paragraph();
                            paragraphCheckBox.Add(new Chunk("         Regime terapêutico ", textoColuna));
                            _ = referenciacao.NE.RegimeTerapeutico ? paragraphCheckBox.Add(new Paragraph(Ischecked, f)) : paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            cell = new PdfPCell(paragraphCheckBox);
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableNE.AddCell(cell);

                            paragraphCheckBox = new Paragraph();
                            paragraphCheckBox.Add(new Chunk("         Auto-cuidados ", textoColuna));
                            _ = referenciacao.NE.AutoCuidados ? paragraphCheckBox.Add(new Paragraph(Ischecked, f)) : paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            cell = new PdfPCell(paragraphCheckBox);
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableNE.AddCell(cell);

                            paragraphCheckBox = new Paragraph();
                            paragraphCheckBox.Add(new Chunk("         Outros ", textoColuna));
                            if (referenciacao.NE.Outras != null)
                            {
                                paragraphCheckBox.Add(new Paragraph(Ischecked, f));
                                paragraphCheckBox.Add(new Chunk("  " + referenciacao.NE.Outras, textoOutras));
                            }
                            else paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            cell = new PdfPCell(paragraphCheckBox);
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                            tableNE.AddCell(cell);

                            pdfDoc.Add(tableNE);

                            /**
                             * AVALIAÇÃO DO NÍVEL DE DEPENDÊNCIA
                             */
                            pdfDoc.NewPage();
                            pdfDoc.Add(table);  //cabeçalho

                            PdfPTable tableAND = new PdfPTable(1);
                            cabecalho = new PdfPCell(new Phrase(" 6.AVALIAÇÃO DO NÍVEL DE DEPENDÊNCIA", textoCabecalho));
                            tableAND.AddCell(cabecalho);

                            cell = new PdfPCell(new Phrase("      Score da Escala de Barthel  " + referenciacao.AND, FontFactory.GetFont("Arial", 9, Font.ITALIC, BaseColor.BLACK)));
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableAND.AddCell(cell);
                            cell = new PdfPCell(new Phrase("\n"));
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                            tableAND.AddCell(cell);

                            pdfDoc.Add(tableAND);

                            /**
                             * Necessidade de ensino
                             */
                            PdfPTable tableECE = new PdfPTable(1);
                            cabecalho = new PdfPCell(new Phrase(" 7.ESTADO DE CONSCIÊNCIA e de EXPRESSÃO", textoCabecalho));
                            tableECE.AddCell(cabecalho);

                            paragraphCheckBox = new Paragraph();
                            paragraphCheckBox.Add(new Chunk("         Confusão ", textoColuna));
                            _ = referenciacao.ECE.Confusao ? paragraphCheckBox.Add(new Paragraph(Ischecked, f)) : paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            cell = new PdfPCell(paragraphCheckBox);
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableECE.AddCell(cell);

                            paragraphCheckBox = new Paragraph();
                            paragraphCheckBox.Add(new Chunk("         Desorientação ", textoColuna));
                            _ = referenciacao.ECE.Desorientacao ? paragraphCheckBox.Add(new Paragraph(Ischecked, f)) : paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            cell = new PdfPCell(paragraphCheckBox);
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableECE.AddCell(cell);

                            paragraphCheckBox = new Paragraph();
                            paragraphCheckBox.Add(new Chunk("         Demência ", textoColuna));
                            _ = referenciacao.ECE.Demencia ? paragraphCheckBox.Add(new Paragraph(Ischecked, f)) : paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            cell = new PdfPCell(paragraphCheckBox);
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableECE.AddCell(cell);

                            paragraphCheckBox = new Paragraph();
                            paragraphCheckBox.Add(new Chunk("         Afasia ", textoColuna));
                            _ = referenciacao.ECE.Afasia ? paragraphCheckBox.Add(new Paragraph(Ischecked, f)) : paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            cell = new PdfPCell(paragraphCheckBox);
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableECE.AddCell(cell);

                            paragraphCheckBox = new Paragraph();
                            paragraphCheckBox.Add(new Chunk("         Disartria ", textoColuna));
                            _ = referenciacao.ECE.Disartria ? paragraphCheckBox.Add(new Paragraph(Ischecked, f)) : paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            cell = new PdfPCell(paragraphCheckBox);
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableECE.AddCell(cell);

                            paragraphCheckBox = new Paragraph();
                            paragraphCheckBox.Add(new Chunk("         Inconsciente ", textoColuna));
                            _ = referenciacao.ECE.Inconsciente ? paragraphCheckBox.Add(new Paragraph(Ischecked, f)) : paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            cell = new PdfPCell(paragraphCheckBox);
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableECE.AddCell(cell);

                            pdfDoc.Add(tableECE);

                            /**
                             * Cuidador
                             */
                            PdfPTable tableCuidadorDetalhes = new PdfPTable(1);
                            cabecalho = new PdfPCell(new Phrase(" 8.CUIDADOR", textoCabecalho));
                            tableCuidadorDetalhes.AddCell(cabecalho);

                            paragraphCheckBox = new Paragraph();
                            paragraphCheckBox.Add(new Chunk("         Sobrecarga física/emocional do cuidador ", textoColuna));
                            _ = referenciacao.CuidadorDetalhes.Sobrecarga ? paragraphCheckBox.Add(new Paragraph(Ischecked, f)) : paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            cell = new PdfPCell(paragraphCheckBox);
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableCuidadorDetalhes.AddCell(cell);

                            paragraphCheckBox = new Paragraph();
                            paragraphCheckBox.Add(new Chunk("         Doença do cuidador principal ", textoColuna));
                            _ = referenciacao.CuidadorDetalhes.DCP ? paragraphCheckBox.Add(new Paragraph(Ischecked, f)) : paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            cell = new PdfPCell(paragraphCheckBox);
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableCuidadorDetalhes.AddCell(cell);

                            paragraphCheckBox = new Paragraph();
                            paragraphCheckBox.Add(new Chunk("         Ausência de suporte familiar ", textoColuna));
                            _ = referenciacao.CuidadorDetalhes.ASF ? paragraphCheckBox.Add(new Paragraph(Ischecked, f)) : paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            cell = new PdfPCell(paragraphCheckBox);
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableCuidadorDetalhes.AddCell(cell);

                            paragraphCheckBox = new Paragraph();
                            paragraphCheckBox.Add(new Chunk("         Cuidador com idade avançada ", textoColuna));
                            _ = referenciacao.CuidadorDetalhes.CuidadorIdadeAvancada ? paragraphCheckBox.Add(new Paragraph(Ischecked, f)) : paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            cell = new PdfPCell(paragraphCheckBox);
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableCuidadorDetalhes.AddCell(cell);

                            pdfDoc.Add(tableCuidadorDetalhes);

                            /**
                             * INDICADORES DE RISCO SOCIAL
                             */
                            PdfPTable tableIRS = new PdfPTable(1);
                            cabecalho = new PdfPCell(new Phrase(" 9.INDICADORES DE RISCO SOCIAL", textoCabecalho));
                            tableIRS.AddCell(cabecalho);

                            paragraphCheckBox = new Paragraph();
                            paragraphCheckBox.Add(new Chunk("         Isolamento social/geográfico ", textoColuna));
                            _ = referenciacao.IRS.Isolamento ? paragraphCheckBox.Add(new Paragraph(Ischecked, f)) : paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            cell = new PdfPCell(paragraphCheckBox);
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableIRS.AddCell(cell);

                            paragraphCheckBox = new Paragraph();
                            paragraphCheckBox.Add(new Chunk("         Ausência de suporte familiar ", textoColuna));
                            _ = referenciacao.IRS.ASF ? paragraphCheckBox.Add(new Paragraph(Ischecked, f)) : paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            cell = new PdfPCell(paragraphCheckBox);
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableIRS.AddCell(cell);

                            paragraphCheckBox = new Paragraph();
                            paragraphCheckBox.Add(new Chunk("         História de conflitualidade familiar/rutura familiar ", textoColuna));
                            _ = referenciacao.IRS.Conflitualidade ? paragraphCheckBox.Add(new Paragraph(Ischecked, f)) : paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            cell = new PdfPCell(paragraphCheckBox);
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableIRS.AddCell(cell);

                            paragraphCheckBox = new Paragraph();
                            paragraphCheckBox.Add(new Chunk("         Antecedentes pessoais/familiares de violência ", textoColuna));
                            _ = referenciacao.IRS.AntecedentesViolencia ? paragraphCheckBox.Add(new Paragraph(Ischecked, f)) : paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            cell = new PdfPCell(paragraphCheckBox);
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableIRS.AddCell(cell);

                            paragraphCheckBox = new Paragraph();
                            paragraphCheckBox.Add(new Chunk("         Suspeita de maus tratos ", textoColuna));
                            _ = referenciacao.IRS.MausTratos ? paragraphCheckBox.Add(new Paragraph(Ischecked, f)) : paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            cell = new PdfPCell(paragraphCheckBox);
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableIRS.AddCell(cell);

                            paragraphCheckBox = new Paragraph();
                            paragraphCheckBox.Add(new Chunk("         Negligência na prestação de cuidados ", textoColuna));
                            _ = referenciacao.IRS.Negligencia ? paragraphCheckBox.Add(new Paragraph(Ischecked, f)) : paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            cell = new PdfPCell(paragraphCheckBox);
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableIRS.AddCell(cell);

                            paragraphCheckBox = new Paragraph();
                            paragraphCheckBox.Add(new Chunk("         Dependência do idoso na sua gestão económica/financeira e de bens ", textoColuna));
                            _ = referenciacao.IRS.DependenciaEconomica ? paragraphCheckBox.Add(new Paragraph(Ischecked, f)) : paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            cell = new PdfPCell(paragraphCheckBox);
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableIRS.AddCell(cell);

                            paragraphCheckBox = new Paragraph();
                            paragraphCheckBox.Add(new Chunk("         Baixos rendimentos ", textoColuna));
                            _ = referenciacao.IRS.BaixosRendimentos ? paragraphCheckBox.Add(new Paragraph(Ischecked, f)) : paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            cell = new PdfPCell(paragraphCheckBox);
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableIRS.AddCell(cell);

                            paragraphCheckBox = new Paragraph();
                            paragraphCheckBox.Add(new Chunk("         Habitação degradada ", textoColuna));
                            _ = referenciacao.IRS.HabitacaoDegradada ? paragraphCheckBox.Add(new Paragraph(Ischecked, f)) : paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            cell = new PdfPCell(paragraphCheckBox);
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableIRS.AddCell(cell);

                            paragraphCheckBox = new Paragraph();
                            paragraphCheckBox.Add(new Chunk("         Condições de salubridade precárias ", textoColuna));
                            _ = referenciacao.IRS.SalubridadePrecaria ? paragraphCheckBox.Add(new Paragraph(Ischecked, f)) : paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            cell = new PdfPCell(paragraphCheckBox);
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableIRS.AddCell(cell);

                            paragraphCheckBox = new Paragraph();
                            paragraphCheckBox.Add(new Chunk("         Coabitação/sobrelotação habitacional ", textoColuna));
                            _ = referenciacao.IRS.CoabitacaoHabitacional ? paragraphCheckBox.Add(new Paragraph(Ischecked, f)) : paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            cell = new PdfPCell(paragraphCheckBox);
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableIRS.AddCell(cell);

                            paragraphCheckBox = new Paragraph();
                            paragraphCheckBox.Add(new Chunk("         Barreiras arquitectónicas ", textoColuna));
                            _ = referenciacao.IRS.BarreirasArquitetonicas ? paragraphCheckBox.Add(new Paragraph(Ischecked, f)) : paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            cell = new PdfPCell(paragraphCheckBox);
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                            tableIRS.AddCell(cell);

                            paragraphCheckBox = new Paragraph();
                            paragraphCheckBox.Add(new Chunk("         Outros ", textoColuna));
                            if (referenciacao.IRS.Outras != null)
                            {
                                paragraphCheckBox.Add(new Paragraph(Ischecked, f));
                                paragraphCheckBox.Add(new Chunk("  " + referenciacao.IRS.Outras, textoOutras));
                            }
                            else paragraphCheckBox.Add(new Paragraph(NotChecked, f));
                            cell = new PdfPCell(paragraphCheckBox);
                            cell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                            tableIRS.AddCell(cell);

                            pdfDoc.Add(tableIRS);


                            /**
                             * Rodápé
                             */
                            PdfPTable tableRod = new PdfPTable(1);
                            cabecalho = new PdfPCell(new Phrase("    DATA:  " + String.Format("{0}/{1}/{2}", referenciacao.Criacao.Day, referenciacao.Criacao.Month, referenciacao.Criacao.Year) + "                   ASSINATURA:", textoCabecalho));
                            tableRod.AddCell(cabecalho);

                            pdfDoc.Add(tableRod);


                        }
                        finally
                        {
                            pdfWriter.CloseStream = false;
                            pdfDoc.Close();
                        }
                    }
                }
                bytes = ms.ToArray();
            }
            return bytes;
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
