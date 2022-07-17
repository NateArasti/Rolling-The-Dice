using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileTransformAnimator : MonoBehaviour
{
    [Header("Bridge Tile Animation")]
    [SerializeField] private AnimationCurve _bridgeTileCurve;
    [SerializeField] private float _bridgeTileDuration;

    public void StartBridgeAnimation(Tilemap tilemap, Vector3Int position)
    {
        StartCoroutine(TileScaleAnimation(tilemap, position, _bridgeTileCurve, _bridgeTileDuration));
    }

    private IEnumerator TileScaleAnimation(Tilemap tilemap, Vector3Int position, AnimationCurve curve, float duration)
    {
        var currentTime = 0f;
        while (currentTime <= duration)
        {
            var t = currentTime / duration;
            var matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one * curve.Evaluate(t));
            tilemap.SetTransformMatrix(position, matrix);

            yield return null;
            currentTime += Time.deltaTime;
        }
    }
}
