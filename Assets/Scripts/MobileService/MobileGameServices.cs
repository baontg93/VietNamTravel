using System;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.EssentialKit;

public class MobileGameServices : SingletonBehaviour<MobileGameServices>
{
    public static event Action OnAuthenticated;
    private readonly Dictionary<string, AchievementData> achievementDescriptions = new();

    public bool IsReady()
    {
        return GameServices.IsAuthenticated;
    }

    private void Awake()
    {
        GameServices.Authenticate();
    }

    protected void OnEnable()
    {
        GameServices.OnAuthStatusChange += OnAuthStatusChange;
    }

    protected void OnDisable()
    {
        GameServices.OnAuthStatusChange -= OnAuthStatusChange;
    }

    private void OnAuthStatusChange(GameServicesAuthStatusChangeResult result, Error error)
    {
        Debug.Log("Received auth status change event");
        Debug.Log("Auth status: " + result.AuthStatus);

        if (result.AuthStatus == LocalPlayerAuthStatus.Authenticated)
        {
            GameServices.LoadAchievementDescriptions((result, error) =>
            {
                if (error == null)
                {
                    IAchievementDescription[] descriptions = result.AchievementDescriptions;
                    Debug.Log("Request to load achievement descriptions finished successfully.");
                    Debug.Log("Total achievement descriptions fetched: " + descriptions.Length);
                    Debug.Log("Below are the available achievement descriptions:");
                    for (int iter = 0; iter < descriptions.Length; iter++)
                    {
                        IAchievementDescription description1 = descriptions[iter];
                        description1.LoadImage((textureData, innerError) =>
                        {
                            if (textureData != null && innerError == null)
                            {
                                AchievementData data = new();
                                data.Id = description1.Id;
                                data.Description = description1.AchievedDescription;
                                data.Title = description1.Title;
                                data.Sprite = textureData.GetTexture().GetSprite();
                                achievementDescriptions.TAdd(description1.Id, data);

                                Debug.Log("- data: " + JsonUtility.ToJson(data));
                            }
                        });
                    }
                }
                else
                {
                    Debug.Log("Request to load achievement descriptions failed with error. Error: " + error);
                }

                OnAuthenticated?.Invoke();
            });
        }
    }

    public void SetScore(int score)
    {
        GameServices.ReportScore("1", score, (error) =>
        {
            if (error == null)
            {
                Debug.Log("Request to submit score finished successfully.");
            }
            else
            {
                Debug.Log("Request to submit score failed with error: " + error.Description);
            }
        });

        SetAchievement(score);
    }

    public void SetAchievement(int score)
    {
        Dictionary<string, int> achievementData = new()
        {
            { "1", 1 },
            { "2", 10 },
            { "3", 30 },
            { "4", 63 },
        };

        double percentageCompleted = 0;
        string id = "1";

        foreach (var item in achievementData)
        {
            if (item.Value >= score)
            {
                id = item.Key;
                percentageCompleted = score / item.Value;
                break;
            }
        }

        GameServices.ReportAchievementProgress(id, percentageCompleted, (error) =>
        {
            if (error == null)
            {
                Debug.Log("Request to submit score finished successfully.");
            }
            else
            {
                Debug.Log("Request to submit score failed with error: " + error.Description);
            }
        });
    }


    public void LoadAchievements(Action<List<AchievementData>> onComplete)
    {
        GameServices.LoadAchievements((result, error) =>
        {
            List<AchievementData> datas = null;
            if (error == null)
            {
                // show console messages
                var achievements = result.Achievements;
                Debug.Log("Request to load achievements finished successfully.");
                Debug.Log("Total achievements fetched: " + achievements.Length);
                Debug.Log("Below are the available achievements:");
                datas = new();
                for (int iter = 0; iter < achievements.Length; iter++)
                {
                    var achievement1 = achievements[iter];
                    if(achievementDescriptions.ContainsKey(achievement1.Id))
                    {
                        AchievementData data = achievementDescriptions[achievement1.Id];
                        datas.Add(data);
                        Debug.Log($"achievement {achievement1.Id}: " + JsonUtility.ToJson(data));
                    }
                }

            }
            else
            {
                Debug.Log("Request to load achievements failed with error. Error: " + error);
            }
            onComplete?.Invoke(datas);
        });
    }

    public void LoadLeaderboard(Action<List<UserData>> onComplete)
    {
        ILeaderboard leaderboard = GameServices.CreateLeaderboard("1");
        leaderboard.LoadScoresQuerySize = 1;
        leaderboard.TimeScope = LeaderboardTimeScope.AllTime;
        leaderboard.LoadTopScores((result, error) =>
        {
            List<UserData> datas = null;
            if (result != null && error == null)
            {
                Debug.Log("Scores length : " + result.Scores.Length);
                IScore localPlayerScore = leaderboard.LocalPlayerScore;
                if (localPlayerScore != null)
                {
                    Debug.Log(string.Format("Local Player Score : {0}  Rank : {1}", localPlayerScore.Value, localPlayerScore.Rank));
                }
                else
                {
                    Debug.Log("No local player score available!");
                }

                Debug.Log("Displaying scores..." + result.Scores.Length);
                if (result.Scores.Length > 0)
                {
                    datas = new();
                    int counter = 0;
                    foreach (IScore score in result.Scores)
                    {

                        score.Player.LoadImage((textureData, innerError) =>
                        {
                            if (textureData != null && innerError == null)
                            {
                                UserData user = new()
                                {
                                    Name = score.Player.DisplayName,
                                    Point = score.Value,
                                    Avatar = textureData.GetTexture().GetSprite()
                                };
                                datas.Add(user);
                                counter++;
                                if (counter == result.Scores.Length)
                                {
                                    onComplete?.Invoke(datas);
                                }
                            }
                        });
                    }

                }
                else
                {
                    onComplete?.Invoke(datas);
                }

            }

        });
    }
}
