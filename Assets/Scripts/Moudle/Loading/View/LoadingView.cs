public class LoadingView:View {
    public LoadingView(GameMoudle moudle, GameView view, UIPrefab prefab):base(moudle, view, prefab) {
        GetNode<UIImage>("loadingProcess").FillAmountX = 0;
    }

    public void UpdateLoadingProcess(float process) {
        GetNode<UIImage>("loadingProcess").FillAmountX = process;
    }
}