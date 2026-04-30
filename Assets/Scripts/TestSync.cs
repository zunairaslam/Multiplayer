using Fusion;
using UnityEngine;

public class TestSync : NetworkBehaviour
{

    [Networked, OnChangedRender(nameof(OnColorChanged))]
    public Color MyColor { get; set; }

    public void OnColorChanged()
    {
        GetComponent<Renderer>().material.color = MyColor;
    }

    private void Update()
    {
        //Debug.Log($"value = {myVal}");
    }

    [ContextMenu(nameof(IncrementValue))]
    public void IncrementValue()
    {
        if (HasStateAuthority)
        {
            MyColor = new Color(Random.value, Random.value, Random.value);
        }
    }
}