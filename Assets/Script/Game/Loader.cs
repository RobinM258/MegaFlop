using UnityEngine;

public class Loader : MonoBehaviour
{
    // Cet attribut force Unity à exécuter cette fonction TOUT AU DÉBUT,
    // avant le Awake de n'IMPORTE QUEL autre script dans ton jeu.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void InitiateRandom()
    {
        UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
    }
}
