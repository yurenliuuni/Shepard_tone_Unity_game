using System.Collections;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public Transform connection;
    public KeyCode enterKeyCode = KeyCode.S;
    public Vector3 enterDirection = Vector3.down;
    public Vector3 exitDirection = Vector3.zero;
    public bool Animation = true;

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log($"觸發了！碰到的物件：{other.name}, Tag：{other.tag}");

        if (connection != null && other.CompareTag("Player"))
        {
            Debug.Log("Player 進入範圍，等待按鍵...");

            if (Input.GetKey(enterKeyCode))
            {
                Debug.Log($"按下 {enterKeyCode}！");

                if (other.TryGetComponent(out Player player))
                {
                    Debug.Log("找到 Player 元件，開始進入管道！");
                    StartCoroutine(Enter(player));
                }
                else
                {
                    Debug.LogWarning("找不到 Player 元件！請確認 Player script 是否掛上去");
                }
            }
        }
        else if (connection == null)
        {
            Debug.LogWarning("connection 是 null！請在 Inspector 指定目標");
        }
    }
    private IEnumerator Enter(Player player)
    {
        // player.movement.enabled = false;
        player.GetComponent<PlayerMovement>().enabled = false;

        if (Animation)
        {
            Vector3 enteredPosition = transform.position + enterDirection;
            Vector3 enteredScale = Vector3.one * 0.5f;

            yield return Move(player.transform, enteredPosition, enteredScale);
            yield return new WaitForSeconds(1f);
        }
        else
        {
            //for the castle offset of the box collider 
            Vector3 enteredPosition = transform.position + Vector3.left*3.72f + Vector3.down*1.13f;
            Vector3 enteredScale = Vector3.one * .5f;

            yield return Move(player.transform, enteredPosition, enteredScale);
            yield return new WaitForSeconds(1f);
        }
        

        //if i design underground would need the following 2 lines 
        // var sideSrolling = Camera.main.GetComponent<SideScrollingCamera>();
        // sideSrolling.SetUnderground(connection.position.y < sideSrolling.undergroundThreshold);

        var cam = Camera.main;
        if (cam != null)
        {
            Vector3 camPos = cam.transform.position;
            camPos.x = connection.position.x;
            camPos.y = 6;
            cam.transform.position = camPos;
        }
        if (exitDirection != Vector3.zero)
        {
            
            player.transform.position = connection.position - exitDirection;
            yield return Move(player.transform, connection.position + exitDirection, Vector3.one);
        }
        else
        {
            player.transform.position = connection.position;
            player.transform.localScale = Vector3.one;
        }

        player.movement.enabled = true;
    }

    private IEnumerator Move(Transform player, Vector3 endPosition, Vector3 endScale)
    {
        float elapsed = 0f;
        float duration = 1f; //1 sec 

        Vector3 startPosition = player.position;
        Vector3 startScale = player.localScale;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            player.position = Vector3.Lerp(startPosition, endPosition, t);
            player.localScale = Vector3.Lerp(startScale, endScale, t);
            elapsed += Time.deltaTime;

            yield return null;
        }

        player.position = endPosition;
        player.localScale = endScale;
    }

}
