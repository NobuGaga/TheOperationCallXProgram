public class ModelPlayer:ModelRole {
    private void Start() {
        AddAnimation(State.Stand.ToString(), "DrawBlade");
        AddAnimation(State.Run.ToString(), "Run00");
        AddAnimation(State.ReadyFight.ToString(), "DrawBlade");
    }
}