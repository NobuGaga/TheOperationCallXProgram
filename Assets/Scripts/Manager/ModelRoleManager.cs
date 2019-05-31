using System.Collections.Generic;

public static class ModelRoleManager {
    private static Dictionary<RoleType, Dictionary<int, ModelRoleData>> dicModelData = new Dictionary<RoleType, Dictionary<int, ModelRoleData>>();
    private static Dictionary<string, ModelRoleData> dicPrefabNameModelData = new Dictionary<string, ModelRoleData>();

    public static void Init() {
        ModelRoleData data = new ModelRoleData(RoleType.Player, (int)PlayerType.Master, "Blade_Warrior_Prefab", 260);
        AddModelData(data);
        data = new ModelRoleData(RoleType.Monster, (int)MonsterType.Rubbish, "Monster_Warrior_Prefab", 260);
        AddModelData(data);
    }

    private static void AddModelData(ModelRoleData data) {
        RoleType roleType = data.roleType;
        int roleLevel = data.roleLevel;
        if (!dicModelData.ContainsKey(roleType))
            dicModelData.Add(roleType, new Dictionary<int, ModelRoleData>());
        if (dicModelData[roleType].ContainsKey(roleLevel))
            dicModelData[roleType][roleLevel] = data;
        else dicModelData[roleType].Add(roleLevel, data);
        if (dicPrefabNameModelData.ContainsKey(data.prefabName))
            dicPrefabNameModelData[data.prefabName] = data;
        else dicPrefabNameModelData.Add(data.prefabName, data);
    }

    public static ModelRoleData GetModelRoleData(string nodeName) {
        if (!dicPrefabNameModelData.ContainsKey(nodeName))
            DebugTool.LogError("ModelRoleManager::GetModelRoleData not exit prefab name\t" + nodeName);
        return dicPrefabNameModelData[nodeName];
    }

    public static int GetModelRoleHpPosY(string nodeName) {
        ModelRoleData data = GetModelRoleData(nodeName);
        return data.hpProcessPosY;
    }
}