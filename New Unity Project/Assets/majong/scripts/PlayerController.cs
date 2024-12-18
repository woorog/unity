using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    private Renderer tileRenderer;
    private Color originalColor;

    void Start()
    {
        if (!IsLocalPlayer)
        {
            enabled = false; // 로컬 플레이어가 아니면 Update 비활성화
            return;
        }

        tileRenderer = GetComponent<Renderer>();
        originalColor = tileRenderer.material.color;
    }

    void Update()
    {
        // 로컬 플레이어만 입력 처리
        if (Input.GetMouseButtonDown(0))
        {
            CheckTileClick();
        }
    }

    void CheckTileClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // 클릭한 타일이 자신인지 확인
            if (hit.collider.gameObject == gameObject)
            {
                OnTileClickedServerRpc(NetworkManager.Singleton.LocalClientId);
            }
        }
    }

    [ServerRpc] // 서버에 클릭 이벤트 전달
    void OnTileClickedServerRpc(ulong clientId)
    {
        Debug.Log($"Server: Player {clientId} clicked {gameObject.name}");
        UpdateTileStateClientRpc(clientId);
    }

    [ClientRpc] // 모든 클라이언트에 상태 업데이트
    void UpdateTileStateClientRpc(ulong clientId)
    {
        if (NetworkManager.Singleton.LocalClientId == clientId)
        {
            // 클릭한 클라이언트의 타일 색상 변경
            tileRenderer.material.color = Color.red;
            Debug.Log($"Client {clientId}: {gameObject.name} was clicked and updated!");
        }
    }
}
