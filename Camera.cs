using OpenTK;
using OpenGL_Game.Scenes;
using OpenGL_Game.Components;

namespace OpenGL_Game
{
    class Camera
    {
        public float cameraRadius;
        public Matrix4 view, projection;
        public Vector3 cameraPosition, cameraDirection, cameraUp;
        private Vector3 targetPosition;

        GameScene game = GameScene.gameInstance;

        public Camera()
        {
            cameraPosition = new Vector3(0.0f, 0.0f, 0.0f);
            cameraDirection = new Vector3(0.0f, 0.0f, -1.0f);
            cameraUp = new Vector3(0.0f, 1.0f, 0.0f);
            cameraRadius = 1.5f;
            UpdateView();
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(55), 1.0f, 0.1f, 100f);
        }

        public Camera(Vector3 cameraPos, Vector3 targetPos, float ratio, float near, float far)
        {
            cameraUp = new Vector3(0.0f, 1.0f, 0.0f);
            cameraPosition = cameraPos;
            cameraDirection = targetPos-cameraPos;
            cameraRadius = 1.5f;
            cameraDirection.Normalize();
            UpdateView();
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(55), ratio, near, far);
        }

        public void MoveForward(float move)
        {
            cameraPosition += move*cameraDirection;
            game.player.GetComponent<ComponentPosition>().Position = cameraPosition;
            UpdateView();
        }

        public void Translate(Vector3 move)
        {
            cameraPosition += move;
            UpdateView();
        }


        public void RotateY(float angle)
        {
            cameraDirection = Matrix3.CreateRotationY(angle) * cameraDirection;
            UpdateView();
        }

        public void UpdateView()
        {
            targetPosition = cameraPosition + cameraDirection;
            view = Matrix4.LookAt(cameraPosition, targetPosition, cameraUp);
        }

        // Return the view matrix using the camera position orientation and up vectors.
        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(cameraPosition, cameraPosition + cameraDirection, cameraUp);
        }
    }
}
