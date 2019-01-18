
namespace pattern
{
    /// <summary>
    /// 泛型单利
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> where T : new()
    {
        private static T t = default(T);
        static Singleton()
        {
            if (t == null)
            {
                t = new T();
            }
        }

        public static T Instance
        {
            get
            {
                return t;
            }
        }

        public virtual void BeforeDeleMe()
        {

        }
        public  void DeleteMe()
        {
            BeforeDeleMe();
            if (t != null) t = default(T);
        }
    }
}

