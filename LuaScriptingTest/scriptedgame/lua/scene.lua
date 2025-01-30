import ("NekoLib", "NekoLib.Core")

local scene = {}

function scene:Initialize()
    for id = 1, 2000 do
        AddScriptToGameObject(GameObject("spam "..id), "spammove")
        --AddComponentToGameObject(GameObject("spam "..id), "LuaScriptingTest.SpamMove")
    end
    local gameObject = GameObject("Test");
    --AddComponent(gameObject)
    AddScriptToGameObject(gameObject, "testcomponent");
end

return scene
