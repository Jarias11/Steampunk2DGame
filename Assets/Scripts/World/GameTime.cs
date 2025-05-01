using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameTime : MonoBehaviour {
    public static GameTime Instance;

    [Header("Time Settings")]
    public int minutesPerDay = 1440;
    public float realSecondsPerInGameMinute = 1f;
    private float timer = 0f;

    public int minute;
    public int hour;
    public int day;
    public int week;
    public int month;
    public int year;
    public Season currentSeason;

    [SerializeField] private float secondsPerMinute = 0.5f; // Controls real-time pace
    public enum Season { Spring, Summer, Fall, Winter }

    public event Action<int, int, int> OnTimeChanged; // hour day month
    public event Action<Season> OnSeasonChanged;

    [SerializeField] private Light2D globalLight;
    private float currentIntensity = 1f;
    private Color dayColor = Color.white;
    private Color nightColor = new Color(0.2f, 0.2f, 0.4f); // cool dark blue
    private Color dawnColor = new Color(1f, 0.65f, 0.4f);    // warm orange
    private Color duskColor = new Color(1f, 0.5f, 0.6f);     // pinkish red

    private void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start() {
        minute = 0;
        hour = 6;
        day = 1;
        month = 1;
        year = 1;
        currentSeason = Season.Spring;
    }
    void Update() {
        timer += Time.deltaTime;
        if (timer > secondsPerMinute) {
            timer = 0;
            AdvanceMinute();
        }
    }
    void AdvanceMinute() {
        minute++;
        if (minute >= 60) {
            minute = 0;
            hour++;
            if (hour >= 24) {
                hour = 0;
                AdvanceDay();
            }
        }
        UpdateLighting();
        Debug.Log($"🕒 Time: {hour:D2}:{minute:D2} | Day {day}, Month {month}, Year {year} | Season: {currentSeason}");
        OnTimeChanged?.Invoke(hour, day, month);

    }
    void AdvanceDay() {
        day++;
        if (day % 7 == 1) week++;

        if (day > 30) {
            day = 1;
            month++;
            UpdateSeason();
            if (month > 12) {
                month = 1;
                year++;
            }
        }

    }
    public void UpdateSeason() {
        Season newSeason = (Season)((month - 1) / 3);
        if (newSeason != currentSeason) {
            currentSeason = newSeason;
            OnSeasonChanged?.Invoke(currentSeason);
        }
    }
    private void UpdateLighting() {
        float targetIntensity = 1f;
        Color targetColor = dayColor;

        if (hour >= 20 || hour < 6) {
            targetIntensity = 0.2f;
            targetColor = nightColor;
        }
        else if (hour >= 18) {
            targetIntensity = 0.5f;
            targetColor = duskColor;
        }
        else if (hour >= 6 && hour < 8) {
            targetIntensity = 0.7f;
            targetColor = dawnColor;
        }

        currentIntensity = Mathf.Lerp(currentIntensity, targetIntensity, Time.deltaTime * 2f);
        globalLight.intensity = currentIntensity;

        globalLight.color = Color.Lerp(globalLight.color, targetColor, Time.deltaTime * 2f);
    }

    public GameTimeData GetTimeData() {
        return new GameTimeData {
            minute = this.minute,
            hour = this.hour,
            day = this.day,
            week = this.week,
            month = this.month,
            year = this.year,
            currentSeason = this.currentSeason
        };
    }

    public void SetTimeData(GameTimeData data) {
        this.minute = data.minute;
        this.hour = data.hour;
        this.day = data.day;
        this.week = data.week;
        this.month = data.month;
        this.year = data.year;
        this.currentSeason = data.currentSeason;

        // Optional: update lighting immediately after loading
        UpdateLighting();
    }


}





[System.Serializable]
public class GameTimeData {
    public int minute;
    public int hour;
    public int day;
    public int week;
    public int month;
    public int year;
    public GameTime.Season currentSeason;
}
