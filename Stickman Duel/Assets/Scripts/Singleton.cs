using UnityEngine;

public class Singleton<Instance> : MonoBehaviour where Instance : Singleton<Instance> {
    public static Instance instance;
    [SerializeField]
    bool isPersistant = false;

    public virtual void Awake() {
        if (isPersistant) {
            if (!instance) {
                instance = this as Instance;
                DontDestroyOnLoad(gameObject);
            } else Destroy(gameObject);
        } else instance = this as Instance;
    }
}