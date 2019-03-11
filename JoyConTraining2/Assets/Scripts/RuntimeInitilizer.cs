using UnityEngine;

public class RuntimeInitilizer
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeBeforeSceneLoad()
    {
        // ゲーム中に常に存在するオブジェクトを生成、およびしーんの変更時にも破棄されないようにする
        var manager = new GameObject("JoyconManager", typeof(JoyconManager));
        GameObject.DontDestroyOnLoad(manager);
    }
}
