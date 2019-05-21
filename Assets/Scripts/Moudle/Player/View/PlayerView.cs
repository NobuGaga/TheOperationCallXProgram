public class PlayerView:View {
    public PlayerView(GameMoudle moudle, GameView view, UIPrefab prefab) : base(moudle, view, prefab) {
        GetNode<UIImage>("hpProcess").FillAmountX = 1;
    }

    public void UpdateHP(float hp) {
        GetNode<UIImage>("hpProcess").FillAmountX = hp;
    }
}