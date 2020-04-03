using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float crug_time;
    public Text crug_time_text;
    private void Start()
    {
        
        Coroutine cor = StartCoroutine(TimerToReloadScene());   // запуск карутины и получение ссылки на нее
       // StopCoroutine(cor);                                     // отсановка карутины
    }

    public IEnumerator TimerToReloadScene()
    {
        yield return new WaitForSeconds(crug_time);
        ReloadScene();
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(1);
    }

    private void Update()
    {
        crug_time -= Time.deltaTime;
        crug_time_text.text = crug_time >0 ? crug_time.ToString() : "0";
    }
}
