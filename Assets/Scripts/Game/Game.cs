public class Game : LogicScript {
    protected override void Reset() {
        DebugTool.Log("StartGame::Reset");
    }

    protected override void Awake() {
        DebugTool.Log("StartGame::Awake");
    }

    protected override void OnEnable() {
        DebugTool.Log("StartGame::OnEnable");
    }

    protected override void Start() {
        //DebugTool.Log("StartGame::Start");
        GameManager.StartGame(this);
    }

    protected override void FixedUpdate() {
        //DebugTool.Log("StartGame::FixedUpdate");
    }

    protected override void Update() {
        //DebugTool.Log("StartGame::Update");
        GameManager.UpdateGame();
    }

    protected override void LateUpdate() {
        //DebugTool.Log("StartGame::LateUpdate");
    }

    protected override void OnDisable() {
        DebugTool.Log("StartGame::OnDisable");
    }

    protected override void OnDestroy() {
        DebugTool.Log("StartGame::OnDestroy");
    }
}
