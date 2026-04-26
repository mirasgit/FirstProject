using System.Collections.Generic;
using UnityEngine;

public class ProjectileRegistry : MonoBehaviour
{
    private List<Projectile> _projectiles = new();
    public void Register(Projectile projectile)
    {

        if (projectile == null){ return;}

        if (_projectiles.Contains(projectile)) {return;}

        _projectiles.Add(projectile);
    }
    public void Unregister(Projectile projectile)
    {
        if (projectile == null)
        {
            return;
        }

        _projectiles.Remove(projectile);
    }

    public void DestroyAll()
    {
        for (int index = _projectiles.Count - 1; index >= 0; index--)
        {
            Projectile projectile = _projectiles[index];

            if (projectile == null)
            {
                _projectiles.RemoveAt(index);
                continue;
            }

            Destroy(projectile.gameObject);
        }

        _projectiles.Clear();
    }
}
