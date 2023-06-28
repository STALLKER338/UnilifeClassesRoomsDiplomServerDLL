using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using UnilifeClassesRoomsDiplomServerDLL.ModelsDB;

namespace UnilifeClassesRoomsDiplomServerDLL
{
    // ПРИМЕЧАНИЕ. Можно использовать команду "Переименовать" в меню "Рефакторинг", чтобы изменить имя интерфейса "IUnilifeClassesRoomsDiplomServerDDL" в коде и файле конфигурации.
    [ServiceContract]
    public interface IUnilifeClassesRoomsDiplomServerDDL//Интерфейс для службы WCF(через него осуществляеться взаимодействие клиента с сервером
    {
        [OperationContract]
        string LogInAccount(string login, string password);//OperationContract проверки данных для входа и отправки кода подтверждения
        [OperationContract]
        bool ConfirmLogInAccount(string hashKey, string mailkey);//OperationContract проверки кода подтверждения
        [OperationContract]
        bool CheakHashKey(string hashKey);//OperationContract проверке хеш-ключа для доступа к приложению
        [OperationContract]
        Account GetAccountUser(string hashKey);//OperationContract получения аккаунта пользователя
        [OperationContract]
        User GetUser(int? id);//OperationContract получения пользователя по id
        [OperationContract]
        List<Account> GetAccounts();//OperationContract получения списка аккаунтов
        [OperationContract]
        List<User> GetUsers();//OperationContract получения списка пользователей
        [OperationContract]
        List<Role> GetRoles();//OperationContract получения списка ролей
        [OperationContract]
        bool LogOut(string hashKey);//OperationContract для выхода из приложения и удаления ключа сессии
        [OperationContract]
        void UpdAccount(Account acc);//OperationContract для обновление данных аккаунта хранящтихся в БД
        [OperationContract]
        int GetPowerRole(string hashKey);//OperationContract для получения силы роли аккаунта
        [OperationContract]
        Role GetRole(int? id);//OperationContract для получения роли по id
        [OperationContract]
        int AddAccount(Account acc);//OperationContract для добавления аккаунта в БД
        [OperationContract]
        List<Class> GetClassesUser(string hashKey);//OperationContract для получения списка классов,пользователя
        [OperationContract]
        List<ModelsDB.Task> GetTasksClassFalse(int classId, string hashKey);//OperationContract получение заданий не выполненных пользователем, срок которых не истёк
        [OperationContract]
        List<ModelsDB.Task> GetTasksClass(int classId);//OperationContract получения списка заданий соддержащегося в классе
        [OperationContract]
        bool СheckingTeacher(int classId, string hashKey);//OperationContract проверки на учителя
        [OperationContract]
        List<Account> GetAccountsClass(int classId);//OperationContract получения списка аккаунтов привязанных к классу
        [OperationContract]
        int AddTask(ModelsDB.Task _task, string hashKey);//OperationContract добавления задания
        [OperationContract]
        Job GetJobUser(int userId, int taskId);//OperationContract получения данных о работе пользователя для формирования отчета
        [OperationContract]
        List<Division> GetDivisions();//OperationContract получения списка подразделений
        [OperationContract]
        int AddDivision(Division division);//OperationContract добавления подразделения
        [OperationContract]
        void UpdDivision(Division division); //OperationContract изменения подразделения
        [OperationContract]
        int AddMessage(MessagesTask messaage, string hashKey);//OperationContract добавления сообщения в задании
        [OperationContract]
        List<MessagesTask> GetMessages(int taskId);//OperationContract получения списка сообщений в задании
        [OperationContract]
        List<User> GetUsersClass(int classId);//OperationContract получения списка пользователей класса
        [OperationContract]
        Account GetAccount(int accountId);//OperationContract получения аккаунта по id
        [OperationContract]
        List<FilesTask> GetFileTask(int taskId); //OperationContract получения файлов задания
        [OperationContract]
        List<LinksTask> GetLinksTask(int taskId); //OperationContract получение ссылок задания
        [OperationContract]
        List<FilesJob> GetFilesJob(int jobId);//OperationContract получения файлов работы
        [OperationContract]
        List<Post> GetPosts(); //OperationContract получения списка должностей
        [OperationContract]
        List<Session> GetSessionsAccount(int accountId);//OperationContract получение списка сессий для аккаунта
        [OperationContract]
        int  AddPost(Post post); //OperationContract добавления должности
        [OperationContract]
        void UpdPost(Post post);//OperationContract изменения должности
        [OperationContract]
        void DelSession(Session session);//OperationContract удаления сессии
        [OperationContract]
        void UpdJob(Job job);//OperationContract изменение данных работы учащегося
        [OperationContract]
        int AddUser(User user);//OperationContract добавления пользователя
        [OperationContract]
        void UpdUser(User user);//OperationContract изменения пользователя
        [OperationContract]
        bool AddUserToClass(string hashKey, string key);//OperationContract добавления пользователся в класс по ключу класса
        [OperationContract]
        List<Class> GetClasses();
        [OperationContract]
        int AddClass(Class class1);
        [OperationContract]
        void UpdClass(Class class1);
        [OperationContract]
        List<ClassAccount> GetClassAccounts(int id);
        [OperationContract]
        int AddAccountToClass(ClassAccount classAccount);
        [OperationContract]
        void UpdAccountToClass(ClassAccount classAccount);
        [OperationContract]
        void DelAccountToClass(ClassAccount classAccount);
        [OperationContract]
        Division GetDivision(int divisionId);
        [OperationContract]
        Post GetPost(int postId);
        [OperationContract]
        Job GetJob(string hashKey, int taskId);
        [OperationContract]
        int AddJob(Job job, string hashKey);
    }
}
