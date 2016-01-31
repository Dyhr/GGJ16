using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Todo : MonoBehaviour
{

    public Player player;
    public Text text;

    public Transform Door;

    public string[] MustDo;

    private bool open;

    private void Update()
    {
        if (player.Todo.Count > 0)
        {
            var list = "";
            foreach (var item in player.Todo)
                if(!(!player.CanDo(item) && player.Hidden[player.Todo.IndexOf(item)]))
                    list += "- " + item + "\n";
            text.text = (!player.Control?"You forgot to:\n":"Todo:\n") + list;
        }
        else
        {
            text.text = (player.Control ? "Ready to leave!" : "You did it!");
        }

        var done = true;
        foreach(var must in MustDo)
        {
            if (player.Todo.Contains(must)) done = false;
        }
        if (done && !open)
        {
            open = true;
            Door.Rotate(0,-110,0);
        }
    }

    public int Stress()
    {
        int result = 0;
        foreach (var item in player.Todo)
            if (!(!player.CanDo(item) && player.Hidden[player.Todo.IndexOf(item)]))
                result++;
        return result;
    }
}
