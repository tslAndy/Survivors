using System.Numerics;
using Arch.Core;
using Arch.Core.Extensions;
using Arch.System;

namespace Components.Basic;

struct TransformComp
{
    public Entity? parent;

    public Vector2 position;
    public float rotation,
        scale;
}
