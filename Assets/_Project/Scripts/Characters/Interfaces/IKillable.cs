using System;

namespace FirstProject.Characters
{
    public interface IKillable
    {
        event Action Died;
    }
}

