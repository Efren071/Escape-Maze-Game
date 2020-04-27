using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using OpenTK.Input;
using OpenGL_Game.Managers;

namespace OpenGL_Game.Scenes
{
    class GameOverScene : Scene
    {
        public GameOverScene(SceneManager sceneManager) : base(sceneManager)
        {
            // Set the title of the window
            sceneManager.Title = "Game Over Scene";
            // Set the Render and Update delegates to the Update and Render methods of this class
            sceneManager.renderer = Render;
            sceneManager.updater = Update;

            sceneManager.keyboardDownDelegate += Keyboard_KeyDown;
        }

        public override void Update(FrameEventArgs e)
        {
        }

        public override void Render(FrameEventArgs e)
        {
            GL.Viewport(0, 0, sceneManager.Width, sceneManager.Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, sceneManager.Width, 0, sceneManager.Height, -1, 1);

            GUI.clearColour = Color.Red;

            //Display the Title
            float width = sceneManager.Width, height = sceneManager.Height, fontSize = Math.Min(width, height) / 10f;
            GUI.Label(new Rectangle(0, (int)(fontSize / 2f), (int)width, (int)(fontSize * 2f)), "Game Over!", (int)fontSize, StringAlignment.Center);
            GUI.Label(new Rectangle(0, 70, (int)width, (int)(fontSize * 2f)), "Press <Space> to play again", 18, StringAlignment.Center, Color.White);
            GUI.Label(new Rectangle(0, 100, (int)width, (int)(fontSize * 2f)), "Press <Q> to return to main menu", 18, StringAlignment.Center, Color.White);
            GUI.Render();
        }

        public void Keyboard_KeyDown(KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Space:
                    sceneManager.ChangeScene(SceneTypes.SCENE_GAME);
                    break;
                case Key.Q:
                    sceneManager.ChangeScene(SceneTypes.SCENE_MAIN_MENU);
                    break;
            }
        }

        public override void Close()
        {
            sceneManager.keyboardDownDelegate -= Keyboard_KeyDown;
        }
    }
}