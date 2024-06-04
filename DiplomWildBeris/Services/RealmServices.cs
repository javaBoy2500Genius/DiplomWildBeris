using DiplomWildBeris.Helpers;
using DiplomWildBeris.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Realms;
using System.Linq.Expressions;
using System.Reflection;


namespace DiplomWildBeris.Services
{
    public interface IRealmService<T> where T : RealmObject
    {
        Task<IQueryable<T>> GetItemsAsync(Expression<Func<T, bool>> expression);
        Task<IQueryable<T>> GetItemsAsync();
        Task AddItemAsync(T item);
        Task DeleteItemAsync(string id);
        Realm GetInstance();

    }
    public interface ISeed
    {
        Task SeedData();
        Task AddItemsAsync<Gn>(List<Gn> items) where Gn : RealmObject;

    }
    public class RealmService<T> : ISeed, IRealmService<T> where T : RealmObject
    {
        private readonly Realm _realm;
        private readonly SynchronizationContext _context;
        private readonly IConfiguration? _configuration;
        private readonly IUserService? _userService;
        public RealmService(IConfiguration config, IUserService userService)
        {
            _realm = Realm.GetInstance();
            _context = SynchronizationContext.Current ?? new SynchronizationContext();
            _configuration = config;
            _userService = userService;

        }

        public async Task<IQueryable<T>> GetItemsAsync(Expression<Func<T, bool>> expression)
        {
            IQueryable<T> res = null!;
            await RunOnMainThreadAsync(() =>
            {
                res = _realm.All<T>().Where(expression);
            });
            return res;
        }

        public async Task<IQueryable<T>> GetItemsAsync()
        {
            IQueryable<T> res = null!;
            await RunOnMainThreadAsync(() =>
            {
                res = _realm.All<T>();
            });
            return res;
        }

        public async Task AddItemAsync(T item)
        {
            await RunOnMainThreadAsync(() =>
            {
                _realm.Write(() =>
                {
                    _realm.Add(item);
                });
            });
        }
        public async Task AddItemsAsync<Gn>(List<Gn> items) where Gn : RealmObject
        {
            await RunOnMainThreadAsync(() =>
            {
                _realm.Write(() =>
                {
                    _realm.Add(items);
                });
            });
        }



        public async Task DeleteItemAsync(string id)
        {
            await RunOnMainThreadAsync(() =>
            {
                var item = _realm.Find<T>(id);
                if (item != null)
                {
                    _realm.Write(() =>
                    {
                        _realm.Remove(item);
                    });
                }
            });
        }

        public Task RunOnMainThreadAsync(Action action)
        {
            var tcs = new TaskCompletionSource<bool>();

            _context.Post(_ =>
            {
                try
                {
                    action();
                    tcs.SetResult(true);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            }, null);

            return tcs.Task;
        }

        public Realm GetInstance() => _realm;

        public async Task SeedData()
        {
            try
            {

                Stream fileStream = await FileSystem.Current.OpenAppPackageFileAsync("seed_data.json");
                StreamReader sr = new StreamReader(fileStream);
                var json = await sr.ReadToEndAsync();
                var @namespace = $"{nameof(DiplomWildBeris)}.{nameof(Models)}";
                var jobject = JsonConvert.DeserializeObject(json) as JObject ?? [];
                var classes = ClassInspector.GetClassNamesInNamespace(@namespace);



                foreach (var item in jobject)
                {
                    if (classes.Contains(item.Key))
                    {
                        if (item.Value?.Type is JTokenType.Array)
                        {
                            var objFullPath = $"{@namespace}.{item.Key}";
                            var objType = Type.GetType(objFullPath);

                            if (objType == null)
                                continue;

                            Type listType = typeof(List<>).MakeGenericType(objType);

                            var listInstance = Activator.CreateInstance(listType);

                            foreach (var val in item.Value)
                            {
                                var obj = Activator.CreateInstance(objType);
                                JsonConvert.PopulateObject(val.ToString(), obj);
                                if (objType.Name == nameof(User))
                                {
                                    var passwdProp = objType.GetProperty(nameof(User.Password));
                                    var oldPswd = passwdProp?.GetValue(obj)?.ToString() ?? "";
                                    passwdProp?.SetValue(obj, _userService?.Encrypt(oldPswd));
                                }


                                listType?.GetMethod("Add")?.Invoke(listInstance, new[] { obj });

                            }

                            var typedList = (System.Collections.IList)listInstance!;

                            await AddItemsAsync(typedList.Cast<RealmObject>().ToList());



                        }

                    }
                }


            }
            catch (Exception e)
            {
                throw e;
            }
        }



    }
}
