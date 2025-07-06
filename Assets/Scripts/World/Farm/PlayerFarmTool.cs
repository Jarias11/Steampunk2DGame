using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
public class PlayerFarmTool : MonoBehaviour {
    public Tilemap tilemap; // reference to farm tilemap
    public Tilemap highlightTilemap;
    private Tile highlightTile;
    private Vector3Int? lastHighlightedCell = null;

    IEnumerator Start() {
        yield return new WaitForSeconds(0.2f); // Wait a short time to ensure tilemap is loaded

        if (tilemap == null) {
            GameObject obj = GameObject.FindGameObjectWithTag("FarmGround");
            if (obj != null) {
                tilemap = obj.GetComponent<Tilemap>();
            }
            else {
                Debug.LogWarning("❌ Farm tilemap not found — is tag assigned?");
            }
        }
        GameObject highlightObj = GameObject.FindGameObjectWithTag("FarmHighlight");
        if (highlightObj != null) {
            highlightTilemap = highlightObj.GetComponent<Tilemap>();

            // Dynamically create a blank tile with a tinted color
            highlightTile = ScriptableObject.CreateInstance<Tile>();
            highlightTile.color = new Color(1f, 0.84f, 0f, 0.4f);
            // Generate a full 32x32 white texture at runtime
            Texture2D tex = new Texture2D(32, 32);
            Color fill = Color.white;
            Color[] pixels = new Color[32 * 32];
            for (int i = 0; i < pixels.Length; i++) pixels[i] = fill;
            tex.SetPixels(pixels);
            tex.Apply();

            highlightTile.sprite = tex.ToSprite(new Vector2(0.5f, 0.5f), 32f); // Use correct PPU // add helper below
            highlightTile.transform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
        }
        else {
            Debug.LogWarning("❌ Highlight tilemap not found.");
        }
    }
    void Update() {
        if (tilemap == null || highlightTilemap == null || highlightTile == null) return;

        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cell = tilemap.WorldToCell(mouseWorld);
        Vector3 playerFootPos = GetPlayerFootTile();

        // HIGHLIGHT TILE
        if (FarmTileManager.Instance.IsInInteractableRange(playerFootPos, cell)) {
            if (lastHighlightedCell != cell) {
                ClearHighlight();
                highlightTilemap.SetTile(cell, highlightTile);
                lastHighlightedCell = cell;
            }
        }
        else {
            ClearHighlight();
        }

        // TILL TILE
        if (Input.GetMouseButtonDown(1)) {
            FarmTileManager.Instance.TillTile(cell, playerFootPos);
        }
    }

    private void ClearHighlight() {
        if (highlightTile == null) {
            Debug.Log("Highlight tile is still null!");
        }
        if (lastHighlightedCell.HasValue) {
            highlightTilemap.SetTile(lastHighlightedCell.Value, null);
            lastHighlightedCell = null;
        }
    }
    private Vector3Int GetPlayerFootTile() {
        Vector3 footPos = transform.position + new Vector3(0f, -0.25f, 0f); // tweak if needed
        return tilemap.WorldToCell(footPos);
    }
}
public static class TextureExtensions {
    public static Sprite ToSprite(this Texture2D tex, Vector2 pivot = default, float pixelsPerUnit = 32f) {
        if (pivot == default) pivot = new Vector2(0.5f, 0.5f);
        return Sprite.Create(
            tex,
            new Rect(0, 0, tex.width, tex.height),
            pivot,
            pixelsPerUnit
        );
    }
}