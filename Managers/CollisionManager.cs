using System.Collections.Generic;
using OpenGL_Game.Scenes;
using OpenGL_Game.Objects;
using OpenGL_Game.Components;
using OpenTK;

namespace OpenGL_Game.Managers
{
    enum COLLISIONTYPE
    {
        SPHERE_SPHERE,
        LINE_LINE
    }
    struct Collision
    {
        public Entity entity;
        public COLLISIONTYPE collisionType;
    }

    abstract class CollisionManager
    {
        protected List<Collision> collisionManifold = new List<Collision>();
        public CollisionManager() { }
        public void ClearManifold() { collisionManifold.Clear(); }
        public void CollisionBetweenCamera(Entity entity, COLLISIONTYPE collisionType)
        {
            // do not add if collision is in the manifold
            foreach (Collision coll in collisionManifold)
            {
                if (coll.entity == entity) { return; }
            }
            Collision collision;
            collision.entity = entity;
            collision.collisionType = collisionType;
            collisionManifold.Add(collision);
        }
        public abstract void ProcessCollisions(Entity entity);
    }

    class EntityCollisionManager : CollisionManager
    {
        GameScene gameInstance = GameScene.gameInstance;
        
        public override void ProcessCollisions(Entity entity)
        {
            Components.ComponentAudio entityAudio = entity.GetComponent<Components.ComponentAudio>();
           
            switch (entity.name)
            {
                case "Key 1":
                    entityAudio.PlayAudioOnce();
                    gameInstance.key1 = true;
                    DeleteSelf(entity);
                    break;
                case "Key 2":
                    entityAudio.PlayAudioOnce();
                    gameInstance.key2 = true;
                    DeleteSelf(entity);
                    break;
                case "Key 3":
                    entityAudio.PlayAudioOnce();
                    gameInstance.key3 = true;
                    DeleteSelf(entity);
                    break;
                case "Rolling Ball":
                    Die();
                    break;
                case "Bouncing Ball":
                    Die();
                    break;
                default:
                    break;
            }

            // PORTAL
            if (entity.objectType == OBJECT_TYPE.PORTAL)
            {
                gameInstance.EndGame();
            }
            if (entity.objectType == OBJECT_TYPE.ENEMY)
            {
                // Don't call to delete drone enemy
                // instead call death function from gamescene
                gameInstance.LoseLife();
            }
        }

        private void DeleteSelf(Entity entity)
        {
            entity.Delete();
        }

        private void Die()
        {
            --gameInstance.lives;
            gameInstance.player.GetComponent<ComponentAudio>().PlayAudioOnce();
            gameInstance.camera.cameraPosition = gameInstance.origin;
            gameInstance.camera.cameraDirection = new Vector3(1, 0, 0);
        }
    }

    class LineCollisionManager : CollisionManager
    {
        GameScene gameInstance = GameScene.gameInstance;

        public override void ProcessCollisions(Entity entity)
        {
            // Stops camera/player from exceeding wall boundaries by resetting the position of the player
            gameInstance.camera.cameraPosition = gameInstance.oldPosition;
        }
    }
}
