using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Engine;
using SquawkyCock.Scenes;

namespace SquawkyCock
{
    public class Game1 : ExtendedGame
    {
        public const string STATE_MAIN = "MainState";

        public Game1()
        {
            IsMouseVisible = true;
            windowSize = new Point(440, 700);
            worldSize = new Point(440, 700);
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            // TODO: use this.Content to load your game content here
            GameStateManager.AddGameState(STATE_MAIN, new MainScene(windowSize));
            GameStateManager.SwitchTo(STATE_MAIN);
        }

    }
}
