using UnityEngine;
using UnityEngine.Tilemaps;

public class TileChecker : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;

    private void Start()
    {
        print(_tilemap.GetTile(_tilemap.WorldToCell(transform.position)).name);
    }
}
