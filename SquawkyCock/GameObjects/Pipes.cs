using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SquawkyCock.Scenes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SquawkyCock.GameObjects
{
    class Pipes : GameObject
    {
        SpriteGameObject topPipe = new SpriteGameObject("Sprites/Pipe@2x1", .5f, 0);
        SpriteGameObject bottomPipe = new SpriteGameObject("Sprites/Pipe@2x1", .5f, 1);
        List<SpriteGameObject> pipes = new List<SpriteGameObject>();
        Point windowSize;
        float speed;
        public bool BirdClearedPipe { get; set; }

        public Pipes(Point windowSize, float speed)
        {
            

            pipes.Add(topPipe);
            pipes.Add(bottomPipe);

            for (int i = 0; i < 2; i++)
            {
                pipes[i].Parent = this;
                pipes[i].SetOriginToCenter();

                if (i == 0)
                    pipes[i].LocalPosition = Vector2.Zero;
                else if (i == 1)
                    pipes[i].LocalPosition = new Vector2(0, 833);
            }

            this.windowSize = windowSize;
            this.speed = speed;
            Reset();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (MainScene.CurrentState == MainScene.State.Playing)
                velocity = new Vector2(-speed, 0);
            else
                velocity = Vector2.Zero;

            if (LocalPosition.X < -50)
                Reset();

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            foreach (SpriteGameObject obj in pipes)
                obj.Draw(gameTime, spriteBatch);
        }

        public override void Reset()
        {
            base.Reset();
            var verticalPosition = ExtendedGame.Random.Next(-300, -25);
            LocalPosition = new Vector2(windowSize.X + 50, verticalPosition);
            Active = false;
            BirdClearedPipe = false;
        }

        public List<SpriteGameObject> PipeSet => pipes;

        private MainScene MainScene => (MainScene)ExtendedGame.GameStateManager.GetGameState(Game1.STATE_MAIN);
    }
}
