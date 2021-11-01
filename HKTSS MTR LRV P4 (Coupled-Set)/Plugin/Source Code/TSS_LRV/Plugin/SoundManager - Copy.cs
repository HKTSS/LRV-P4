using OpenBveApi.Runtime;

namespace Plugin {
    /// <summary>The interface to be implemented by the plugin.</summary>
    public partial class Plugin : IRuntime {

        /// <summary>Manages the playback of sounds.</summary>
        internal static class SoundManager {
            private static SoundManager.CarSounds[] SoundHandles;
            private static PlaySoundDelegate PlaySound;
            private static PlayCarSoundDelegate PlayCarSound;

            internal static void Initialise(PlaySoundDelegate playSound, PlayCarSoundDelegate playCarSound, int numIndices) {
                SoundManager.SoundHandles = new SoundManager.CarSounds[256];
                int index = 0;
                while (index < SoundManager.SoundHandles.Length) {
                    SoundManager.SoundHandles[index].CurrentHandles = new SoundHandle[numIndices];
                    SoundManager.SoundHandles[index].IsLooped = new bool[numIndices];
                    SoundManager.SoundHandles[index].LastVolume = new double[numIndices];
                    SoundManager.SoundHandles[index].LastPitch = new double[numIndices];
                    checked { ++index; }
                }
                SoundManager.PlaySound = playSound;
                SoundManager.PlayCarSound = playCarSound;
            }

            internal static void Play(int soundIndex, double volume, double pitch, bool loop) {
                volume = volume < 0.0 ? 0.0 : volume;
                pitch = pitch < 0.0 ? 0.0 : pitch;
                if (soundIndex == -1)
                    return;
                if (SoundManager.SoundHandles[0].CurrentHandles[soundIndex] != null) {
                    if (SoundManager.SoundHandles[0].IsLooped[soundIndex] && SoundManager.SoundHandles[0].CurrentHandles[soundIndex].Playing) {
                        SoundManager.SoundHandles[0].CurrentHandles[soundIndex].Volume = volume;
                        SoundManager.SoundHandles[0].CurrentHandles[soundIndex].Pitch = pitch;
                    } else if (volume == SoundManager.SoundHandles[0].LastVolume[soundIndex] && pitch == SoundManager.SoundHandles[0].LastPitch[soundIndex]) {
                        SoundManager.SoundHandles[0].CurrentHandles[soundIndex].Stop();
                        SoundManager.SoundHandles[0].CurrentHandles[soundIndex] = SoundManager.PlaySound.Invoke(soundIndex, volume, pitch, loop);
                    } else if (SoundManager.SoundHandles[0].CurrentHandles[soundIndex].Playing) {
                        SoundManager.SoundHandles[0].CurrentHandles[soundIndex].Pitch = pitch;
                        SoundManager.SoundHandles[0].CurrentHandles[soundIndex].Volume = volume;
                    } else
                        SoundManager.SoundHandles[0].CurrentHandles[soundIndex] = SoundManager.PlaySound.Invoke(soundIndex, volume, pitch, loop);
                } else
                    SoundManager.SoundHandles[0].CurrentHandles[soundIndex] = SoundManager.PlaySound.Invoke(soundIndex, volume, pitch, loop);
                SoundManager.SoundHandles[0].IsLooped[soundIndex] = loop;
                SoundManager.SoundHandles[0].LastVolume[soundIndex] = volume;
                SoundManager.SoundHandles[0].LastPitch[soundIndex] = pitch;
            }

            internal static void PlayCarriage(int soundIndex, double volume, double pitch, bool loop, int carIndex) {
                if (carIndex > checked(Plugin.totalCar - 1)) return;
                volume = volume < 0.0 ? 0.0 : volume;
                pitch = pitch < 0.0 ? 0.0 : pitch;
                if (soundIndex == -1)
                    return;
                if (SoundManager.SoundHandles[carIndex].CurrentHandles[soundIndex] != null) {
                    if (SoundManager.SoundHandles[carIndex].IsLooped[soundIndex] && SoundManager.SoundHandles[carIndex].CurrentHandles[soundIndex].Playing) {
                        SoundManager.SoundHandles[carIndex].CurrentHandles[soundIndex].Volume = volume;
                        SoundManager.SoundHandles[carIndex].CurrentHandles[soundIndex].Pitch = pitch;
                    } else if (volume == SoundManager.SoundHandles[carIndex].LastVolume[soundIndex] && pitch == SoundManager.SoundHandles[carIndex].LastPitch[soundIndex]) {
                        SoundManager.SoundHandles[carIndex].CurrentHandles[soundIndex].Stop();
                        SoundManager.SoundHandles[carIndex].CurrentHandles[soundIndex] = SoundManager.PlayCarSound.Invoke(soundIndex, volume, pitch, loop, carIndex);
                    } else if (SoundManager.SoundHandles[carIndex].CurrentHandles[soundIndex].Playing) {
                        SoundManager.SoundHandles[carIndex].CurrentHandles[soundIndex].Pitch = pitch;
                        SoundManager.SoundHandles[carIndex].CurrentHandles[soundIndex].Volume = volume;
                    } else
                        SoundManager.SoundHandles[carIndex].CurrentHandles[soundIndex] = SoundManager.PlayCarSound.Invoke(soundIndex, volume, pitch, loop, carIndex);
                } else
                    SoundManager.SoundHandles[carIndex].CurrentHandles[soundIndex] = SoundManager.PlayCarSound.Invoke(soundIndex, volume, pitch, loop, carIndex);
                SoundManager.SoundHandles[carIndex].IsLooped[soundIndex] = loop;
                SoundManager.SoundHandles[carIndex].LastVolume[soundIndex] = volume;
                SoundManager.SoundHandles[carIndex].LastPitch[soundIndex] = pitch;
            }

            internal static void Stop(int soundIndex) {
                if (soundIndex == -1 || SoundManager.SoundHandles[0].CurrentHandles[soundIndex] == null)
                    return;
                SoundManager.SoundHandles[0].CurrentHandles[soundIndex].Stop();
                SoundManager.SoundHandles[0].IsLooped[soundIndex] = false;
            }

            internal static void StopCarriage(int soundIndex, int carIndex) {
                if (carIndex > checked(Plugin.totalCar - 1) || (soundIndex == -1 || SoundManager.SoundHandles[carIndex].CurrentHandles[soundIndex] == null))
                    return;
                SoundManager.SoundHandles[carIndex].CurrentHandles[soundIndex].Stop();
                SoundManager.SoundHandles[carIndex].IsLooped[soundIndex] = false;
            }

            internal static bool IsPlaying(int soundIndex) => soundIndex != -1 && SoundManager.SoundHandles[0].CurrentHandles[soundIndex] != null && SoundManager.SoundHandles[0].CurrentHandles[soundIndex].Playing;

            internal static bool IsPlayingCarriage(int soundIndex, int carIndex) => carIndex <= checked(Plugin.totalCar - 1) && (soundIndex != -1 && SoundManager.SoundHandles[carIndex].CurrentHandles[soundIndex] != null && SoundManager.SoundHandles[carIndex].CurrentHandles[soundIndex].Playing);

            internal static double GetLastPitch(int soundIndex) => SoundManager.IsPlaying(soundIndex) ? SoundManager.SoundHandles[0].LastPitch[soundIndex] : -1.0;

            internal static double GetLastPitchCarriage(int soundIndex, int carIndex) => carIndex > checked(Plugin.totalCar - 1) || !SoundManager.IsPlaying(soundIndex) ? -1.0 : SoundManager.SoundHandles[carIndex].LastPitch[soundIndex];

            internal static double GetLastVolume(int soundIndex) => SoundManager.IsPlaying(soundIndex) ? SoundManager.SoundHandles[0].LastVolume[soundIndex] : -1.0;

            internal static double GetLastVolumeCarriage(int soundIndex, int carIndex) => carIndex > checked(Plugin.totalCar - 1) || !SoundManager.IsPlaying(soundIndex) ? -1.0 : SoundManager.SoundHandles[carIndex].LastVolume[soundIndex];

            internal struct CarSounds {
                internal bool[] IsLooped;
                internal SoundHandle[] CurrentHandles;
                internal double[] LastVolume;
                internal double[] LastPitch;
            }
        }
    }
}
