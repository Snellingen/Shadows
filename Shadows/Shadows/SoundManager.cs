using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Shadows
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class SoundManager : Microsoft.Xna.Framework.GameComponent
    {
        // Her putter du atributtene (alle lydeffektene og sangene) 
        public Dictionary<string, SoundEffect> sounds = new Dictionary<string, SoundEffect>();
        public Dictionary<string, Song> songs = new Dictionary<string, Song>();
        public Dictionary<string, SoundEffectInstance> effectSounds = new Dictionary<string, SoundEffectInstance>();

        public float soundVolume = 1f; 
        public Song currentSong;

        float time = 0f;
        GameTime gameTime; 

        //constructor   
        public SoundManager(Game game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public void setGameTime(GameTime gameTime)
        {
            this.gameTime = gameTime;
        }

        public void TryLoadSound(string assetName, bool createSoundEffectInstance)
        {
            //loader lyder (soundeffects) inn i dictionaryen 
            SoundEffect sound = Game.Content.Load<SoundEffect>(@"Sound/Effects/"+ assetName);

             if (createSoundEffectInstance)
                 effectSounds.Add(assetName, sound.CreateInstance()); 

            sounds.Add(assetName, sound); 
            
        }

        public void TryLoadSong(string assetName )
        {
            songs.Add(assetName, Game.Content.Load<Song>(@"Sound/Music/" + assetName));
            
        }


        // denne vil spille av en lydeffect etter hva den heter. f.eks om du har en lydeffekt i dictionaryen som heter "growl" s� kan man skrive PlaySound("growl"); 
        public void PlaySound(string name)
        {
             SoundEffect effect; 
            if (sounds.TryGetValue(name, out effect)) // Henter verdien fra dictonary ut i fra navnet p� lyden som er lagret
                effect.Play(soundVolume,  0f, 0f); // soundvolme sier seg selv, 0f la v�re(pitch), 0f la v�re(pan)
        }

        public void PlaySoundContinuously(string name, float miliseconds)
        {
            time -= gameTime.ElapsedGameTime.Milliseconds;
            if (time < 0)
            {
                PlaySound(name);
                this.time = miliseconds; 
            }
            
        }

        public void PlaySoundLoop(string name)
        {
            SoundEffectInstance instance;
            if (effectSounds.TryGetValue(name, out instance))
            {
                if(!instance.IsLooped)
                instance.IsLooped = true;
                instance.Play(); 
            }
        }

        public void StopSoundLoop(string name)
        {
            SoundEffectInstance instance;
            if (effectSounds.TryGetValue(name, out instance))
            { 
                instance.Stop();
            }
        }

        public void PlaySong(string name)
        {
            Song song;
            if (songs.TryGetValue(name, out song))
            {
                MediaPlayer.Play(song);
                currentSong = song; 
            }
        }

        public void PauseSong()
        {
            if (currentSong != null)
                MediaPlayer.Pause();
        }

        public void StopSong()
        {
            if (currentSong != null)
                MediaPlayer.Stop(); 
        }
        
        public void setVolumeEffects(float volume)
        {
            soundVolume = volume; 
        }

        public void setVolumeMediaPlayer(float volume)
        {
            MediaPlayer.Volume = volume; 
        }

        public override void Update(GameTime gameTime)
        {


            base.Update(gameTime);
        }

        // SE HVORDAN MAN BRUKER DEN I GAME1 UPDATE(); 
    }
}
