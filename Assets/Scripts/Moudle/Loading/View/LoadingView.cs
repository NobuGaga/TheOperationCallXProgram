public class LoadingView:View {
    public LoadingView(UIView view):base(view) {
        GetNode<UIImage>("loadingProcess").FillAmountX = 0;
    }

    public void UpdateLoadingProcess(float process) {
        GetNode<UIImage>("loadingProcess").FillAmountX = process;
    }
}