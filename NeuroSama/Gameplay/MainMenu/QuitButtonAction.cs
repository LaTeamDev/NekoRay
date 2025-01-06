using NekoLib.Core;
using NekoRay;
using NeuroSama.UI;

namespace NeuroSama.Gameplay.MainMenu;

public class QuitButtonAction : Behaviour {
    void OnButtonPressed(Button sender) { //TODO: click
        Program.Quit();
    }
}