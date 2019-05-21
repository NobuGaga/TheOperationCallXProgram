using UnityEngine.UI;

public class VirtualButtonView:View {
    public VirtualButtonView(GameMoudle moudle, GameView view, UIPrefab prefab):base(moudle, view, prefab) {
        GetNode<Button>("btnNormalAttack").onClick.AddListener(() => OnClick(SRoleAttack.AttackLevel.Normal));
        GetNode<Button>("btnSkiilOne").onClick.AddListener(() => OnClick(SRoleAttack.AttackLevel.SkillOne));
        GetNode<Button>("btnSkiilTwo").onClick.AddListener(() => OnClick(SRoleAttack.AttackLevel.SkillTwo));
        GetNode<Button>("btnSkiilThree").onClick.AddListener(() => OnClick(SRoleAttack.AttackLevel.SkillThree));
    }

    private void OnClick(SRoleAttack.AttackLevel level) {
        EventManager.Dispatch(GameMoudle.Player, GameEvent.Type.Attack, level);
    }
}