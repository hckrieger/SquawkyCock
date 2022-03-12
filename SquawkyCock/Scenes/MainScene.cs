using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SquawkyCock.GameObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace SquawkyCock.Scenes
{
    class MainScene : GameState
    {
        List<SpriteGameObject> groundPool = new List<SpriteGameObject>();
        List<Pipes> pipePool = new List<Pipes>();
        float speed = 166f;
        SpriteGameObject background = new SpriteGameObject("Sprites/Background", .1f);
        float pipeTimer, startPipeTimer;
        Bird bird = new Bird();

        TextGameObject scoreFont = new TextGameObject("Fonts/Score", 1f, Color.White, TextGameObject.Alignment.Center);
        Menu menu;
        int score;

        public State CurrentState { get; set; }

        public enum State
        {
            Start,
            Playing,
            Fail,
            End
        }

        public MainScene(Point windowSize)
        {
            gameObjects.AddChild(bird);
            gameObjects.AddChild(scoreFont);
            gameObjects.AddChild(background);
            menu = new Menu(windowSize);
            gameObjects.AddChild(menu);
            scoreFont.LocalPosition = new Vector2(windowSize.X / 2, 110);

            for (int i = 0; i < 3; i++)
            {
                pipePool.Add(new Pipes(windowSize, speed));
                gameObjects.AddChild(pipePool[i]);
            }
            
            for (int i = 0; i < 3; i++)
            {
                groundPool.Add(new SpriteGameObject("Sprites/Ground", .7f));
                groundPool[i].LocalPosition = new Vector2(i * 440, 525);
                gameObjects.AddChild(groundPool[i]);
            }

            pipeTimer = 1.6f;
            startPipeTimer = pipeTimer;
            Reset();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;


            //Scroll the ground seamlessly overlapping 2 sections of ground sprites
            foreach (var ground in groundPool)
            {
                if (CurrentState == State.Start || CurrentState == State.Playing)
                    ground.LocalPosition -= new Vector2(speed, 0) * dt;

                if (ground.LocalPosition.X <= -440)
                    ground.LocalPosition = new Vector2(440 - 3, 525);


            }

            if (CurrentState == State.Playing)
                pipeTimer -= dt;

            //Active a new pipe every x seconds.
            if (pipeTimer <= 0)
            {
                foreach (Pipes obj in pipePool)
                {
                    if (!obj.Active)
                    {
                        obj.Active = true;
                        break;
                    }
                }

                pipeTimer = startPipeTimer;
            }

            
            foreach (Pipes obj in pipePool)
            {
                var topPipeCollision = CollisionDetection.ShapesIntersect(obj.PipeSet[0].BoundingBox, bird.CollisionCircle);
                var bottomPipeCollision = CollisionDetection.ShapesIntersect(obj.PipeSet[1].BoundingBox, bird.CollisionCircle);
                //Execute loss behavior when bird hits pipe.
                if ((topPipeCollision || bottomPipeCollision && bird.InControl))
                {
                    bird.InControl = false;
                    CurrentState = State.Fail;

                }

                if (topPipeCollision)
                    bird.ThrowDown(200);
                else if (bottomPipeCollision)
                    bird.ThrowDown(100);


                //Add to the score if the bird clears the pipe
                if (bird.GlobalPosition.X > obj.GlobalPosition.X && !obj.BirdClearedPipe)
                {
                    score++;
                    obj.BirdClearedPipe = true;
                }

            }


            scoreFont.Text = score.ToString();


            if (CurrentState == State.End)
            {
                menu.Visible = true;
                scoreFont.Visible = false;
            }
                

        }

        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);

            if ((inputHelper.KeyPressed(Keys.Space) || menu.ClickedReset) && CurrentState == State.End)
                Reset();
        }

        public float GroundPosition => groundPool[0].GlobalPosition.Y;

        public int Score => score;

        public override void Reset()
        {
            base.Reset();
            CurrentState = State.Start;
            score = 0;
            bird.Reset();
            menu.Reset();
            foreach (var obj in pipePool)
                obj.Reset();
            scoreFont.Visible = true;
        }

    }
}
