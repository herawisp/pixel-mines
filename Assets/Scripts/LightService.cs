using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightService : MonoBehaviour {
    
    [Range(0, 100)]
    public float Fuel;
    public float MaximumFuel;
    public float DepletionRate;
    public Transform LightBar;

    Light2D _light;
    float _maxLightRadius = 7f;
    float _timer = 0f;

    void Awake() {
        _light = GetComponent<Light2D>();
    }

    void Update() {
        _timer += Time.deltaTime;
        if(_timer >= 1f) {
            _timer = 0f;
            AdjustFuel(- DepletionRate);
            UpdateLightRadius();
        }    
    }

    void UpdateLightRadius() {
        float ratio = Fuel / MaximumFuel;
        _light.pointLightOuterRadius = _maxLightRadius * ratio;
    }

    void UpdateLightBar()
    {
        float ratio = Fuel / MaximumFuel;
        float yPos = - (80 - (80 * ratio));
        LightBar.localPosition = new Vector3(0, yPos, 0);
    }

    public void AdjustFuel(float amount)
    {
        Fuel = Mathf.Clamp(Fuel + amount, 0, 100);
        UpdateLightBar();
    }
}
