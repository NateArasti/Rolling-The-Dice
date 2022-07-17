using System;
using System.Collections.Generic;
using UnityEngine;

public class DiceColoring : MonoBehaviour
{
    [SerializeField] private DiceColoringPreset _coloringPreset;
    [Header("Sides")]
    [SerializeField] private SpriteRenderer _farSide;
    [SerializeField] private SpriteRenderer _topSide;
    [SerializeField] private SpriteRenderer _leftSide;
    [SerializeField] private SpriteRenderer _closeSide;
    [SerializeField] private SpriteRenderer _rightSide;
    [SerializeField] private SpriteRenderer _downSide;
    [Header("UI")]
    [SerializeField] private UIDiceColoring _uiColoring;

    private readonly Dictionary<Transform, SpriteRenderer> _sidesTransforms = new();

    private void Start()
    {
        _sidesTransforms[_farSide.transform] = _farSide;
        _sidesTransforms[_topSide.transform] = _topSide;
        _sidesTransforms[_leftSide.transform] = _leftSide;
        _sidesTransforms[_closeSide.transform] = _closeSide;
        _sidesTransforms[_rightSide.transform] = _rightSide;
        _sidesTransforms[_downSide.transform] = _downSide;
        FixUIColoring();
    }

    public void FixUIColoring()
    {
        var currentFarSide = transform;
        foreach (var side in _sidesTransforms.Keys)
        {
            if (side.position.z > currentFarSide.position.z)
                currentFarSide = side;
        }
        var farSprite = _sidesTransforms[currentFarSide].sprite;

        var currentTopSide = transform;
        foreach (var side in _sidesTransforms.Keys)
        {
            if (side.position.y > currentTopSide.position.y)
                currentTopSide = side;
        }
        var topSprite = _sidesTransforms[currentTopSide].sprite;

        var currentLeftSide = transform;
        foreach (var side in _sidesTransforms.Keys)
        {
            if (side.position.x < currentLeftSide.position.x)
                currentLeftSide = side;
        }
        var leftSprite = _sidesTransforms[currentLeftSide].sprite;

        var currentCloseSide = transform;
        foreach (var side in _sidesTransforms.Keys)
        {
            if (side.position.z < currentCloseSide.position.z)
                currentCloseSide = side;
        }
        var closeSprite = _sidesTransforms[currentCloseSide].sprite;

        var currentRightSide = transform;
        foreach (var side in _sidesTransforms.Keys)
        {
            if (side.position.x > currentRightSide.position.x)
                currentRightSide = side;
        }
        var rightSprite = _sidesTransforms[currentRightSide].sprite;

        var currentDownSide = transform;
        foreach (var side in _sidesTransforms.Keys)
        {
            if (side.position.y < currentDownSide.position.y)
                currentDownSide = side;
        }
        var downSprite = _sidesTransforms[currentDownSide].sprite;

        UIDiceColoring.SetUIColoring(
            farSprite,
            topSprite,
            leftSprite,
            closeSprite,
            rightSprite,
            downSprite
            );
    }

    public int GetFutureTopSideCount(Direction direction)
    {
        Sprite chosenSprite;
        var currentBottomSide = transform;
        switch (direction)
        {
            case Direction.Up:
                foreach (var side in _sidesTransforms.Keys)
                {
                    if (side.position.y < currentBottomSide.position.y)
                        currentBottomSide = side;
                }
                chosenSprite = _sidesTransforms[currentBottomSide].sprite;
                break;
            case Direction.Down:
                foreach (var side in _sidesTransforms.Keys)
                {
                    if (side.position.y > currentBottomSide.position.y)
                        currentBottomSide = side;
                }
                chosenSprite = _sidesTransforms[currentBottomSide].sprite;
                break;
            case Direction.Right:
                foreach (var side in _sidesTransforms.Keys)
                {
                    if (side.position.x < currentBottomSide.position.x)
                        currentBottomSide = side;
                }
                chosenSprite = _sidesTransforms[currentBottomSide].sprite;
                break;
            case Direction.Left:
                foreach (var side in _sidesTransforms.Keys)
                {
                    if (side.position.x > currentBottomSide.position.x)
                        currentBottomSide = side;
                }
                chosenSprite = _sidesTransforms[currentBottomSide].sprite;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }

        if (int.TryParse(chosenSprite.name, out var result))
            return result;

        throw new UnityException($"Sprite name is not a number!!! - {chosenSprite.name}");
    }

    private void OnValidate()
    {
        SetColoringPreset();
    }

    private void SetColoringPreset()
    {
        if(_coloringPreset == null) return;
        _farSide.sprite = _coloringPreset.FarSide;
        _topSide.sprite = _coloringPreset.TopSide;
        _leftSide.sprite = _coloringPreset.LeftSide;
        _closeSide.sprite = _coloringPreset.CloseSide;
        _rightSide.sprite = _coloringPreset.RightSide;
        _downSide.sprite = _coloringPreset.DownSide;
        if(_uiColoring != null)
        {
            _uiColoring.SetUIEditColoring(
                _coloringPreset.FarSide,
                _coloringPreset.TopSide,
                _coloringPreset.LeftSide,
                _coloringPreset.CloseSide,
                _coloringPreset.RightSide,
                _coloringPreset.DownSide
            );
        }
    }
}
