using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Notlarim101.BusinessLayer;
using Notlarim101.Entity;
using Notlarim101.WebApp.Models;

namespace Notlarim101.WebApp.Controllers
{
    public class NoteController : Controller
    {
        private NoteManager nm = new NoteManager();
        CategoryManager cm = new CategoryManager();
        LikedManager lm = new LikedManager();

        public ActionResult Index()
        {
            var notes = nm.QList().Include("Category").Include("Owner").Where(
                x => x.Owner.Id == CurrentSession.User.Id).OrderByDescending(
                x => x.ModifiedOn).ToList();
            return View(notes);
        }
        public ActionResult MyLikedNotes()
        {
            var notes = lm.QList().Include("LikedUser").Include("Note").Where(s => s.LikedUser.Id == CurrentSession.User.Id).Select(x => x.Note).Include("Category").Include("Owner").OrderByDescending(s => s.ModifiedOn).ToList();
            var notes1 = lm.List(x => x.LikedUser.Id == CurrentSession.User.Id).Select(x => x.Note).ToList();
            return View("Index", notes);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = nm.Find(s => s.Id == id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(cm.List(), "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Note note)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");
            if (ModelState.IsValid)
            {
                note.Owner = CurrentSession.User;
                nm.Insert(note);
                return RedirectToAction("Index");
            }
            return View(note);
        }

        // GET: Note/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note =nm.Find(s => s.Id == id);
            if (note == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(cm.List(), "Id", "Title", note.CategoryId);
            return View(note);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Note note)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");
            if (ModelState.IsValid)
            {
                Note dbNote = nm.Find(s => s.Id == note.Id);
                dbNote.IsDraft = note.IsDraft;
                dbNote.CategoryId = note.CategoryId;
                dbNote.Text = note.Text;
                dbNote.Title = note.Title;
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(cm.List(), "Id", "Title", note.CategoryId);
            return View(note);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = nm.Find(s => s.Id == id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Note note = nm.Find(s => s.Id == id);
            nm.Delete(note);
            return RedirectToAction("Index");
        }
    }
}
