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
        public float soundVolume = 1f; 
        public Song currentSong;

        //constructor   
        public SoundManager(Game game)
            : base(game)
        {
        }

        public override void Initialize()
        {

            base.Initialize();
        }

        public void LoadSound(string assetName)
        {
            //loader lyder (soundeffects) inn i dictionaryen 
            sounds.Add(assetName, Game.Content.Load<SoundEffect>(@"Sound/Effects/"+ assetName)); 
            
        }

        public void LoadSong(string assetName)
        {
            songs.Add(assetName, Game.Content.Load<Song>(@"Sound/Music/" + assetName)); 
        }


        // denne vil spille av en lydeffect etter hva den heter. f.eks om du har en lydeffekt i dictionaryen som heter "growl" så kan man skrive PlaySound("growl"); 
        public void PlaySound(string name)
        {
             SoundEffect effect; 
            if (sounds.TryGetValue(name, out effect)) // Henter verdien fra dictonary ut i fra navnet på lyden som er lagret
                effect.Play(soundVolume,  0f, 0f); // soundvolme sier seg selv, 0f la være(pitch), 0f la være(pan)
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
