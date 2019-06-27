// This code is part of the Fungus library (http://fungusgames.com) maintained by Chris Gregan (http://twitter.com/gofungus).
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;

namespace Fungus
{
    /// <summary>
    /// Writes text in a dialog box.
    /// </summary>
    [CommandInfo("Opendream", 
                 "Say3D", 
                 "Writes text in a dialog box.")]
    [AddComponentMenu("")]
    public class Say3D : Say
    {
        [SerializeField] protected string animationName;
        public override void OnEnter()
        {
            if (!showAlways && executionCount >= showCount)
            {
                Continue();
                return;
            }

            executionCount++;

            // Override the active say dialog if needed
            if (character != null && character.SetSayDialog != null)
            {
                SayDialog.ActiveSayDialog = character.SetSayDialog;
            }

            if (setSayDialog != null)
            {
                SayDialog.ActiveSayDialog = setSayDialog;
            }

            Say3DDialog sayDialog = (Say3DDialog)SayDialog.GetSayDialog();
            if (sayDialog == null)
            {
                Continue();
                return;
            }
    
            var flowchart = GetFlowchart();

            sayDialog.SetActive(true);

            sayDialog.SetCharacter(character);
            sayDialog.SetCharacterImage(portrait);

            string displayText = storyText;

            var activeCustomTags = CustomTag.activeCustomTags;
            for (int i = 0; i < activeCustomTags.Count; i++)
            {
                var ct = activeCustomTags[i];
                displayText = displayText.Replace(ct.TagStartSymbol, ct.ReplaceTagStartWith);
                if (ct.TagEndSymbol != "" && ct.ReplaceTagEndWith != "")
                {
                    displayText = displayText.Replace(ct.TagEndSymbol, ct.ReplaceTagEndWith);
                }
            }

            string subbedText = flowchart.SubstituteVariables(displayText);

            sayDialog.Say3D(subbedText, !extendPrevious, waitForClick, fadeWhenDone, stopVoiceover, waitForVO, voiceOverClip, delegate {
                Continue();
            }, animationName);
        }
    }
}