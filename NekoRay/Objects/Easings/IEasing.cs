namespace NekoRay.Easings;

//TODO: use delegates
public interface IEasing {
    /// <summary>
    /// Calculates eased t (where t is a linear time from 0 to 1)
    /// </summary>
    /// <param name="x">linear time from 0 1</param>
    /// <returns>eased time</returns>
    public float Eval(float x); 
}