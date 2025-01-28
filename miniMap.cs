using UnityEngine;

public class MinimapController : MonoBehaviour
{
    public RectTransform playerMarker; // ミニマップ上のマーカー
    public Transform player;          // プレイヤー（キャラクター）のTransform
    public RectTransform minimap;     // ミニマップのRectTransform

    public Vector2 mapSize = new Vector2(10, 10); // ワールド上の地図サイズ（X=幅, Y=高さ）

    void Update()
    {
        // ワールド座標を取得
        Vector3 playerPosition = player.position;

        // ワールド座標を0～1の範囲に正規化
        float normalizedX = (playerPosition.x + mapSize.x / 2) / mapSize.x;
        float normalizedZ = (playerPosition.z + mapSize.y / 2) / mapSize.y;

        // ミニマップのローカル座標に変換
        float markerX = normalizedX * minimap.rect.width - minimap.rect.width / 2;
        float markerY = normalizedZ * minimap.rect.height - minimap.rect.height / 2;

        // マーカーの位置を更新
        playerMarker.anchoredPosition = new Vector2(markerX, markerY);
    }


void OnGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.Label("");
        GUILayout.EndVertical(); // EndVerticalを忘れるとエラーが発生
    }

}
