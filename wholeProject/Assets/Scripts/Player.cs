using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] int cruHP;
    [SerializeField] int maxHp;
    [SerializeField] int coins;
    [SerializeField] bool hasKey;
    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField] LayerMask moveLayerMask;

    private void Move(Vector2 dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 1f, moveLayerMask );

        if (hit.collider == null)
        {
            transform.position += new Vector3(dir.x, dir.y, 0);
            EnemyManager.Instance.OnPlayerMove();
            Generation.Instance.OnPlayerMove();
        }
    }

    public void OnMoveUp (InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            Move(Vector2.up);
        }
    }

    public void OnMoveDown(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Move(Vector2.down);
        }
    }
    public void OnMoveLeft(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Move(Vector2.left);
        }
    }
    public void OnMoveRight(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            Move(Vector2.right);
        }
    }
    public void OnAttackUp(InputAction.CallbackContext context)
    {
       if (context.phase == InputActionPhase.Performed) { TryAttack(Vector2.up); }
    }
    public void OnAttackDown(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) { TryAttack(Vector2.down); }
    }
    public void OnAttackLeft(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) { TryAttack(Vector2.left); }
    }
    public void OnAttackRight(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) { TryAttack(Vector2.right); }
    }

    public void TakeDamage(int damageToTake)
    {
         cruHP -= damageToTake;

        UI.instance.UpdateHealth(cruHP);

        StartCoroutine(DamageFlash());

        if (cruHP <= 0)
        {
            SceneManager.LoadScene(0);
        }
    }

    IEnumerator DamageFlash()
    {
        Color defaultColor = spriteRenderer.color;
        spriteRenderer.color = Color.white;

        yield return new WaitForSeconds(0.05f);

        spriteRenderer.color = defaultColor;
    }

    void TryAttack(Vector2 dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 1.0f, 1 << 8);

        if (hit.collider != null)
        {
            hit.transform.GetComponent<Enemy>().TakeDamage(1);
        }
    }


    public void AddCoins(int amount)
    {
        coins += amount;
        UI.instance.UpdateCoinText(coins);
    }

    public void RecoverLife(int amount)
    {
        if (cruHP < maxHp)
        {
            cruHP += amount;
            UI.instance.UpdateHealth(cruHP);
        }

        if (cruHP > maxHp)
        {
            cruHP = maxHp;
        }
    }

    public void GetKey()
    {
        hasKey = true;
    }

    public bool HasTheKey()
    {
        return hasKey;
    }

    public int GetCurrentHealth()
    {
        return cruHP;
    }

    public int GetCurrentCoins()
    {
        return coins;
    }

    public void SetCoins(int targetCoin)
    {
        coins = targetCoin;
    }

    public void SetHealth(int targetHealth)
    {
        cruHP = targetHealth;
    }
}
