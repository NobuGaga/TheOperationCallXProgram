using UnityEngine;

public class Game : LogicScript {
    protected override void Reset() {
        Debug.Log("StartGame::Reset");
    }

    protected override void Awake() {
        Debug.Log("StartGame::Awake");
    }

    protected override void OnEnable() {
        Debug.Log("StartGame::OnEnable");
    }

    protected override void Start() {
        //Debug.Log("StartGame::Start");
        GameManager.StartGame(this);
    }

    protected override void FixedUpdate() {
        //Debug.Log("StartGame::FixedUpdate");
    }

    protected override void Update() {
        //Debug.Log("StartGame::Update");
        GameManager.UpdateGame();
    }

    protected override void LateUpdate() {
        //Debug.Log("StartGame::LateUpdate");
    }

    protected override void OnDisable() {
        Debug.Log("StartGame::OnDisable");
    }

    protected override void OnDestroy() {
        Debug.Log("StartGame::OnDestroy");
    }
}
