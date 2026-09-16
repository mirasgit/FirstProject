using System;

namespace FirstProject.Core.Characters
{
    public interface IKillable
    {
        event Action Died;
    }
}

