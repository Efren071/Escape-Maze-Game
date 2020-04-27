using OpenTK;
using OpenTK.Input;
using OpenTK.Audio.OpenAL;
using OpenTK.Graphics.OpenGL;
using OpenGL_Game.Components;
using OpenGL_Game.Managers;
using OpenGL_Game.Systems;
using OpenGL_Game.Objects;
using System;
using System.Drawing;
using System.Collections.Generic;

namespace OpenGL_Game.Scenes
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    class GameScene : Scene
    {
        #region // GLOBAL VARIABLES ...
        public static float dt = 0;
        // Managers
        EntityManager entityManager;
        SystemManager systemManager;
        PhysicsManager phsyicsManager;
        EntityCollisionManager collisionManager;
        LineCollisionManager lineCollisionManager;

        //
        public Camera camera;
        public SkyBox skybox;
        public Vector3 oldPosition;

        public static GameScene gameInstance;

        // KeyPress
        public bool[] keyPressed = new bool[255];

        // Entities
        public Entity currentEntity, portal, player, openPortal, rollingBall, bouncingBall;
        // Light entities
        public Entity blueSpot, yellowSpot;
        public bool portalActive = false;
        public bool key1 = false, key2 = false, key3 = false;

        // Texts
        public int lives = 3;
        public string heart = "♥";

        // Positions
        public Vector3 origin = new Vector3(-22.5f, 1.0f, -22.0f); // new Vector3(-19f, 1f, -10f);
        public Vector3 cameraDirection = new Vector3(1.0f, 1f, -13.0f);
        public Vector3 portalPosition = new Vector3(-22.5f, -0.25f, -22.0f);
        public Vector3 key1Position = new Vector3(22.5f, 0.75f, -22.5f);
        public Vector3 key2Position = new Vector3(22.5f, 0.75f, 22.5f);
        public Vector3 key3Position = new Vector3(-22.5f, 0.75f, 22.5f);

        // Colours
        public Vector3 fire = new Vector3(255f, 153f, 51f);
        public Vector3 fireSpec = new Vector3(2.55f, 2.33f, 2.33f);
        public Vector3 keyLight = new Vector3(2.55f, 2.55f, 2.04f);
        public Vector3 portalLight = new Vector3(0.0f, 102.0f, 204.0f);
        public Vector3 BlueLight = new Vector3(0.0f, 191.0f, 255.0f);
        public Vector3 YellowLight = new Vector3(255.0f, 215.0f, 0.0f);
        #endregion

        public GameScene(SceneManager sceneManager) : base(sceneManager)
        {
            gameInstance = this;
            entityManager = new EntityManager();
            systemManager = new SystemManager();
            phsyicsManager = new PhysicsManager();
            collisionManager = new EntityCollisionManager();
            lineCollisionManager = new LineCollisionManager();

            // Set the title of the window
            sceneManager.Title = "Game";
            // Set the Render and Update delegates to the Update and Render methods of this class
            sceneManager.renderer = Render;
            sceneManager.updater = Update;
            // Set Keyboard events to go to a method in this class
            sceneManager.keyboardDownDelegate += Keyboard_KeyDown;
            sceneManager.keyboardUpDelegate += Keyboard_KeyUp;

            // Enable Depth Testing
            GL.Enable(EnableCap.DepthTest);
            GL.DepthMask(true);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            // Set Camera
            camera = new Camera(origin, cameraDirection, (float)(sceneManager.Width) / (float)(sceneManager.Height), 0.1f, 100f);

            GL.ClearColor(Color.White);

            CreateEntities();
            CreateSystems();
            skybox = new SkyBox();
        }

        private void CreateEntities()
        {
            // Map
            currentEntity = new Entity("Map", OBJECT_TYPE.ENVIRONMENT);
            currentEntity.AddComponent(new ComponentPosition(new Vector3(0.0f, 0.0f, 0.0f)));
            currentEntity.AddComponent(new ComponentGeometry("Geometry/Map/maze.obj"));
            currentEntity.AddComponent(new ComponentCollisionLine(WallBoundaries.SetMapWalls()));
            currentEntity.AddComponent(new ComponentInScene());
            entityManager.AddEntity(currentEntity);

            currentEntity = new Entity("Floor", OBJECT_TYPE.FLOOR);
            currentEntity.AddComponent(new ComponentPosition(new Vector3(0.0f, 0.0f, 0.0f)));
            currentEntity.AddComponent(new ComponentGeometry("Geometry/Map/floor.obj"));
            currentEntity.AddComponent(new ComponentInScene());
            entityManager.AddEntity(currentEntity);

            // Portal
            portal = new Entity("Portal", OBJECT_TYPE.PORTAL);
            portal.AddComponent(new ComponentPosition(portalPosition));
            portal.AddComponent(new ComponentGeometry("Geometry/Portal/inactivePortal.obj"));
            portal.AddComponent(new ComponentAudio("Audio/PortalOff.wav", true));
            portal.AddComponent(new ComponentInScene());
            entityManager.AddEntity(portal);

            // Player
            player = new Entity("Player", OBJECT_TYPE.PLAYER);
            player.AddComponent(new ComponentPosition(camera.cameraPosition));
            player.AddComponent(new ComponentAudio("Audio/death.wav", false));
            player.AddComponent(new ComponentInScene());
            entityManager.AddEntity(player);

            // Key 1
            currentEntity = new Entity("Key 1", OBJECT_TYPE.KEY);
            currentEntity.AddComponent(new ComponentPosition(key1Position));
            currentEntity.AddComponent(new ComponentVelocity(new Vector3(0.0f, 0.2f, 0.0f)));
            currentEntity.AddComponent(new ComponentGeometry("Geometry/Map/key.obj"));
            currentEntity.AddComponent(new ComponentAudio("Audio/collectedKey.wav", false));
            currentEntity.AddComponent(new ComponentCollisionSphere(0.5f));
            currentEntity.AddComponent(new ComponentInScene());
            entityManager.AddEntity(currentEntity);
            CreatePointLight("Key1Light", currentEntity, key1Position, keyLight, 2, 5, 10);    // Light
            // Key 2
            currentEntity = new Entity("Key 2", OBJECT_TYPE.KEY);
            currentEntity.AddComponent(new ComponentPosition(key2Position));
            currentEntity.AddComponent(new ComponentVelocity(new Vector3(0.0f, 0.2f, 0.0f)));
            currentEntity.AddComponent(new ComponentGeometry("Geometry/Map/key.obj"));
            currentEntity.AddComponent(new ComponentAudio("Audio/collectedKey.wav", false));
            currentEntity.AddComponent(new ComponentCollisionSphere(0.5f));
            currentEntity.AddComponent(new ComponentInScene());
            entityManager.AddEntity(currentEntity);
            CreatePointLight("Key2Light", currentEntity, key2Position, keyLight, 2, 5, 10);    // Light

            // Key 3
            currentEntity = new Entity("Key 3", OBJECT_TYPE.KEY);
            currentEntity.AddComponent(new ComponentPosition(key3Position));
            currentEntity.AddComponent(new ComponentVelocity(new Vector3(0.0f, 0.2f, 0.0f)));
            currentEntity.AddComponent(new ComponentGeometry("Geometry/Map/key.obj"));
            currentEntity.AddComponent(new ComponentAudio("Audio/collectedKey.wav", false));
            currentEntity.AddComponent(new ComponentCollisionSphere(0.5f));
            currentEntity.AddComponent(new ComponentInScene());
            CreatePointLight("Key3Light", currentEntity, key3Position, keyLight, 2, 5, 10);    // Light

            entityManager.AddEntity(currentEntity);

            //----------------------------------//
            //              Enemies             //
            //----------------------------------//

            // Drone
            currentEntity = new Entity("Drone", OBJECT_TYPE.ENEMY);
            currentEntity.AddComponent(new ComponentPosition(new Vector3(0.0f, 0.5f, 0.0f)));
            currentEntity.AddComponent(new ComponentGeometry("Geometry/Wraith_Raider_Starship/Wraith_Raider_Starship.obj"));
            currentEntity.AddComponent(new ComponentAudio("Audio/buzz.wav", true));
            currentEntity.AddComponent(new ComponentCollisionSphere(1.5f));
            currentEntity.AddComponent(new ComponentInScene());
            entityManager.AddEntity(currentEntity);

            // Rolling Ball
            rollingBall = new Entity("Rolling Ball", OBJECT_TYPE.ROLLINGBALL);
            //29.5f, left = 14.0f, top = -29.5f, bot = -14.5f
            rollingBall.AddComponent(new ComponentPosition(GenRandomSpawn(29.5f, 14.5f, -14.5f, -29.5f)));
            rollingBall.AddComponent(new ComponentVelocity(new Vector3(-2f, 0.0f, 2f)));
            rollingBall.AddComponent(new ComponentGeometry("Geometry/Balls/marble.obj"));
            rollingBall.AddComponent(new ComponentAudio("Audio/rolling.wav", true));
            rollingBall.AddComponent(new ComponentCollisionSphere(1f));
            rollingBall.AddComponent(new ComponentInScene());
            entityManager.AddEntity(rollingBall);

            // Bouncing Ball
            bouncingBall = new Entity("Bouncing Ball", OBJECT_TYPE.BOUNCINGBALL);
            bouncingBall.AddComponent(new ComponentPosition(GenRandomSpawn(29.5f, 14.5f, 29.5f, 14.5f)));
            bouncingBall.AddComponent(new ComponentVelocity(new Vector3(-2f, 0.2f, 2f), 0.2f));
            bouncingBall.AddComponent(new ComponentGeometry("Geometry/Moon/moon.obj"));
            bouncingBall.AddComponent(new ComponentAudio("Audio/drop.wav", false));
            bouncingBall.AddComponent(new ComponentCollisionSphere(1f));
            bouncingBall.AddComponent(new ComponentInScene());
            entityManager.AddEntity(bouncingBall);

            //----------------------------------//
            //             Lighting             //
            //----------------------------------//

            // Main Light
            CreatePointLight("MainLight", currentEntity, new Vector3(0.0f, 10.0f, 0.0f), keyLight, 1.0f, 0.3f, 0.1f);

            // Room Lights and Torches
            GenerateTorch(currentEntity, "Room1", 30.0f, 1.5f, -30.0f);
            GenerateTorch(currentEntity, "Room2", 30.0f, 1.5f, 30.0f);
            GenerateTorch(currentEntity, "Room4_1", -30.0f, 1.5f, -30.0f);
            CreatePointLight("PointLightRoom1", currentEntity, new Vector3(30.0f, 3.0f, -30.0f), fire, 10.0f, 5.0f, 20.0f);
            CreatePointLight("PointLightRoom2", currentEntity, new Vector3(30.0f, 3.0f, 30.0f), fire, 10.0f, 5.0f, 20.0f);
            CreatePointLight("PointLightRoom4", currentEntity, new Vector3(-30.0f, 3.0f, -30.0f), fire, 10.0f, 5.0f, 20.0f);
            CreatePointLight("Portal Light", currentEntity, new Vector3(-22.5f, 2.5f, -22.5f), portalLight, 10.0f, 5.0f, 20.0f);

            // Corridors
            GenerateTorch(currentEntity, "TopTorch", 0.0f, 1.5f, -25.0f);
            GenerateTorch(currentEntity, "RightTorch", 25.0f, 1.5f, 0.0f);
            GenerateTorch(currentEntity, "BottomTorch", 0.0f, 1.5f, 25.0f);
            GenerateTorch(currentEntity, "LeftTorch", -25.0f, 1.5f, 0.0f);
            CreatePointLight("PointLightTop", currentEntity, new Vector3(0.0f, 3.0f, -25.0f), fire, 10.0f, 5.0f, 20.0f);
            CreatePointLight("PointLightRight", currentEntity, new Vector3(25.0f, 3.0f, 0.0f), fire, 10.0f, 5.0f, 20.0f);
            CreatePointLight("PointLightBot", currentEntity, new Vector3(0.0f, 3.0f, 25.0f), fire, 10.0f, 5.0f, 20.0f);
            CreatePointLight("PointLightLeft", currentEntity, new Vector3(-25.0f, 3.0f, 0.0f), fire, 10.0f, 5.0f, 20.0f);

            #region SPOTLIGHT ATTEMPT -- Uncomment from main FS shader
            //blueSpot = new Entity("blueSpot", OBJECT_TYPE.SPOTLIGHT);
            //blueSpot.AddComponent(new ComponentPosition(new Vector3(-22.5f, 1.0f, 17.5f)));
            //blueSpot.AddComponent(new ComponentSpotLight(new Vector3(-22.5f, 0.0f, 17.5f),
            //    new Vector3(0.1f, 0.1f, 0.1f), new Vector3(0.7f, 0.7f, 0.7f), new Vector3(BlueLight), 0.0f, 1.0f));
            //blueSpot.AddComponent(new ComponentInScene());
            //entityManager.AddEntity(blueSpot);

            //yellowSpot = new Entity("yellowSpot", OBJECT_TYPE.SPOTLIGHT);
            //yellowSpot.AddComponent(new ComponentPosition(new Vector3(-22.5f, 1.0f, 27.5f)));
            //yellowSpot.AddComponent(new ComponentSpotLight(new Vector3(-22.5f, 0.0f, 27.5f),
            //    new Vector3(0.1f, 0.1f, 0.1f), new Vector3(0.7f, 0.7f, 0.7f), new Vector3(YellowLight), 0.0f, 1.0f));
            //yellowSpot.AddComponent(new ComponentInScene());
            //entityManager.AddEntity(yellowSpot);
            #endregion

            blueSpot = new Entity("blue spot light", OBJECT_TYPE.SPOTLIGHT);
            blueSpot.AddComponent(new ComponentPosition(-22.5f, 0.2f, 17.5f));
            blueSpot.AddComponent(new ComponentLightSource(BlueLight, BlueLight * 0.1f, BlueLight, 0.0f, 60.0f, 500.0f));
            blueSpot.AddComponent(new ComponentInScene());
            entityManager.AddEntity(blueSpot);

            yellowSpot = new Entity("yellow spot light", OBJECT_TYPE.SPOTLIGHT);
            yellowSpot.AddComponent(new ComponentPosition(-22.5f, 0.2f, 27.5f));
            yellowSpot.AddComponent(new ComponentLightSource(YellowLight, YellowLight * 0.1f, YellowLight, 0.0f, 60.0f, 500.0f));
            yellowSpot.AddComponent(new ComponentInScene());
            entityManager.AddEntity(yellowSpot);
        }

        private void CreateSystems()
        {
            ISystem newSystem;

            newSystem = new SystemRender();
            systemManager.AddSystem(newSystem);

            newSystem = new SystemLighting();
            systemManager.AddSystem(newSystem);

            newSystem = new SystemPhysics(phsyicsManager);
            systemManager.AddSystem(newSystem);

            newSystem = new SystemAudio();
            systemManager.AddSystem(newSystem);

            newSystem = new SystemCollisionSphere(collisionManager, camera);
            systemManager.AddSystem(newSystem);

            newSystem = new SystemCollisionLine(lineCollisionManager, camera);
            systemManager.AddSystem(newSystem);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="e">Provides a snapshot of timing values.</param>
        public override void Update(FrameEventArgs e)
        {
            dt = (float)e.Time;
            System.Console.WriteLine("fps=" + (int)(1.0 / dt));
            float width = sceneManager.Width, height = sceneManager.Height, fontSize = Math.Min(width, height) / 10f;

            if (GamePad.GetState(1).Buttons.Back == ButtonState.Pressed)
                sceneManager.Exit();

            // NEW for Audio
            // Update OpenAL Listener Position and Orientation based on the camera
            AL.Listener(ALListener3f.Position, ref camera.cameraPosition);
            AL.Listener(ALListenerfv.Orientation, ref camera.cameraDirection, ref camera.cameraUp);


            // store old position as current position
            oldPosition = camera.cameraPosition;

            // MOVEMENT KEYS
            if (keyPressed[(char)Key.Up] || keyPressed[(char)Key.W])
            {
                camera.MoveForward(0.1f);
            }
            if (keyPressed[(char)Key.Down] || keyPressed[(char)Key.S])
            {
                camera.MoveForward(-0.1f);
            }
            if (keyPressed[(char)Key.Left] || keyPressed[(char)Key.A])
            {
                camera.RotateY(-0.025f);
            }
            if (keyPressed[(char)Key.Right] || keyPressed[(char)Key.D])
            {
                camera.RotateY(0.025f);
            }
            if (keyPressed[(char)Key.M])
            {
                entityManager.Close();
                sceneManager.ChangeScene(SceneTypes.SCENE_MAIN_MENU);
            }
            if (keyPressed[(char)Key.Q])
            {
                entityManager.Close();
                sceneManager.ChangeScene(SceneTypes.SCENE_GAME_OVER);
            }

            // Check each object to see if they are NOT collidable

            if (key1 && key2 && key3 && !portalActive)
            {
                portal.GetComponent<ComponentAudio>().PauseAudio();

                // Open Portal
                portal.Delete();

                Entity turnOnPortalSound = new Entity("portal turn on sound", OBJECT_TYPE.ENVIRONMENT);
                turnOnPortalSound.AddComponent(new ComponentPosition(camera.cameraPosition));
                turnOnPortalSound.AddComponent(new ComponentAudio("Audio/switchOnPortal.wav", false));
                turnOnPortalSound.AddComponent(new ComponentInScene());
                entityManager.AddEntity(turnOnPortalSound);
                turnOnPortalSound.GetComponent<ComponentAudio>().PlayAudioOnce();

                OpenPortal();
                portalActive = true;
            }

            if (key1)
            {
                GUI.Label(new Rectangle(-30, 0, (int)width, (int)(fontSize * 2f)), $"⚷", 24, StringAlignment.Center, Color.Red);
            }

            if (key2)
            {
                GUI.Label(new Rectangle(0, 0, (int)width, (int)(fontSize * 2f)), $"⚷", 24, StringAlignment.Center, Color.Green);

            }

            if (key3)
            {
                GUI.Label(new Rectangle(30, 0, (int)width, (int)(fontSize * 2f)), $"⚷", 24, StringAlignment.Center, Color.Blue);
            }

            // Death Scene
            if (lives == 0)
            {
                entityManager.Close();
                sceneManager.ChangeScene(SceneTypes.SCENE_GAME_OVER);
            }

            // Rotate lights
            // blueSpot.GetComponent<ComponentPosition>().position *= Matrix3.CreateRotationY(0.0125f);
        }
       
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="e">Provides a snapshot of timing values.</param>
        public override void Render(FrameEventArgs e)
        {
            GL.Viewport(0, 0, sceneManager.Width, sceneManager.Height);
            GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Action ALL systems
            systemManager.ActionSystems(entityManager);

            skybox.RenderSkyBox();

            // Render score
            GUI.clearColour = Color.Transparent;
            float width = sceneManager.Width, height = sceneManager.Height, fontSize = Math.Min(width, height) / 10f;
            GUI.Label(new Rectangle(0, -30, (int)width, (int)(fontSize * 2f)), $"Keys collected", 18, StringAlignment.Center, Color.White);
            PrintLives(width, fontSize, lives);

            GL.Flush();
            GUI.Render();
        }

        /// <summary>
        /// This is called when the game exits.
        /// </summary>
        public override void Close()
        {
            sceneManager.keyboardDownDelegate -= Keyboard_KeyDown;
            sceneManager.keyboardUpDelegate -= Keyboard_KeyUp;
            // WallBoundaries.Close();
        }

        public void Keyboard_KeyDown(KeyboardKeyEventArgs e)
        {
            keyPressed[(char)e.Key] = true;
        }

        public void Keyboard_KeyUp(KeyboardKeyEventArgs e)
        {
            keyPressed[(char)e.Key] = false;
        }

        private void PrintLives(float width, float fontSize, int livesLeft)
        {
            GUI.Label(new Rectangle(0, 0, (int)width, (int)(fontSize * 2f)), $"Lives: {livesLeft} {heart}", 18, StringAlignment.Near, Color.Red);
        }

        public void EndGame()
        {
            entityManager.Close();
            sceneManager.ChangeScene(SceneTypes.SCENE_WIN);
        }

        public void LoseLife()
        {
            --lives;
            player.GetComponent<ComponentAudio>().PlayAudioOnce();
            camera.cameraPosition = origin;
            camera.cameraDirection = new Vector3(1, 0, 0);
        }
        private void CreatePointLight(string name, Entity entity, Vector3 position, Vector3 colour, float con, float lin, float quad)
        {
            entity = new Entity(name, OBJECT_TYPE.LIGHTEMITTER);
            entity.AddComponent(new ComponentPosition(position));
            entity.AddComponent(new ComponentLightSource(colour, colour * 0.1f, colour * 0.1f, con, lin, quad));
            entity.AddComponent(new ComponentInScene());
            entityManager.AddEntity(entity);
        }

        private void GenerateTorch(Entity entity, string name, float x, float y, float z)
        {
            entity = new Entity(name, OBJECT_TYPE.SPOTLIGHT);
            entity.AddComponent(new ComponentPosition(x, y, z));
            entity.AddComponent(new ComponentGeometry("Geometry/Map/torch.obj"));
            entity.AddComponent(new ComponentInScene());
            entityManager.AddEntity(entity);
        }

        public Vector3 GenRandomSpawn(float xMax, float xMin, float zMax, float zMin)
        {
            Random rng = new Random();
            float x = rng.Next((int)(xMin + 1), (int)(xMax - 1));
            float z = rng.Next((int)(zMin + 1), (int)(zMax - 1));

            return new Vector3(x, 0.0f, z);
        }

        public void OpenPortal()
        {
            openPortal = new Entity("Portal Open", OBJECT_TYPE.PORTAL);
            openPortal.AddComponent(new ComponentPosition(portalPosition));
            openPortal.AddComponent(new ComponentGeometry("Geometry/Portal/activePortal.obj"));
            openPortal.AddComponent(new ComponentCollisionSphere(1.5f));
            openPortal.AddComponent(new ComponentAudio("Audio/PortalOn.wav", true));
            openPortal.AddComponent(new ComponentInScene());
            entityManager.AddEntity(openPortal);
        }
    }
}