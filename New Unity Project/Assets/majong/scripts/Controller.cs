using UnityEngine;

public class Controller : MonoBehaviour
{
    public Sprite[] sprites; // 스프라이트 배열 (Inspector에서 설정)
    private SpriteRenderer spriteRenderer;

    // 원하는 크기를 설정 (Square의 기준 크기)
    public Vector2 fixedSize = new Vector2(1f, 1f); 

    void Start()
    {
        // SpriteRenderer 컴포넌트를 가져옵니다.
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 초기 스프라이트 크기 고정
        if (spriteRenderer != null)
        {
            AdjustSpriteSize();
        }
    }

    void OnMouseDown()
    {
        if (sprites.Length > 0)
        {
            // 현재 스프라이트의 이름을 출력
            Debug.Log("현재 스프라이트: " + (spriteRenderer.sprite != null ? spriteRenderer.sprite.name : "None"));

            // 랜덤한 스프라이트 선택
            Sprite randomSprite = sprites[Random.Range(0, sprites.Length)];
            spriteRenderer.sprite = randomSprite;

            // 스프라이트 크기 고정
            AdjustSpriteSize();

            // 새로운 스프라이트 이름 출력
            Debug.Log("새로운 스프라이트로 변경: " + randomSprite.name);
        }
    }

    // 스프라이트 크기를 고정하는 함수
    void AdjustSpriteSize()
    {
        // SpriteRenderer의 DrawMode를 Sliced로 설정
        spriteRenderer.drawMode = SpriteDrawMode.Sliced;
        spriteRenderer.size = fixedSize; // 설정한 크기로 조정
    }
}
