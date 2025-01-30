import "System.Numerics"
import ("NekoRay", "NekoRay")
local spammove = {}

function spammove:Update() 
    self:GetTransform().Position = Vector3(0, math.sin(Time.CurrentTime), 0);
end

return spammove