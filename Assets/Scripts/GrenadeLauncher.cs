using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static System.Net.Mime.MediaTypeNames;

public class GrenadeLauncher : MonoBehaviour
{
    public GameObject grenadePrefab; // Bomba prefab'ı
    public LineRenderer lineRenderer; // Çizgi gösterimi için LineRenderer
    public Transform handTransform; // Oyuncunun el pozisyonunu temsil eden transform
    public float targetDistance = 70f; // Hedef alınacak uzaklık
    public float throwSpeed = 10f;
    public int maxGrenades = 10;
    public int availableGrenades = 0;
    public Slider grenadeSlider;
    public TextMeshProUGUI grenadeCountText;

    private Camera mainCamera;
    private Vector3 targetPosition;
    private bool isAiming = false;

    void Start()
    {
        mainCamera = Camera.main;
        lineRenderer.positionCount = 50; // Çizgi 50 noktadan oluşur (başlangıç ve bitiş)

        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Standard"));
        lineRenderer.textureMode = LineTextureMode.Tile;
        lineRenderer.colorGradient = new Gradient();
        lineRenderer.colorGradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.red, 0.0f), new GradientColorKey(Color.yellow, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
        );
        lineRenderer.alignment = LineAlignment.View;
        lineRenderer.enabled = false;


        if (grenadeSlider != null)
        {
            grenadeSlider.minValue = 0;
            grenadeSlider.maxValue = maxGrenades;

            UpdateGrenadeSlider();
        }

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && availableGrenades > 0) // Sağ tık basıldığında
        {
            StartAiming();
        }

        if (Input.GetMouseButton(1) && isAiming) // Sağ tık basılı tutulduğunda
        {
            UpdateTrajectory();
        }

        if (Input.GetMouseButtonUp(1) && isAiming) // Sağ tık bırakıldığında
        {
            ThrowGrenade();
        }
    }

    void StartAiming()
    {
        isAiming = true;
        lineRenderer.enabled = true;
    }

    void UpdateTrajectory()
    {
        Ray ray = new Ray(handTransform.position, handTransform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Hedefi belirli bir uzaklıkta bir nokta olarak ayarla
            targetPosition = ray.GetPoint(targetDistance);

            // Çizgiyi çiz
            DrawTrajectory(handTransform.position, targetPosition);
            isAiming = true; // isAiming'i true olarak ayarla
        }
    }

    void DrawTrajectory(Vector3 start, Vector3 end)
    {
        // Bezier eğrisi kullanarak kavisli çizgi çiz
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            float t = i / (float)(lineRenderer.positionCount - 1);
            Vector3 point = CalculateBezierPoint(t, start, (start + end) / 2 + Vector3.up * 5f, end);
            lineRenderer.SetPosition(i, point);
        }
    }

    Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p2;

        return p;
    }

    void ThrowGrenade()
    {
        if (isAiming)
        {
            isAiming = false;
            lineRenderer.enabled = false;

            GameObject grenade = Instantiate(grenadePrefab, handTransform.position, Quaternion.identity);
            grenade.GetComponent<GrenadeTrigger>().SetThrown();
            Rigidbody rb = grenade.GetComponent<Rigidbody>();

            Vector3 throwDirection = (targetPosition - handTransform.position).normalized;

            // Hareketi başlatmadan önce fizik parametrelerini ayarla
            rb.velocity = throwDirection * throwSpeed;
            rb.useGravity = true;

            availableGrenades--;
            UpdateGrenadeSlider();
            // Bombayı fırlatırken yukarı çıkarken yavaşlat ve aşağı inerken ivmeli hızlan
            StartCoroutine(SimulateProjectile(rb, throwDirection, throwSpeed));
        }
    }

    IEnumerator SimulateProjectile(Rigidbody rb, Vector3 throwDirection, float initialSpeed)
    {
        float elapsed_time = 0;
        float total_time = .01f * initialSpeed / Mathf.Abs(Physics.gravity.y);

        while (elapsed_time < total_time)
        {
            float verticalSpeed = initialSpeed - Mathf.Abs(Physics.gravity.y) * elapsed_time;
            Vector3 currentVelocity = throwDirection * initialSpeed;
            currentVelocity.y = Mathf.Clamp(verticalSpeed, 0, initialSpeed);
            rb.velocity = currentVelocity;

            elapsed_time += Time.deltaTime;
            yield return null;
        }
    }

    public void UpdateGrenadeSlider()
    {
        // Eğer Slider bağlı değilse veya null ise işlem yapma
        if (grenadeSlider == null)
            return;
        grenadeSlider.value = availableGrenades;
        grenadeCountText.text = availableGrenades.ToString();
    }
}
