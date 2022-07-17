using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(TileTransformAnimator))]
public class GridController : MonoBehaviour
{
    public static readonly UnityEvent OnCurrentGridFinished = new();
    public static readonly UnityEvent OnCurrentGridFailed = new();

    [Header("Tilemaps")]
    [SerializeField] private Tilemap _backgroundTilemap;
    [SerializeField] private Tilemap _foregroundTilemap;
    [Header("Background Tiles")]
    [SerializeField] private TileBase _bridgeTile;
    [SerializeField] private TileBase _deadZoneTile;
    [Header("Foreground Tiles")]
    [SerializeField] private TileBase _coinTile;
    [SerializeField] private TileBase _finishTile;
    [SerializeField] private Transform _finishPivot;
    [SerializeField] private int _coinCount;
    [Header("Audio")]
    [SerializeField] private AudioClip _bridgeBuildSound;
    [SerializeField] private AudioClip _finishSound;
    [SerializeField] private AudioClip _failSound;
    [SerializeField] private AudioClip _coinSound;
    [Header("Effects")]
    [SerializeField] private ParticleSystem _failParticles;

    private TileTransformAnimator _tileAnimator;

    private Color _normalFinishColor;

    private void Awake()
    {
        _tileAnimator = GetComponent<TileTransformAnimator>();

        _foregroundTilemap.SetTileFlags(
            _foregroundTilemap.WorldToCell(_finishPivot.position),
            TileFlags.None);
        _normalFinishColor = _foregroundTilemap.GetColor(
            _foregroundTilemap.WorldToCell(_finishPivot.position)
        );

        _foregroundTilemap.SetColor(
            _foregroundTilemap.WorldToCell(_finishPivot.position),
            _coinCount != 0 ? new Color(1, 1, 1, 0.25f) : _normalFinishColor
        );
    }

    public void AddBridgeTiles(Vector2 startPosition, int count, Direction direction)
    {
        var mapPosition = _backgroundTilemap.WorldToCell(startPosition);
        var delta = direction switch
        {
            Direction.Up => new Vector3Int(0, 1),
            Direction.Down => new Vector3Int(0, -1),
            Direction.Right => new Vector3Int(1, 0),
            Direction.Left => new Vector3Int(-1, 0),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

        StartCoroutine(SpawnEnumerator());

        IEnumerator SpawnEnumerator()
        {
            for (var i = 0; i < count; i++)
            {
                var setPosition = mapPosition + i * delta;
                if (_backgroundTilemap.HasTile(setPosition))
                {
                    var tile = _backgroundTilemap.GetTile(setPosition);
                    if (tile == _bridgeTile)
                    {
                        count++;
                        continue;
                    }

                    if (tile == _deadZoneTile)
                    {
                        Instantiate(_failParticles, _backgroundTilemap.CellToWorld(setPosition) + new Vector3(0.5f, 0.5f), Quaternion.identity);
                        AudioManager.PlaySound(_failSound);
                        yield return CoroutineExtensions.Wait(0.5f);
                        OnCurrentGridFailed.Invoke();
                        yield break;
                    }
                }
                _backgroundTilemap.SetTile(setPosition, _bridgeTile);
                _tileAnimator.StartBridgeAnimation(_backgroundTilemap, setPosition);
                AudioManager.PlaySound(_bridgeBuildSound);
                yield return CoroutineExtensions.Wait(0.2f);
            }
        }
    }

    public void CheckCoinAtPosition(Vector2 position)
    {
        var mapPosition = _backgroundTilemap.WorldToCell(position);
        if (_foregroundTilemap.HasTile(mapPosition) &&
            _foregroundTilemap.GetTile(mapPosition) == _coinTile)
        {
            _foregroundTilemap.SetTile(mapPosition, null);
            _coinCount -= 1;

            _foregroundTilemap.SetColor(
                _foregroundTilemap.WorldToCell(_finishPivot.position),
                _coinCount != 0 ? new Color(1, 1, 1, 0.5f) : _normalFinishColor
            );

            AudioManager.PlaySound(_coinSound);
        }
    }

    public bool HasBridgeAtPosition(Vector2 position)
    {
        var mapPosition = _backgroundTilemap.WorldToCell(position);
        return _backgroundTilemap.HasTile(mapPosition) &&
               _backgroundTilemap.GetTile(mapPosition).name == _bridgeTile.name;
    }

    public bool CheckFinishAtPosition(Vector2 position)
    {
        var mapPosition = _foregroundTilemap.WorldToCell(position);
        var check = _foregroundTilemap.HasTile(mapPosition) &&
                    _foregroundTilemap.GetTile(mapPosition) == _finishTile &&
                    _coinCount == 0;
        if (check)
        {
            OnCurrentGridFinished.Invoke();
            AudioManager.PlaySound(_finishSound);
        }
        return check;
    }
}
