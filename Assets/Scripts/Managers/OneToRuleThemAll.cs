using UnityEngine;

public class OneToRuleThemAll : MonoBehaviour
{
    static OneToRuleThemAll _instance;


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(this.gameObject);

    }
}
