using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class panel : MonoBehaviour
{
    public TextMeshProUGUI tilkiSayisi;
    public TextMeshProUGUI inekSayisi;
    public int tilkiOldurme = 0;
    public int inekKurtarma = 0;

    void Update()
    {
        
    }

    public void YeniOyun()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void TilkiOldu()
    {
        tilkiOldurme++; // Skoru arttýr
        tilkiSayisi.text = "Skor: " + tilkiOldurme.ToString(); // Skoru TMP üzerine yazdýr
    }
}
