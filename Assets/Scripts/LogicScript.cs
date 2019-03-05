using UnityEngine;

public abstract class LogicScript : MonoBehaviour {
    protected abstract void Reset();

    protected abstract void Awake();

    protected abstract void OnEnable();

    protected abstract void Start();

    protected abstract void FixedUpdate();

    protected abstract void Update();

    protected abstract void LateUpdate();

    protected abstract void OnDisable();

    protected abstract void OnDestroy();
}
