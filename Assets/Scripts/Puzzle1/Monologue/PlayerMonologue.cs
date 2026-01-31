using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerMonologue", menuName = "Player Monologue")]
public class PlayerMonologue : ScriptableObject
{
    public string npcName;
    public Sprite npcPortrait;
    public string[] dialogueLines;
    public bool[] autoProgressLines;
    public float autoProgressDelay = 1.5f;

    public float typingSpeed = 0.05f;
    public AudioClip voiceSound;
    public float voicePitch = 1f;
}
