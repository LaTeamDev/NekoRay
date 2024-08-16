namespace HotlineSPonyami; 

public static class Extensions {
    public static Animation ToAnimation(this AnimationFrame[] animationFrames) {
        return new Animation(animationFrames);
    }
}