using System.Collections.Generic;
using UnityEngine;
using YG;
using Plugins.Audio.Core;


public class GameController : MonoBehaviour
{
    public enum GameDifficulty { Easy, Normal, Hard, Free };


    [Header("Режим игры")]
    [SerializeField] private bool isHardmode = false;
    private int roundsCount = 20;
    [SerializeField] private int roundsPassed = 0;
    [SerializeField] private int roundNum = 0;
    [SerializeField] private GameDifficulty gameDifficulty;
    private float roundDuration = 10;
    [SerializeField] private int score = 0;
    [SerializeField] private bool isRevived = false;


    [Header("Игровые объекты")]
    [SerializeField] private GameObject level;
    [SerializeField] private Transform levelPosition;
    [SerializeField] private GameObject defeatMenu;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject currentMenu;
    [SerializeField] private GameObject victoryMenu;
    [SerializeField] private GameObject difficultyMenu;
    [SerializeField] private GameObject leaderboardMenu;
    [SerializeField] private GameObject howToPlayMenu;
    [SerializeField] private SourceAudio _source;

    [Header("Песни")]
    [SerializeField] private List<SongObject> songs = new List<SongObject>();
    [SerializeField] private List<SongObject> roundSongs = new List<SongObject>();

    private void Start()
    {
        if (YandexGame.SDKEnabled)
        {
            LoadSaveCloud();
        }
    }

    private void LoadSaveCloud()
    {
        score = YandexGame.savesData.score;
        Debug.Log("Got score");
    }
    public void SetHardmode(bool newHardmodeState)
    {
        isHardmode = newHardmodeState;
    }
    public void SetGameDifficulty(int newDifficulty)
    {
        gameDifficulty = (GameDifficulty)newDifficulty;

    }

    public void StartNewGame()
    {
        score = 0;

        foreach (SongObject song in songs)
        {
            if (gameDifficulty == GameDifficulty.Free)
            {
                song.songName = string.Format("Assets/Songs/Songs/{0}.mp3", song.GetSongName());
                roundSongs.Add(song);
            }
            else
            {
                if (song.GetDifficulty() == (int)gameDifficulty)
                {
                    song.songName = string.Format("Assets/Songs/Songs/{0}.mp3", song.GetSongName());
                    roundSongs.Add(song);
                }
            }

        };

        Destroy(currentMenu);
        SongObject[] levelSongs = new SongObject[4];
        string[] songNames = new string[4];
        List<SongObject> options = new List<SongObject>(songs);

        SongObject answerSong = roundSongs[Random.Range(0, roundSongs.Count - 1)];
        int answerIndex = Random.Range(0, levelSongs.Length);
        levelSongs[answerIndex] = answerSong;
        songNames[answerIndex] = answerSong.GetNameInGame();

        for (int i = 0; i < 4; i++)
        {
            if (i != answerIndex)
            {
                while (true)
                {
                    levelSongs[i] = options[Random.Range(0, options.Count - 1)];
                    if (levelSongs[i].GetNameInGame() != answerSong.GetNameInGame())
                    {
                        break;
                    }
                }
                options.Remove(levelSongs[i]);
                songNames[i] = levelSongs[i].GetNameInGame();

            }


        }



        roundSongs.Remove(answerSong);



        CreateRound(songNames, answerSong.GetSongKey(), answerSong.GetNameInGame());

    }

    public void ContinueGame()
    {
        if (roundSongs.Count < 4 || roundNum == roundsCount)
        {
            CreateVictoryMenu();
        }
        else
        {
            SongObject[] levelSongs = new SongObject[4];
            string[] songNames = new string[4];
            List<SongObject> options = new List<SongObject>(songs);

            SongObject answerSong = roundSongs[Random.Range(0, roundSongs.Count - 1)];
            int answerIndex = Random.Range(0, levelSongs.Length);
            levelSongs[answerIndex] = answerSong;
            songNames[answerIndex] = answerSong.GetNameInGame();
            Debug.Log(levelSongs[answerIndex].songName);

            for (int i = 0; i < 4; i++)
            {
                if (i != answerIndex)
                {
                    while (true)
                    {
                        levelSongs[i] = options[Random.Range(0, options.Count - 1)];
                        if (levelSongs[i].GetNameInGame() != answerSong.GetNameInGame())
                        {
                            break;
                        }
                    }
                    options.Remove(levelSongs[i]);
                    songNames[i] = levelSongs[i].GetNameInGame();

                }


            }

            roundSongs.Remove(answerSong);



            CreateRound(songNames, answerSong.GetSongKey(), answerSong.GetNameInGame());
        }

    }


    public void CreateRound(string[] answers_, string opKey, string answer)
    {
        GameObject round = Instantiate(level, levelPosition);
        roundNum++;
        round.GetComponent<RoundManager>().InitializeRound(answers_, opKey, answer, this.gameObject, roundDuration, isHardmode, levelPosition);


    }

    public void UpdateScore(bool roundPassed)
    {
        if (isHardmode)
        {
            if (roundPassed)
            {
                if ((int)gameDifficulty + 1 == 4) score += 15 * 2;
                else score += 15 * ((int)gameDifficulty + 1);

            }
            else
            {

            }
        }
        else
        {
            if (roundPassed)
            {
                if ((int)gameDifficulty + 1 == 4) score += 10 * 2;
                else score += 10 * ((int)gameDifficulty + 1);
            }
            else
            {

            }
        }

    }

    public void CreateDefeatMenu()
    {

        GameObject defeatMenu_ = Instantiate(defeatMenu, levelPosition);

        defeatMenu_.GetComponent<DefeatMenu>().Initializer(this.gameObject);
    }

    public void CreateMenu()
    {
        isHardmode = false;
        GameObject menu_ = Instantiate(menu, levelPosition);
        menu_.GetComponent<Menu>().SetGameManagaer(this.gameObject);
        currentMenu = menu_;

    }

    public int GetScore()
    {
        return score;
    }
    private void CreateVictoryMenu()
    {
        GameObject victory = Instantiate(victoryMenu, levelPosition);
        victory.GetComponent<VictoryMenu>().Initializer(this.gameObject, roundsPassed);

    }

    public void ResetGameParametrs()
    {
        roundSongs.Clear();
        roundsPassed = 0;
        roundNum = 0;
        isRevived = false;

        YandexGame.savesData.score += score;
        YandexGame.SaveProgress();
        YandexGame.NewLeaderboardScores("AnimeSongDB", YandexGame.savesData.score);

    }

    public void BackFromLevel()
    {
        roundSongs.Clear();
        roundsPassed = 0;
        roundNum = 0;
        isRevived = false;
    }

    public void ChangeIsRevived(bool newState)
    {
        isRevived = newState;
    }
    public bool GetIsRevived()
    {
        return isRevived;
    }

    public void AddPassedRound()
    {
        roundsPassed += 1;
    }
    public void CreateDifficultyMenu()
    {
        GameObject difficultyMenu_ = Instantiate(difficultyMenu, levelPosition);
        difficultyMenu_.GetComponent<DifficultyMenu>().SetGameManager(this.gameObject);
    }

    public void CreateLeaderboardMenu()
    {
        GameObject leaderboardMenu_ = Instantiate(leaderboardMenu, levelPosition);
        leaderboardMenu_.GetComponent<LeaderboardMenu>().SetGameManager(this.gameObject);
    }

    public void CreateHowToPlay()
    {
        GameObject howToPlay_ = Instantiate(howToPlayMenu, levelPosition);
        howToPlay_.GetComponent<HowToPlay>().SetGameManager(this.gameObject);
    }

    public void PlayButtonSound()
    {
        _source.Volume = 1f;
        _source.Play("Assets/Sounds/ButtonClick.mp3");
    } 
    public int GetRoundNum() => roundNum;

}
