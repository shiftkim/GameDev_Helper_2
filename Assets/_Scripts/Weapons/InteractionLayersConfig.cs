using UnityEngine;

[CreateAssetMenu(fileName = "New Config", menuName = "Configs/Explosives/Layer config")]
public class InteractionLayersConfig : ScriptableObject
{
   public LayerMask triggerLayers;
   public LayerMask affectedLayers;
}
