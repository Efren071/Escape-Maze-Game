using System;
using OpenGL_Game.Managers;
using OpenTK;
using OpenTK.Audio.OpenAL;

namespace OpenGL_Game.Components
{
    class ComponentAudio : IComponent
    {
        public int audioBuffer, audioSource;
        public Vector3 audioPosition;

        public ComponentAudio(string fileName, bool loop)
        {
            audioBuffer = ResourceManager.LoadAudio(fileName);
            audioSource = AL.GenSource();
            AL.Source(audioSource, ALSourcei.Buffer, audioBuffer); // attach the buffer to a source
            if (loop)
            {
                PlayLoopedAudio(); // play the ausio source
            }
        }

        public void SetPosition(Vector3 emitterPosition)
        {
            audioPosition = emitterPosition;
            AL.Source(audioSource, ALSource3f.Position, ref emitterPosition);
        }

        public void SetVelocity(Vector3 velocity)
        {
            audioPosition += velocity;
        }

        public void PlayLoopedAudio()
        {
            AL.Source(audioSource, ALSourceb.Looping, true);
            AL.SourcePlay(audioSource); // Start Audio
        }

        public void PlayAudioOnce()
        {
            AL.Source(audioSource, ALSourceb.Looping, false);
            AL.SourcePlay(audioSource); // Start Audio
        }

        public void PauseAudio()
        {
            AL.SourcePause(audioSource);    // Stop the audio
        }

        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_AUDIO; }
        }

        public void Close()
        {
            AL.SourceStop(audioSource);
            AL.DeleteSource(audioSource);
            AL.DeleteBuffer(audioBuffer);
        }
    }
}
