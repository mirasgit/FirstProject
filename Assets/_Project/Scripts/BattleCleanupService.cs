using UnityEngine;

public class BattleCleanupService : MonoBehaviour
{
    [SerializeField] private ProjectileRegistry _projectileRegistry;
    public void ClearAllProjectiles()
    {
        _projectileRegistry.DestroyAll();
    }
    public void DestroyCharacter(Character character)
    {
        if (character == null)
            return;


        Destroy(character.gameObject);
    }
}
