using Box2D;
using NekoLib.Core;
using NekoLib.Scenes;
using NekoRay.Easings;
using Tomlyn.Syntax;

namespace NeuroSama.Gameplay.MiniGame;

public class AnswerTrigger : Behaviour {
    public bool IsCorrect = false;
    void OnSensorEnter2D(SensorEvents.BeginTouchEvent contact) {
        if (IsCorrect) OnCorrect();
        else OnFalse();
    }

    void OnCorrect() {
        SceneManager.LoadScene(new SplashScene(new OutroSceneGood(), "mg_correct"));
    }

    void OnFalse() {
        SceneManager.LoadScene(new SplashScene(new OutroSceneBad(), "mg_incorrect"));
    }
}