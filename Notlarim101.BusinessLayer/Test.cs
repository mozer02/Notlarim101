using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notlarim101.DataAccessLayer;
using Notlarim101.DataAccessLayer.EntityFramework;
using Notlarim101.Entity;

namespace Notlarim101.BusinessLayer
{
    public class Test
    {
        private Repository<NotlarimUser> ruser = new Repository<NotlarimUser>();
        private Repository<Category> rcat = new Repository<Category>();
        private Repository<Comment> rcom = new Repository<Comment>();
        private Repository<Note> rnote = new Repository<Note>();
        private Repository<Liked> rlike = new Repository<Liked>();

        //private bool deneme = true;
        public Test()
        {
            //NotlarimContext db = new NotlarimContext();
            //var a= db.NotlarimUsers.ToList();
            //db.Database.CreateIfNotExists();

            //var test = rcat.List();

            //var test1 = rcat.List(x=>x.Id>5);
            //var test5 = rnote.List(x => x.Id > 5);
            

            //var test4 = test1.Where(x => x.Id > 3).ToList();
            //var test2 = rcat.QList(x=>x.Id>5);
            //var test3 = test2.ToList();
        }

        public void InsertTest()
        {
            int result = ruser.Insert(new NotlarimUser()
            {
                Name = "Abuzittin",
                Surname = "Zerdali",
                Email = "abuzer@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = false,
                Username = "abuzer1",
                Password = "123",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                ModifiedUsername = "abuzer1"
            });
        }

        public void UpdateTest()
        {
            NotlarimUser user = ruser.Find(s => s.Username == "abuzer1");
            if (user!=null)
            {
                user.Password = "1111111";
                ruser.Update(user);
            }
        }

        public void DeleteTest()
        {
            NotlarimUser user = ruser.Find(s => s.Username == "abuzer1");
            if (user != null)
            {
                ruser.Delete(user);
            }
        }

        public void CommentTest()
        {
            NotlarimUser user = ruser.Find(s => s.Id == 1);
            Note note = rnote.Find(x => x.Id == 3);

            Comment comment = new Comment()
            {
                Text = "Bu bir test yorumudur.",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                ModifiedUsername = "onuragici",
                Note = note,
                Owner = user
            };
            rcom.Insert(comment);
        }
    }
}
