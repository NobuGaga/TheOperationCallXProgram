public class Game:LogicScript {
    protected override void Reset() {
        DebugTool.Log("Game::Reset");
    }

    protected override void Awake() {
        DebugTool.Log("Game::Awake");
    }

    protected override void OnEnable() {
        DebugTool.Log("Game::OnEnable");
    }

    protected override void Start() {
        DebugTool.Log("Game::Start");
        GameManager.Start(this);
    }

    protected override void FixedUpdate() {
        GameManager.PhysicsUpdate();
    }

    protected override void Update() {
        GameManager.FrameUpdate();
    }

    protected override void LateUpdate() {
        GameManager.LastUpdate();
    }

    protected override void OnDisable() {
        DebugTool.Log("Game::OnDisable");
    }

    protected override void OnDestroy() {
        DebugTool.Log("Game::OnDestroy");
    }
}