using System;
using Notlarim101.BusinessLayer.Abstract;
using Notlarim101.Common.Helper;
using Notlarim101.DataAccessLayer.EntityFramework;
using Notlarim101.Entity;
using Notlarim101.Entity.Messages;
using Notlarim101.Entity.ValueObject;

namespace Notlarim101.BusinessLayer
{
    public class NotlarimUserManager : ManagerBase<NotlarimUser>
    {
        //Kullanici username kontrolu yapmaliyim
        //kullnici email kontrolu yapmaliyim
        //Kayit islemini gerceklestirmeliyim
        //Activasyon e-postasi gonderimi

        BusinessLayerResult<NotlarimUser> res = new BusinessLayerResult<NotlarimUser>();

        public BusinessLayerResult<NotlarimUser> RegisterUser(RegisterViewModel data)
        {
            NotlarimUser user = Find(s => s.Username == data.Username || s.Email == data.Email);

            if (user != null)
            {
                if (user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExist, "Kullanici adi kayitli");
                }

                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailalreadyExist, "Email kayitli");
                }
                //throw new Exception("Kayitli kullanici yada e-posta adresi");
            }
            else
            {
                int dbResult = base.Insert(new NotlarimUser()
                {
                    Name = data.Name,
                    Surname = data.Surname,
                    Username = data.Username,
                    Email = data.Email,
                    Password = data.Password,
                    ProfileImageFilename = "User1.png",
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = false,
                    IsAdmin = false,
                    //repository e tasindi
                    //ModifiedOn = DateTime.Now,
                    //CreatedOn = DateTime.Now,
                    //ModifiedUsername = "system"
                });
                if (dbResult > 0)
                {
                    res.Result = Find(s => s.Email == data.Email && s.Username == data.Username);

                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activateUri = $"{siteUri}/Home/UserActivate/{res.Result.ActivateGuid}";
                    string body =
                        $"Merhaba {res.Result.Username}; <br><br> Hesabinizi aktiflestirmek icin <a href='{activateUri}' target='_blank'> Tiklayin </a>.";

                    MailHelper.SendMail(body, res.Result.Email, "Notlarim101 hesap aktiflestirme");


                    //activasyon mail i atilacak
                    //lr.Result.ActivateGuid;
                }
            }

            return res;
        }

        public BusinessLayerResult<NotlarimUser> LoginUser(LoginViewModel data)
        {
            //Giris kontrolu
            //Hesap aktif edilmismi kontrolu


            res.Result = Find(s => s.Username == data.Username && s.Password == data.Password);
            if (res.Result != null)
            {
                if (!res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserIsNotActive, "Kullanici adi aktiflestirilmemis!!!");
                    res.AddError(ErrorMessageCode.CheckYourEmail, "Lutfen Mailinizi kontrol edin...");
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UsernameOrPasswordWrong, "kullanici adi yada sifre uyusmuyor.");
            }

            return res;
        }

        public BusinessLayerResult<NotlarimUser> ActivateUser(Guid id)
        {

            res.Result = Find(x => x.ActivateGuid == id);

            if (res.Result != null)
            {
                if (res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserAlreadyActive, "Bu hesap daha once aktif edilmis!!! ");
                    return res;
                }

                res.Result.IsActive = true;
                Update(res.Result);
            }
            else
            {
                res.AddError(ErrorMessageCode.ActivateIdDoesNotExist, "Muhammed yine mi sen");
            }

            return res;
        }

        public BusinessLayerResult<NotlarimUser> GetUserById(int id)
        {

            res.Result = Find(s => s.Id == id);
            if (res.Result == null)
            {
                res.AddError(ErrorMessageCode.UserNotFound, "Kullanici bulunamadi.");
            }

            return res;
        }

        public BusinessLayerResult<NotlarimUser> UpdateProfile(NotlarimUser data)
        {
            NotlarimUser user =
                Find(s => s.Id != data.Id && (s.Username == data.Username || s.Email == data.Email));

            if (user != null && user.Id != data.Id)
            {
                if (user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExist, "Bu kullanici adi daha once kaydedilmis.");
                }
                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailalreadyExist, "Bu email daha once kaydedilmis.");
                }

                return res;
            }

            res.Result = Find(s => s.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.Username = data.Username;
            if (!string.IsNullOrEmpty(data.ProfileImageFilename))
            {
                res.Result.ProfileImageFilename = data.ProfileImageFilename;
            }

            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.ProfileCouldNotUpdate, "Profil guncellenemedi.");
            }
            return res;
        }

        public BusinessLayerResult<NotlarimUser> RemoveUserById(int id)
        {
            NotlarimUser user = Find(s => s.Id == id);

            if (user != null)
            {
                if (Delete(user) == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotRemove, "Kullanici silinemedi...");
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UserCouldNotFind, "Kullanici bulunamadi...");
            }

            return res;
        }


        // Hiding Method...
        public new BusinessLayerResult<NotlarimUser> Insert(NotlarimUser data)
        {
            NotlarimUser user = Find(s => s.Username == data.Username || s.Email == data.Email);
            res.Result = data;
            if (user != null)
            {
                if (user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExist, "Kullanici adi kayitli");
                }

                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailalreadyExist, "Email kayitli");
                }
                //throw new Exception("Kayitli kullanici yada e-posta adresi");
            }
            else
            {
                res.Result.ProfileImageFilename = "user1.png";
                res.Result.ActivateGuid = Guid.NewGuid();

                if (base.Insert(res.Result) == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotInserted, "Kullanici eklenemedi");
                }
            }

            return res;
        }

        public new BusinessLayerResult<NotlarimUser> Update(NotlarimUser data)
        {
            NotlarimUser user =
               Find(s => s.Id != data.Id && (s.Username == data.Username || s.Email == data.Email));

            if (user != null && user.Id != data.Id)
            {
                if (user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExist, "Bu kullanici adi daha once kaydedilmis.");
                }
                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailalreadyExist, "Bu email daha once kaydedilmis.");
                }

                return res;
            }

            res.Result = Find(s => s.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.Username = data.Username;
            res.Result.IsActive = data.IsActive;
            res.Result.IsAdmin = data.IsAdmin;
            

            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.UserCouldNotUpdated, "Kullanıcı güncellenemedi.");
            }
            return res;
        }
    }
}
