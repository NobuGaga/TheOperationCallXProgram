using UnityEngine;

/// <summary>
/// MB class explain script's life cycle function
/// Awake() -> OnEnable() -> Start() -> OnDisable() -> OnDestroy()
/// </summary>
public abstract class LogicScript:MonoBehaviour {
    protected abstract void Reset();

    /// <summary>
    /// unity run function in first time loading MB component(also component disactive)
    /// </summary>
    protected abstract void Awake();

    /// <summary>
    /// enable component run function, first time after Awake()
    /// </summary>
    protected abstract void OnEnable();

    /// <summary>
    /// in first time loading MB component after OnEnable()
    /// </summary>
    protected abstract void Start();

    /// <summary>
    /// physics updaste function
    /// </summary>
    protected abstract void FixedUpdate();

    /// <summary>
    /// frame update function
    /// </summary>
    protected abstract void Update();

    /// <summary>
    /// frame update function after Update()
    /// </summary>
    protected abstract void LateUpdate();

    /// <summary>
    /// disable component run function
    /// </summary>
    protected abstract void OnDisable();

    /// <summary>
    /// destroy node or function run function after OnDisable()
    /// </summary>
    protected abstract void OnDestroy();
}
