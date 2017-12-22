using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LocalTheatre.Models;
using System.Web.Security;

namespace LocalTheatre.Controllers
///<summary>
/// this class manages the contents of the Posts page and the features in it, like getting the data from database
/// creating data in the database, editing it and deleting it
/// 
/// </summary>
{
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Posts+
        public ActionResult Index()
        {
            return View(db.Posts.Include("Comments").ToList());
        }

        [HttpPost, ActionName("details")]
        public ActionResult Details(LocalTheatre.Models.Comment com)
        {

            com.UserName = User.Identity.Name;
            com.CommentDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.Comments.Add(com);
                db.SaveChanges();
                return RedirectToAction("Details", com.PostID);
            }

            return View();
        }

        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Posts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Post post)
        {
            post.PostDate = DateTime.Now;
            post.UserName = User.Identity.Name;
            db.Posts.Add(post);     
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        // GET: Posts/Edit/5
        [Authorize(Roles = "Admin,User")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var post = db.Posts.Where(b => b.PostID == id).Include("Comments").FirstOrDefault();
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,User")]
        public ActionResult Edit([Bind(Include = "PostID,UserName,PostTitle,PostDesc,PostDate")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: Posts/Delete/5
        [Authorize(Roles = "Admin,User")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var post = db.Posts.Where(b => b.PostID == id).Include("Comments").FirstOrDefault();
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,User")]
        public ActionResult DeleteConfirmed(int id)
        {
            var post = db.Posts.Where(b => b.PostID == id).Include("Comments").FirstOrDefault();
            db.Posts.Remove(post);
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

        [ValidateAntiForgeryToken]
        [HttpPost, ActionName("DeleteComment")]
        [Authorize(Roles = "Admin,User")]
        public ActionResult DeleteComment(Comment com)
        {
            var modeltodelete = db.Comments.FirstOrDefault(s => s.CommentID == com.CommentID);

            if (ModelState.IsValid)
            {
                db.Comments.Remove(modeltodelete);
                db.SaveChanges();
                return RedirectToAction("Edit", "Posts", new { @id = com.PostID });

            }
            return View();
        }

    }
}
