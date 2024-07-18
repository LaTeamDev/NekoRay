using Box2D.NetStandard.Collision;
using Box2D.NetStandard.Dynamics.Contacts;
using Box2D.NetStandard.Dynamics.World;
using Box2D.NetStandard.Dynamics.World.Callbacks;

namespace NekoRay.Physics2D; 

public class NekoRayContactListener : ContactListener {

    public override void BeginContact(in Contact contact) {
        var RigidbodyA = (Rigidbody2D) contact.GetFixtureA().GetBody().UserData;
        var RigidbodyB = (Rigidbody2D) contact.GetFixtureB().GetBody().UserData;
        if (contact.GetFixtureA().IsSensor() || contact.GetFixtureA().IsSensor()) {
            RigidbodyA.GameObject.SendMessage("OnBeginSensor2D", contact);
            RigidbodyB.GameObject.SendMessage("OnBeginSensor2D", contact);
            return;
        }
        RigidbodyA.GameObject.SendMessage("OnBeginContact2D", contact);
        RigidbodyB.GameObject.SendMessage("OnBeginContact2D", contact);
    }

    public override void EndContact(in Contact contact) {
        var RigidbodyA = (Rigidbody2D) contact.GetFixtureA().GetBody().UserData;
        var RigidbodyB = (Rigidbody2D) contact.GetFixtureB().GetBody().UserData;
        if (contact.GetFixtureA().IsSensor() || contact.GetFixtureA().IsSensor()) {
            RigidbodyA.GameObject.SendMessage("OnEndSensor2D", contact);
            RigidbodyB.GameObject.SendMessage("OnEndSensor2D", contact);
            return;
        }
        RigidbodyA.GameObject.SendMessage("OnEndContact2D", contact);
        RigidbodyB.GameObject.SendMessage("OnEndContact2D", contact);
    }

    public override void PreSolve(in Contact contact, in Manifold oldManifold) {
        //TODO:
    }

    public override void PostSolve(in Contact contact, in ContactImpulse impulse) {
        //TODO:
    }
}