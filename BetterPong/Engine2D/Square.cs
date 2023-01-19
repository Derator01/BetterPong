using SharpDX;
using System;
using System.Collections.Generic;

namespace BetterPong.Engine2D;

public class Square : IGameObject
{
    public bool IsActive { get; private set; } = false;

    public bool IsStatic { get; }

    private float mass;
    private float _mass;
    private bool _automaticMass;

    /// <summary>
    /// X and Y of the center.
    /// </summary>
    public Vector2 Position { get; private set; }
    private Vector2 _prevPosition;

    public float Width { get; private set; }
    private float _prevWidth;

    public float Height { get; private set; }
    private float _prevHeight;

    public float Speed { get; private set; }

    public Vector2 Velocity { get; private set; }

    public Vector2[] Vertices { get; private set; } = new Vector2[4];

    public Square(bool isStatic, Vector2 position, float width, float height, float mass = 0)
    {
        IsStatic = isStatic;
        Position = position;

        Width = width;
        Height = height;

        if (mass == 0)
        {
            _automaticMass = true;
            return;
        }

        _mass = mass;
    }

    /// <summary>
    /// Updates <see cref="Vertices"/> position according to changed <see cref="Position"/>.
    /// </summary>
    public void Update()
    {
        if (!IsActive)
            return;
        if (Position == _prevPosition && Width == _prevWidth && Height == _prevHeight)
            return;

        Vertices[0] = new Vector2(-Width / 2, -Height / 2);
        Vertices[1] = new Vector2(Width / 2, -Height / 2);
        Vertices[2] = new Vector2(Width / 2, Height / 2);
        Vertices[3] = new Vector2(-Width / 2, Height / 2);
    }

    public float GetMass()
    {
        if (_automaticMass)
        {
            return Width * Height;
        }

        return mass;
    }
    public void SetMass(float value)
    {
        if (_automaticMass)
            return;

        mass = value;
    }

    public bool Intersects(IGameObject other)
    {
        List<Vector2> selfVertices = new List<Vector2>(Vertices);
        List<Vector2> other2Vertices = new List<Vector2>(other.Vertices);

        List<Vector2> intersection = new List<Vector2>();

        // Perform the Sutherland-Hodgman algorithm for each edge of the first polygon
        for (int i = 0; i < selfVertices.Count; i++)
        {
            Vector2 a = selfVertices[i];
            Vector2 b = selfVertices[(i + 1) % selfVertices.Count];
            List<Vector2> inputList = new List<Vector2>(other2Vertices);
            other2Vertices.Clear();

            Vector2 s = inputList[inputList.Count - 1];
            for (int j = 0; j < inputList.Count; j++)
            {
                Vector2 e = inputList[j];
                if (IsInside(a, b, e))
                {
                    if (!IsInside(a, b, s))
                    {
                        Vector2 intersectionPoint = GetIntersection(a, b, s, e);
                        intersection.Add(intersectionPoint);
                        other2Vertices.Add(intersectionPoint);
                    }
                    other2Vertices.Add(e);
                }
                else if (IsInside(a, b, s))
                {
                    Vector2 intersectionPoint = GetIntersection(a, b, s, e);
                    intersection.Add(intersectionPoint);
                    other2Vertices.Add(intersectionPoint);
                }
                s = e;
            }
        }

        return intersection.Count > 0;
    }

    private static bool IsInside(Vector2 a, Vector2 b, Vector2 c)
    {
        // Determine if a point is inside the square
        return (a.X - c.X) * (b.Y - c.Y) > (a.Y - c.Y) * (b.X - c.X);
    }

    private static Vector2 GetIntersection(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
    {
        // Get the intersection point of two lines
        float denominator = (c.Y - d.Y) * (a.X - b.X) - (c.X - d.X) * (a.Y - b.Y);
        float numerator1 = (c.X - d.X) * (a.Y - c.Y) - (c.Y - d.Y) * (a.X - c.X);
        float ua = numerator1 / denominator;
        return new Vector2(a.X + ua * (b.X - a.X), a.Y + ua * (b.Y - a.Y));
    }

    /// <summary>
    /// Moves the center of square. Then calls <see cref="Update(TimeSpan)"/> method.
    /// </summary>
    public void Move(TimeSpan deltaTime)
    {
        Position += new Vector2(Velocity.X * (float)deltaTime.TotalSeconds, Velocity.Y * (float)deltaTime.TotalSeconds);
        Update();
    }

    public void Enable()
    {
        Update();
        IsActive = true;
    }

    public void Disable()
    {
        IsActive = false;
    }
}
