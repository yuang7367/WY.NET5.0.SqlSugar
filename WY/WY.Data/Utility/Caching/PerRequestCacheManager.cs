using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace WY.Data.Utility.Caching
{
    public partial class PerRequestCacheManager : ICacheManager
    {
        #region Ctor

        public PerRequestCacheManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            _locker = new ReaderWriterLockSlim();
        }

        #endregion

        #region Utilities

        public static readonly IDictionary<object, object> appCache = new Dictionary<object, object>();

        protected virtual IDictionary<object, object> GetItems()
        {
            return appCache ?? new Dictionary<object, object>();
        }

        #endregion

        #region Fields

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ReaderWriterLockSlim _locker;

        #endregion

        #region Methods

        /// <summary>
        /// 获取缓存项。如果它还不在缓存中，则加载并缓存它
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="GetDataFunc"></param>
        /// <param name="cacheTime"></param>
        /// <returns>与指定键关联的缓存值</returns>
        public virtual T Get<T>(string key, Func<T> GetDataFunc = null, int? cacheTime = null)
        {
            IDictionary<object, object> items;

            using (new ReaderWriteLockDisposable(_locker, ReaderWriteLockType.Read))
            {
                items = GetItems();


                if (items.TryGetValue(key, out object cacheData))
                {
                    return (T)cacheData;
                }
            }

            T data = default(T);

            if (GetDataFunc != null)
            {
                //todo 耗时过长 优化
                data = GetDataFunc();
            }

            if (data == null || (cacheTime ?? CachingDefaults.CacheTime) <= 0)
                return data;

            //并在缓存中设置（如果定义了缓存时间）
            using (new ReaderWriteLockDisposable(_locker))
            {
                //items[key] = data;
                items.TryAdd(key, (object)data);
            }

            return data;
        }

        /// <summary>
        /// 将指定的键和对象添加到缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="cacheTime"></param>
        public virtual void Set(string key, object data, int cacheTime = 0)
        {
            if (data == null)
            {
                return;
            }
            if (cacheTime == 0)
            {
                return;
            }


            using (new ReaderWriteLockDisposable(_locker))
            {
                var items = GetItems();
                if (items == null)
                    return;

                items.TryAdd(key, data);
            }
        }

        /// <summary>
        /// 获取一个值，该值指示是否缓存与指定键关联的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns>如果项已在缓存中，则为True；否则为false</returns>
        public virtual bool IsSet(string key)
        {
            using (new ReaderWriteLockDisposable(_locker, ReaderWriteLockType.Read))
            {
                var items = GetItems();

                return items.TryGetValue(key, out object cacheData);
            }
        }

        /// <summary>
        /// 从缓存中删除具有指定键的值
        /// </summary>
        /// <param name="key"></param>
        public virtual void Remove(string key)
        {
            using (new ReaderWriteLockDisposable(_locker))
            {
                var items = GetItems();
                items?.Remove(key);
            }
        }

        /// <summary>
        /// 按键前缀删除项
        /// </summary>
        /// <param name="prefix"></param>
        public virtual void RemoveByPrefix(string prefix)
        {
            using (new ReaderWriteLockDisposable(_locker, ReaderWriteLockType.UpgradeableRead))
            {
                var items = GetItems();
                if (items == null)
                    return;

                //获取匹配模式的缓存键
                var regex = new Regex(prefix,
                    RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
                var matchesKeys = items.Keys.Select(p => p.ToString()).Where(key => regex.IsMatch(key)).ToList();

                if (!matchesKeys.Any())
                    return;

                using (new ReaderWriteLockDisposable(_locker))
                {
                    //删除匹配值
                    foreach (var key in matchesKeys)
                    {
                        items.Remove(key);
                    }
                }
            }
        }

        /// <summary>
        /// 清除所有缓存数据
        /// </summary>
        public virtual void Clear()
        {
            using (new ReaderWriteLockDisposable(_locker))
            {
                var items = GetItems();
                items?.Clear();
            }
        }

        /// <summary>
        /// 释放缓存管理器
        /// </summary>
        public virtual void Dispose()
        {
            //nothing special
        }

        #endregion
    }
}
