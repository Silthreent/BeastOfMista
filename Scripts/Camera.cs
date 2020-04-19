using Godot;

public class Camera : Camera2D
{
    float Speed = 250f;

    public override void _Process(float delta)
    {
        var mousePos = GetViewport().GetMousePosition();

        var slideDir = Vector2.Zero;
        if (mousePos.x >= GetViewport().Size.x - 5)
        {
            slideDir.x = 1;
        }
        if (mousePos.x <= 0)
        {
            slideDir.x = -1;
        }
        if (mousePos.y >= GetViewport().Size.y - 5)
        {
            slideDir.y = 1;
        }
        if (mousePos.y <= 0)
        {
            slideDir.y = -1;
        }

        Position += slideDir * (Speed * delta);
    }
}
