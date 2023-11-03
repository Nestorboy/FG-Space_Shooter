using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Nessie.SpaceShooter
{
    public static class MathUtilities
    {
        public static float2 MoveTowards(float2 current, float2 target, float maxDelta)
        {
            float2 toTarget = target - current;
            float sqrDist = math.lengthsq(toTarget);
            if (sqrDist == 0f || maxDelta >= 0f && sqrDist <= maxDelta * maxDelta)
                return target; // Close enough to target.
            
            float dist = math.sqrt(sqrDist);
            return current + toTarget / dist * maxDelta;
        }
    }
}
