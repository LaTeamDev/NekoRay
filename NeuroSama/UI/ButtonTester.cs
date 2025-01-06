using NekoLib.Core;
using Serilog;

namespace NeuroSama.UI;

public class ButtonTester : Behaviour {
    void OnButtonMouseHover(Button sender) {
        //Log.Information("OnButtonMouseHover");
    }
    
    void OnButtonMouseEnter(Button sender) {
        Log.Information("OnButtonMouseEnter");
    }
    
    void OnButtonMouseLeave(Button sender) {
        Log.Information("OnButtonMouseLeave");
    }
    
    void OnButtonDown(Button sender) {
        //Log.Information("OnButtonDown");
    }
    
    void OnButtonReleased(Button sender) {
        Log.Information("OnButtonUp");
    }
    
    void OnButtonPressed(Button sender) {
        Log.Information("OnButtonPressed");
    }
}