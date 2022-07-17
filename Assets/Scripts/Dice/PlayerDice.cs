using System;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(DiceColoring))]
public class PlayerDice : MonoBehaviour
{
    private const string HorizontalAxis = "Horizontal";
    private const string VerticalAxis = "Vertical";

    [Header("Scene References")]
    [SerializeField] private GridController _gridController;
    [Header("Movement Params")]
    [SerializeField] private Vector2Int _moveDelta;
    [SerializeField] private float _keyCheckCooldown = 0.5f;
    [Header("Audio")]
    [SerializeField] private AudioClip _moveSound;
    [SerializeField] private AudioClip _rolloverSound;
    private float _currentCooldown;
    private DiceColoring _coloring;

    private void Awake()
    {
        _coloring = GetComponent<DiceColoring>();
    }

    private void Update()
    {
        if(GameUIController.Paused) return;

        var horizontal = Input.GetAxis(HorizontalAxis);
        var vertical = Input.GetAxis(VerticalAxis);
        _currentCooldown -= Time.deltaTime;

        if (horizontal == 0 && vertical == 0 || _currentCooldown > 0) return;

        _currentCooldown = _keyCheckCooldown;

        Direction direction;
        if (horizontal > 0)
            direction = Direction.Right;
        else if (horizontal < 0)
            direction = Direction.Left;
        else if (vertical > 0)
            direction = Direction.Up;
        else
            direction = Direction.Down;

        if (Input.GetKey(KeyCode.Space) || Input.GetKeyDown(KeyCode.Space))
        {
            var bridgeTilesCount = _coloring.GetFutureTopSideCount(direction);
            AudioManager.PlaySound(_rolloverSound);
            RollToNeighborCell(direction);
            _gridController.AddBridgeTiles(transform.position, bridgeTilesCount, direction);
        }
        else
        {
            var futurePosition = GetNeighborPosition(direction);

            if (_gridController.HasBridgeAtPosition(futurePosition))
                MoveToPosition(futurePosition);
            else
                _currentCooldown *= 0.3f;
        }
    }

    private void MoveToPosition(Vector3 position)
    {
        AudioManager.PlaySound(_moveSound);
        var tween = transform.DOMove(position, _keyCheckCooldown * 0.5f).SetEase(Ease.InOutSine);
        tween.onComplete += CheckCurrentForegroundCell;
        tween.onComplete += _coloring.FixUIColoring;
    }

    private void CheckCurrentForegroundCell()
    {
        _gridController.CheckFinishAtPosition(transform.position);
        _gridController.CheckCoinAtPosition(transform.position);
    }

    private Vector3 GetNeighborPosition(Direction direction)
    {
        var newPosition = transform.position;
        newPosition += direction switch
        {
            Direction.Up => new Vector3(0, _moveDelta.y, 0),
            Direction.Down => new Vector3(0, -_moveDelta.y, 0),
            Direction.Right => new Vector3(_moveDelta.x, 0, 0),
            Direction.Left => new Vector3(-_moveDelta.x, 0, 0),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

        return newPosition;
    }

    private void RollToNeighborCell(Direction direction)
    {
        var rotation = direction switch
        {
            Direction.Up => new Vector3(90, 0, 0),
            Direction.Down => new Vector3(-90, 0, 0),
            Direction.Right => new Vector3(0, -90, 0),
            Direction.Left => new Vector3(0, 90, 0),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
        transform
            .DORotate(rotation, _keyCheckCooldown * 0.5f, RotateMode.WorldAxisAdd)
            .SetEase(Ease.InOutSine);

        MoveToPosition(GetNeighborPosition(direction));
    }
}
