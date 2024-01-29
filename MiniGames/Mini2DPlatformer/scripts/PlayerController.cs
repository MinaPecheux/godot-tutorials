using Godot;

namespace MiniGame_Mini2DPlatformer
{

	public partial class PlayerController : CharacterBody2D
	{
		
		public float moveSpeed = 150.0f;
		public float jumpVelocity = 400.0f;
		
		// get gravity from project settings (keep everything synced)
		public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
		
		private TileMap _tilemap;
		private Sprite2D _idleSprite;
		private AnimatedSprite2D _walkSprite;
		
		public override void _Ready()
		{
			_tilemap = GetNode<TileMap>("/root/Root/TileMap");

			// get sprite references
			_idleSprite = GetNode<Sprite2D>("IdleSprite");
			_walkSprite = GetNode<AnimatedSprite2D>("WalkSprite");
		}

		public override void _PhysicsProcess(double delta)
		{
			Vector2 velocity = Velocity;
			
			// apply gravity if in the air
			if (!IsOnFloor()) {
				velocity.Y += gravity * (float)delta;
			}
			
			// handle jump (if grounded)
			if (Input.IsKeyPressed(Key.Space) && IsOnFloor())
				velocity.Y = -jumpVelocity;
			
			// handle horizontal movement (keyboard arrow keys)
			velocity.X = 0;
			if (Input.IsKeyPressed(Key.Left))
				velocity.X = -moveSpeed;
			else if (Input.IsKeyPressed(Key.Right))
				velocity.X =  moveSpeed;
			
			_UpdateSpriteRenderer(velocity.X);
			
			Velocity = velocity;
			MoveAndSlide();
		}
		
		private void _UpdateSpriteRenderer(float velX) {
			bool walking = velX != 0;
			_idleSprite.Visible = !walking;
			_walkSprite.Visible = walking;
			
			if (walking) {
				_walkSprite.Play();
				_walkSprite.FlipH = velX < 0;
			}
		}

		private float _GetSpeedModifier() {
			Vector2I mapPos = _tilemap.LocalToMap(Position);
			mapPos.Y++; // look at cell below the player
			var data = _tilemap.GetCellTileData(0, mapPos);
			if (data == null) return 1f;
			return data.GetCustomData("speed_modifier").AsSingle();
		}
		
	}

}
