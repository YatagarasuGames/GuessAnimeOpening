using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Song", menuName = "Song", order = 51)]
public class SongObject : ScriptableObject
{
    enum Difficulty
    {
        Easy,
        Normal,
        Hard,
        Insane
    };

    [SerializeField] public string songName;
    [SerializeField] public string songNameInGame;
    [SerializeField] private Difficulty difficulty;
    [SerializeField] private AudioClip music;
    public AudioClip GetSong()
    {
        return music;
    }
    public string GetNameInGame()
    {
        return songNameInGame;
    }

    public int GetDifficulty()
    {
        return (int)difficulty;
    }

    public string GetSongKey() => songName;
    public string GetSongName() => music.name;



}
