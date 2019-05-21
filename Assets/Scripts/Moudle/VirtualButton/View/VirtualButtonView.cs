using UnityEngine.UI;

public class VirtualButtonView:View {
    public VirtualButtonView(GameMoudle moudle, GameView view, UIPrefab prefab):base(moudle, view, prefab) {
        GetNode<Button>("btnNormalAttack").onClick.AddListener(() => OnClick(ModelAttackLevel.Normal));
        GetNode<Button>("btnSkiilOne").onClick.AddListener(() => OnClick(ModelAttackLevel.SkillOne));
        GetNode<Button>("btnSkiilTwo").onClick.AddListener(() => OnClick(ModelAttackLevel.SkillTwo));
        GetNode<Button>("btnSkiilThree").onClick.AddListener(() => OnClick(ModelAttackLevel.SkillThree));
    }

    private void OnClick(ModelAttackLevel level) {
        EventManager.Dispatch(GameMoudle.Player, GameEvent.Type.Attack, level);
    }
}