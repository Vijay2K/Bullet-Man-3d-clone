using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static IEnumerator HitImpact(SkinnedMeshRenderer renderer,Color start, Color end, float speed)
    {
        yield return ColorChanger(renderer, start, end, speed);
        yield return ColorChanger(renderer, end, start, speed);
    }

    public static IEnumerator ColorChanger(SkinnedMeshRenderer renderer,Color start, Color end, float speed)
    {
        float lerp = 0;
        do
        {
            lerp += Time.deltaTime * speed;
            renderer.material.color = Color.Lerp(start, end, lerp);
            yield return null;
        } while (lerp < 1f);
    }
}
