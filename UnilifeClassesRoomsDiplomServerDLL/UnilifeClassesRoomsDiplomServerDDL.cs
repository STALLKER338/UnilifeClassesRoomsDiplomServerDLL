using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data.SqlClient;
using System.Runtime.Remoting.Contexts;
using UnilifeClassesRoomsDiplomServerDLL.ModelsDB;
using System.Threading.Tasks;
using System.Runtime;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Collections;

namespace UnilifeClassesRoomsDiplomServerDLL
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "UnilifeClassesRoomsDiplomServerDDL" в коде и файле конфигурации.
    public class UnilifeClassesRoomsDiplomServerDDL : IUnilifeClassesRoomsDiplomServerDDL//Реализация OperationContract-ов
    {
        UnilifeDB _db = new UnilifeDB();

        public string LogInAccount(string _login, string _password)
        {
            var res = (from a in _db.Accounts
                       where a.Password == _password && a.Login == _login && a.Active == true
                       select a).FirstOrDefault();
            if (res != null)
            {
                res.MailKey = Generator().ToString();

                // отправитель - устанавливаем адрес и отображаемое в письме имя
                MailAddress from = new MailAddress("unilife2023@yandex.ru", "Unilife_ClassesRoom");
                // кому отправляем

                MailAddress to = new MailAddress(res.Mail.ToString());//это адрес на который отправляем
                                                                      // создаем объект сообщения
                MailMessage m = new MailMessage(from, to);
                // тема письма
                m.Subject = "Код подтверждения";
                // текст письма
                m.Body = "Код подтверждения для входа в Unilife_ClassesRoom " + res.MailKey;
                // письмо представляет код html
                m.IsBodyHtml = true;
                // адрес smtp-сервера и порт, с которого будем отправлять письмо
                SmtpClient smtp = new SmtpClient("smtp.yandex.ru", 587);
                // логин и пароль
                smtp.Credentials = new NetworkCredential("unilife2023@yandex.ru", "ynunavzspenewerw");
                smtp.EnableSsl = true;
                smtp.Send(m);

                Session s = new Session();
                s.SessionKey = Generator().ToString();
                s.AccountId = res.Id;
                _db.Sessions.Add(s);
                _db.SaveChanges();
                return s.SessionKey.ToString();
            }
            else
            {
                return "0";
            }
        }
        public bool ConfirmLogInAccount(string hashKey, string mailkey)
        {
            var res1 = (from a in _db.Accounts
                        join s in _db.Sessions on a.Id equals s.AccountId
                        where a.MailKey == mailkey && s.SessionKey == hashKey
                        select s).FirstOrDefault();//Поиск кода подтверждения
            if (res1 != null)//Если код найден статус сессии меняеться на пройденная валидацию.
            {
                res1.Confirm = true;
                _db.Entry(res1).State = System.Data.Entity.EntityState.Modified;
                _db.SaveChanges();
                return true;
            }
            else// возвращаеться false. Пользователь остаёться на форме подтверждения ключа.
            {
                return false;
            }
        }
        public List<Account> GetAccounts()
        {
            _db.Configuration.ProxyCreationEnabled = false;// отключение проксирования сущностей EF.(Без отключения не работает сервер по причине тайм-аутов из-за подгрузки связанных данных и проблемм с сереализацией виртуальных объектов)
            return _db.Accounts.ToList();// возвращение списка аккаунтов
        }
        public List<User> GetUsers()
        {

            _db.Configuration.ProxyCreationEnabled = false;
            return _db.Users.ToList();// возвращение списка пользователей
        }
        public List<Role> GetRoles()
        {

            _db.Configuration.ProxyCreationEnabled = false;
            return _db.Roles.ToList();// возвращение списка ролей
        }
        public bool CheakHashKey(string hashKey)
        {
            var res = (from s in _db.Sessions
                       where s.SessionKey == hashKey && s.Confirm == true
                       select s).FirstOrDefault();//поиск хеш-ключа в базе
            if (res != null)//Если найден отправляем true.
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Account GetAccountUser(string hashKey)
        {
            _db.Configuration.ProxyCreationEnabled = false;
            Account acc = (from a in _db.Accounts
                           join s in _db.Sessions on a.Id equals s.AccountId
                           where s.SessionKey == hashKey
                           select a).FirstOrDefault();//получение списка аккаунтов связанных с классом
            return acc;
        }
        public bool LogOut(string hashKey)
        {
            Session res = (from s in _db.Sessions
                       where s.SessionKey == hashKey
                       select s).FirstOrDefault();//Перепроверка существования хэш-ключа и получение сессии
            if (res != null)
            {
                _db.Sessions.Remove(res);//Удаление сессии
                _db.SaveChanges();
                return true;
            }
            return false;
        }
        public User GetUser(int? id)
        {
            _db.Configuration.ProxyCreationEnabled = false;
            User res = (from s in _db.Users
                        where s.Id == id
                        select s).FirstOrDefault();//Получение пользователя по id
            return res;
        }
        public void UpdAccount(Account acc)
        {
            _db.Entry(acc).State = System.Data.Entity.EntityState.Modified;//изменение данных аккаунта
            _db.SaveChanges();//сохранение изменений в бд

        }
        public int GetPowerRole(string hashKey)
        {
            int power = (from r in _db.Roles
                         join a in _db.Accounts on r.Id equals a.RoleId
                         join s in _db.Sessions on a.Id equals s.AccountId
                         where s.SessionKey == hashKey
                         select r.PowerRole).FirstOrDefault();// получение силы роли пользователя
            return power;
        }
        public Role GetRole(int? id)
        {
            _db.Configuration.ProxyCreationEnabled = false;
            Role res = (from s in _db.Roles
                        where s.Id == id
                        select s).FirstOrDefault();//Получение роли по id
            return res;
        }
        public int AddAccount(Account acc)
        {
            _db.Accounts.Add(acc);//Добавление аккаунта в БД
            _db.SaveChanges();
            return acc.Id;
        }
        public List<Class> GetClassesUser(string hashKey)
        {
            _db.Configuration.ProxyCreationEnabled = false;
            List<Class> res = (from c in _db.Classes
                               join ca in _db.ClassAccounts on c.Id equals ca.ClassId
                               join a in _db.Accounts on ca.AccountId equals a.Id
                               join s in _db.Sessions on a.Id equals s.AccountId
                               where s.SessionKey == hashKey
                               select c).ToList();//Получение списка классов пользователя
            return res;
        }
        public List<ModelsDB.Task> GetTasksClassFalse(int classId, string hashKey)
        {

            _db.Configuration.ProxyCreationEnabled = false;
            List<ModelsDB.Task> res = (from t in _db.Tasks
                                       join c in _db.Classes on t.ClassId equals c.Id
                                       join j in _db.Jobs on t.Id equals j.TaskId
                                       join a in _db.Accounts on j.AccountId equals a.Id
                                       join s in _db.Sessions on a.Id equals s.AccountId
                                       where c.Id == classId && j.Deleted == false && t.DelivaryTime > DateTime.Now && s.SessionKey == hashKey
                                       select t).ToList();// Получение списказаданий где выполненны или сданны на проверку работы. Данным пользователем.(дата сдачи которых не истекла)

            List<ModelsDB.Task> res2 = (from t in _db.Tasks
                                        join c in _db.Classes on t.ClassId equals c.Id

                                        where c.Id == classId && t.DelivaryTime > DateTime.Now
                                        select t).ToList();//Получение списка заданий класса(дата сдачи которых не истекла)


            return res2.Except(res).ToList(); //Возвращение списка не сданных/выполненных работ пользователя(дата сдачи которых не истекла)
        }
        public List<ModelsDB.Task> GetTasksClass(int classId)
        {

            _db.Configuration.ProxyCreationEnabled = false;

            List<ModelsDB.Task> res = (from t in _db.Tasks
                                       join c in _db.Classes on t.ClassId equals c.Id
                                       where c.Id == classId
                                       select t).ToList();// Получение списка заданий в классе
            return res;
        }
        public bool СheckingTeacher(int classId, string hashKey)
        {

            _db.Configuration.ProxyCreationEnabled = false;

            var res = (from a in _db.Accounts
                       join ca in _db.ClassAccounts on a.Id equals ca.AccountId
                       join s in _db.Sessions on a.Id equals s.AccountId
                       where s.SessionKey == hashKey && ca.ClassId == classId
                       select ca.Teacher).FirstOrDefault();//проверка являеться ли пользователь учителем в классе
            return res;
        }
        public List<Account> GetAccountsClass(int classId)
        {
            _db.Configuration.ProxyCreationEnabled = false;
            var res1 = from a in _db.Accounts
                       join j in _db.ClassAccounts on a.Id equals j.AccountId
                       where j.ClassId == classId
                       select a;// Получения списка аккаунтов связанных с классом
            return res1.ToList();
        }
        public int AddTask(ModelsDB.Task _task, string hashKey)
        {
            using (UnilifeDB db = new UnilifeDB())
            {

                _task.AccountId = (from a in db.Accounts
                                   join s in db.Sessions on a.Id equals s.AccountId
                                   where s.SessionKey == hashKey
                                   select a.Id).FirstOrDefault();
                db.Tasks.Add(_task);
                db.SaveChanges();
                var listMail = from a in db.Accounts
                               join ca in db.ClassAccounts on a.Id equals ca.AccountId
                               where ca.ClassId == _task.ClassId && ca.Teacher == false
                               select a.Mail;
                string className = (from c in db.Classes
                                    where c.Id == _task.ClassId
                                    select c.Name).FirstOrDefault();
                // отправитель - устанавливаем адрес и отображаемое в письме имя
                MailAddress from = new MailAddress("unilife2023@yandex.ru", "Unilife_ClassRoom");
                // кому отправляем


                foreach (var l in listMail)
                {
                    MailAddress to = new MailAddress(l.ToString());//это адрес на который отправляем
                    MailMessage m = new MailMessage(from, to);
                    // тема письма
                    m.Subject = "Новое задание";
                    // текст письма
                    m.Body = $"Новое задание в классе {className}. {_task.Name}";
                    // письмо представляет код html
                    m.IsBodyHtml = true;
                    // адрес smtp-сервера и порт, с которого будем отправлять письмо
                    SmtpClient smtp = new SmtpClient("smtp.yandex.ru", 587);
                    // логин и пароль
                    smtp.Credentials = new NetworkCredential("unilife2023@yandex.ru", "ynunavzspenewerw");
                    smtp.EnableSsl = true;                                                   // создаем объект сообщения
                    smtp.Send(m);
                }
                return _task.Id;
            }
        }
        public Job GetJobUser(int userId, int taskId)
        {
            _db.Configuration.ProxyCreationEnabled = false;
            Job job = (from j in _db.Jobs
                       where j.TaskId == taskId && j.Deleted == false && j.Account.User.Id == userId
                       select j).FirstOrDefault();//Получение работы пользователя
            return job;
        }
        public Job GetJob(string hashKey, int taskId)
        {
            _db.Configuration.ProxyCreationEnabled = false;
            int? userId=(from s in _db.Sessions
                       where s.SessionKey == hashKey
                       select s.Account.UserId).FirstOrDefault();
            Job job = (from j in _db.Jobs
                       where j.TaskId == taskId && j.Deleted == false && j.Account.User.Id == userId
                       select j).FirstOrDefault();//Получение работы пользователя
            return job;
        }
        public List<Division> GetDivisions()
        {
            _db.Configuration.ProxyCreationEnabled = false;
            return _db.Divisions.ToList();//Возвращение списка подразделений
        }
        public int AddDivision(Division division)
        {
            _db.Divisions.Add(division);//Добавление подразделения в БД
            _db.SaveChanges();
            return division.Id;
        }
        public void UpdDivision(Division division)
        {
            _db.Entry(division).State = System.Data.Entity.EntityState.Modified;//Изменения подразделения в БД
            _db.SaveChanges();
        }
        public int AddMessage(MessagesTask messaage, string hashKey)
        {
            _db.Configuration.ProxyCreationEnabled = false;
            messaage.AccountId = (from a in _db.Accounts
                                  join s in _db.Sessions on a.Id equals s.AccountId
                                  where s.SessionKey == hashKey
                                  select a.Id).FirstOrDefault();

            _db.MessagesTasks.Add(messaage);//Добавление сообщения в БД
            _db.SaveChanges();

            return messaage.AccountId;
        }
        public List<MessagesTask> GetMessages(int taskId)
        {
            _db.Configuration.ProxyCreationEnabled = false;
            var res = (from m in _db.MessagesTasks
                       where m.TaskId == taskId
                       select m).ToList();//Получения списка сообщений, задания
            return res;
        }
        public List<User> GetUsersClass(int classId)
        {
            _db.Configuration.ProxyCreationEnabled = false;
            List<User> Users = new List<User>((from u in _db.Users
                                               join a in _db.Accounts on u.Id equals a.UserId
                                               join ca in _db.ClassAccounts on a.Id equals ca.AccountId
                                               join c in _db.Classes on ca.ClassId equals c.Id
                                               where ca.Teacher != true && c.Id == classId
                                               orderby u.Name
                                               select u).ToList());//Получение списка пользователей класса

            return Users;
        }
        public Account GetAccount(int accountId)
        {
            _db.Configuration.ProxyCreationEnabled = false;
            Account acc = (from a in _db.Accounts
                           where a.Id == accountId
                           select a).FirstOrDefault();//Получение аккаунта по id
            return acc;
        }
        public List<FilesTask> GetFileTask(int taskId)
        {
            _db.Configuration.ProxyCreationEnabled = false;
            var res = from f in _db.FilesTasks
                      where f.TaskId == taskId
                      select f;//Получение списка файлов задания по id задания.
            return res.ToList();
        }
        public List<LinksTask> GetLinksTask(int taskId)
        {
            _db.Configuration.ProxyCreationEnabled = false;
            var res = from f in _db.LinksTasks
                      where f.TaskId == taskId
                      select f;//Получение списка ссылок задания по id задания.
            return res.ToList();
        }
        public List<FilesJob> GetFilesJob(int jobId)
        {
            _db.Configuration.ProxyCreationEnabled = false;
            var res = from f in _db.FilesJobs
                      where f.JobId == jobId
                      select f;//Получение списка файлов работ по id раблоты.
            return res.ToList();
        }
        public void UpdJob(Job job)
        {
            _db.Entry(job).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            ModelsDB.Task task = (from j in _db.Jobs
                                  join t in _db.Tasks on j.TaskId equals t.Id
                                  where j.Id == job.Id
                                  select t).FirstOrDefault();
            Account acc = (from j in _db.Jobs
                           join a in _db.Accounts on j.AccountId equals a.Id
                           where j.Id == job.Id
                           select a).FirstOrDefault();
            MailAddress from = new MailAddress("unilife2023@yandex.ru", "Unilife_ClassRoom");

            MailAddress to = new MailAddress(acc.Mail.ToString());//это адрес на который отправляем
            MailMessage m = new MailMessage(from, to);
            // тема письма
            m.Subject = $"Отметка за работу. Задание {task.Name}";
            // текст письма
            m.Body = $"Вам {job.Score} за работу по заданию {task.Name}";
            // письмо представляет код html
            m.IsBodyHtml = true;
            // адрес smtp-сервера и порт, с которого будем отправлять письмо
            SmtpClient smtp = new SmtpClient("smtp.yandex.ru", 587);
            // логин и пароль
            smtp.Credentials = new NetworkCredential("unilife2023@yandex.ru", "ynunavzspenewerw");
            smtp.EnableSsl = true;                                                   // создаем объект сообщения
            smtp.Send(m);
        }
        public List<Post> GetPosts()
        {
            _db.Configuration.ProxyCreationEnabled = false;
            return _db.Posts.ToList();//Получение списка должностей.
        }
        public int AddPost(Post post)
        {
            _db.Posts.Add(post);//Добавление должности
            _db.SaveChanges();
            return post.Id;
        }
        public void UpdPost(Post post)
        {
            _db.Entry(post).State = System.Data.Entity.EntityState.Modified;//Изменение должности
            _db.SaveChanges();
        }
        public List<Session> GetSessionsAccount(int accountId)
        {
            _db.Configuration.ProxyCreationEnabled = false;
            var res = from s in _db.Sessions
                      where s.AccountId == accountId
                      select s;//Получение списка сессий для аккаунта
            return res.ToList();
        }
     
        public void DelSession(Session session)
        {
            Session ses = (from s in _db.Sessions
                          where s.SessionKey == session.SessionKey
                          select s).FirstOrDefault();
            _db.Sessions.Remove(ses);// удаление сессии
            _db.SaveChanges();
        }
        public int AddUser(User user)
        {
            _db.Users.Add(user);//Добавление пользователя
            _db.SaveChanges();
            return user.Id;
        }
        public void UpdUser(User user)
        {       
            _db.Entry(user).State = System.Data.Entity.EntityState.Modified;//изменение пользователя
            _db.SaveChanges();
        }
        public bool AddUserToClass(string hashKey, string key)
        {
            var resClass = (from c in _db.Classes
                            where c.KeyClass == key
                            select c).FirstOrDefault();//Получение класса
            if (resClass != null)
            {
                Account resAccount = (from a in _db.Accounts
                                      join s in _db.Sessions on a.Id equals s.AccountId
                                      where s.SessionKey == hashKey
                                      select a).Single();//Получение аккаунта

                var check = from c in _db.ClassAccounts
                            where c.Account.Id == resAccount.Id && c.ClassId == resClass.Id
                            select c;//Проверка есть ли данный аккаунт в данном классе
                if (check.Count() == 0)//если нет, то добавляеться связь между аккаунтом и классом
                {
                    ClassAccount classAccount = new ClassAccount();
                    classAccount.Class = resClass;

                    classAccount.Account = resAccount;
                    classAccount.Teacher = false;
                    _db.ClassAccounts.Add(classAccount);
                    _db.SaveChanges();
                    return true;
                }             
            }
            return false;
        }
        public List<Class> GetClasses()
        {
            _db.Configuration.ProxyCreationEnabled = false;
            return _db.Classes.ToList();
        }
        public List<ClassAccount> GetClassAccounts(int id)
        {
            _db.Configuration.ProxyCreationEnabled = false;
            var res = from a in _db.ClassAccounts
                      where a.ClassId == id
                      select a;
            return res.ToList();
        }
        public int AddClass(Class class1)
        {
            _db.Classes.Add(class1);
            _db.SaveChanges();
            return class1.Id;
        }
        public void UpdClass(Class class1)
        {
            _db.Entry(class1).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
        }
        public int AddAccountToClass(ClassAccount classAccount)
        {
            _db.ClassAccounts.Add(classAccount);
            _db.SaveChanges();
            return classAccount.Id;
        }
        public void UpdAccountToClass(ClassAccount classAccount)
        {
            _db.Entry(classAccount).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
        }
        public int AddJob(Job job,string hashKey)
        {
            _db.Configuration.ProxyCreationEnabled = false;
            job.AccountId=(from s in _db.Sessions
                          where s.SessionKey==hashKey
                          select s.AccountId).FirstOrDefault();
            _db.Jobs.Add(job);
            _db.SaveChanges();
            return job.Id;
        }
        public void DelAccountToClass(ClassAccount classAccount)
        {
            ClassAccount ses = (from s in _db.ClassAccounts
                                where s.ClassId == classAccount.ClassId
                                && s.AccountId == classAccount.AccountId
                           select s).FirstOrDefault();
            _db.ClassAccounts.Remove(ses);
            _db.SaveChanges();
        }

        public Division GetDivision(int divisionId)
        {
            _db.Configuration.ProxyCreationEnabled = false;
            Division division=(from s in _db.Divisions
                              where s.Id == divisionId
                              select s).FirstOrDefault();
            return division;
        }
        public Post GetPost(int postId)
        {
            _db.Configuration.ProxyCreationEnabled = false;
            Post post = (from s in _db.Posts
                             where s.Id == postId
                             select s).FirstOrDefault();

            return post;
        }
        StringBuilder Generator()// Генератор кодов
        {
            StringBuilder _key = new StringBuilder();
            Random random = new Random();
            var rand = new Random();
            int y;

            for (int i = 0; i < 24; i++)
            {
                y = rand.Next(1, 3);
                if (y == 1)
                {
                    _key.Append(Convert.ToChar(rand.Next(97, 124)));
                }
                else
                {
                    _key.Append(rand.Next(1, 10));
                }
            }
            return _key;
        }
    }
}
