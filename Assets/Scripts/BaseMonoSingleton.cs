using UnityEngine;

public abstract class BaseMonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance != null)
            {
                return _instance;
            }
            
            GameObject go = new GameObject($"{typeof(T)} (Singleton)");
            DontDestroyOnLoad(go);
            _instance = go.AddComponent<T>();

            return _instance;
        }
    }
}
