using Engine;
using Engine.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SquawkyCock.Scenes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SquawkyCock.GameObjects
{
    class Menu : SpriteGameObject
    {
        TextGameObject textDescription = new TextGameObject("Fonts/TextDescription", 1f, Color.Black, TextGameObject.Alignment.Center);
        TextGameObject scoreFont = new TextGameObject("Fonts/Score", 1f, Color.Black, TextGameObject.Alignment.Center);
        TextGameObject retry = new TextGameObject("Fonts/Retry", 1f, Color.Black, TextGameObject.Alignment.Center);
        SpriteGameObject playAgainButton = new Button("Sprites/PlayAgain", .95f);
        int highScore;
        public bool ClickedReset { get; set; }

        public Menu(Point windowSize) : base("Sprites/GameOverMenu", .9f)
        {

            highScore = 0;

            SetOriginToCenter();
            LocalPosition = new Vector2(windowSize.X / 2, windowSize.Y / 2.5f);

            textDescription.Parent = this;
            scoreFont.Parent = this;

            textDescription.LocalPosition = new Vector2(0, -117);
            textDescription.Text = "Score\n\n\n\n Best";

            scoreFont.LocalPosition = new Vector2(0, -90);
            

            playAgainButton.Parent = this;
            playAgainButton.SetOriginToCenter();
            playAgainButton.LocalPosition = new Vector2(0, 105);

            retry.Parent = playAgainButton;
            retry.LocalPosition = new Vector2(0, -12);
            retry.Text = "Retry";

            Reset();

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (MainScene.CurrentState == MainScene.State.End)
            {
                if (MainScene.Score > highScore)
                    highScore = MainScene.Score;
            }


            scoreFont.Text = $"{MainScene.Score}\n\n\n{highScore}";
        }



        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            if (Visible)
            {
                retry.Draw(gameTime, spriteBatch);
                playAgainButton.Draw(gameTime, spriteBatch);
                textDescription.Draw(gameTime, spriteBatch);
                scoreFont.Draw(gameTime, spriteBatch);
            }

        }

        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);

            if (Visible)
            {
                if (playAgainButton.BoundingBox.Contains(inputHelper.MousePositionWorld) &&
                    inputHelper.MouseLeftButtonPressed())
                        ClickedReset = true;

            }

        }

        public override void Reset()
        {
            base.Reset();
            Visible = false;
            ClickedReset = false;
        }

        private MainScene MainScene => (MainScene)ExtendedGame.GameStateManager.GetGameState(Game1.STATE_MAIN);
    }
}
