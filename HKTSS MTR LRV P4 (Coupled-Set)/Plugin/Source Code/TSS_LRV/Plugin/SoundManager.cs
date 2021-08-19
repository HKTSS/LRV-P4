using OpenBveApi.Runtime;

namespace Plugin {
    /// <summary>The interface to be implemented by the plugin.</summary>
    public partial class Plugin : IRuntime {

        /// <summary>Manages the playback of sounds.</summary>
        internal static class SoundManager {
            private static CarSounds[] SoundHandles;
            private static PlaySoundDelegate PlaySound;
            private static PlayCarSoundDelegate PlayCarSound;

            internal static void Initialise(PlaySoundDelegate playSound, PlayCarSoundDelegate playCarSound, int numIndices) {
                SoundHandles = new CarSounds[256];
                int i = 0;
                while (i < SoundHandles.Length) {
                    SoundHandles[i].CurrentHandles = new SoundHandle[numIndices];
                    SoundHandles[i].IsLooped = new bool[numIndices];
                    SoundHandles[i].LastVolume = new double[numIndices];
                    SoundHandles[i].LastPitch = new double[numIndices];
                    i++;
                }
                PlaySound = playSound;
                PlayCarSound = playCarSound;
            }

            internal static void Play(int soundIndex, double volume, double pitch, bool loop) {
                volume = volume < 0.0 ? 0.0 : volume;
                pitch = pitch < 0.0 ? 0.0 : pitch;
                if (soundIndex == -1) return;
                if (SoundHandles[0].CurrentHandles[soundIndex] != null) {
                    /* A handle already exists on car 0 (Cab), so... */
                    if (SoundHandles[0].IsLooped[soundIndex] && SoundHandles[0].CurrentHandles[soundIndex].Playing) {
                        /* The sound is looped... */
                        /* It is indeed playing already, so just modify the pitch and volume */
                        SoundHandles[0].CurrentHandles[soundIndex].Volume = volume;
                        SoundHandles[0].CurrentHandles[soundIndex].Pitch = pitch;
                    } else if (volume == SoundHandles[0].LastVolume[soundIndex] && pitch == SoundHandles[0].LastPitch[soundIndex]) {
                        /* Handle play-once sounds... */
                        /* The pitch and volume for this sound handle are the same as they were in the last call, so start playing a new sound instead */
                        SoundHandles[0].CurrentHandles[soundIndex].Stop();
                        SoundHandles[0].CurrentHandles[soundIndex] = PlaySound.Invoke(soundIndex, volume, pitch, loop);
                    } else if (SoundHandles[0].CurrentHandles[soundIndex].Playing) {
                        /* The handle is already playing and the pitch or volume has been changed since the last playback of this sound handle, so alter the pitch and volume, and continue the sound */
                        SoundHandles[0].CurrentHandles[soundIndex].Pitch = pitch;
                        SoundHandles[0].CurrentHandles[soundIndex].Volume = volume;
                    } else {
	                    /* Neither pitch or volume have been changed, so start playback with a new sound handle */
	                    SoundHandles[0].CurrentHandles[soundIndex] = PlaySound.Invoke(soundIndex, volume, pitch, loop);
                    }
                } else {
	                /* There is no valid handle, so create a new handle for playback */
	                SoundHandles[0].CurrentHandles[soundIndex] = PlaySound.Invoke(soundIndex, volume, pitch, loop);
                }

                SoundHandles[0].IsLooped[soundIndex] = loop;
	            SoundHandles[0].LastVolume[soundIndex] = volume;
	            SoundHandles[0].LastPitch[soundIndex] = pitch;
            }

            internal static void PlayCar(int soundIndex, double volume, double pitch, bool loop, int carIndex) {
                if (carIndex > Plugin.totalCar - 1) return;
                volume = volume < 0.0 ? 0.0 : volume;
                pitch = pitch < 0.0 ? 0.0 : pitch;
                if (soundIndex == -1) return;

                if (SoundHandles[carIndex].CurrentHandles[soundIndex] != null) {
                    /* A handle already exists on the specified car, so... */
                    if (SoundHandles[carIndex].IsLooped[soundIndex] && SoundHandles[carIndex].CurrentHandles[soundIndex].Playing) {
                        /* The sound is looped... */
                        /* It is indeed playing already, so just modify the pitch and volume */
                        SoundHandles[carIndex].CurrentHandles[soundIndex].Volume = volume;
                        SoundHandles[carIndex].CurrentHandles[soundIndex].Pitch = pitch;
                    } else if (volume == SoundHandles[carIndex].LastVolume[soundIndex] && pitch == SoundHandles[carIndex].LastPitch[soundIndex]) {
                        /* Handle play-once sounds... */
                        /* The pitch and volume for this sound handle are the same as they were in the last call, so start playing a new sound instead */
                        SoundHandles[carIndex].CurrentHandles[soundIndex].Stop();
                        SoundHandles[carIndex].CurrentHandles[soundIndex] = PlayCarSound.Invoke(soundIndex, volume, pitch, loop, carIndex);
                    } else if (SoundHandles[carIndex].CurrentHandles[soundIndex].Playing) {
                        /* Neither pitch or volume have been changed, so start playback with a new sound handle */
                        SoundHandles[carIndex].CurrentHandles[soundIndex].Pitch = pitch;
                        SoundHandles[carIndex].CurrentHandles[soundIndex].Volume = volume;
                    } else {
	                    SoundHandles[carIndex].CurrentHandles[soundIndex] = PlayCarSound.Invoke(soundIndex, volume, pitch, loop, carIndex);
                    }
                } else {
	                /* Neither pitch or volume have been changed, so start playback with a new sound handle */
	                SoundHandles[carIndex].CurrentHandles[soundIndex] = PlayCarSound.Invoke(soundIndex, volume, pitch, loop, carIndex);
                }

	            SoundHandles[carIndex].IsLooped[soundIndex] = loop;
	            SoundHandles[carIndex].LastVolume[soundIndex] = volume;
	            SoundHandles[carIndex].LastPitch[soundIndex] = pitch;
            }

            internal static void Stop(int soundIndex) {
                if (soundIndex == -1 || SoundHandles[0].CurrentHandles[soundIndex] == null) return;
                SoundHandles[0].CurrentHandles[soundIndex].Stop();
                SoundHandles[0].IsLooped[soundIndex] = false;
            }

            internal static void StopCar(int soundIndex, int carIndex) {
                /* If the carIndex specified is larger than the train's car length, or the handle of soundIndex within car specified is null, don't proceed and return */
                if (carIndex > Plugin.totalCar - 1 || (soundIndex == -1 || SoundHandles[carIndex].CurrentHandles[soundIndex] == null)) return;
                SoundHandles[carIndex].CurrentHandles[soundIndex].Stop();
                SoundHandles[carIndex].IsLooped[soundIndex] = false;
            }

            internal static bool IsPlaying(int soundIndex) {
                return soundIndex != -1 && SoundHandles[0].CurrentHandles[soundIndex] != null && SoundHandles[0].CurrentHandles[soundIndex].Playing;
            }

            internal static bool IsPlayingOnCar(int soundIndex, int carIndex) {
                return carIndex <= Plugin.totalCar - 1 && (soundIndex != -1 && SoundHandles[carIndex].CurrentHandles[soundIndex] != null && SoundHandles[carIndex].CurrentHandles[soundIndex].Playing);
            }

            internal struct CarSounds {
                internal bool[] IsLooped;
                internal SoundHandle[] CurrentHandles;
                internal double[] LastVolume;
                internal double[] LastPitch;
            }
        }
    }
}
