using UnityEngine;

[CreateAssetMenu]
public class DiceColoringPreset : ScriptableObject
{
    [SerializeField] private Sprite _farSide;
    [SerializeField] private Sprite _topSide;
    [SerializeField] private Sprite _leftSide;
    [SerializeField] private Sprite _closeSide;
    [SerializeField] private Sprite _rightSide;
    [SerializeField] private Sprite _downSide;

    public Sprite FarSide => _farSide;

    public Sprite TopSide => _topSide;

    public Sprite LeftSide => _leftSide;

    public Sprite CloseSide => _closeSide;

    public Sprite RightSide => _rightSide;

    public Sprite DownSide => _downSide;
}
