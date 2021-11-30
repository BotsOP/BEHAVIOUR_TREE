using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum TextDisplay
{
    Patrolling,
    Noticing,
    Attack,
    Happy
}

public class EnemyUI : MonoBehaviour
{
    public Slider slider;
    public TMP_Text text;
    
    public void UpdateSlider(float min, float max, float value)
    {
        float sliderValue = Remap(value, min, max, 0, 1);
        slider.value = sliderValue;
    }

    public void UpdateText(TextDisplay textDisplay)
    {
        switch (textDisplay)
        {
            case TextDisplay.Noticing:
                text.text = "?";
                break;
            case TextDisplay.Patrolling:
                text.text = "0";
                break;
            case TextDisplay.Attack:
                text.text = "!";
                break;
            case TextDisplay.Happy:
                text.text = ":)";
                break;
        }
    }
    
    public float Remap (float from, float fromMin, float fromMax, float toMin,  float toMax)
    {
        var fromAbs  =  from - fromMin;
        var fromMaxAbs = fromMax - fromMin;      
       
        var normal = fromAbs / fromMaxAbs;
 
        var toMaxAbs = toMax - toMin;
        var toAbs = toMaxAbs * normal;
 
        var to = toAbs + toMin;
       
        return to;
    }
}
