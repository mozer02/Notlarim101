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
    public class CommentController : Controller
    {
        CommentManager cmm = new CommentManager();
        NoteManager nm = new NoteManager();
        // GET: Comment
        public ActionResult Index()
        {
            return View(cmm.List());
        }

        // GET: Comment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = cmm.Find(s => s.Id == id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comment/Create
        public ActionResult Create()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        public ActionResult Create(Comment comment, int? notId)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");
            if (ModelState.IsValid)
            {
                if (notId == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Note note = nm.Find(s => s.Id == notId);
                if (note == null)
                {
                    return new HttpNotFoundResult();
                }
                comment.Note = note;
                comment.Owner = CurrentSession.User;
                if (cmm.Insert(comment) > 0)
                {
                    return Json(new { result = true }, JsonRequestBehavior.AllowGet);
                }
                return RedirectToAction("Index");
            }

            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }

        // GET: Comment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = cmm.Find(s => s.Id == id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(comment);
        }

        // GET: Comment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
