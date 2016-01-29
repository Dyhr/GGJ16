using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Todo : MonoBehaviour
{

    public Player player;
    public Text text;

    private void Update()
    {
        if (player.Todo.Count > 0)
        {
            var list = "";
            foreach (var item in player.Todo)
                list += "- " + item + "\n";
            text.text = (!player.Control?"You forgot to:\n":"Todo:\n") + list;
        }
        else
        {
            text.text = (player.Control ? "Ready to leave!" : "You did it!");
        }
    }
}
