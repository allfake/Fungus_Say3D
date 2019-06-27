// This code is part of the Fungus library (http://fungusgames.com) maintained by Chris Gregan (http://twitter.com/gofungus).
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;
using System;

namespace Fungus
{
    /// <summary>
    /// A Character that can be used in dialogue via the Say, Conversation and Portrait commands.
    /// </summary>
    [ExecuteInEditMode]
    public class Character3D : Character
    {
        [Tooltip("Model 3D.")]
        [SerializeField] protected GameObject model3D;

        [Tooltip("Animator.")]
        [SerializeField] public Animator animator;
    }
}
