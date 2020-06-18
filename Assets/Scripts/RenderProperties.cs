using UnityEngine;

[CreateAssetMenu(fileName = "Tech Draw Settings",menuName ="Tech Drawing/Settings Script. Obj.",order =1)]
public class RenderProperties : ScriptableObject
{
    public Color color;
    public Material material;
    public float width = 0.2f;
}
