// This code is part of the Fungus library (http://fungusgames.com) maintained by Chris Gregan (http://twitter.com/gofungus).
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Text;

namespace Fungus
{

    /// <summary>
    /// Writes text using a typewriter effect to a UI text object.
    /// </summary>
    public class Writer3D : Writer
    {
        [SerializeField]
        protected string animationName;
        [SerializeField]
        protected int animationTalk = Animator.StringToHash("Talk");
        [SerializeField]
        protected Animator animator;

        protected void LateUpdate()
        {
            if (this.IsWaitingForInput && animator != null)
            {
                animator.SetBool(animationTalk, false);
            }
        }

        /// <summary>
        /// Writes text using a typewriter effect to a UI text object.
        /// </summary>
        /// <param name="content">Text to be written</param>
        /// <param name="clear">If true clears the previous text.</param>
        /// <param name="waitForInput">Writes the text and then waits for player input before calling onComplete.</param>
        /// <param name="stopAudio">Stops any currently playing audioclip.</param>
        /// <param name="waitForVO">Wait for the Voice over to complete before proceeding</param>
        /// <param name="audioClip">Audio clip to play when text starts writing.</param>
        /// <param name="onComplete">Callback to call when writing is finished.</param>
        public virtual IEnumerator Write3D(string content, bool clear, bool waitForInput, bool stopAudio, bool waitForVO, AudioClip audioClip, Action onComplete, Animator animator, string animationName)
        {
            if (clear)
            {
                textAdapter.Text = "";
                visibleCharacterCount = 0;
            }

            if (!textAdapter.HasTextObject())
            {
                yield break;
            }

            // If this clip is null then WriterAudio will play the default sound effect (if any)
            NotifyStart(audioClip);

            this.animator = animator;

            if (animator != null)
            {
                if (animationName != "")
                {
                    this.animator.SetTrigger(animationName);
                }
                this.animator.SetBool(animationTalk, true);
            }

            string tokenText = TextVariationHandler.SelectVariations(content);
            
            if (waitForInput)
            {
                tokenText += "{wi}";
            }

            if(waitForVO)
            {
                tokenText += "{wvo}";
            }

            List<TextTagToken> tokens = TextTagParser.Tokenize(tokenText);

            gameObject.SetActive(true);

            yield return StartCoroutine(ProcessTokens(tokens, stopAudio, onComplete));
        }

        protected override IEnumerator DoWait(float duration)
        {
            NotifyPause();
            if (animator != null)
            {
                animator.SetBool(animationTalk, false);
            }

            float timeRemaining = duration;
            while (timeRemaining > 0f && !exitFlag)
            {
                if (instantComplete && inputFlag)
                {
                    break;
                }

                timeRemaining -= Time.deltaTime;
                yield return null;
            }

            if (animator != null)
            {
                animator.SetBool(animationTalk, true);
            }
            NotifyResume();
        }
    }
}
