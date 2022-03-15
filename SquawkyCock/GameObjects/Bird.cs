using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SquawkyCock.Scenes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SquawkyCock.GameObjects
{
    class Bird : SpriteGameObject
    {
        float fallSpeed, jumpSpeed = 425;
        public bool IsFalling { get; set; }
        Circle circleCollider;
        public bool InControl { get; set; }
        private float xPos = 125;

        public Bird() : base("Sprites/Bird", .8f)
        {
            SetOriginToCenter();
            Reset();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            circleCollider = new Circle(24, LocalPosition);

            if (MainScene.CurrentState == MainScene.State.Playing ||
                MainScene.CurrentState == MainScene.State.Fail)
                velocity.Y += fallSpeed;

            fallSpeed = 25f;


            if (GlobalPosition.Y >= MainScene.GroundPosition - 20)
                MainScene.CurrentState = MainScene.State.End;

            if (MainScene.CurrentState == MainScene.State.End)
                LocalPosition = new Vector2(xPos, MainScene.GroundPosition - Height / 2.5f);

            
            if (MainScene.CurrentState == MainScene.State.Fail ||
                MainScene.CurrentState == MainScene.State.End)
                Rotation = (float)Math.PI / 2;
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);

            if ((inputHelper.KeyPressed(Keys.Space) || inputHelper.MouseLeftButtonPressed()) && InControl && GlobalPosition.Y > -50)
            {
                velocity.Y = -jumpSpeed;
                if (MainScene.CurrentState == MainScene.State.Start)
                    MainScene.CurrentState = MainScene.State.Playing;

                
            }
        }

        public void ThrowDown(float speed)
        {
            fallSpeed += speed;
        }

        public Circle CollisionCircle => circleCollider;

        public override void Reset()
        {
            base.Reset();
            LocalPosition = new Vector2(xPos, 200);
            InControl = true;
            Rotation = MathHelper.ToRadians(-15);
        }

        private MainScene MainScene => (MainScene)ExtendedGame.GameStateManager.GetGameState(Game1.STATE_MAIN);
    }
}
