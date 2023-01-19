using SharpDX;
using System;
using System.Collections.Generic;

namespace BetterPong.Engine2D;

public class GameScene
{
    private readonly List<IGameObject> _gameObjects;

    #region Utilities
    public float AirFriction = 0;

    public Vector2 GAcceleration = new(0, 10f);
    #endregion

    #region Constructors
    public GameScene(float airFriction = 0)
    {
        AirFriction = airFriction;
    }
    public GameScene(Vector2 gAcceleration, float airFriction = 0)
    {
        AirFriction = airFriction;
        GAcceleration = gAcceleration;
    }
    #endregion

    public void Update(TimeSpan deltaTime)
    {

    }
}
