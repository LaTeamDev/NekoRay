using Box2D;

namespace NekoRay.Physics2D; 

public class NekoRayWorld : World {
    public NekoRayWorld(WorldDef def) : base(def) { }
    public NekoRayWorld() : base(new WorldDef()) { }
    public override void Step(float timeStep, int subStepCount) {
        base.Step(timeStep, subStepCount);
        var contactEvents = GetContactEvents();
        // I think it is better to make it collide with other instead of passing events as is but it will work for now
        // TODO: FIXME
        foreach (var beginTouchEvent in contactEvents.BeginEvents) {
            ((Rigidbody2D)beginTouchEvent.ShapeA.Body.UserData).GameObject.SendMessage("OnBeginContact2D", beginTouchEvent);
            ((Rigidbody2D)beginTouchEvent.ShapeB.Body.UserData).GameObject.SendMessage("OnBeginContact2D", beginTouchEvent);
        }
        foreach (var endTouchEvent in contactEvents.EndEvents) {
            ((Rigidbody2D)endTouchEvent.ShapeA.Body.UserData).GameObject.SendMessage("OnEndContact2D", endTouchEvent);
            ((Rigidbody2D)endTouchEvent.ShapeB.Body.UserData).GameObject.SendMessage("OnEndContact2D", endTouchEvent);
        }
        foreach (var hitEvent in contactEvents.HitEvents) {
            ((Rigidbody2D)hitEvent.ShapeA.Body.UserData).GameObject.SendMessage("OnHitContact2D", hitEvent);
            ((Rigidbody2D)hitEvent.ShapeB.Body.UserData).GameObject.SendMessage("OnHitContact2D", hitEvent);
        }

        var sensorEvents = GetSensorEvents();
        foreach (var beginEvent in sensorEvents.BeginEvents) {
            ((Rigidbody2D)beginEvent.SensorShape.Body.UserData).GameObject.SendMessage("OnSensorEnter2D", beginEvent);
            ((Rigidbody2D)beginEvent.VisitorShape.Body.UserData).GameObject.SendMessage("OnSensorEnter2D", beginEvent);
        }
        foreach (var endEvent in sensorEvents.EndEvents) {
            ((Rigidbody2D)endEvent.SensorShape.Body.UserData).GameObject.SendMessage("OnSensorExit2D", endEvent);
            ((Rigidbody2D)endEvent.VisitorShape.Body.UserData).GameObject.SendMessage("OnSensorExit2D", endEvent);
        }
    }
}