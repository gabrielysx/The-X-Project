using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Opinion
{
    Friendly,
    Hate,
    Neutral
}

public class LevelManager : MonoBehaviour, ISubject
{
    public static LevelManager instance;

    public List<IObserver> observers = new List<IObserver>();
    public Opinion crabOpinion;
    [SerializeField] private int maxCrabHateValue;
    [SerializeField] private int crabHateValue;
    // Start is called before the first frame update

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
            crabHateValue = 0;
            crabOpinion = Opinion.Neutral;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadTargetScene(int index)
    {
        QuestManager.instance.RemoveObserver(UIManager.instance);
        ClearObservers();
        SceneManager.LoadScene(index);
    }

    public void AddCrabHateValue(int value)
    {
        if(crabOpinion == Opinion.Neutral)
        {
            crabHateValue += value;
            if(crabHateValue > maxCrabHateValue)
            {
                crabOpinion = Opinion.Hate;
                NotifyObservers();
            }
        }
    }

    public void AddObserver(IObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        observers.Remove(observer);
    }

    public void NotifyObservers()
    {
        foreach (IObserver observer in observers)
        {
            observer.OnNotify();
        }
    }

    public void ClearObservers()
    {
        observers.Clear();
    }
}
