using Godot;
using System;

public class Player : KinematicBody
{
    [Signal]
    public delegate void Hit();

    [Export]
    public int Speed = 14;

    [Export]
    public int FallAcceleration = 75;

    [Export]
    public int JumpImpulse = 20;

    [Export]
    public int BounceImpulse = 16;

    private Vector3 _velocity = Vector3.Zero;

    private void Die()
    {
        EmitSignal(nameof(Hit));
        Hide();
    }

    public override void _PhysicsProcess(float delta)
    {
        var direction = Vector3.Zero;
        var pivot = GetNode<Spatial>("Pivot");
        var animation = GetNode<AnimationPlayer>("AnimationPlayer");

        if (Input.IsActionPressed("move_right"))
        {
            direction.x += 1f;
        }
        if (Input.IsActionPressed("move_left"))
        {
            direction.x -= 1f;
        }

        if (Input.IsActionPressed("move_back"))
        {
            direction.z += 1f;
        }
        if (Input.IsActionPressed("move_forward"))
        {
            direction.z -= 1f;
        }

        if (direction != Vector3.Zero)
        {
            direction = direction.Normalized();
            pivot.LookAt(Translation + direction, Vector3.Up);
            animation.PlaybackSpeed = 4;
        }
        else
        {
            animation.PlaybackSpeed = 1;
        }

        _velocity.x = direction.x * Speed;
        _velocity.z = direction.z * Speed;

        if (IsOnFloor() && Input.IsActionPressed("jump"))
        {
            _velocity.y += JumpImpulse;
        }

        _velocity.y -= FallAcceleration * delta;
        _velocity = MoveAndSlide(_velocity, Vector3.Up);

        for (int i = 0; i < GetSlideCount(); i++)
        {
            KinematicCollision collision = GetSlideCollision(i);
            if (collision.Collider is Mob mob && mob.IsInGroup("mob"))
            {
                if (Vector3.Up.Dot(collision.Normal) > 0.1f)
                {
                    mob.Squash();
                    _velocity.y = BounceImpulse;
                }
            }
        }

        pivot.Rotation = new Vector3(Mathf.Pi / 6f * _velocity.y / JumpImpulse, pivot.Rotation.y, pivot.Rotation.z);
    }

    public void OnMobDetectorBodyEntered(Node body)
    {
        Die();
    }
}
