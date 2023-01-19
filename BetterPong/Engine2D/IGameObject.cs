using SharpDX;
using System;

namespace BetterPong.Engine2D;

public interface IGameObject
{
    bool IsActive { get; }
    bool IsStatic { get; }

    float Speed { get; }
    Vector2 Velocity { get; }

    Vector2[] Vertices { get; }

    void Update();

    float GetMass();
    void SetMass(float mass);
    bool Intersects(IGameObject other);
    void Move(TimeSpan deltaTime);
}
