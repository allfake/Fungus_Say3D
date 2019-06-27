// This code is part of the Fungus library (http://fungusgames.com) maintained by Chris Gregan (http://twitter.com/gofungus).
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Fungus
{
    /// <summary>
    /// Display story text in a visual novel style dialog box.
    /// </summary>
    public class Say3DDialog : SayDialog
    {
        #region Public members
        public override void Say(string text, bool clearPrevious, bool waitForInput, bool fadeWhenDone, bool stopVoiceover, bool waitForVO, AudioClip voiceOverClip, Action onComplete)
        {
            StartCoroutine(DoSay(text, clearPrevious, waitForInput, fadeWhenDone, stopVoiceover, waitForVO, voiceOverClip, onComplete));
        }

        public void Say3D(string text, bool clearPrevious, bool waitForInput, bool fadeWhenDone, bool stopVoiceover, bool waitForVO, AudioClip voiceOverClip, Action onComplete, string animationName)
        {
            StartCoroutine(DoSay3D(text, clearPrevious, waitForInput, fadeWhenDone, stopVoiceover, waitForVO, voiceOverClip, onComplete, animationName));
        }

        /// <summary>
        /// Write a line of story text to the Say Dialog. Must be started as a coroutine.
        /// </summary>
        /// <param name="text">The text to display.</param>
        /// <param name="clearPrevious">Clear any previous text in the Say Dialog.</param>
        /// <param name="waitForInput">Wait for player input before continuing once text is written.</param>
        /// <param name="fadeWhenDone">Fade out the Say Dialog when writing and player input has finished.</param>
        /// <param name="stopVoiceover">Stop any existing voiceover audio before writing starts.</param>
        /// <param name="voiceOverClip">Voice over audio clip to play.</param>
        /// <param name="onComplete">Callback to execute when writing and player input have finished.</param>
        public IEnumerator DoSay3D(string text, bool clearPrevious, bool waitForInput, bool fadeWhenDone, bool stopVoiceover, bool waitForVO, AudioClip voiceOverClip, Action onComplete, string animationName)
        {
            Writer3D writer = (Writer3D)GetWriter();

            if (writer.IsWriting || writer.IsWaitingForInput)
            {
                writer.Stop();
                while (writer.IsWriting || writer.IsWaitingForInput)
                {
                    yield return null;
                }
            }

            if (closeOtherDialogs)
            {
                for (int i = 0; i < activeSayDialogs.Count; i++)
                {
                    var sd = activeSayDialogs[i];
                    if (sd.gameObject != gameObject)
                    {
                        sd.SetActive(false);
                    }
                }
            }
            gameObject.SetActive(true);

            this.fadeWhenDone = fadeWhenDone;

            // Voice over clip takes precedence over a character sound effect if provided

            AudioClip soundEffectClip = null;
            if (voiceOverClip != null)
            {
                WriterAudio writerAudio = GetWriterAudio();
                writerAudio.OnVoiceover(voiceOverClip);
            }
            else if (speakingCharacter != null)
            {
                soundEffectClip = speakingCharacter.SoundEffect;
            }

            writer.AttachedWriterAudio = writerAudio;

            yield return StartCoroutine(writer.Write3D(text, clearPrevious, waitForInput, stopVoiceover, waitForVO, soundEffectClip, onComplete, ((Character3D)speakingCharacter).animator, animationName));
        }
        #endregion
    }
}
